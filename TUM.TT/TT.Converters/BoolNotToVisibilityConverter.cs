﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TT.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolNotToVisibilityConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(bool)value)
                return Visibility.Visible;
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility v = (Visibility)value;

            if (v == Visibility.Visible)
                return false;
            else
                return true;
        }
    }
}
