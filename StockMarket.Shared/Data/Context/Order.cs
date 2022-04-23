using System;
using System.Collections.Generic;

namespace StockMarket.Shared.Data.Context
{
    public partial class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int StockId { get; set; }
        public string OrderCode { get; set; }
        public string PurchaseCode { get; set; }
        public string StockTicker { get; set; }
        public decimal OrderedPrice { get; set; }
        public decimal OrderTotal { get; set; }
        public int Volume { get; set; }
        public int IsBuy { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
