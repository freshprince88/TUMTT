
using System;
using System.Globalization;
using System.Windows.Data;

namespace TT.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class TwoBoolToBoolConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool Player1TopPlayer2Bottom = (bool)values[0];
            bool Player2TopPlayer1Bottom = (bool)values[1];

            return Player1TopPlayer2Bottom || Player2TopPlayer1Bottom;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}