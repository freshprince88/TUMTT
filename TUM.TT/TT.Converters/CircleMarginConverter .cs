using System;
using System.Windows;
using System.Windows.Data;

namespace TT.Converters
{
    public class CircleMarginConverter : BaseConverter, IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var width = (double)values[0];
            var height = (double)values[1];
            var diagonal = Math.Sqrt(width * width + height * height);
            var horzmargin = (diagonal - width) / 2;
            var vertmargin = (diagonal - height) / 2;
            //return new Thickness(horzmargin, vertmargin, horzmargin, vertmargin);
            return new Thickness(1, 1, 1, 1);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
