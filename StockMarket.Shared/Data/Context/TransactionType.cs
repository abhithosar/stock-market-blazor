using System;
using System.Collections.Generic;

namespace StockMarket.Shared.Data.Context
{
    public partial class TransactionType
    {
        public int Id { get; set; }
        public string TransactionType1 { get; set; }
        public string TransactionCode { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
