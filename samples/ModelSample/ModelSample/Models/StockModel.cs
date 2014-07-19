using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;

namespace ModelSample
{
    public class StockModel : DataModel
    {
        public StockModel(string symbol, IStockQuoteProvider quoteProvider)
        {
            _symbol = symbol;
            _quoteProvider = quoteProvider;
        }

        protected override void OnActivated()
        {
            VerifyCalledOnUIThread();

            base.OnActivated();

            _timer = new DispatcherTimer(DispatcherPriority.Background);
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += delegate { ScheduleUpdate(); };
            _timer.Start();

            ScheduleUpdate();
        }

        protected override void OnDeactivated()
        {
            VerifyCalledOnUIThread();

            base.OnDeactivated();

            _timer.Stop();
            _timer = null;
        }

        private void ScheduleUpdate()
        {
            VerifyCalledOnUIThread();

            // Queue a work item to fetch the quote
            if (ThreadPool.QueueUserWorkItem(new WaitCallback(FetchQuoteCallback)))
            {
                this.State = ModelState.Fetching;
            }
        }

        /// <summary>
        /// Gets the stock symbol.
        /// </summary>
        public string Symbol
        {
            get { return _symbol; }
        }

        /// <summary>
        /// Gets the current quote for the stock. Only valid if State == Active.
        /// </summary>
        public double Quote
        {
            get
            {
                VerifyCalledOnUIThread();

                return _quote;
            }

            private set
            {
                VerifyCalledOnUIThread();

                if (_quote != value)
                {
                    _quote = value;
                    SendPropertyChanged("Quote");
                }
            }
        }

        /// <summary>
        /// Callback on background thread to fecth quote.
        /// </summary>
        private void FetchQuoteCallback(object state)
        {
            double fetchedQuote;
            if (_quoteProvider.TryGetQuote(_symbol, out fetchedQuote))
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new ThreadStart(delegate
                    {
                        this.Quote = fetchedQuote;
                        this.State = ModelState.Valid;
                    }));
            }
            else
            {
                this.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new ThreadStart(delegate
                    { this.State = ModelState.Invalid; }));
            }
        }

        private string _symbol;
        private double _quote;
        private IStockQuoteProvider _quoteProvider;
        private DispatcherTimer _timer;
    }
}
