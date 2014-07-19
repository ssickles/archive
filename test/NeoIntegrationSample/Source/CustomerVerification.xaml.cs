using System.Windows;

namespace NeoIntegrationSample
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CustomerVerification : Window
    {
        public CustomerVerification(string id, string name)
        {
            InitializeComponent();

            txbId.Text = id;
            txbName.Text = name;
        }

        private void butSuccess_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void butFailure_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
