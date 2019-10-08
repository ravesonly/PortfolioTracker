using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioTracker.Data.Models
{
    public class PortfolioDTO
    {
        public List<Trade> Trades { get; set;}
        public List<TradePNL> TradesPNL {get; set;}
    }
}
