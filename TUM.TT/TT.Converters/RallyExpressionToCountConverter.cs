using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Zhucai.LambdaParser;
using System.Linq;
using TT.Models;

namespace TT.Converters
{
    [ValueConversion(typeof(IEnumerable<Rally>), typeof(int))]
    public class RallyExpressionToCountConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<Rally> rallies = (IEnumerable<Rally>)values[0];
            string expression = (string)values[1];
            MatchPlayer? player = values.Length > 2 ? (MatchPlayer?)values[2] : null;
            
            expression = expression.Replace("$$Player$$", player != null ? parameter.ToString() : "");
            if (player != null)
                expression = expression.Replace("$$PlayerString$$", player.Value.ToString());

            expression = ReplaceExpression(expression);

            Func<Rally, bool> func = ExpressionParser.Compile<Func<Rally, bool>>(expression);

            try
            {
                var test = rallies.Where(func).Count();
                return test;
            }
           catch (NullReferenceException)
            {
                return 0;
            }


        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

       



    }
}
