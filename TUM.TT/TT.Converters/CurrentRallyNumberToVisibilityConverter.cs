using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TT.Models;

namespace TT.Converters
{
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class CurrentRallyNumberToVisibilityConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int currentRallyNumber = (int)value;
            if (currentRallyNumber == 1) { 
                        return Visibility.Visible;
                    }

            else
            {
                return Visibility.Hidden;

            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }       
    }
}