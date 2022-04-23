using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockMarket.Server.Models;
using StockMarket.Server.Services;
using StockMarket.Shared.Data.Context;

namespace StockMarket.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly StockMarketContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrdersController(StockMarketContext context, UserManager<ApplicationUser> userManager)
        {
            _dbcontext = context;
            _userManager = userManager;
        }

        [HttpGet]
        public List<Order> GetAllOrders()
        {
            try
            {
                var userid = _userManager.GetUserId(User);

                return _dbcontext.Orders.Where(x => x.UserId == userid).ToList();
            }
            catch (Exception ex)
            {
                return new List<Order>();
            }
        }

        [HttpGet]
        public List<OrderHistory> GetAllArchivedOrders()
        {
            try
            {
                var userid = _userManager.GetUserId(User);

                return _dbcontext.OrderHistories.Where(x => x.UserId == userid).ToList();
            }
            catch (Exception ex)
            {
                return new List<OrderHistory>();
            }
        }

        [HttpGet]
        public IActionResult CancelOrder(string orderId)
        {
            try
            {
                var order = _dbcontext.Orders.Single(x => x.Id == Convert.ToInt32(orderId));
                var orderHistory = new OrderHistory()
                {
                    CreatedOn = order.CreatedOn,
                    ExecutedPrice = order.OrderedPrice,
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
                    Status = 0

                };
                _dbcontext.OrderHistories.Add(orderHistory);
                _dbcontext.Orders.Remove(order);
                _dbcontext.SaveChanges();
                StockMarketService.Instance.UpdateOrders();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
