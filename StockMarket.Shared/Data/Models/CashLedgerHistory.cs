using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Shared.Data.Context
{
    public partial class CashLedgerHistory
    {
        [NotMapped]
        public string TransactionType
        {
            get
            {
                if (TransactionCode == "DESP")
                    return "Deposit";

                if (TransactionCode == "WDRL")
                    return "Withdrawl";

                return string.Empty;


            }
        }

    }
}
