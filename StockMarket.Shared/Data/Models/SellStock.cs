using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Shared.Data.Models
{
    public  class SellStock
    {
        public int Id { get; set; }
        public int SellAmount { get; set; }
        public decimal OrderedPrice { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
