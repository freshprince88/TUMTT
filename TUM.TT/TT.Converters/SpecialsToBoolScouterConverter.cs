using System;
using System.Globalization;
using System.Windows.Data;

namespace TT.Converters
{

    public class SpecialsToBoolScouterConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] != null)
            {

                string specials = (string)values[0];
                if (specials.ToLower() == "edgenettable")
                {
                    return true;
                }
                string btnName = (string)values[1];
                return specials.ToLower() == btnName.ToLower();
            }
            return false;

        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
