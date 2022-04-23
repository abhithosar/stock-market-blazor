using StockMarket.Shared.Data.Context;
using StockMarket.Shared.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Shared.Contracts
{
    public interface IMarketHoursContract
    {
        Task<bool> UpdateMarketHours(List<MarketHour> marketHours);
        Task<List<MarketHour>> GetAllMarketHours();
    }
}
