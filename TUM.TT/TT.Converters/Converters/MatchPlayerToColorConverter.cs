using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TT.Models;

namespace TT.Converters
{
    [ValueConversion(typeof(MatchPlayer), typeof(Color))]
    public class MatchPlayerToColorConverter : BaseConverter, IValueConverter
    {

        private static MatchPlayerToBrushConverter matchPlayerToBrushConverter = new MatchPlayerToBrushConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush brush = (Brush) matchPlayerToBrushConverter.Convert(value, typeof(Brush), null, CultureInfo.CurrentCulture);
            return ((SolidColorBrush)brush).Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
