using System;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(Stroke.Point), typeof(bool))]
    public class PointP1P2ToBoolConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Stroke.Point point = (Stroke.Point)values[0];
            string btnName = (string)values[1];

            switch (point)
            {
                case Stroke.Point.Player1:
                    return btnName.ToString().ToLower() == "pointplayer1";
                case Stroke.Point.Player2:
                    return btnName.ToString().ToLower() == "pointplayer2";
                case Stroke.Point.Both:
                    return true;
                case Stroke.Point.None:
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
