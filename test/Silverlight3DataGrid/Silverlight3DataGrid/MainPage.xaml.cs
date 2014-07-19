using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using IdentityStream.Data;
using System.Collections.ObjectModel;
using Silverlight3DataGrid.IdentityServiceReference;
using System.ComponentModel;

namespace Silverlight3DataGrid
{
    public partial class MainPage : UserControl
    {
        private RemoteCollectionView<Person, int> _remoteCollectionView;
        private PagedCollectionView _collectionView;

        public MainPage()
        {
            InitializeComponent();
            BindGrid();
        }

        private ICollectionView CollectionView
        {
            get
            {
                if (_collectionView == null) _collectionView = new PagedCollectionView(new Person().GetPersons());
                return _collectionView;
                //if (_remoteCollectionView == null) _remoteCollectionView = new RemotePagedCollectionView<Person, int>(new Person());
                //return _remoteCollectionView;
            }
        }

        private void BindGrid()
        {
            PersonGrid.ItemsSource = null;
            CollectionView.GroupDescriptions.Add(new
                PropertyGroupDescription("Country"));
            PersonGrid.ItemsSource = CollectionView;
        }

        private void SortCombo_SelectionChanged(object sender, 
            SelectionChangedEventArgs e)
        {
            ComboBoxItem person = SortCombo.SelectedItem as ComboBoxItem;
            CollectionView.GroupDescriptions.Clear();
            CollectionView.GroupDescriptions.Add(new 
                PropertyGroupDescription(person.Content.ToString()));

            PersonGrid.ItemsSource = null;
            PersonGrid.ItemsSource = CollectionView;
        }
    }
}
