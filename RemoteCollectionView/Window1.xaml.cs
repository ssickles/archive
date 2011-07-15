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
using IdentityStream.Data;
using System.Collections;
using System.ComponentModel;
using IdentityStream.Windows.Controls;

namespace RemoteCollectionViewApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window, INotifyPropertyChanged
    {
        public Window1()
        {
            InitializeComponent();

            SetSource();
        }

        private void LoadData(object sender, RoutedEventArgs e)
        {
            BindListCollectionView();
        }

        private void LoadData2(object sender, RoutedEventArgs e)
        {

        }

        private IList _sourceList;

        private string _testText;
        public string TestText
        {
            get { return _testText; }
            set
            {
                if (_testText != value)
                {
                    _testText = value;
                    OnPropertyChanged("TestText");
                }
            }
        }
        private RemoteCollectionView<Person> _pagedItemsSource;
        public RemoteCollectionView<Person> PagedItemsSource
        {
            get { return _pagedItemsSource; }
            set
            {
                if (_pagedItemsSource != value)
                {
                    _pagedItemsSource = value;
                    OnPropertyChanged("PagedItemsSource");
                }
            }
        }

        private void BindRepository()
        {
            RemoteCollectionView<Person> rcv = new RemoteCollectionView<Person>(new Person().Get);
            //using (IDisposable defer = rcv.DeferRefresh())
            //{
                PeopleGrid.ItemsSource = rcv;
                //rcv.SortDescriptions.Add(new SortDescription("Age", ListSortDirection.Descending));
                //rcv.GroupDescriptions.Add(new PropertyGroupDescription("Country"));
            //}
        }

        private void BindListCollectionView()
        {
            QueryObject q = new QueryObject(null, new List<SortObject>(), 3, 0);
            int totalResults = -1;
            _sourceList = (IList)new Person().Get(q, out totalResults);
            ListCollectionView lcv = new ListCollectionView(_sourceList);
            using (IDisposable defer = lcv.DeferRefresh())
            {
                PeopleGrid.ItemsSource = lcv;
                lcv.SortDescriptions.Add(new SortDescription("Age", ListSortDirection.Descending));
                lcv.GroupDescriptions.Add(new PropertyGroupDescription("Country"));
            }
        }

        private void BindListCollectionView2()
        {
            QueryObject q = new QueryObject(null, new List<SortObject>(), 3, 3);
            int totalResults = -1;
            _sourceList = (IList)new Person().Get(q, out totalResults);
        }

        private void BindList()
        {
            PeopleGrid.ItemsSource = new Person().GetPersons();
        }

        private void SetSource()
        {
            RemoteCollectionView<Person> rcv = new RemoteCollectionView<Person>(new Person().Get);
            PagedItemsSource = rcv;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler obj = PropertyChanged;
            if (obj != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
