using System;
using System.Globalization;
using System.Windows.Data;
using TT.Models;

namespace TT.Converters
{
    [ValueConversion(typeof(MatchPlayer), typeof(string))]
    public class MatchPlayerToStringConverter : BaseConverter, IValueConverter
    {
        private Player firstPlayer;
        private Player secondPlayer;

        public MatchPlayerToStringConverter(Player firstPlayer, Player secondPlayer)
        {
            this.firstPlayer = firstPlayer;
            this.secondPlayer = secondPlayer;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MatchPlayer player = (MatchPlayer)value;

            switch (player)
            {
                case MatchPlayer.First:
                    return firstPlayer.Name;
                case MatchPlayer.Second:
                    return secondPlayer.Name;
                default:
                    return "None";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string name = (string)value;

            if (value.Equals(firstPlayer.Name))
                return MatchPlayer.First;
            else if (value.Equals(secondPlayer.Name))
                return MatchPlayer.Second;
            else
                return MatchPlayer.None;
        }
    }
}
