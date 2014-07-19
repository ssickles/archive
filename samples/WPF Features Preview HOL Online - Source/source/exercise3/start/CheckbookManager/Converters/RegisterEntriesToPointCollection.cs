namespace CheckbookManager.Converters
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;
    using CheckbookManager.Data;
    using System.Collections.Generic;

    public class RegisterEntriesToPointCollection : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var rc = value as IEnumerable<RegisterTransaction>;
            if (rc == null || rc.Count() == 0)
                return null;

            // Cheat a little here and get the min/max ranges for scaling
            double minValue = rc.Min(rt => rt.TotalBalance);
            double maxValue = rc.Max(rt => rt.TotalBalance);
            double increment = 100.0 / rc.Count();

            maxValue = Math.Max(Math.Abs(minValue), Math.Abs(maxValue));
            minValue = -maxValue;

            var pc = new PointCollection();
            double x = 0;
            foreach (var reg in rc)
            {
                double yPos = ((-minValue + reg.TotalBalance)/(maxValue - minValue)) * 100; 
                pc.Add(new Point(x, yPos));
                x += increment;
            }

            return pc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
