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
using SyncRepositoryDomainModel;

namespace SyncRepositoryClient
{
    /// <summary>
    /// Interaction logic for AddIdentity.xaml
    /// </summary>
    public partial class AddIdentity : Window
    {
        public AddIdentity()
        {
            Identity = new Identity();

            InitializeComponent();
        }

        public Identity Identity { get; private set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Repository.Current.Identities.Add(Identity);
            DialogResult = true;
        }
    }
}
