using System;
using System.Windows.Input;
using PortfolioTracker.Data.Models;
//using PortfolioTracker.Data.Services;
using PortfolioTracker.Infra;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Linq;
using PortfolioTracker.BL;

namespace PortfolioTracker.ViewModels
{
    public class MainWindowViewModel : ViewModelBase

    {
        #region Constructor
        private readonly ITrackerBusinessLogic trackerBusinessLogic;
        private const string APIKEY = "B4G5PPCSVPTPOCHS";
        public MainWindowViewModel(ITrackerBusinessLogic trackerBusinessLogic)
        {
            try
            {
                this.trackerBusinessLogic = trackerBusinessLogic;
                Stocks = trackerBusinessLogic.GetStocks();
                TradeTypes = trackerBusinessLogic.GetTradeTypes();
                SelectedStock = Stocks.Find(s => s.Ticker == "MSFT");
                Date = DateTime.Today;
                GetTradePortfolio();
            }
            catch(Exception ex)
            {
                
            }
        }
        #endregion

        #region Private methods


        private bool ValidatePrice()
        {
            if (SelectedStock != null && !string.IsNullOrEmpty(Quantity.ToString()))
                return true;
            else return false;

        }

        private void SaveTrade()
        {
            Trade trade = new Trade
            {
                StockID = SelectedStock.StockID,
                TradeType = SelectedTradeType,
                Price = Price,
                Date = Date,
                Quantity = Quantity
            };

            trackerBusinessLogic.SaveTrade(trade);
            GetTradePortfolio();
        }

        private void GetTradePortfolio()
        {
            PortfolioDTO dto= trackerBusinessLogic.GetTradePortfolio();
            Trades = dto.Trades;
            TradesPNL = dto.TradesPNL;
        }
        #endregion

        #region Properties
        private ICommand _saveTradeCommand;
        public ICommand SaveTradeCommand
        {
            get
            {
                if (_saveTradeCommand == null)
                {
                    _saveTradeCommand = new RelayCommand(param => this.SaveTrade(), param => ValidateTrade);
                }

                return _saveTradeCommand;
            }
        }
        private ICommand _getPrice;
        public ICommand GetPrice
        {
            get
            {
                if (_getPrice == null)
                {
                    _getPrice = new RelayCommand(param => Price = trackerBusinessLogic.GetPriceByTickerAndDate(
                        SelectedStock.Ticker, Date.ToString("yyyy-MM-dd")), param => ValidatePrice());
                }

                return _getPrice;
            }
        }

        public bool ValidateTrade
        {
            get
            {
                if (Quantity == 0 || string.IsNullOrEmpty(SelectedTradeType))
                    return false;
                else
                    return true;
            }
        }
        public ICommand ClickCommand
        {
            get
            {
                return null;
            }
        }
        private Stock _selectedStock;
        public Stock SelectedStock
        {
            get => _selectedStock; set
            {
                _selectedStock = value;
                Price = trackerBusinessLogic.GetPriceByTickerAndDate(_selectedStock.Ticker, Date.ToString("yyyy-MM-dd"));
            }
        }

        public string SelectedTradeType { get; set; }

        public string Ticker { get; set; }
        public string TradeType { get; set; }

        private decimal _price = 0;
        public decimal Price
        {
            get => _price;
            set => Set(ref _price, value);
        }
        public int Quantity { get; set; }

        private DateTime _d;
        public DateTime Date
        {
            get => _d; set
            {
                _d = value;
                Price = trackerBusinessLogic.GetPriceByTickerAndDate(SelectedStock.Ticker, _d.ToString("yyyy-MM-dd"));
            }
        }

        private List<Trade> _t;
        public List<Trade> Trades
        {
            get => _t;
            set => Set(ref _t, value);
        }

        private List<TradePNL> _p;
        public List<TradePNL> TradesPNL
        {
            get => _p;
            set => Set(ref _p, value);
        }
        public List<Stock> Stocks { get; private set; }
        public List<string> TradeTypes { get; private set; }
        #endregion
    }

}