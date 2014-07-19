namespace CheckbookManager.Converters
{
    using System;
    using System.Windows.Data;
    using System.Windows.Media;

    [ValueConversion(typeof(double), typeof(Brush))]
    public class PositiveNumberToColorConverter : IValueConverter
    {
        public Brush PositiveColor { get; set; }
        public Brush NegativeColor { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((double)value >= 0)
                       ? PositiveColor
                       : NegativeColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
