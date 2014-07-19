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
using System.Windows.Shapes;

namespace WickedSick.Workflow.Designer
{
    /// <summary>
    /// Interaction logic for PropertyRules.xaml
    /// </summary>
    public partial class DecisionProperties : Window
    {
        List<string> items = new List<string>();

        public DecisionProperties()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            items.Add("One");
            items.Add("Two");
            items.Add("Three");

            ConditionList.ItemsSource = items;
        }
    }
}
