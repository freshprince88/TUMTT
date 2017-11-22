using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using TT.Models;
using TT.Models.Util;

namespace TT.Converters
{
    public class VideoSourceAndRallyCountToBoolConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int TimeMode = (int)values[0];
            int ralliesCount = (int) values[1];

            if (ralliesCount>1)
            {
                switch (TimeMode)
                {
                    default:
                    case 0: return false;
                    case 1: return true;
                    case 2: return false;
                }
            }
            else
                return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

