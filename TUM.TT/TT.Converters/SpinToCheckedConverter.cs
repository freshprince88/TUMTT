using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Converters
{
    [ValueConversion(typeof(HashSet<Stroke.Spin>), typeof(bool))]
    public class SpinToCheckedConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            HashSet<Stroke.Spin> set = (HashSet<Stroke.Spin>)values[0];
            string btnName = (string)values[1];

            foreach (var spin in set)
            {
                var test = spin.ToString().ToLower();
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
