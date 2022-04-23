using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockMarket.Server.Models;
using StockMarket.Server.Services;
using StockMarket.Shared.Data.Context;
using System.Security.Claims;
using SM = StockMarket.Shared.Data.Models;

namespace StockMarket.Server.Controllers
{
    [Route("api/[controller]/[action]")]

    [ApiController]
    [Authorize]
    public class StocksController : ControllerBase
    {
        private readonly StockMarketContext _stockMarketContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public StocksController(StockMarketContext stockMarketContext, UserManager<ApplicationUser> userManager)
        {
            _stockMarketContext = stockMarketContext;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> CreateStock(SM.Stock stock)
        {
            if (stock == null)
            {
                return BadRequest();
            }
            if (_stockMarketContext.Stocks.Any(x => x.TickerName.ToLower() == stock.TickerName.ToLower() ||
                                                x.CompanyName.ToLower() == stock.CompanyName.ToLower()))
            {
                return BadRequest();
            }
            try
            {
                var stockToAdd = new Stock()
                {
                    TickerName = stock.TickerName,
                    CompanyName = stock.CompanyName,
                    CreatedOn = DateTime.Now,
                    CurrentPrice = stock.InitialPrice,
                    InitialPrice = stock.InitialPrice,
                    IsEnabledForTrading = 1,
                    InitialStockVolume = stock.InitialStockVolume,
                    RemainingStockVolume = stock.InitialStockVolume,
                    UpdatedOn = DateTime.Now,
                    LastTradedShareVolume = 0

                };
                _stockMarketContext.Stocks.Add(stockToAdd);
                await _stockMarketContext.SaveChangesAsync();
                StockMarketService.Instance.UpdateStocks(stockToAdd.TickerName);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStock(string stockId)
        {
            if (string.IsNullOrEmpty(stockId))
            {
                return BadRequest();
            }
            try
            {
                var stockToRemove = _stockMarketContext.Stocks.FirstOrDefault(x => x.Id == int.Parse(stockId));
                if (stockToRemove != null){
                    stockToRemove.IsEnabledForTrading = 0;
                    StockMarketService.Instance.RemoveDeletedStock(stockToRemove.TickerName);
                }
                await _stockMarketContext.SaveChangesAsync();
                
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
                throw;
            }
        }

        [HttpGet]
        public List<SM.Stock> GetAllStocks()
        {
            try
            {
                return _stockMarketContext.Stocks.Where(x => x.IsEnabledForTrading == 1)
                                                 .Select(y => new SM.Stock()
                                                 {
                                                     Id = y.Id,
                                                     CompanyName = y.CompanyName,
                                                     CreatedOn = y.CreatedOn,
                                                     CurrentPrice = y.CurrentPrice,
                                                     InitialPrice = y.InitialPrice,
                                                     InitialStockVolume = y.InitialStockVolume,
                                                     IsEnabledForTrading = y.IsEnabledForTrading,
                                                     LastTradedShareVolume = y.LastTradedShareVolume,
                                                     RemainingStockVolume = y.RemainingStockVolume,
                                                     TickerName = y.TickerName,
                                                     UpdatedOn = y.UpdatedOn

                                                 })
                                                 .ToList();
            }
            catch (Exception ex)
            {
                return new List<SM.Stock>();
            }
        }

        [HttpGet]
        public List<SM.Stock> GetAllStocksByIds(List<int> ids)
        {
            try
            {
                return _stockMarketContext.Stocks.Where(x => x.IsEnabledForTrading == 1 && ids.Contains(x.Id))
                                                 .Select(y => new SM.Stock()
                                                 {
                                                     Id = y.Id,
                                                     CompanyName = y.CompanyName,
                                                     CreatedOn = y.CreatedOn,
                                                     CurrentPrice = y.CurrentPrice,
                                                     InitialPrice = y.InitialPrice,

                                                     InitialStockVolume = y.InitialStockVolume,
                                                     IsEnabledForTrading = y.IsEnabledForTrading,
                                                     LastTradedShareVolume = y.LastTradedShareVolume,
                                                     RemainingStockVolume = y.RemainingStockVolume,
                                                     TickerName = y.TickerName,
                                                     UpdatedOn = y.UpdatedOn

                                                 })
                                                 .ToList();
            }
            catch (Exception ex)
            {
                return new List<SM.Stock>();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public List<SM.RunningDayStockLedger> GetRunningDayStocks()
        {
            try
            {
               

                var runningDayLedgers = _stockMarketContext.RunningDayStockLedgers.ToList();
                List<SM.RunningDayStockLedger> ledgers = new List<SM.RunningDayStockLedger>();
                foreach (var ledger in runningDayLedgers)
                {
                    ledger.Stock = _stockMarketContext.Stocks.First(x => x.Id == ledger.StockId);
                    ledgers.Add(new SM.RunningDayStockLedger(ledger));
                }
                return ledgers;
            }
            catch (Exception ex)
            {
                return new List<SM.RunningDayStockLedger>();
            }
        }


        [HttpGet]
        public SM.Stock GetStock(string id)
        {

            try
            {
                var stockContext = _stockMarketContext.Stocks.Single(y => y.Id.ToString() == id);

                if (StockMarketService.Instance.IsMarketRunning())
                {
                    var ledger = _stockMarketContext.RunningDayStockLedgers.Where(x => x.StockId.ToString() == id).First();

                    var stock = new SM.Stock()
                    {
                        Id = ledger.StockId,
                        TickerName = ledger.StockTicker,
                        CurrentPrice = ledger.CurrentPrice,
                        RemainingStockVolume = stockContext.RemainingStockVolume,

                    };
                    return stock;
                }
                else
                {
                    return new SM.Stock()
                    {
                        CompanyName = stockContext?.CompanyName,
                        CurrentPrice = stockContext != null ? stockContext.CurrentPrice : 0,
                        RemainingStockVolume = stockContext != null ? stockContext.RemainingStockVolume : 0,
                        InitialPrice = stockContext != null ? stockContext.InitialPrice : 0,
                        LastTradedShareVolume = stockContext != null ? stockContext.LastTradedShareVolume : 0,
                        TickerName = stockContext?.TickerName,
                        IsEnabledForTrading = stockContext != null ? stockContext.IsEnabledForTrading : 0,
                        CreatedOn = stockContext != null ? stockContext.CreatedOn : DateTime.Now,
                        InitialStockVolume = stockContext != null ? stockContext.InitialStockVolume : 0,
                        UpdatedOn = stockContext != null ? stockContext.UpdatedOn : DateTime.Now,
                        Id = stockContext != null ? stockContext.Id : 0

                    };
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public IActionResult BuyStock(SM.BuyStock stockToBuy)
        {
            try
            {
                var userid = _userManager.GetUserId(User);
                var res = StockService.BuyStock(stockToBuy, _stockMarketContext, userid);
                return res ? Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult SellStock(SM.SellStock stockToSell)
        {
            try
            {
                var userid = _userManager.GetUserId(User);
                var res = StockService.SellStock(stockToSell, _stockMarketContext, userid);
                return res ? Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }

}
