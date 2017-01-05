using System;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(int), typeof(bool))]
    public class MediaSpeedToBoolConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int mediaSpeed = (int)values[0];
            string btnName = (string)values[1];

            switch (mediaSpeed)
            {
                case 25:
                    return btnName.ToString().ToLower() == "slow25button";
                case 50:
                    return btnName.ToString().ToLower() == "slow50button";
                case 75:
                    return btnName.ToString().ToLower() == "slow75button";
                case 100:
                    return btnName.ToString().ToLower() == "slow100button";
                case 150:
                    return btnName.ToString().ToLower() == "slow150button";
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
