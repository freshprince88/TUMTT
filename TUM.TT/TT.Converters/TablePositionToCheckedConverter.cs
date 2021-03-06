﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Converters
{
    [ValueConversion(typeof(HashSet<Positions.Table>), typeof(bool))]
    public class TablePositionToCheckedConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            HashSet<Positions.Table> set = (HashSet<Positions.Table>)values[0];
            string btnName = (string)values[1];

            foreach (var serv in set)
            {
                var test = serv.ToString().ToLower();
                if (btnName.ToLower().Contains(test))
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
