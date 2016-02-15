using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Zhucai.LambdaParser;
using Zhucai.LambdaParser.ObjectDynamicExtension;
using System.Linq;
using TT.Lib.Models;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(List<Rally>), typeof(int))]
    public class RallyExpressionToCountConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            List<Rally> rallies = (List<Rally>)values[0];
            string expression = (string)values[1];
            expression = expression.Replace('\'', '"');
            expression = expression.Replace("MatchPlayer.None", "\"None\"");
            expression = expression.Replace("MatchPlayer.First", "\"First\"");
            expression = expression.Replace("MatchPlayer.Second", "\"Second\"");
            expression = expression.Replace(".Winner", ".Winner.ToString()");
            expression = expression.Replace(".Spieler", ".Spieler.ToString()");
            expression = expression.Replace(".Server", ".Server.ToString()");
            Func<Rally, bool> func = ExpressionParser.Compile<Func<Rally, bool>>(expression);
            //var test = rallies.ToArray().AsQueryable().Where(expression, null).Count();
            var test = rallies.Where(func).Count();
            return test;

            //TODO
            // Basic Filter wird nicht berücksichtigt!!

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

       



    }
}
