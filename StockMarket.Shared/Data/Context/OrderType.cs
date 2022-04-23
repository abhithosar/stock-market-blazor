using System;
using System.Collections.Generic;

namespace StockMarket.Shared.Data.Context
{
    public partial class OrderType
    {
        public int Id { get; set; }
        public string OrderType1 { get; set; }
        public string OrderTypeCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
