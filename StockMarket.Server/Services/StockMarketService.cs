using Microsoft.AspNetCore.SignalR;
using StockMarket.Server.Hubs;
using StockMarket.Shared.Data.Context;
using StockMarket.Shared.Data.Models;
using System.Collections.Concurrent;
using MarketHour = StockMarket.Shared.Data.Context.MarketHour;
using RunningDayStockLedger = StockMarket.Shared.Data.Models.RunningDayStockLedger;

namespace StockMarket.Server.Services
{
    public class StockMarketService
    {

        private readonly static Lazy<StockMarketService> _instance = new Lazy<StockMarketService>(
            () => new StockMarketService());

        private readonly ConcurrentDictionary<string, RunningDayStockLedger> RunningDayLedger = new ConcurrentDictionary<string, RunningDayStockLedger>();

        private readonly double _rangePercent = 0.20;
        private readonly Random _updateOrNotRandom = new Random();

        private BlockingCollection<MarketHour>? MarketHours { get; set; }
        private BlockingCollection<Order>? Orders { get; set; }
        private bool IsMarketOpen { get; set; }
        private bool HasStocksLoadedInLedger { get; set; }


        private readonly object _updateStockPricesLock = new object();
        private volatile bool _updatingStockPrices;

        Timer marketWatch;
        Timer priceUpdate;
        public static IHubContext<MarketHub>? HubContext;
        public static IHubContext<PortfolioHub>? portfolioHubContext;

        public static StockMarketService Instance
        {
            get
            {
                return _instance.Value;
            }
        }


        public void Initialize()
        {
            LoadOrders();

            LoadMarketHours();

            StartMarketStatusCheck();

            StartStockPriceUpdates();

            UpdateStockPrices();
        }

        private void LoadOrders()
        {
            var context = new StockMarketContext();
            Orders = new BlockingCollection<Order>();
            context.Orders.ToList().ForEach(h => Orders.Add(h));
            context.Dispose();
        }

        private void StartMarketStatusCheck()
        {
            marketWatch = new Timer(async (args) =>
            {
                await CheckMarketStatus();

            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(6));

        }

        private void StartStockPriceUpdates()
        {
            priceUpdate = new Timer((args) =>
            {
                UpdateStockPrices();

            }, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));
        }

        private void LoadMarketHours()
        {
            //Load market hours
            var context = new StockMarketContext();
            MarketHours = new BlockingCollection<MarketHour>();
            context.MarketHours.ToList().ForEach(h =>
                                {
                                    var mh = new MarketHour()
                                    {
                                        ClosingHours = h.ClosingHours,
                                        CreatedOn = h.CreatedOn,
                                        DayOfWeek = h.DayOfWeek,
                                        Id = h.Id,
                                        IsMarketDay = h.IsMarketDay,
                                        OpeningHours = h.OpeningHours,
                                        UpdatedOn = h.UpdatedOn
                                    };
                                    MarketHours.Add(mh);
                                });
            context.Dispose();

        }

        private void UpdateStockPrices()
        {
            lock (_updateStockPricesLock)
            {
                if (IsMarketOpen)
                    if (!_updatingStockPrices)
                    {
                        _updatingStockPrices = true;

                        foreach (var stock in RunningDayLedger.Values)
                        {
                            if (TryUpdateStockPrice(stock))
                            {
                                var context = new StockMarketContext();

                                var ledger = context.RunningDayStockLedgers.FirstOrDefault(x => x.StockId == stock.Id);
                                var st = context.Stocks.FirstOrDefault(x => x.Id == stock.Id);
                                if (ledger != null && st != null)
                                {
                                    st.CurrentPrice = stock.CurrentPrice;
                                    ledger.CurrentPrice = stock.CurrentPrice;
                                    context.SaveChanges();
                                }
                                BroadcastStockPrice(stock);
                            }
                        }

                        _updatingStockPrices = false;
                    }
            }
        }


        private bool TryUpdateStockPrice(RunningDayStockLedger ledger)
        {

            // Update the stock price by a random factor of the range percent
            var random = new Random((int)Math.Floor(ledger.CurrentPrice));
            var percentChange = random.NextDouble() * _rangePercent;
            var pos = random.NextDouble() > 0.51;
            var change = Math.Round(ledger.CurrentPrice * (decimal)percentChange, 2);
            change = pos ? change : -change;

            ledger.CurrentPrice += change;
            if (ledger.CurrentPrice > ledger.DayHighPrice)
            {
                ledger.DayHighPrice = ledger.CurrentPrice;
            }
            if (ledger.CurrentPrice < ledger.DayLowPrice)
            {
                ledger.DayLowPrice = ledger.CurrentPrice;
            }
            return true;
        }

