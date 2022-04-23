using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockMarket.Server.Services;
using StockMarket.Shared.Data.Context;

namespace StockMarket.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MarketConfigController : ControllerBase
    {


        private readonly StockMarketContext _stockMarketContext;
        public MarketConfigController(StockMarketContext stockMarketContext)
        {
            _stockMarketContext = stockMarketContext;
        }

        [HttpPost]
        public IActionResult UpdateMarketHour(List<MarketHour> marketHours)
        {
            try
            {
                foreach (var marketHour in marketHours)
                {
                    if (marketHour != null)
                        _stockMarketContext.MarketHours.Update(marketHour);

                }
                _stockMarketContext.SaveChanges();
                StockMarketService.Instance.UpdateMarketHours();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public List<MarketHour> GetMarketHours()
        {
            try
            {
                return _stockMarketContext.MarketHours.ToList();
            }
            catch (Exception ex)
            {
                return new List<MarketHour>();
            }
        }

        [HttpGet]
        public bool IsMarketOpen()
        {
            return StockMarketService.Instance.IsMarketRunning();   
        }


    }
}
