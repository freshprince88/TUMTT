using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TT.Models;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(MatchPlayer), typeof(Brush))]
    public class MatchPlayerToBrushConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var brushConverter = new BrushConverter();
            switch ((MatchPlayer) value)
            {
                // original player colors from report:
                // First:   #FF4F81BD"
                // Second:  #FFC0504D"
                // new colors: saturation - 25, brightness + 10
                default:
                case MatchPlayer.First: return (Brush)brushConverter.ConvertFromString("#ff90afd6");
                case MatchPlayer.Second: return (Brush)brushConverter.ConvertFromString("#ffd98f8d");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

