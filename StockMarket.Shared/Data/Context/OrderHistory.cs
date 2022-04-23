using System;
using System.Collections.Generic;

namespace StockMarket.Shared.Data.Context
{
    public partial class OrderHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int StockId { get; set; }
        public string OrderCode { get; set; }
        public string OrderId { get; set; }
        public string PurchaseCode { get; set; }
        public string StockTicker { get; set; }
        public decimal ExecutedPrice { get; set; }
        public int Volume { get; set; }
        public int IsBuy { get; set; }
        public int Status { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
