using System;
using System.Globalization;
using System.Windows.Data;

namespace TT.Converters
{
    public class RallyLengthToVisibilityConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int length = (int) value;
            
            if (length == 1)
                return System.Windows.Visibility.Visible;
            else
                return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();

        }
        
    }
}
