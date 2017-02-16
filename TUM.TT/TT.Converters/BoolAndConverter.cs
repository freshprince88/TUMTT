using System;
using System.Globalization;
using System.Windows.Data;

namespace TT.Converters
{
    [ValueConversion(typeof(bool[]), typeof(bool))]
    public class BoolAndConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)values[0] && (bool)values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if ((bool) value)
                return new object[] {true, true};
            return new object[] {false, true};
        }
    }
}
