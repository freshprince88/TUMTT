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
            Func<Rally, bool> func = ExpressionParser.Compile<Func<Rally, bool>>(expression);
            //var test = rallies.ToArray().AsQueryable().Where(expression, null).Count();
            var test = rallies.Where(func).Count();
            return test;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
