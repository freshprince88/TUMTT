using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Zhucai.LambdaParser;
using System.Linq;
using TT.Models;
using System.Diagnostics;

namespace TT.Converters
{
    [ValueConversion(typeof(IEnumerable<Rally>), typeof(int))]
    public class RallyExpressionToCountConverter : BaseConverter, IMultiValueConverter
    {
        //private static int compilingCount;
        //private static int invokingCount;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<Rally> rallies = (IEnumerable<Rally>)values[0];
            string expression = (string)values[1];
            MatchPlayer? p = values.Length >= 3 ? (MatchPlayer?)values[2] : null;
            int? strokeNumber = values.Length >= 4 ? (int?)values[3] : null;
            if (strokeNumber != null)
                rallies = new List<Rally>(rallies.Where(r => strokeNumber == int.MaxValue || r.Strokes.Count >= strokeNumber.Value));

            expression = ReplaceExpression(expression, param: parameter, player: p, strokeNumber: strokeNumber);
            Func<Rally, bool> func = ExpressionParser.Compile<Func<Rally, bool>>(expression);

            var count = 0;

            int? originalStrokeNumber = strokeNumber;
            int? lastUsedStrokeNumber = originalStrokeNumber;
            foreach (var r in rallies)
            {
                var start = 0;
                var limit = 1;
                if (originalStrokeNumber != null)
                {
                    if (originalStrokeNumber.Value == -1)
                        limit = r.Strokes.Count;
                    else if (originalStrokeNumber.Value == int.MaxValue)
                    {
                        var lastWinnerStroke = r.LastWinnerStroke();
                        if (lastWinnerStroke == null)
                            continue;
                        start = lastWinnerStroke.Number - 1;
                        limit = lastWinnerStroke.Number;
                    }
                    else
                    {
                        start = originalStrokeNumber.Value - 1;
                        limit = originalStrokeNumber.Value;
                    }
                }
                for (int i = start; i < limit; i++)
                {
                    strokeNumber = i + 1;

                    if (originalStrokeNumber != null && lastUsedStrokeNumber != strokeNumber)
                    {

                        expression = ReplaceExpression((string)values[1], param: parameter, player: p, strokeNumber: strokeNumber);
                        func = ExpressionParser.Compile<Func<Rally, bool>>(expression);
                        //Debug.WriteLine("Compiling new function ({0}) for stroke {2} based on Ex: {1}", ++compilingCount, expression, strokeNumber);
                    }
                    count += func.Invoke(r) ? 1 : 0;
                    //Debug.WriteLine("Invoking function ({0}) on Rally {1}", ++invokingCount, r.Number);

                    lastUsedStrokeNumber = strokeNumber;
                }
            }
            return count;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

       



    }
}
