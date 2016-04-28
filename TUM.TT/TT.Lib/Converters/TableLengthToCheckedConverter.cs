using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Models.Converters
{
    [ValueConversion(typeof(HashSet<Positions.Length>), typeof(bool))]
    public class TableLengthToCheckedConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            HashSet<Positions.Length> set = (HashSet<Positions.Length>)values[0];
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
