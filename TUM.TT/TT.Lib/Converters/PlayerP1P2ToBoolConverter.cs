using System;
using System.Globalization;
using System.Windows.Data;
using TT.Lib.Util.Enums;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(Stroke.Player), typeof(bool))]
    public class PlayerP1P2ToBoolConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Stroke.Player player = (Stroke.Player)values[0];
            string btnName = (string)values[1];

            switch (player)
            {
                case Stroke.Player.Player1:
                    return btnName.ToString().ToLower() == "playerplayer1";
                case Stroke.Player.Player2:
                    return btnName.ToString().ToLower() == "playerplayer2";
                case Stroke.Player.Both:
                    return true;
                case Stroke.Player.None:
                    return false;
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

