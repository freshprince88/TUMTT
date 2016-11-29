using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Lib.Converters
{
    
    public class ServiceToBoolScouterConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] != null) { 
            string service = (string)values[0];
            string btnName = (string)values[1];
            return service.ToLower() == btnName.ToLower();
            }
            return false;

        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
