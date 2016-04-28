
using System;
using System.Globalization;
using System.Windows.Data;
using TT.Lib.Models;
using TT.Lib.Util.Enums;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(MatchPlayer), typeof(bool))]
    public class CurrentRallyWinnerToButtonIsEnabledConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            MatchPlayer winner = (MatchPlayer)values[0];
            string btnName = (string)values[1];

            switch (winner)
            {
                case MatchPlayer.First:
                    return !(btnName.ToString().ToLower() == "player1button");
                case MatchPlayer.Second:
                    return !(btnName.ToString().ToLower() == "player2button");
                case MatchPlayer.None:
                    return true;
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

