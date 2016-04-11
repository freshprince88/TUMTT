using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class MinRallyLengthToVisibleConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int num = (int)values[0];
            string btname = (string)values[1];

            switch (btname)
            {
                case "FilterRallyLength1Button":
                    if (num > 0)
                    {
                        return Visibility.Collapsed;
                    }
                    else
                        return Visibility.Visible;

                case "FilterRallyLength2Button":
                    if (num > 1)
                    {
                        return Visibility.Collapsed;
                    }
                    else
                        return Visibility.Visible;

                case "FilterRallyLength3Button":
                    if (num > 2)
                    {
                        return Visibility.Collapsed;
                    }
                    else
                        return Visibility.Visible;

             


            }

            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
