using System;
using System.Collections.Generic;

namespace StockMarket.Shared.Data.Context
{
    public partial class StockLedger
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public DateTime Date { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal DayLowPrice { get; set; }
        public decimal DayHighPrice { get; set; }
        public long DayVolume { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual Stock Stock { get; set; }
    }
}
