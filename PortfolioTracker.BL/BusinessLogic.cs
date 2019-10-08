using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using PortfolioTracker.Data.Models;
using PortfolioTracker.Data.Services;
using System.Collections.ObjectModel;


namespace PortfolioTracker.BL
{
    public class TrackerBusinessLogic:ITrackerBusinessLogic
    {
        private readonly IDatabaseService databaseService;
        private readonly IStorageService storageService;
        private const string APIKEY = "B4G5PPCSVPTPOCHS";
        public TrackerBusinessLogic (IDatabaseService databaseService, IStorageService storageService)
        {
            try
            {
                this.databaseService = databaseService;
                this.storageService = storageService;
                databaseService.InitializeDatabase();
            }
            catch (Exception ex)
            {

            }
        }

        public List<Stock> GetStocks()
        {
            return databaseService.GetStocks();
        }

        public List<string> GetTradeTypes()
        {
            return databaseService.GetTradeTypes();
        }
        public Decimal GetPriceByTickerAndDate(string Ticker, string AsofDate)
        {
            Decimal price = 0;
            string page = string.Empty;
            if (AsofDate == DateTime.Today.ToString("yyyy-MM-dd"))
                price = GetCurrentPriceByTicker(Ticker);
            else
                using (HttpClient client = new HttpClient())
                {
                    if (!string.IsNullOrEmpty(Ticker))
                        page = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=" + Ticker + "&apikey=APIKEY";

                    var response = client.GetStringAsync(page).Result;
                    dynamic data = JObject.Parse(response);
                    if (data["Time Series (Daily)"] != null && data["Time Series (Daily)"][AsofDate] != null
                        && Decimal.TryParse(data["Time Series (Daily)"][AsofDate]["4. close"].ToString(), out decimal d))
                        price = Convert.ToDecimal(data["Time Series (Daily)"][AsofDate]["4. close"]);
                }
            return price;
        }

        public Decimal GetCurrentPriceByTicker(string Ticker)
        {
            string page = string.Empty;
            Decimal currentPrice = 0;
            if (!string.IsNullOrEmpty(Ticker))
                page = "https://www.alphavantage.co/query?function=GLOBAL_QUOTE&symbol=" + Ticker + "&apikey=APIKEY";
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetStringAsync(page).Result;
                dynamic data = JObject.Parse(response);
                string unparsedPrice = (data == null || data["Global Quote"] == null) ? string.Empty : data["Global Quote"]["05. price"];
                if (!string.IsNullOrEmpty(unparsedPrice) && Decimal.TryParse(unparsedPrice, out decimal d))
                    currentPrice = Convert.ToDecimal(data["Global Quote"]["05. price"]);
            }
            return currentPrice;
        }
        public PortfolioDTO GetTradePortfolio()
        {
            PortfolioDTO dto = new PortfolioDTO();
            dto.Trades = databaseService.GetTrades();
            dto.TradesPNL = CalculateTradesPNL(dto.Trades);
            return dto;
        }

        public List<TradePNL> CalculateTradesPNL(List<Trade> trades)
        {
            var m = trades.GroupBy(t => t.StockTicker).Select(trade => new
            TradePNL
            {
                Ticker = trade.Key,
                Cost = trade.Sum(x => (x.TradeType == "Sell" ? -1 : 1) * x.Cost),
                Quantity = trade.Sum(x => (x.TradeType == "Sell" ? -1 : 1) * x.Quantity),
                //    Price= GetCurrentPriceByTicker(trade.Key),
                //    PrevClose= GetPriceByTickerAndDate(trade.Key, DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd")),
                //    MarketValue= GetCurrentPriceByTicker(trade.Key),
                //    DailyPNL = GetPriceByTickerAndDate(trade.Key, DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd")) * Quantity
            }).ToList();
            foreach (TradePNL p in m)
            {
                p.Price = GetCurrentPriceByTicker(p.Ticker);
                p.PrevClose = GetPriceByTickerAndDate(p.Ticker, DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"));
                p.MarketValue = p.Price * p.Quantity;
                p.DailyPNL = (p.Price - p.PrevClose) * p.Quantity;
                p.InceptionPNL = p.MarketValue - p.Cost;
            }
            return m;
        }


        public void SaveTrade(Trade trade)
        {
            databaseService.SaveTrade(trade);
        }
    }

    
}
