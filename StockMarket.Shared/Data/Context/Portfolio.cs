using System;
using System.Collections.Generic;

namespace StockMarket.Shared.Data.Context
{
    public partial class Portfolio
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int StockId { get; set; }
        public string StockTicker { get; set; }
        public int StockAmount { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual User User { get; set; }
    }
}
