using System;
using System.Globalization;
using System.Windows.Data;

namespace TT.Converters
{
    public class TabSizeConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double actualWidth = (double)values[0] > 0 ? (double)values[0] : 100;
            int count = (int)values[1] > 0 ? (int)values[1] : 1;

            double width = actualWidth / (count-1);
            //Subtract 1, otherwise we could overflow to two rows.
            return (width <= 1) ? 0 : (width - 1);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
