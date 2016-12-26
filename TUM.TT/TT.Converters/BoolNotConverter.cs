using System;
using System.Globalization;
using System.Windows.Data;

namespace TT.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolNotConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
