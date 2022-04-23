using System;
using System.Collections.Generic;

namespace StockMarket.Shared.Data.Context
{
    public partial class MarketHour
    {
        public int Id { get; set; }
        public string DayOfWeek { get; set; }
        public TimeSpan? OpeningHours { get; set; }
        public TimeSpan? ClosingHours { get; set; }
        public int IsMarketDay { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
