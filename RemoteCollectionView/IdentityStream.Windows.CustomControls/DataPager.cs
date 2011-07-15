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
using System.Windows.Controls.Primitives;
using System.Collections;
using IdentityStream.Data;
using System.ComponentModel;

namespace IdentityStream.Windows.CustomControls
{
    public class DataPager : Control
    {
        private ButtonBase PART_Previous;
        private ButtonBase PART_Next;
        private ButtonBase PART_First;
        private ButtonBase PART_Last;

        public static readonly DependencyProperty PagedItemsSourceProperty;
        public static readonly DependencyProperty TotalItemCountProperty;
        public static readonly DependencyProperty PageIndexProperty;
        public static readonly DependencyProperty PageCountProperty;
        public static readonly DependencyProperty PageSizeProperty;
        public static readonly DependencyProperty ResultsTextProperty;
        public static readonly DependencyProperty PageTextProperty;
        public static readonly DependencyProperty PageOfTextProperty;

        static DataPager()
        {
            PagedItemsSourceProperty = DependencyProperty.Register("PagedItemsSource"
                , typeof(IPagedCollectionView)
                , typeof(DataPager)
                , new FrameworkPropertyMetadata(new PropertyChangedCallback(PagedItemsSourceChanged)));
            TotalItemCountProperty = DependencyProperty.Register("TotalItemCount"
                , typeof(int)
                , typeof(DataPager));
            PageIndexProperty = DependencyProperty.Register("PageIndex"
                , typeof(int)
                , typeof(DataPager));
            PageCountProperty = DependencyProperty.Register("PageCount"
                , typeof(int)
                , typeof(DataPager));
            PageSizeProperty = DependencyProperty.Register("PageSize"
                , typeof(int)
                , typeof(DataPager));
            ResultsTextProperty = DependencyProperty.Register("ResultsText"
                , typeof(string)
                , typeof(DataPager));
            PageTextProperty = DependencyProperty.Register("PageText"
                , typeof(string)
                , typeof(DataPager));
            PageOfTextProperty = DependencyProperty.Register("PageOfText"
                , typeof(string)
                , typeof(DataPager));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataPager), new FrameworkPropertyMetadata(typeof(DataPager)));
        }

        public IPagedCollectionView PagedItemsSource
        {
            get { return (IPagedCollectionView)GetValue(PagedItemsSourceProperty); }
            set { SetValue(PagedItemsSourceProperty, value); }
        }
        public int TotalItemCount
        {
            get { return (int)GetValue(TotalItemCountProperty); }
            private set { SetValue(TotalItemCountProperty, value); }
        }
        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            private set { SetValue(PageIndexProperty, value); }
        }
        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            private set { SetValue(PageCountProperty, value); }
        }
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            private set { SetValue(PageSizeProperty, value); }
        }
        public string ResultsText
        {
            get { return (string)GetValue(ResultsTextProperty); }
            set { SetValue(ResultsTextProperty, value); }
        }
        public string PageText
        {
            get { return (string)GetValue(PageTextProperty); }
            set { SetValue(PageTextProperty, value); }
        }
        public string PageOfText
        {
            get { return (string)GetValue(PageOfTextProperty); }
            set { SetValue(PageOfTextProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            PART_Previous = GetTemplateChild("PART_Previous") as ButtonBase;
            PART_Next = GetTemplateChild("PART_Next") as ButtonBase;
            PART_First = GetTemplateChild("PART_First") as ButtonBase;
            PART_Last = GetTemplateChild("PART_Last") as ButtonBase;

            if (PART_Previous != null)
                PART_Previous.Click += new RoutedEventHandler(PART_Previous_Click);
            if (PART_Next != null)
                PART_Next.Click += new RoutedEventHandler(PART_Next_Click);
            if (PART_First != null)
                PART_First.Click += new RoutedEventHandler(PART_First_Click);
            if (PART_Last != null)
                PART_Last.Click += new RoutedEventHandler(PART_Last_Click);

            EnableDisable();
        }

        private void SetPropertyChangedHandlers(INotifyPropertyChanged oldValue, INotifyPropertyChanged newValue)
        {
            if (oldValue != null)
                oldValue.PropertyChanged -= new PropertyChangedEventHandler(PagedItemsSource_PropertyChanged);
            if (newValue != null)
                newValue.PropertyChanged += new PropertyChangedEventHandler(PagedItemsSource_PropertyChanged);
        }
        private void PagedItemsSource_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            IPagedCollectionView ipcv = sender as IPagedCollectionView;
            if (ipcv != null)
            {
                if (e.PropertyName == "TotalItemCount")
                {
                    TotalItemCount = ipcv.TotalItemCount;
                    PageCount = CalculatePageCount(TotalItemCount, PageSize);
                }
                if (e.PropertyName == "PageIndex")
                    PageIndex = ipcv.PageIndex;
                if (e.PropertyName == "PageSize")
                {
                    PageSize = ipcv.PageSize;
                    PageCount = CalculatePageCount(TotalItemCount, PageSize);
                }
                EnableDisable();
            }
        }
        private void PART_Last_Click(object sender, RoutedEventArgs e)
        {
            if (PagedItemsSource == null) throw new Exception("The PagedItemsSource must first be set.");
            PagedItemsSource.MoveToLastPage();
            EnableDisable();
        }
        private void PART_First_Click(object sender, RoutedEventArgs e)
        {
            if (PagedItemsSource == null) throw new Exception("The PagedItemsSource must first be set.");
            PagedItemsSource.MoveToFirstPage();
            EnableDisable();
        }
        private void PART_Next_Click(object sender, RoutedEventArgs e)
        {
            if (PagedItemsSource == null) throw new Exception("The PagedItemsSource must first be set.");
            PagedItemsSource.MoveToNextPage();
            EnableDisable();
        }
        private void PART_Previous_Click(object sender, RoutedEventArgs e)
        {
            if (PagedItemsSource == null) throw new Exception("The PagedItemsSource must first be set.");
            PagedItemsSource.MoveToPreviousPage();
            EnableDisable();
        }

        private static void PagedItemsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            DataPager dp = sender as DataPager;
            dp.SetPropertyChangedHandlers(e.OldValue as INotifyPropertyChanged, e.NewValue as INotifyPropertyChanged);

            IPagedCollectionView ipcv = e.NewValue as IPagedCollectionView;
            dp.TotalItemCount = ipcv.TotalItemCount;
            dp.PageSize = ipcv.PageSize;
            dp.PageIndex = ipcv.PageIndex;
            dp.PageCount = dp.CalculatePageCount(dp.TotalItemCount, dp.PageSize);
            dp.EnableDisable();
        }

        private void EnableDisable()
        {
            if (PageIndex < PageCount)
            {
                if (PART_Next != null) PART_Next.Visibility = Visibility.Visible;
                if (PART_Last != null) PART_Last.Visibility = Visibility.Visible;
            }
            else
            {
                if (PART_Next != null) PART_Next.Visibility = Visibility.Collapsed;
                if (PART_Last != null) PART_Last.Visibility = Visibility.Collapsed;
            }

            if (PageIndex > 1)
            {
                if (PART_Previous != null) PART_Previous.Visibility = Visibility.Visible;
                if (PART_First != null) PART_First.Visibility = Visibility.Visible;
            }
            else
            {
                if (PART_Previous != null) PART_Previous.Visibility = Visibility.Collapsed;
                if (PART_First != null) PART_First.Visibility = Visibility.Collapsed;
            }
        }
        private int CalculatePageCount(int totalResults, int pageSize)
        {
            int numPages = totalResults / pageSize;
            numPages += (totalResults % pageSize) > 0 ? 1 : 0;
            return numPages;
        }
    }
}
