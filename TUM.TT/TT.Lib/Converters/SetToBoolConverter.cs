using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Models.Converters
{
    [ValueConversion(typeof(HashSet<int>), typeof(bool))]
    public class SetToBoolConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            HashSet<int> SelectedSets = (HashSet<int>)values[0];
            string btnName = (string)values[1];

            foreach (var set in SelectedSets)
            {
                if (set==1 && btnName=="FilterSet1Button")
                    return true;
                else if (set == 2 && btnName == "FilterSet2Button")
                    return true;
                else if (set == 3 && btnName == "FilterSet3Button")
                    return true;
                else if (set == 4 && btnName == "FilterSet4Button")
                    return true;
                else if (set == 5 && btnName == "FilterSet5Button")
                    return true;
                else if (set == 6 && btnName == "FilterSet6Button")
                    return true;
                else if (set == 7 && btnName == "FilterSet7Button")
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

