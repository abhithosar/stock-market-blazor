using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockMarket.Server.Models;
using StockMarket.Shared.Data.Context;

namespace StockMarket.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {

        private readonly StockMarketContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;

        public PortfolioController(StockMarketContext context, UserManager<ApplicationUser> userManager)
        {
            _dbcontext = context;
            _userManager = userManager;
        }

        [HttpGet]
        public List<Portfolio> GetPortfolio()
        {
            try
            {
                var userid = _userManager.GetUserId(User);
                var portfolios = _dbcontext.Portfolios.Where(x => x.UserId == userid).ToList();
                foreach (var portfolio in portfolios)
                {
                    var stock = _dbcontext.Stocks.Single(x => x.Id == portfolio.StockId);
                    if (stock != null)
                    {
                        portfolio.PortfolioValue = portfolio.StockAmount * stock.CurrentPrice;
                        portfolio.CompanyName = stock.CompanyName;
                    }
                }
                return portfolios;
            }
            catch (Exception ex)
            {
                return new List<Portfolio>();
            }
        }

        [HttpGet]
        public decimal GetCashLedger()
        {
            try
            {
                var userid = _userManager.GetUserId(User);
                var cashLedger = _dbcontext.CashLedgers.Single(x => x.UserId == userid);
                if (cashLedger != null)
                    return cashLedger.Amount;

                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
