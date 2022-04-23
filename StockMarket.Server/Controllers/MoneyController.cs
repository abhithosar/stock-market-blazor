using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockMarket.Server.Models;
using StockMarket.Shared.Data.Context;
using StockMarket.Shared.Data.Models;

namespace StockMarket.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MoneyController : ControllerBase
    {
        private readonly StockMarketContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public MoneyController(StockMarketContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        public IActionResult AddMoney(AddMoney money)
        {
            try
            {
                var userid = _userManager.GetUserId(User);
                money.UserId = userid;
                var ledger = _context.CashLedgers.FirstOrDefault(x => x.UserId == money.UserId);
                if (ledger == null)
                {
                    var newLedger = new CashLedger()
                    {
                        Amount = money.AmountToAdd,
                        UserId = money.UserId,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };
                    _context.CashLedgers.Add(newLedger);



                }
                else
                {
                    ledger.Amount += money.AmountToAdd;
                    ledger.UpdatedDate = DateTime.Now;
                }
                var cashLedgerHistory = new CashLedgerHistory()
                {
                    Amount = money.AmountToAdd,
                    TransactionCode = "DESP",
                    UserId = userid,
                    TransactionDate = DateTime.Now
                };
                _context.CashLedgerHistories.Add(cashLedgerHistory);

                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult RemoveMoney(AddMoney money)
        {
            try
            {
                var userid = _userManager.GetUserId(User);
                money.UserId = userid;
                var ledger = _context.CashLedgers.FirstOrDefault(x => x.UserId == money.UserId);
                if (ledger == null)
                {
                    return BadRequest();
                }
                else if (ledger.Amount == 0)
                    return BadRequest();
                else
                {
                    if (money.AmountToAdd > ledger.Amount)
                        ledger.Amount = 0;
                    else
                        ledger.Amount -= money.AmountToAdd;

                    ledger.UpdatedDate = DateTime.Now;
                }

                var cashLedgerHistory = new CashLedgerHistory()
                {
                    Amount = money.AmountToAdd,
                    TransactionCode = "WDRL",
                    UserId = userid,
                    TransactionDate = DateTime.Now
                };
                _context.CashLedgerHistories.Add(cashLedgerHistory);

                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpGet]
        public List<CashLedgerHistory> GetTransactions()
        {
            try
            {
                var userid = _userManager.GetUserId(User);

                var list = _context.CashLedgerHistories.Where(x => x.UserId == userid)?.ToList();
                if (list == null)
                    return new List<CashLedgerHistory>();

                return list;

            }
            catch (Exception ex)
            {
                return new List<CashLedgerHistory>();
            }


        }

        [HttpGet]
        public decimal GetAvailableMoney()
        {
            var userid = _userManager.GetUserId(User);
            try
            {
                return _context.CashLedgers.Single(x => x.UserId == userid).Amount;
            }
            catch (Exception ex)
            {

                return 0;
            }
        }
    }
}
