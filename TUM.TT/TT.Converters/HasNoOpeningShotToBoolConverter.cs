using System;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Converters
{
    [ValueConversion(typeof(EnumRally.HasNoOpeningShot), typeof(bool))]
    public class HasNoOpeningShotToBoolConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           EnumRally.HasNoOpeningShot hasNoOpeningShot = (EnumRally.HasNoOpeningShot)value;
            return hasNoOpeningShot == EnumRally.HasNoOpeningShot.NoOpeningShot;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}