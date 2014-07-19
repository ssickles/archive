using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Threading;

namespace LoadingAdorner
{
    /// <summary>
    /// Controller that takes care of all the logic for the DemoPage window
    /// </summary>
    public class DemoPageController : INotifyPropertyChanged
    {
        #region Commands
        /// <summary>
        /// Command to execute the search
        /// </summary>
        public static readonly RoutedUICommand SearchCommand
            = new RoutedUICommand("Search", "Search", typeof(DemoPageController));
        #endregion

        #region Properties
        private bool searchInProgress = false;
        /// <summary>
        /// Gets a flag indicating the state of the search
        /// </summary>
        public bool SearchInProgress
        {
            get { return searchInProgress; }
            set
            {
                searchInProgress = value;
                OnPropertyChanged("SearchInProgress");
            }
        }

        /// <summary>
        /// Gets the list of person object
        /// </summary>
        public ThreadableObservableCollection<Person> Data { get; private set; }

        private DemoModel model = new DemoModel();

        #endregion

        /// <summary>
        /// consructor
        /// </summary>
        public DemoPageController()
        {
            Data = new ThreadableObservableCollection<Person>(
                Application.Current.Dispatcher, null);

            Data.AddRange(model.GetData());

            //register the search command
            CommandManager.RegisterClassCommandBinding(typeof(Control),
                new CommandBinding(SearchCommand, SearchExecuted, SearchCanExecute));
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Notifies listeners that a property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region Command handlers
        private void SearchCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Parameter == null)
                e.CanExecute = false;
            else
                e.CanExecute = !String.IsNullOrEmpty(
                    e.Parameter.ToString());
        }

        private void SearchExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SearchInProgress = true;

            //Run on a seperate thread ....
            ThreadPool.QueueUserWorkItem(delegate
            {
                //simulate some work load
                System.Threading.Thread.Sleep(2000);

                Data.Clear();
                Data.AddRange(model.GetDataByName(e.Parameter.ToString()));
                SearchInProgress = false;
            });
        }
        #endregion
    }
}
