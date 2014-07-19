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

namespace LoadingAdorner
{
    /// <summary>
    /// Interaction logic for DemoPage.xaml
    /// </summary>
    public partial class DemoPage : Window
    {
        DemoPageController controller = new DemoPageController();

        public DemoPage()
        {
            DataContext = controller.Data;
            InitializeComponent();

            AttachLoadingAdorner();
        }

        private void AttachLoadingAdorner()
        {
            LoadingAdorner loading = new LoadingAdorner(mainPane);
            loading.FontSize = 15;
            loading.OverlayedText = "loading...";
            loading.Typeface = new Typeface(FontFamily, FontStyles.Italic, 
                FontWeights.Bold, FontStretch);
            Binding bind = new Binding("SearchInProgress");
            bind.Source = controller;
            bind.Converter = new VisibilityConverter();
            loading.SetBinding(LoadingAdorner.VisibilityProperty, bind);
            AdornerLayer.GetAdornerLayer(mainPane).Add(loading);
        }
    }

    /// <summary>
    /// Converter that converts a bool to a Visibility status
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
