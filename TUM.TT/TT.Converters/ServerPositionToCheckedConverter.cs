using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Converters
{
    [ValueConversion(typeof(HashSet<Positions.Server>), typeof(bool))]
    public class ServerPositionToCheckedConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            HashSet<Positions.Server> set = (HashSet<Positions.Server>)values[0];
            string btnName = (string)values[1];

            foreach (var serv in set)
            {
                var test = serv.ToString().ToLower();
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
