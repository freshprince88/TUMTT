using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace TT.Lib.Util
{
    public class TabSizeConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double actual = (double)values[0];
            double count = (double)(values[1] as ItemCollection).Count;
            double height = actual / count;
            //Subtract 1, otherwise we could overflow to two rows.
            return (height <= 1) ? 0 : (height - 1);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
