using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Shared.Data.Context
{
    public partial class Portfolio
    {
        [NotMapped]
        public decimal PortfolioValue { get; set; }

        [NotMapped]
        public string CompanyName { get; set; }
        [NotMapped]
        public decimal CurrentPrice { get; set; }
    }
}
