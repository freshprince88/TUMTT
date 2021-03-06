﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace TT.Converters
{
    public class RallyLengthToCheckedConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int length = (int)values[0];
            string name = (string)values[1];

            if (name == "n.a.")
                return length == 0;

            int expected = System.Convert.ToInt32(name);

            if (length == expected)
                return true;
            else
                return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
