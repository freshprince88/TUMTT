using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TT.Converters
{
    [ValueConversion(typeof(int), typeof(bool))]
    public class VideoSourceToBoolConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int TimeMode = (int) value;
            //string videoOrNot = (string) value;
            switch (TimeMode)
            {
                default:
                case 0: return false;
                case 1: return true;
                case 2: return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

