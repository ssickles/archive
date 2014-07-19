namespace CheckbookManager.Converters
{
    using System;
    using System.Windows.Data;
    using System.Windows;

    [ValueConversion(typeof(object), typeof(Visibility))]
    class IsTypeToVisibilityConverter : IValueConverter
    {
        public Type Type { get; set; }
        public Visibility IsType { get; set; }
        public Visibility IsNotType { get; set; }

        public IsTypeToVisibilityConverter()
        {
            IsType = Visibility.Visible;
            IsNotType = Visibility.Hidden;    
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value != null && value.GetType() == Type) 
                ? IsType : IsNotType;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
