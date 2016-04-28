using System;
using System.Globalization;
using System.Windows.Data;

namespace TT.Models.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public class TimeSpanToMillisecondsConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((TimeSpan)value).TotalMilliseconds;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeSpan.FromMilliseconds((double)value);
        }
    }
}
