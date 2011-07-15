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
using SecuredWpfApplication.SecuredService;
using System.ServiceModel;

namespace SecuredWpfApplication
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

        private void GetLoggedInUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ServerName.Text = BiometricContext.Current.GetLoggedInUser();
            }
            catch (FaultException<ErrorDetails> ex)
            {
                MessageBox.Show(ex.Detail.MethodName);
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BiometricContext.Current.Login(Username.Text, "sas0927");
            }
            catch (FaultException<ErrorDetails> ex)
            {
                MessageBox.Show(ex.Detail.MethodName);
            }
            try
            {
                LoginStatus.Text = BiometricContext.Current.IsLoggedIn().ToString();
            }
            catch (FaultException<ErrorDetails> ex)
            {
                MessageBox.Show(ex.Detail.MethodName);
            }
        }
    }
}
