using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace TT.Converters

{
    [ValueConversion(typeof(HashSet<Models.Util.Enums.Stroke.Services>), typeof(bool))]
    public class AggressivenessToBoolConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            HashSet<Models.Util.Enums.Stroke.Aggressiveness> set = (HashSet<Models.Util.Enums.Stroke.Aggressiveness>)values[0];
            string btnName = (string)values[1];

            foreach (var agg in set)
            {
                var test = agg.ToString().ToLower();
                if (test == btnName.ToLower())
                    return true;
            }
            return false;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