        private void BroadcastStockPrice(RunningDayStockLedger ledger)
        {

            ExecuteOrder(ledger);


            var obj = new RunningDayStockLedger()
            {
                ClosePrice = ledger.ClosePrice,
                CurrentPrice = ledger.CurrentPrice,
                Date = ledger.Date,
                DayHighPrice = ledger.DayHighPrice,
                DayLowPrice = ledger.DayLowPrice,
                DayVolume = ledger.DayVolume,
                Id = ledger.Id,
                OpenPrice = ledger.OpenPrice,
                StockId = ledger.StockId,
                StockTicker = ledger.StockTicker
            };

            var context = new StockMarketContext();
            var dledger = context.RunningDayStockLedgers.FirstOrDefault(x => x.StockId == ledger.StockId);
            if (dledger != null)
            {
                dledger.CurrentPrice = obj.CurrentPrice;
                dledger.DayHighPrice = obj.DayHighPrice;
                dledger.DayLowPrice = obj.DayLowPrice;
                dledger.UpdatedOn = DateTime.Now;

            }
            context.SaveChanges();
            context.Dispose();

            HubContext?.Clients?.All.SendAsync("StockPriceUpdate", obj);
            portfolioHubContext?.Clients?.All.SendAsync("StockPriceUpdate", obj);

        }

        private void ExecuteOrder(RunningDayStockLedger ledger)
        {
            //1. Check if volume is available
            //2. Check if order is of same date range and price

            var orders = Orders.Where(x => x.StockId == ledger.StockId);
            if (orders == null)
                return;

            var context = new StockMarketContext();

            var stockOrders = orders.ToList();

            if (stockOrders.Count() == 0)
                return;

            foreach (var order in stockOrders)
            {
                //1. Order is for current market day
                //2. Order is expiry date
                //3. volume is not available
                var stock = context.Stocks.First(x => x.Id == ledger.StockId);

                if (order.IsBuyOrder && stock.RemainingStockVolume < order.Volume && order.ExpiryDate >= DateTime.Now)
                    continue;

                if ((order.ExpiryDate >= DateTime.Now && ledger.CurrentPrice <= order.OrderedPrice) || order.ExpiryDate < DateTime.Now)
                {

                    var orderHistory = new OrderHistory()
                    {
                        CreatedOn = order.CreatedOn,
                        ExecutedPrice = ledger.CurrentPrice,
                        IsBuy = order.IsBuy,
                        OrderCode = order.OrderCode,
                        OrderDate = order.OrderDate,
                        PurchaseCode = order.PurchaseCode,
                        OrderId = order.Id.ToString(),
                        StockId = order.StockId,
                        StockTicker = order.StockTicker,
                        UpdatedOn = DateTime.Now,
                        UserId = order.UserId,
                        Volume = order.Volume,
                        Status = order.ExpiryDate < DateTime.Now ? 0 : 1

                    };
                    context.OrderHistories.Add(orderHistory);
                    var ord = context.Orders.Single(x => x.Id == order.Id);
                    context.Orders.Remove(ord);

                    var dbledger = context.RunningDayStockLedgers.FirstOrDefault(x => x.StockId == stock.Id);
                    if (dbledger != null)
                    {
                        dbledger.DayVolume += order.IsBuyOrder ? order.Volume : -1 * order.Volume;
                        ledger.DayVolume += order.IsBuyOrder ? order.Volume : -1 * order.Volume;
                    }

                    //Adjust available volume according to buy/sell

                    stock.RemainingStockVolume += order.IsBuyOrder ? order.Volume : -1 * order.Volume;

                    var portfolio = context.Portfolios.FirstOrDefault(x => x.StockId == order.StockId);

                    if (portfolio != null)
                    {
                        portfolio.StockAmount += order.IsBuyOrder ? order.Volume : -1 * order.Volume;
                        portfolio.UpdatedOn = DateTime.Now;
                    }
                    else
                    {
                        var newPortfolio = new Portfolio()
                        {
                            CreatedOn = DateTime.Now,
                            StockAmount = order.Volume,
                            StockId = order.StockId,
                            StockTicker = order.StockTicker,
                            UpdatedOn = DateTime.Now,
                            UserId = order.UserId
                        };
                        context.Portfolios.Add(newPortfolio);
                    }

                    var cashLedger = context.CashLedgers.Where(x => x.UserId == order.UserId).FirstOrDefault();
                    if (cashLedger != null)
                    {
                        cashLedger.Amount -= order.IsBuyOrder ? order.OrderTotal : -1 * order.OrderTotal;
                    }

                    context.SaveChanges();
                }

            }
            context.Dispose();
            LoadOrders();
        }

