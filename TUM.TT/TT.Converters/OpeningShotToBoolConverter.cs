using System;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Converters
{
    [ValueConversion(typeof(Stroke.OpeningShot), typeof(bool))]
    public class OpeningShotToBoolConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Stroke.OpeningShot openingShot = (Stroke.OpeningShot)value;
            return openingShot == Stroke.OpeningShot.OpeningShot;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
