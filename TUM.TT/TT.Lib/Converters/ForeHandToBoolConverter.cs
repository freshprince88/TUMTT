﻿using System;
using System.Globalization;
using System.Windows.Data;
using TT.Lib.Util.Enums;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(Stroke.Hand), typeof(bool))]
    public class ForeHandToBoolConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Stroke.Hand hand = (Stroke.Hand)value;
            return hand == Stroke.Hand.Back;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Stroke.Hand.Back : Stroke.Hand.None;
        }
    }
}
