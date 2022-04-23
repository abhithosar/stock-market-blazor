using System;
using System.Collections.Generic;

namespace StockMarket.Shared.Data.Context
{
    public partial class PurchaseType
    {
        public int Id { get; set; }
        public string PurchaseCode { get; set; }
        public string PurchaseType1 { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
