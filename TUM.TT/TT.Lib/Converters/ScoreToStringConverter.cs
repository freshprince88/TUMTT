using Caliburn.Micro;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TT.Lib.Managers;
using TT.Models;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(Score), typeof(string))]
    public class ScoreToStringConverter : BaseConverter, IMultiValueConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object spaceInScoreSeparator, CultureInfo culture)
        {
            return Convert(new object[] {value}, targetType, spaceInScoreSeparator, culture);
        }

        public object Convert(object[] values, Type targetType, object spaceInScoreSeparator, CultureInfo culture)
        {
            if (values == null)
                return null;

            bool someUnset = false;
            foreach (object value in values)
            {
                someUnset |= value == DependencyProperty.UnsetValue;
            }
            if (someUnset)
                return null;

            string scoreSeparator = spaceInScoreSeparator != null && (bool)spaceInScoreSeparator ? " : " : ":";
            switch (values.Length)
            {
                case 0: return null;
                case 1: return ((Score)values[0]).First + scoreSeparator + ((Score)values[0]).Second;
                default:
                    Score rallyScore = (Score)values[0];
                    Score setScore = (Score)values[1];
                    return rallyScore.First + scoreSeparator + rallyScore.Second + " (" + setScore.First + scoreSeparator + setScore.Second + ")";
            }
        }

        public object ConvertBack(object value, Type targetType, object spaceInScoreSeparator, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object spaceInScoreSeparator, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
