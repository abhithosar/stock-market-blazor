using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Shared.Data.Context
{
    public partial class MarketHour
    {
        [NotMapped]
        public bool IsMarketDayBool
        {
            get
            {
                return Convert.ToBoolean(IsMarketDay);
            }
            set
            {
                IsMarketDay = Convert.ToInt32(value);
            }
        }
    }
}
