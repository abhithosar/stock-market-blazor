using System;
using System.Collections.Generic;

namespace StockMarket.Shared.Data.Context
{
    public partial class CashLedgerHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionCode { get; set; }
        public DateTime TransactionDate { get; set; }

        public virtual User User { get; set; }
    }
}
