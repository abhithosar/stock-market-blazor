using System;
using System.Collections.Generic;

namespace StockMarket.Shared.Data.Context
{
    public partial class CashLedger
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }
    }
}
