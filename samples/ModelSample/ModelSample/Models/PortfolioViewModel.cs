using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace ModelSample
{
    /// <summary>
    /// View model for the portfolio view
    /// </summary>
    public class PortfolioViewModel
    {
        public PortfolioViewModel(IStockQuoteProvider quoteProvider)
        {
            _quoteProvider = quoteProvider;
            _stockModels = new ObservableCollection<StockModel>();
            _stockModels.Add(new StockModel("MSFT", _quoteProvider));

            _addCommand = new AddCommand(this);
            _removeCommand = new RemoveCommand(this);
        }

        /// <summary>
        /// The list of StockModels for the page.
        /// </summary>
        public ObservableCollection<StockModel> Stocks
        {
            get { return _stockModels; }
        }

        /// <summary>
        /// A CommandModel for Add. The command parameter should be the symbol to add.
        /// </summary>
        public CommandModel AddCommandModel
        {
            get { return _addCommand; }
        }

        /// <summary>
        /// A CommandModel for Remove. The command parameter should be the StockModel to remove.
        /// </summary>
        public CommandModel RemoveCommandModel
        {
            get { return _removeCommand; }
        }

        /// <summary>
        /// Private implementation of the Add Command
        /// </summary>
        private class AddCommand : CommandModel
        {
            public AddCommand(PortfolioViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public override void OnQueryEnabled(object sender, CanExecuteRoutedEventArgs e)
            {
                string symbol = e.Parameter as string;
                e.CanExecute = (!string.IsNullOrEmpty(symbol));
                e.Handled = true;
            }

            public override void OnExecute(object sender, ExecutedRoutedEventArgs e)
            {
                string symbol = e.Parameter as string;
                _viewModel._stockModels.Add(new StockModel(symbol, _viewModel._quoteProvider));    
            }

            private PortfolioViewModel _viewModel;
        }

        /// <summary>
        /// Private implementation of the Remove command
        /// </summary>
        private class RemoveCommand : CommandModel
        {
            public RemoveCommand(PortfolioViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public override void OnQueryEnabled(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = e.Parameter is StockModel;
                e.Handled = true;
            }

            public override void OnExecute(object sender, ExecutedRoutedEventArgs e)
            {
                _viewModel._stockModels.Remove(e.Parameter as StockModel);
            }

            private PortfolioViewModel _viewModel;
        }

        private ObservableCollection<StockModel> _stockModels;
        private CommandModel _addCommand;
        private CommandModel _removeCommand;
        private IStockQuoteProvider _quoteProvider;
    }
}
