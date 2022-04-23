using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Shared.Data.Models
{
    public class RunningDayStockLedger
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        [Display(Name = "Stock Ticker")]
        public string StockTicker { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "Current Price")]
        public decimal CurrentPrice { get; set; }   
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        [Display(Name = "Day Low Price")]
        public decimal DayLowPrice { get; set; }
        [Display(Name = "Day High Price")]
        public decimal DayHighPrice { get; set; }
        [Display(Name = "Day Volume")]
        public long DayVolume { get; set; }
        public decimal InitialStockVolume
        {
            get; set;
        }
        public decimal MarketCapitallization
        {
            get { return InitialStockVolume * CurrentPrice; }
        }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }

        public RunningDayStockLedger(Context.RunningDayStockLedger ledger)
        {
            this.Id = ledger.Id;
            this.ClosePrice = ledger.ClosePrice;
            this.CurrentPrice = ledger.CurrentPrice;
            this.DayLowPrice = ledger.DayLowPrice;
            this.DayHighPrice = ledger.DayHighPrice;
            this.DayVolume = ledger.DayVolume;
            this.Date = ledger.Date;
            this.CompanyName = ledger.Stock.CompanyName;
            this.StockId = ledger.StockId;
            this.OpenPrice = ledger.OpenPrice;
            this.UpdatedOn = ledger.UpdatedOn;
            this.CreatedOn = ledger.CreatedOn;
            this.StockTicker = ledger.StockTicker;
            this.InitialStockVolume = ledger.Stock.InitialStockVolume;

        }
        public RunningDayStockLedger()
        {

        }

    }
}
