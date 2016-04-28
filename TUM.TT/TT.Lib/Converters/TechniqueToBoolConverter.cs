using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(HashSet<Stroke.Services>), typeof(bool))]
    public class TechniqueToBoolConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            HashSet<Stroke.Technique> set = (HashSet<Stroke.Technique>)values[0];
            string btnName = (string)values[1];

            foreach (var tec in set)
            {
                var test = tec.ToString().ToLower();
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
