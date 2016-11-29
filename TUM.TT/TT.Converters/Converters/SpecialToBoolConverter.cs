using System;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(Stroke.Specials), typeof(bool))]
    public class SpecialToBoolConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Stroke.Specials specials = (Stroke.Specials)values[0];
            string btnName = (string)values[1];

            switch (specials)
            {
                case Stroke.Specials.EdgeTable:
                    return btnName.ToString().ToLower() == "edgetable";
                case Stroke.Specials.EdgeNet:
                    return btnName.ToString().ToLower() == "edgenet";
                case Stroke.Specials.Both:
                    return true;
                case Stroke.Specials.None:
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
