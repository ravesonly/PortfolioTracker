using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Data.Models
{
    public class TradePNL
    {
        //enum TradeType
        //{
        //    Buy,
        //    Sell
        //}
        public int ID { get; set; }
        public int StockID { get; set; }

        public string Ticker { get; set; }

        public DateTime AsOfDate { get { return DateTime.Today; } }

        public decimal Cost { get; set; }
        public int Quantity { get; set; }   
        public decimal Price { get; set; }

        public decimal MarketValue { get; set; }

        public decimal PrevClose { get; set; }

        public decimal DailyPNL { get; set; }

        public decimal InceptionPNL { get; set; }

    }
}
