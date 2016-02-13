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
    [ValueConversion(typeof(object), typeof(int))]
    public class TechniqueCountToRankingRowConverter : BaseConverter, IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            List<Rally> rallies = (List<Rally>)values[0];
            string flipString = (string)values[1];
            flipString = flipString.Replace('\'', '"');
            Func<Rally, bool> func1 = ExpressionParser.Compile<Func<Rally, bool>>(flipString);
            //var test = rallies.ToArray().AsQueryable().Where(expression, null).Count();
            double flip = rallies.Where(func1).Count() + 0.01;

            string pushShortString = (string)values[2];
            pushShortString = pushShortString.Replace('\'', '"');
            Func<Rally, bool> func2 = ExpressionParser.Compile<Func<Rally, bool>>(pushShortString);
            //var test = rallies.ToArray().AsQueryable().Where(expression, null).Count();
            double pushShort = rallies.Where(func2).Count() + 0.02;

            string pushHalfLongString = (string)values[3];
            pushHalfLongString = pushHalfLongString.Replace('\'', '"');
            Func<Rally, bool> func3 = ExpressionParser.Compile<Func<Rally, bool>>(pushHalfLongString);
            //var test = rallies.ToArray().AsQueryable().Where(expression, null).Count();
            double pushHalfLong = rallies.Where(func3).Count() + 0.03;

            string pushLongString = (string)values[4];
            pushLongString = pushLongString.Replace('\'', '"');
            Func<Rally, bool> func4 = ExpressionParser.Compile<Func<Rally, bool>>(pushLongString);
            //var test = rallies.ToArray().AsQueryable().Where(expression, null).Count();
            double pushLong = rallies.Where(func4).Count() + 0.04;

            string topspinDiagonalString = (string)values[5];
            topspinDiagonalString = topspinDiagonalString.Replace('\'', '"');
            Func<Rally, bool> func5 = ExpressionParser.Compile<Func<Rally, bool>>(topspinDiagonalString);
            //var test = rallies.ToArray().AsQueryable().Where(expression, null).Count();
            double topspinDiagonal = rallies.Where(func5).Count() + 0.05;

            string topspinMiddleString = (string)values[6];
            topspinMiddleString = topspinMiddleString.Replace('\'', '"');
            Func<Rally, bool> func6 = ExpressionParser.Compile<Func<Rally, bool>>(topspinMiddleString);
            //var test = rallies.ToArray().AsQueryable().Where(expression, null).Count();
            double topspinMiddle = rallies.Where(func6).Count() + 0.06;

            string topspinParallelString = (string)values[7];
            topspinParallelString = topspinParallelString.Replace('\'', '"');
            Func<Rally, bool> func7 = ExpressionParser.Compile<Func<Rally, bool>>(topspinParallelString);
            //var test = rallies.ToArray().AsQueryable().Where(expression, null).Count();
            double topspinParallel = rallies.Where(func7).Count() + 0.07;



            string gridName = (string)values[8];

            //int pushShort = Int32.Parse((string)values[1]);
            //int pushHalfLong = Int32.Parse((string)values[2]);
            //int pushLong = Int32.Parse((string)values[3]);

            //int [] ranking = new int[] { flip, pushshort, pushhalflong, pushlong };
            //array.sort(ranking);
            //array.reverse(ranking);

            List<double> unranked = new List<double> { flip, pushShort, pushHalfLong, pushLong, topspinDiagonal, topspinMiddle, topspinParallel };
            unranked.Sort();
            unranked.Reverse();
            switch (gridName)
            {

                case "Flip":
                    if (unranked.IndexOf(flip) < 4)
                        return unranked.IndexOf(flip) + 1;
                    else
                        return unranked.IndexOf(flip) - 3;
                case "PushShort":
                    if (unranked.IndexOf(pushShort) < 4)
                        return unranked.IndexOf(pushShort) + 1;
                    else
                        return unranked.IndexOf(pushShort) - 3;
                case "PushHalfLong":
                    if (unranked.IndexOf(pushHalfLong) < 4)
                        return unranked.IndexOf(pushHalfLong) + 1;
                    else
                        return unranked.IndexOf(pushHalfLong) - 3;
                case "PushLong":
                    if (unranked.IndexOf(pushLong) < 4)
                        return unranked.IndexOf(pushLong) + 1;
                    else
                        return unranked.IndexOf(pushLong) - 3;
                case "TopspinDiagonal":
                    if (unranked.IndexOf(topspinDiagonal) < 4)
                        return unranked.IndexOf(topspinDiagonal) + 1;
                    else
                        return unranked.IndexOf(topspinDiagonal) - 3;
                case "TopspinMiddle":
                    if (unranked.IndexOf(topspinMiddle) < 4)
                        return unranked.IndexOf(topspinMiddle) + 1;
                    else
                        return unranked.IndexOf(topspinMiddle) - 3;
                case "TopspinParallel":
                    if (unranked.IndexOf(topspinParallel) < 4)
                        return unranked.IndexOf(topspinParallel) + 1;
                    else
                        return unranked.IndexOf(topspinParallel) - 3;
                default:
                    return 0;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }





    }
}
