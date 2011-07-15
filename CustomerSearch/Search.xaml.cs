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

using IdentityStream.Client.Data;
using IdentityStream.Windows.Controls;
using IdentityStream.Client.Biometrics;
using IdentityStream.Integration;



namespace CustomerSearch
{
    /// <summary>
    /// Interaction logic for Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        private const string IDS_APPLICATION_CODE = "IDS";
        private const string CLASS_NAME = "IdentityStream.Customer.Search";

        public Search()
        {
            InitializeComponent();
            this.LastNameSearch.Focus();
        }

        private void Search_Click( object sender, RoutedEventArgs e )
        {
            PerformSearch();
        }

        private void CustomerSearchResults_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            if ( this.CustomerSearchResults.SelectedItems.Count > 0 )
            {
                Identity selectedCustomer = ( Identity )this.CustomerSearchResults.SelectedItem;
                BioVerifyCustomer( selectedCustomer );
            }
        }

        private void Search_KeyDown( object sender, KeyEventArgs e )
        {
            if ( e.Key == Key.Return )
            {
                PerformSearch();
            }
        }

        private void ClearSearch_Click( object sender, RoutedEventArgs e )
        {
            ClearSearchResults();
        }

        private void Close_Click( object sender, RoutedEventArgs e )
        {
            this.DialogResult = true;
        }

        private void PerformSearch()
        {
            string firstNameSearch = null;
            string lastNameSearch = null;
            string t24Id = null;

            if ( this.FirstNameSearch.Text.Length > 0 )
            {
                firstNameSearch = string.Format( "%{0}%", this.FirstNameSearch.Text );
            }

            if ( this.LastNameSearch.Text.Length > 0 )
            {
                lastNameSearch = string.Format( "%{0}%", this.LastNameSearch.Text );
            }

            if ( this.CustomerId.Text.Length > 0 )
            {
                t24Id = string.Format( "%{0}%", this.CustomerId.Text );
            }

            ErrorCollection EC = new ErrorCollection();
            List<Identity> customers = BiometricAccess.GetCustomersByNameOrId(out EC, firstNameSearch, lastNameSearch, t24Id);
            if (EC.HasErrors)
            {
                TaskDialog.Show(EC.ContactAdmin, EC.ErrorOccured, MessageBoxButton.OK, EC.ToString());
            }
            else
            {
                this.CustomerSearchResults.ItemsSource = customers;
            }
        }

        private void ClearSearchResults()
        {
            this.FirstNameSearch.Text = string.Empty;
            this.LastNameSearch.Text = string.Empty;
            this.CustomerId.Text = string.Empty;
            this.CustomerSearchResults.ItemsSource = null;
        }

        private void BioVerifyCustomer( Identity currentIdentity )
        {
            BioChallenge challenge = new BioChallenge( "CIDLOGIN", currentIdentity );
            bool? result = challenge.ShowDialog();

            if ( challenge.Status == VerificationStatus.Success )
            {
                TaskDialog.Show( string.Format("User verification PASSED for ", currentIdentity.T24Id), MessageBoxButton.OK );
            }
            else
            {
                MessageBox.Show("Unable to verify the customer.", string.Format("Verification failed for customer {0}", currentIdentity.T24Id), MessageBoxButton.OK);
                //TaskDialog.Show( "User verification FAILED", MessageBoxButton.OK );
            }
        }

        void integration_AutomationSuccess(object sender, AutomationSuccessEventArgs e)
        {
            throw new NotImplementedException();
        }

        void integration_AutomationError(object sender, AutomationErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        void integration_IntegrationError(object sender, IntegrationErrorEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
