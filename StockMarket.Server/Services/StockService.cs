using StockMarket.Shared.Data.Context;
using StockMarket.Shared.Data.Models;

namespace StockMarket.Server.Services
{
    public class StockService
    {
        public static bool BuyStock(BuyStock stockToBuy, Shared.Data.Context.StockMarketContext context, string userid)
        {
            try
            {
                var stock = context.Stocks.First(x => x.Id == stockToBuy.Id && x.IsEnabledForTrading == 1);
                if (stock == null)
                {
                    return false;
                }
                if (stock.RemainingStockVolume < stockToBuy.OrderedAmount)
                {
                    return false;
                }
                UpdateExpiryData(stockToBuy, context);

                var ledgerEntry = context?.RunningDayStockLedgers.FirstOrDefault(x => x.Date.Date == DateTime.Now.Date && x.StockId == stockToBuy.Id);

                //order will be executed with ledger price or current price if 0
                decimal currentPrice = stockToBuy.OrderedPrice == 0 ? (ledgerEntry != null ? ledgerEntry.CurrentPrice : stockToBuy.OrderedPrice) : stockToBuy.OrderedPrice;

                var order = new Order()
                {
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    OrderCode = stockToBuy.ExpiryDate.Date == DateTime.Today && stock.CurrentPrice > stockToBuy.OrderedPrice ? "NOR" : "LIM",
                    ExpiryDate = stockToBuy.ExpiryDate,
                    OrderDate = DateTime.Now,
                    OrderedPrice = currentPrice,
                    PurchaseCode = string.Empty,
                    StockId = stockToBuy.Id,
                    IsBuy = 1,
                    StockTicker = stock.TickerName,
                    Volume = stockToBuy.OrderedAmount,
                    UserId = userid,
                    OrderTotal = currentPrice * stockToBuy.OrderedAmount

                };
                context?.Orders.Add(order);

                var cashLedger = context.CashLedgers.FirstOrDefault(x => x.UserId == userid);
                if (cashLedger != null)
                {
                    cashLedger.Amount -= currentPrice * stockToBuy.OrderedAmount;
                }

                context?.SaveChanges();
                StockMarketService.Instance.UpdateOrders();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void UpdateExpiryData(dynamic stockToBuy, Shared.Data.Context.StockMarketContext context)
        {
            var day = DateTime.Now.DayOfWeek.ToString();
            var marketClosing = context.MarketHours.SingleOrDefault(x => x.DayOfWeek == day && x.IsMarketDay == 1);
            DateTime expirydate = DateTime.Now;

            if (marketClosing != null)
            {
                if (marketClosing.ClosingHours.HasValue)
                    stockToBuy.ExpiryDate = stockToBuy.ExpiryDate.Add(marketClosing.ClosingHours.Value);
                else
                    stockToBuy.ExpiryDate = stockToBuy.ExpiryDate.Add(TimeSpan.FromHours(17));
            }
        }

        internal static bool SellStock(SellStock stockToSell, StockMarketContext context, string userid)
        {

            try
            {
                var stock = context.Stocks.First(x => x.Id == stockToSell.Id && x.IsEnabledForTrading == 1);
                if (stock == null)
                {
                    return false;
                }

                UpdateExpiryData(stockToSell, context);

                var ledgerEntry = context?.RunningDayStockLedgers.FirstOrDefault(x => x.Date.Date == DateTime.Now.Date && x.StockId == stockToSell.Id);

                //order will be executed with ledger price or current price if 0
                decimal currentPrice = stockToSell.OrderedPrice == 0 ? ledgerEntry.CurrentPrice : stockToSell.OrderedPrice;

                var order = new Order()
                {
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    OrderCode = stockToSell.ExpiryDate.Date == DateTime.Today ? "NOR" : "LIM",
                    ExpiryDate = stockToSell.ExpiryDate,
                    OrderDate = DateTime.Now,
                    OrderedPrice = currentPrice,
                    PurchaseCode = string.Empty,
                    StockId = stockToSell.Id,
                    IsBuy = 0,
                    StockTicker = stock.TickerName,
                    Volume = stockToSell.SellAmount,
                    UserId = userid,
                    OrderTotal = currentPrice * stockToSell.OrderedPrice

                };
                context?.Orders.Add(order);

                var cashLedger = context.CashLedgers.FirstOrDefault(x => x.UserId == userid);
                if (cashLedger != null)
                {
                    cashLedger.Amount -= currentPrice * stockToSell.SellAmount;
                }
                var cashLedgerHisoty = new CashLedgerHistory() { 
                                            Amount = cashLedger.Amount, 
                                            TransactionCode = "DESP", 
                                            UserId = userid, TransactionDate = DateTime.Now }; 
                if(cashLedgerHisoty != null)
                {
                    context.CashLedgerHistories.Add(cashLedgerHisoty);
                }
              
                context?.SaveChanges();
                StockMarketService.Instance.UpdateOrders();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
