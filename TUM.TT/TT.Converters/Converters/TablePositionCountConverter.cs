
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(Dictionary<string, int>), typeof(int))]
    public class TablePositionCountConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Dictionary<string, int> set = (Dictionary<string, int>)values[0];
            string btnName = (string)values[1];
            string key = btnName.Split('_')[0];
            int count = set[key];
            return count;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
