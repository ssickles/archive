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
using SyncRepositoryDomainModel;
using System.Collections.ObjectModel;

namespace SyncRepositoryClient
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            Repository.Current.Activate();
        }

        public ObservableCollection<Identity> Identities
        {
            get
            {
                return Repository.Current.Identities;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddIdentity addIdentity = new AddIdentity();
            addIdentity.ShowDialog();
            addIdentity = null;
        }
    }
}
