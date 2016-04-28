using System;
using System.Globalization;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Models.Converters
{
    [ValueConversion(typeof(Stroke.Hand), typeof(bool))]
    public class ForeBackHandToBoolConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Stroke.Hand hand = (Stroke.Hand)values[0];
            string btnName = (string)values[1];

            switch (hand)
            {
                case Stroke.Hand.Back:                   
                        return btnName.ToString().ToLower() == "backhand";
                case Stroke.Hand.Fore:
                    return btnName.ToString().ToLower() == "forehand";
                case Stroke.Hand.Both:
                    return true;
                case Stroke.Hand.None:
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