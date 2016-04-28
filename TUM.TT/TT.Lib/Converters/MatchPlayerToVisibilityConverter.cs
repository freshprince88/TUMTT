using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TT.Models;

namespace TT.Lib.Converters
{
    public class MatchPlayerToVisibilityConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string name = (string)values[0];
            MatchPlayer server = (MatchPlayer)values[1];

            switch (server)
            {
                case MatchPlayer.None:
                    return Visibility.Hidden;
                case MatchPlayer.First:
                    if (name == "First")
                        return Visibility.Visible;
                    else
                        return Visibility.Hidden;
                case MatchPlayer.Second:
                    if (name == "Second")
                        return Visibility.Visible;
                    else
                        return Visibility.Hidden;
                default:
                    return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
