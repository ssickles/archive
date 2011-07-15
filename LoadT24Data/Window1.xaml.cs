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
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections;
using System.Data;

namespace LoadT24Data
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Parse(string Type)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "csv file (*.csv;*.txt)|*.csv;*.txt";
            dialog.Multiselect = false;
            bool? dialogResult = dialog.ShowDialog();
            if (dialogResult.HasValue)
            {
                if (dialogResult.Value == true)
                {
                    DataSet data = t24Parsing.ParseCSV(dialog.FileName, Type);
                    UploadPreview preview = new UploadPreview(data.Tables[Type]);
                    preview.ShowDialog();
                }
            }
        }

        private void ImportUsers_Click(object sender, RoutedEventArgs e)
        {
            Parse("t24Users");
        }

        private void ImportCustomers_Click(object sender, RoutedEventArgs e)
        {
            Parse("t24Customers");
        }

        private void ImportCompanies_Click(object sender, RoutedEventArgs e)
        {
            Parse("t24Companies");
        }

        private void ImportDAO_Click(object sender, RoutedEventArgs e)
        {
            Parse("t24AccountOfficers");
        }

        private void ImportLanguages_Click(object sender, RoutedEventArgs e)
        {
            Parse("t24Languages");
        }
    }
}
