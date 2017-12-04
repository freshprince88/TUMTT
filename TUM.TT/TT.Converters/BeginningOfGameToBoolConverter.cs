using System;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Converters
{
    [ValueConversion(typeof(Stroke.Crunch), typeof(bool))]
    public class BeginningOfGameToBoolConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Stroke.BeginningOfGame beginningOfGame = (Stroke.BeginningOfGame)value;
            return beginningOfGame == Stroke.BeginningOfGame.BeginningOfGame;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();


        }
    }
}