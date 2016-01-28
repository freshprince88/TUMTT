using System;
using System.Globalization;
using System.Windows.Data;
using TT.Lib.Util.Enums;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(Stroke.Hand), typeof(bool))]
    public class BackHandToBoolConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Stroke.Hand hand = (Stroke.Hand)value;
            return hand == Stroke.Hand.Back || hand == Stroke.Hand.Both;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
