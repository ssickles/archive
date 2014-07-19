using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace TodoList.WpfClient.Converters
{
    class ExceptionToTextConverter:IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ValidationError error = (ValidationError) value;
            if(error.Exception.InnerException != null)
                return error.Exception.InnerException.Message;
            return error.ErrorContent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
