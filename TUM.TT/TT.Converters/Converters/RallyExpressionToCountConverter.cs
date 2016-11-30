﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Zhucai.LambdaParser;
using System.Linq;
using TT.Models;

namespace TT.Converters
{
    [ValueConversion(typeof(List<Rally>), typeof(int))]
    public class RallyExpressionToCountConverter : BaseConverter, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            List<Rally> rallies = (List<Rally>)values[0];
            string expression = (string)values[1];
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
