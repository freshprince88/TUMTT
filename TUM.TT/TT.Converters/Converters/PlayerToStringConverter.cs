using System;
using System.Globalization;
using System.Windows.Data;
using TT.Models;

namespace TT.Converters
{
    [ValueConversion(typeof(Player), typeof(string))]
    public class PlayerToStringConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Player player = (Player)value;

            switch ((string)parameter)
            {
                case "first":
                    return player.Name.Split(' ')[1];
                case "last":
                    return player.Name.Split(' ')[0];
                default:
                    return player.Name;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