        /*
         * Timer  callback
         * ck to check market open and close hours
         */
        private async Task CheckMarketStatus()
        {
            await Task.Run(async () =>
            {

                var today = MarketHours.FirstOrDefault(x => x.DayOfWeek.ToLower() == DateTime.Now.DayOfWeek.ToString().ToLower() && x.IsMarketDayBool);
                if (RunningDayLedger.Count() == 0)
                    HasStocksLoadedInLedger = false;
                if (today != null)
                {
                    if (DateTime.Now > DateTime.Today.Add(today.OpeningHours.Value) && DateTime.Now < DateTime.Today.Add(today.ClosingHours.Value))
                    {
                        IsMarketOpen = true;
                        if (!HasStocksLoadedInLedger)
                        {
                            ArchiveRunningLedger();

                            var context = new StockMarketContext();
                            var stocks = context.Stocks.Where(x => x.RemainingStockVolume > 0 && x.IsEnabledForTrading == 1).ToList();
                            if (stocks != null && stocks.Count > 0)
                            {
                                stocks.ForEach(x =>
                                {
                                    var ledger = new StockMarket.Shared.Data.Context.RunningDayStockLedger()
                                    {
                                        OpenPrice = x.CurrentPrice,
                                        CurrentPrice = x.CurrentPrice,
                                        ClosePrice = x.CurrentPrice,
                                        Date = DateTime.Now,
                                        DayHighPrice = x.CurrentPrice,
                                        DayLowPrice = x.CurrentPrice,
                                        DayVolume = 0,
                                        StockId = x.Id,
                                        StockTicker = x.TickerName,
                                        CreatedOn = DateTime.Now,
                                        UpdatedOn = DateTime.Now


                                    };
                                    context.RunningDayStockLedgers.Add(ledger);

                                    RunningDayLedger.TryAdd(ledger.StockTicker, new RunningDayStockLedger(ledger));

                                });
                                await context.SaveChangesAsync();
                                HasStocksLoadedInLedger = true;
                            }
                        }
                    }
                    else
                    {
                        IsMarketOpen = false;
                        Console.WriteLine("IsMarketOpen : " + IsMarketOpen);
                        HasStocksLoadedInLedger = false;
                        PerformMarketClosingProcedures();
                    }
                }
                NotifyMarketStatus();
            });

        }

        private void PerformMarketClosingProcedures()
        {
            UpdateStockMaster();
            ArchiveRunningLedger();

        }

        private void UpdateStockMaster()
        {
            var context = new StockMarketContext();
            foreach (var item in RunningDayLedger)
            {
                var ledger = item.Value;
                var stock = context.Stocks.FirstOrDefault(x => x.Id == ledger.StockId);
                if (stock != null)
                {
                    stock.CurrentPrice = ledger.CurrentPrice;
                    stock.UpdatedOn = DateTime.Now;

                    stock.LastTradedShareVolume = ledger.DayVolume;
                    stock.RemainingStockVolume -= ledger.DayVolume;
                }
            }
            context.SaveChanges();
        }

        private void ArchiveRunningLedger()
        {
            var context = new StockMarketContext();

            if (context.RunningDayStockLedgers.Any())
            {
                foreach (var ledger in context.RunningDayStockLedgers)
                {
                    var ledgerHistory = new StockLedger()
                    {
                        ClosePrice = ledger.ClosePrice,
                        Date = ledger.Date,
                        DayHighPrice = ledger.DayHighPrice,
                        DayLowPrice = ledger.DayLowPrice,
                        DayVolume = ledger.DayVolume,
                        OpenPrice = ledger.OpenPrice,
                        //Id = ledger.Id,
                        StockId = ledger.StockId,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now

                    };
                    context.StockLedgers.Add(ledgerHistory);

                }
                context.RunningDayStockLedgers.RemoveRange(context.RunningDayStockLedgers.ToList());
                context.SaveChanges();
            }
            context.Dispose();
        }
        private void NotifyMarketStatus()
        {
            HubContext?.Clients?.All.SendAsync("MarketStatusUpdated", IsMarketOpen);
        }

        public void UpdateMarketHours()
        {
            try
            {
                LoadMarketHours();
            }
            catch (Exception ex)
            {

            }
        }
        public void UpdateOrders()
        {
            try
            {
                LoadOrders();
            }
            catch (Exception ex)
            {

            }
        }
        public void UpdateStocks(string tickerName)
        {
            var context = new StockMarketContext();
            var stocks = context.Stocks.Where(x => x.RemainingStockVolume > 0 && x.TickerName == tickerName && x.IsEnabledForTrading == 1).ToList();
            if (stocks != null)
                stocks.ForEach(x =>
                {
                    var ledger = new StockMarket.Shared.Data.Context.RunningDayStockLedger()
                    {
                        OpenPrice = x.CurrentPrice,
                        CurrentPrice = x.CurrentPrice,
                        ClosePrice = x.CurrentPrice,
                        Date = DateTime.Now,
                        DayHighPrice = x.CurrentPrice,
                        DayLowPrice = x.CurrentPrice,
                        DayVolume = 0,
                        StockId = x.Id,
                        StockTicker = x.TickerName,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now


                    };
                    if (!RunningDayLedger.ContainsKey(ledger.StockTicker))
                    {
                        ledger.Stock = x;
                        RunningDayLedger.TryAdd(ledger.StockTicker, new RunningDayStockLedger(ledger));
                    }
                });
            context.Dispose();
        }
        public void RemoveDeletedStock(string ticker)
        {
            if (RunningDayLedger.ContainsKey(ticker))
            {
                var value = RunningDayLedger[ticker];
                RunningDayLedger.TryRemove(new KeyValuePair<string, RunningDayStockLedger>(ticker, value));
            }
        }
        public bool IsMarketRunning()
        {
            return IsMarketOpen;
        }
    }
}
