using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Shared.Data.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string TickerName { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public long RemainingStockVolume { get; set; }
        public long InitialStockVolume { get; set; }
        public long? LastTradedShareVolume { get; set; }
        public int IsEnabledForTrading { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
