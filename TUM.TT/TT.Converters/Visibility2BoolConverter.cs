using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace TT.Converters
{
    public class Visibility2BoolConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //if (values == null) return Visibility.Visible;
            //if (values.Length != 2) return Visibility.Visible;
            if (values.Length != 2) return Visibility.Collapsed;
            if (values[0] as bool? == null || values[1] as bool? == null)
            {
                Debug.WriteLine(values[0].ToString());
                Debug.WriteLine(values[1].ToString());
                return Visibility.Collapsed;
            }
            //if (values.Length != 2 && values[0] as bool? == null || values[1] as bool? == null) return Visibility.Visible;
            try
            {
                Debug.WriteLine(((bool)values[0]).ToString() + " " + ((bool)values[1]).ToString());
                if ((bool)values[0] && (bool)values[1]) return Visibility.Visible;
                else return Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, "Visibility2BoolConverter");
                Debug.WriteLine(values[0].ToString());
                Debug.WriteLine(values[1].ToString());
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
