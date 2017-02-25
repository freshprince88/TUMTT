using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TT.Converters
{
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class VideoSourceToNotVisibilityConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int TimeMode = (int)value;
            //string videoOrNot = (string) value;
            switch (TimeMode)
            {
                default:
                case 0: return Visibility.Collapsed;
                case 1: return Visibility.Collapsed;
                case 2: return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}


