using PortfolioTracker.Data.Models;
using System;
using System.Collections.Generic;

namespace PortfolioTracker.BL
{
    public interface ITrackerBusinessLogic
    {
        List<Stock> GetStocks();
        List<string> GetTradeTypes();

        Decimal GetPriceByTickerAndDate(string Ticker, string AsofDate);
        Decimal GetCurrentPriceByTicker(string Ticker);
        PortfolioDTO GetTradePortfolio();
        List<TradePNL> CalculateTradesPNL(List<Trade> trades);
        void SaveTrade(Trade trade);

    }
}