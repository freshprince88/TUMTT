using Caliburn.Micro;
using System;
using System.Globalization;
using System.Windows.Data;
using TT.Lib.Managers;
using TT.Lib.Models;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(MatchPlayer), typeof(string))]
    public class MatchPlayerToStringConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            MatchPlayer player = (MatchPlayer)value;
            IMatchManager manager = IoC.Get<IMatchManager>();

            switch (player)
            {
                case MatchPlayer.First:
                    return manager.Match.FirstPlayer.Name;
                case MatchPlayer.Second:
                    return manager.Match.SecondPlayer.Name;
                default:
                    return "None";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IMatchManager manager = IoC.Get<IMatchManager>();
            string name = (string)value;

            if (value.Equals(manager.Match.FirstPlayer.Name))
                return MatchPlayer.First;
            else if (value.Equals(manager.Match.SecondPlayer.Name))
                return MatchPlayer.Second;
            else
                return MatchPlayer.None;
        }
    }
}
