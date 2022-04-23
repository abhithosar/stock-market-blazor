using System;
using System.Collections.Generic;

namespace StockMarket.Shared.Data.Context
{
    public partial class User
    {
        public User()
        {
            CashLedgerHistories = new HashSet<CashLedgerHistory>();
            CashLedgers = new HashSet<CashLedger>();
            Portfolios = new HashSet<Portfolio>();
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual ICollection<CashLedgerHistory> CashLedgerHistories { get; set; }
        public virtual ICollection<CashLedger> CashLedgers { get; set; }
        public virtual ICollection<Portfolio> Portfolios { get; set; }
    }
}
