using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Shared.Data.Context
{
    public partial class Order
    {
        [NotMapped]
        public string OrderType
        {
            get
            {
                if (OrderCode == "NOR")
                    return "Normal";
                if (OrderCode == "LIM")
                    return "Limit";

                return string.Empty;
            }
        }
        [NotMapped]
        public bool IsBuyOrder
        {
            get
            {
                return IsBuy == 1;
            }
        }
    }
}
