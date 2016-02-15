using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(int), typeof(Visibility))]
    public class NumToPrevVisibleConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int num = (int)value;

            if (num > 1)
                return Visibility.Visible;
            else
                return Visibility.Hidden;            

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
