namespace CheckbookManager.Converters
{
    using System;
    using System.Linq;
    using System.Windows.Data;
    using CheckbookManager.Data;

    public class RegisterPositionCalculator : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var rval = value as RegisterTransaction;
            if (rval == null || CheckBook.Register.Count == 0)
                return 0;

            // Cheat a little here and get the min/max ranges for scaling
            double minValue = CheckBook.Register.Min(rt => rt.TotalBalance);
            double maxValue = CheckBook.Register.Max(rt => rt.TotalBalance);
            double increment = 100.0 / CheckBook.Register.Count;

            maxValue = Math.Max(Math.Abs(minValue), Math.Abs(maxValue));
            minValue = -maxValue;

            if (parameter != null) // yPos
            {
                return ((-minValue + rval.TotalBalance) / (maxValue - minValue)) * 100; 
            }

            // xPos
            return CheckBook.Register.IndexOf(rval) * increment;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
