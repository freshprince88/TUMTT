using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Zhucai.LambdaParser;
using Zhucai.LambdaParser.ObjectDynamicExtension;
using System.Linq;
using TT.Models;
using System.Windows.Controls;

namespace TT.Models.Converters
{
    [ValueConversion(typeof(object), typeof(int))]
    public class LabelBackhandForehandRowConverter : BaseConverter, IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int row = (int)values[0];
            string tag = (string)values[1];

            if (row >= 3)
            {
                switch (tag)
                {

                    case "Label":
                        return row - 3;
                    case "Forehand":
                        return row - 2;
                    case "Backhand":
                        return row - 1;
                    default:
                        return 0;
                }
            }
            else
                return 0;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }





    }
}
