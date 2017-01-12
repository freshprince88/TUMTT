using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TT.Converters
{
    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class VisibilityToBoolConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool invert = parameter != null && parameter is bool ? (bool)parameter : false;
            switch ((Visibility)value)
            {
                default:
                case Visibility.Collapsed:
                case Visibility.Hidden: return invert;
                case Visibility.Visible: return !invert;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
