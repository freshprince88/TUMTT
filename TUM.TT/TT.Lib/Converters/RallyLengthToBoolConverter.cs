using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using TT.Lib.Util.Enums;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(HashSet<int>), typeof(bool))]
    public class RallyLengthToBoolConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            HashSet<int> SelectedRallyLengths = (HashSet<int>)values[0];
            string btnName = (string)values[1];

            foreach (var length in SelectedRallyLengths)
            {
                if (length == 1 && btnName == "FilterRallyLength1Button")
                    return true;
                else if (length == 2 && btnName == "FilterRallyLength2Button")
                    return true;
                else if (length == 3 && btnName == "FilterRallyLength3Button")
                    return true;
                else if (length == 4 && btnName == "FilterRallyLength4Button")
                    return true;
                else if (length == 5 && btnName == "FilterRallyLength5Button")
                    return true;
                else if (length == 6 && btnName == "FilterRallyLength5UpButton")
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

