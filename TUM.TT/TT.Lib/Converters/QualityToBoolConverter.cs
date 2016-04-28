using System;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Models.Converters
{
    [ValueConversion(typeof(Stroke.Quality), typeof(bool))]
    public class QualityToBoolConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Stroke.Quality quality = (Stroke.Quality)values[0];
            string btnName = (string)values[1];

            switch (quality)
            {
                case Stroke.Quality.Good:
                    return btnName.ToString().ToLower() == "goodquality";
                case Stroke.Quality.Bad:
                    return btnName.ToString().ToLower() == "badquality";
                case Stroke.Quality.Both:
                    return true;
                case Stroke.Quality.None:
                    return false;
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
