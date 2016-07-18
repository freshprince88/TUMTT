using Caliburn.Micro;
using System;
using System.Globalization;
using System.Windows.Data;
using TT.Lib.Managers;
using TT.Models;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(Score), typeof(string))]
    public class ScoreToStringConverter : BaseConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Score score = (Score)value;
            return score.First + " : " + score.Second;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
