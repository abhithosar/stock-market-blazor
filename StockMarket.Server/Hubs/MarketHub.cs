using Microsoft.AspNetCore.SignalR;

namespace StockMarket.Server.Hubs
{
    public class MarketHub : Hub
    {
       
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
