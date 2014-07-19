using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;

namespace DataBinding
{
    public class PriceToBackgroundConverter : IValueConverter
    {
        private decimal minimumPriceToHighlight;
        public decimal MinimumPriceToHighlight
        {
            get { return minimumPriceToHighlight; }
            set { minimumPriceToHighlight = value; }
        }

        private Brush highlightBrush;
        public Brush HighlightBrush
        {
            get { return highlightBrush; }
            set { highlightBrush = value; }
        }

        private Brush defaultBrush;
        public Brush DefaultBrush
        {
            get { return defaultBrush; }
            set { defaultBrush = value; }
        }

        public object Convert(object value, Type targetType, object parameter,
          System.Globalization.CultureInfo culture)
        {
            decimal price = (decimal)value;
            if (price >= MinimumPriceToHighlight)
                return HighlightBrush;
            else
                return DefaultBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
          System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }


}
