using PortfolioTracker.Data.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PortfolioTracker.Data.Services
{
    public interface IDatabaseService
    {
        List<Stock> GetStocks();
        List<string> GetTradeTypes();
        List<Trade> GetTrades();
        void SaveTrade(Trade trade);
        void InitializeDatabase();
    }
}
