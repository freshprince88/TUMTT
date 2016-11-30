using System;
using System.Windows;
using System.Windows.Data;

namespace TT.Converters
{
    public class Column2TabConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool expanded = (bool)value;
            if (expanded)
                return new GridLength(1, GridUnitType.Star);
            else
                return GridLength.Auto;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
