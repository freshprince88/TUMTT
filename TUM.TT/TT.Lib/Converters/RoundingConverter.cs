using System;
using System.Globalization;
using System.Windows.Data;

namespace TT.Lib.Converters
{
    public class RoundingConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)(((double)value)*100)) / 100.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
