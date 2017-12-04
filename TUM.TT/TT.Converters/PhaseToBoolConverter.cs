using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Converters
{

    public class PhaseToBoolConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] != DependencyProperty.UnsetValue && values[1] != DependencyProperty.UnsetValue && values[2] != DependencyProperty.UnsetValue &&
                values[0] != null && values[1] != null)
            {
                Stroke.GamePhase GamePhase = (Stroke.GamePhase)values[0];
      
                string btnName = (string)values[1];

                if (btnName == "FilterGameBeginningOfGameButton")
                {
                    return GamePhase == Stroke.GamePhase.BeginningOfGame;
                }
                else if (btnName == "FilterGameCrunchTimeButton")
                {
                    return GamePhase == Stroke.GamePhase.CrunchTime;
                }
                
            }
            return false;

        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

