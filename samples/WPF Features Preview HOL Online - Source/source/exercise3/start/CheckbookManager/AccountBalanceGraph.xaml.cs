using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CheckbookManager.Data;
using System.ComponentModel;

namespace CheckbookManager
{
    /// <summary>
    /// Interaction logic for AccountBalanceGraph.xaml
    /// </summary>
    public partial class AccountBalanceGraph : UserControl
    {
        public AccountBalanceGraph()
        {
            InitializeComponent();
        }

        private void BalanceGraph_Loaded(object sender, RoutedEventArgs e)
        {
            // We use our own copy of the register - for some reason the
            // data grid throws an exception if we refresh the data binding 
            // to a collection being modified by the grid.
            // To work around this, we just copy the collection off
            // and then refresh it when it changes.
            InvalidateData(null, null);

            // Watch for changes.
            CheckBook.Register.CollectionChanged += InvalidateData;
            ((INotifyPropertyChanged)CheckBook.Register).PropertyChanged += InvalidateData;
        }

        private void InvalidateData(object sender, EventArgs e)
        {
            // Refresh the data from the list
            DataContext = CheckBook.Register.ToList();
        }
    }
}
