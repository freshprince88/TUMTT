using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TT.Converters
{

    public class SpinToBoolScouterConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
                if (values[0] != DependencyProperty.UnsetValue && values[1] != DependencyProperty.UnsetValue &&
                    values[2] != DependencyProperty.UnsetValue && values[3] != DependencyProperty.UnsetValue && values[4] != DependencyProperty.UnsetValue &&
                    values[0] != null && values[1] != null && values[2] != null && values[3] != null && values[4] != null)
            {
                string US = (string)values[0];
                string TS = (string)values[1];
                string SL = (string)values[2];
                string SR = (string)values[3];
                string No = (string)values[4];
                string btnName = (string)values[5];

                if (btnName == "TSSL")
                {
                    return US == "0" && TS == "1" && SL == "1" && SR == "0" && No == "0";
                }
                else if (btnName == "TS")
                {
                    return US == "0" && TS == "1" && SL == "0" && SR == "0" && No == "0";
                }
                else if (btnName == "TSSR")
                {
                    return US == "0" && TS == "1" && SL == "0" && SR == "1" && No == "0";
                }
                else if (btnName == "SL")
                {
                    return US == "0" && TS == "0" && SL == "1" && SR == "0" && No == "0";
                }
                else if (btnName == "SR")
                {
                    return US == "0" && TS == "0" && SL == "0" && SR == "1" && No == "0";
                }
                else if (btnName == "No")
                {
                    return US == "0" && TS == "0" && SL == "0" && SR == "0" && No == "1";
                }
                else if (btnName == "USSL")
                {
                    return US == "1" && TS == "0" && SL == "1" && SR == "0" && No == "0";
                }
                else if (btnName == "US")
                {
                    return US == "1" && TS == "0" && SL == "0" && SR == "0" && No == "0";
                }
                else if (btnName == "USSR")
                {
                    return US == "1" && TS == "0" && SL == "0" && SR == "1" && No == "0";
                }
                else if (btnName == "Hidden")
                {
                    return US == "" || TS == "" || SL == "" || SR == "" || No == "";
                }
            }
            return false;

        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
