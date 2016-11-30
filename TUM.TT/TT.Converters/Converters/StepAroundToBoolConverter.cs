using System;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Converters
{
    [ValueConversion(typeof(Stroke.StepAround), typeof(bool))]
    public class StepAroundToBoolConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Stroke.StepAround stepAround = (Stroke.StepAround)value;
            return stepAround == Stroke.StepAround.StepAround;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
