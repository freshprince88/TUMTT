using System;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Converters
{
    [ValueConversion(typeof(Stroke.Crunch), typeof(bool))]
    public class CrunchToBoolConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Stroke.Crunch crunch = (Stroke.Crunch)value;
            return crunch == Stroke.Crunch.CrunchTime;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
