using System;
using System.Globalization;
using System.Windows.Data;
using TT.Models;

namespace TT.Converters
{

    public class CourseToBoolScouterConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] != null && values[1] != null && values[2] != null && values[3] != null)
            {
                int crLength = (int) values[0];
                int csNumber = (int) values[1];
                MatchPlayer crWinner = (MatchPlayer)values[2];
                MatchPlayer csPlayer = (MatchPlayer)values[3];
                string btnName = (string)values[4];

                if (csNumber < crLength)
                {
                    return btnName == "continue";
                }
                if (csNumber == crLength)
                {
                    if (csPlayer == crWinner)
                    {
                        return btnName == "Winner";
                    }
                    else
                        return btnName == "NetOut";
                }
                else
                    return false;


                //Rally currentRally = (Rally)values[0];
                //Models.Stroke currentStroke = (Models.Stroke)values[1];
                //string btnName = (string)values[2];

                //if (currentStroke.Number < currentRally.Length)
                //{
                //    return btnName == "continue";
                //}
                //if (currentStroke.Number == currentRally.Length)
                //{
                //    if (currentStroke.Player == currentRally.Winner)
                //    {
                //        return btnName == "Winner";
                //    }
                //    else
                //        return btnName == "NetOut";
                //}
                //else
                //    return false;

            }
            return false;

        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
