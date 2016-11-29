using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TT.Models.Util.Enums;

namespace TT.Lib.Converters
{

    public class StrokeTechniqueToBoolScouterConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] != DependencyProperty.UnsetValue && values[1] != DependencyProperty.UnsetValue && values[2] != DependencyProperty.UnsetValue &&
                values[0] != null && values[1] != null && values[2] != null)
            {
                string Type = (string)values[0];
                string Option = (string)values[1];
                string btnName = (string)values[2];

                if (btnName == "Push")
                {
                    return Type == "Push";
                }
                else if (btnName == "PushAggressive")
                {
                    return Type == "Push" && Option == "aggressive";
                }
                else if (btnName == "Flip")
                {
                    return Type == "Flip";
                }
                else if (btnName == "Banana")
                {
                    return Type == "Flip" && Option == "Banana";
                }
                else if (btnName == "Topspin")
                {
                    return Type == "Topspin";
                }
                else if (btnName == "TopspinSpin")
                {
                    return Type == "Topspin" && Option == "Spin";
                }
                else if (btnName == "TopspinTempo")
                {
                    return Type == "Topspin" && Option == "Tempo";
                }
                else if (btnName == "Block")
                {
                    return Type == "Block";
                }
                else if (btnName == "BlockChop")
                {
                    return Type == "Block" && Option == "Chop";
                }
                else if (btnName == "BlockTempo")
                {
                    return Type == "Block" && Option == "Tempo";
                }
                else if (btnName == "Chop")
                {
                    return Type == "Chop";
                }
                else if (btnName == "Lob")
                {
                    return Type == "Lob";
                }
                else if (btnName == "Smash")
                {
                    return Type == "Smash";
                }
                else if (btnName == "Counter")
                {
                    return Type == "Counter";
                }
                else if (btnName == "Special")
                {
                    return Type == "Special";
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

