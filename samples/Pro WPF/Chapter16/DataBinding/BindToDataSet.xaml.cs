using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;

namespace DataBinding
{
    /// <summary>
    /// Interaction logic for BindToDataSet.xaml
    /// </summary>

    public partial class BindToDataSet : System.Windows.Window
    {

        public BindToDataSet()
        {
            InitializeComponent();
        }


        private DataTable products;

        private void cmdGetProducts_Click(object sender, RoutedEventArgs e)
        {
            products = App.StoreDB2.GetProducts();
            lstProducts.ItemsSource = products.DefaultView;
        }

        private void cmdDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            //products.Rows.Remove((DataRowView)lstProducts.SelectedItem);
            ((DataRowView)lstProducts.SelectedItem).Row.Delete();
        }

        private void cmdAddProduct_Click(object sender, RoutedEventArgs e)
        {
            //products.Add(new Product("00000","?",0,"?"));
        }

        
    }
}