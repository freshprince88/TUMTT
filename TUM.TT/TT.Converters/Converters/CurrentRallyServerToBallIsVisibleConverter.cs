using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TT.Models;

namespace TT.Converters
{
    [ValueConversion(typeof(MatchPlayer), typeof(Visibility))]
    public class CurrentRallyServerToBallIsVisibleConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            MatchPlayer server = (MatchPlayer)values[0];
            string gridName = (string)values[1];

            switch (server)
            {
                case MatchPlayer.First:
                    if (gridName.ToString().ToLower() == "player1servergrid")
                    {
                        return Visibility.Visible;
                    }

                    else
                        return Visibility.Hidden;
                case MatchPlayer.Second:
                    if (gridName.ToString().ToLower() == "player2servergrid")
                    {
                        return Visibility.Visible;
                    }

                    else
                        return Visibility.Hidden;
                case MatchPlayer.None:
                    return Visibility.Hidden;
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}