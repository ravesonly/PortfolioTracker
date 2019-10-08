using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Data.Models
{
    public class Trade
    {
        //enum TradeType
        //{
        //    Buy,
        //    Sell
        //}
        public int ID { get; set; }
        public int StockID { get; set; }

        public string StockTicker { get; set; }
        public string TradeType { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal Cost { get { return Price * Quantity; } }
    }
}
