namespace CheckbookManager.Converters
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(double), typeof(double))]
    public class AmountToCreditDebitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double dval = (double) value;
            return (string) parameter == "0"
                       ? ((dval <= 0) ? -dval : DependencyProperty.UnsetValue)
                       : ((dval > 0) ? dval : DependencyProperty.UnsetValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                double dval;
                if (double.TryParse(value.ToString().TrimStart('$', '(').TrimEnd(')'), out dval))
                    return (string)parameter == "0" ? -dval : dval;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}
