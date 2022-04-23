
using StockMarket.Shared.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Shared.Contracts
{
    public interface IStocksController
    {
        Task<bool> CreateStock(Stock stock);
        Task<bool> DeleteStock(string stockId);
        Task<List<Stock>> GetAllStocks();

    }
}
