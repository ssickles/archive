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

namespace GlobalControlTest
{
    /// <summary>
    /// Interaction logic for MainWindow2.xaml
    /// </summary>
    public partial class MainWindow2 : Window
    {
        public MainWindow2()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MediaElement media = App.Current.Resources["Media"] as MediaElement;
            if (media != null)
            {
                media.Stop();
            }
            else
            {
                MessageBox.Show("Can't access MediaElement");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MediaElement media = App.Current.Resources["Media"] as MediaElement;
            if (media != null)
            {
                media.Play();
            }
            else
            {
                MessageBox.Show("Can't access MediaElement");
            }
        }
    }
}
