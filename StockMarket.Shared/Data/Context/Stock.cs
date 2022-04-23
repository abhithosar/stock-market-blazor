using System;
using System.Collections.Generic;

namespace StockMarket.Shared.Data.Context
{
    public partial class Stock
    {
        public Stock()
        {
            RunningDayStockLedgers = new HashSet<RunningDayStockLedger>();
            StockLedgers = new HashSet<StockLedger>();
        }

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

        public virtual ICollection<RunningDayStockLedger> RunningDayStockLedgers { get; set; }
        public virtual ICollection<StockLedger> StockLedgers { get; set; }
    }
}
