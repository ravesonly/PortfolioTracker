using LiteDB;
using PortfolioTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace PortfolioTracker.Data.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly int numberOfStcoks = 10;
        private readonly int numberOfpnl = 5;

        private string filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\PortfolioTracker\\data.db";
        private string directoryPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\PortfolioTracker\\";


        public List<Stock> GetStocks()
        {
            using (var connection = new LiteDatabase(filePath))
            {
                //var stocks = connection.GetCollection<Stock>();
                var stocks = new Stock[] { new Stock { StockID=0, Name = "Microsoft", Ticker="MSFT" },
                    new Stock { StockID=1, Name = "Google", Ticker="GOOG" },
                    new Stock { StockID=2, Name = "Netflix", Ticker="NFLX" }};

                return stocks.ToList();
            }
        }

        public List<string> GetTradeTypes()
        {
            var TradeTypes = new List<string>{ "Buy","Sell" };
            return TradeTypes;
        }

        //public PortfolioDTO GetPortfolio()
        //{
        //    PortfolioDTO portfolioDTO = new PortfolioDTO();
        //    portfolioDTO.Trades = GetTrades();
        //    portfolioDTO.TradesPNL=GetTradesPNL()
        //}

        public List<Trade> GetTrades()
        {
            using (var connection = new LiteDatabase(filePath))
            {
                var tradesColl = connection.GetCollection<Trade>();
                var trades = tradesColl.FindAll().ToList();
                foreach (Trade trade in trades)
                    trade.StockTicker = GetStocks().Find(stock => stock.StockID == trade.StockID).Ticker;
                return trades;
            }
        }

        public void SaveTrade(Trade trade)
        {
            using (var connection = new LiteDatabase(filePath))
            {
                var trades = connection.GetCollection<Trade>();
                trades.Insert(trade);
            }
        }

        public void InitializeDatabase()
        {
            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var connection = new LiteDatabase(filePath))
            {
                var stocks = connection.GetCollection<Stock>();
                var trades = connection.GetCollection<Trade>();

                int result = stocks.Count();
                if (result == 0)
                {
                }
            }
        }        
    }
    //public class Trades : ObservableCollection<Trade>
    //{

    //    public Trades() : base() { }
    //}
}
