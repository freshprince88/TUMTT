using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TT.Models;

namespace TT.Converters
{
    public class NumAndLengthToLastVisibilityConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                Rally currentRally = (Rally)values[0];
                ObservableCollection<Stroke> strokes = (ObservableCollection<Stroke>)values[1];
                Stroke currentStroke = (Stroke)values[2];

                if (currentRally.Winner == strokes[strokes.Count - 1].Player)
                {
                    if (currentStroke.Number == (strokes.Count))
                    {
                        return Visibility.Hidden;
                    }
                    else
                        return Visibility.Visible;

                }
                else if (currentRally.Winner == strokes[strokes.Count - 2].Player)
                {
                    if (currentStroke.Number == (strokes.Count - 1))
                    {
                        return Visibility.Hidden;
                    }
                    else
                        return Visibility.Visible;
                }
                else
                    return Visibility.Visible;

            }
            catch (Exception)
            {
                return Visibility.Hidden;
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
