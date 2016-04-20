using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TT.Lib.Converters
{
    public class NumAndLengthToNextVisibilityConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int num = (int)values[0];
                int length = (int)values[1];

                if (num == length || length == 0)
                    return Visibility.Hidden;
                else
                    return Visibility.Visible;
            }
            catch (Exception ex)
            {
                return Visibility.Hidden;
            }
            
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
