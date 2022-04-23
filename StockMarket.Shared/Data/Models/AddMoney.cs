using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Shared.Data.Models
{
    public  class AddMoney
    {
        public string UserId { get; set; }
        public decimal AmountToAdd { get; set; }
    }
}
