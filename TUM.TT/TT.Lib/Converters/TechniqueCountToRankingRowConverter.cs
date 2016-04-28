using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Zhucai.LambdaParser;
using Zhucai.LambdaParser.ObjectDynamicExtension;
using System.Linq;
using TT.Models;

namespace TT.Lib.Converters
{
    [ValueConversion(typeof(object), typeof(int))]
    public class TechniqueCountToRankingRowConverter : BaseConverter, IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            List<Rally> rallies = (List<Rally>)values[0];
            string flipString = (string)values[1];
            flipString = ReplaceExpression(flipString);
            Func<Rally, bool> func1 = ExpressionParser.Compile<Func<Rally, bool>>(flipString);

            double flip = rallies.Where(func1).Count() + 0.11;

            string pushShortString = (string)values[2];
            pushShortString = ReplaceExpression(pushShortString);
            Func<Rally, bool> func2 = ExpressionParser.Compile<Func<Rally, bool>>(pushShortString);
            double pushShort = rallies.Where(func2).Count() + 0.10;

            string pushHalfLongString = (string)values[3];
            pushHalfLongString = ReplaceExpression(pushHalfLongString);
            Func<Rally, bool> func3 = ExpressionParser.Compile<Func<Rally, bool>>(pushHalfLongString);
            double pushHalfLong = rallies.Where(func3).Count() + 0.09;

            string pushLongString = (string)values[4];
            pushLongString = ReplaceExpression(pushLongString);
            Func<Rally, bool> func4 = ExpressionParser.Compile<Func<Rally, bool>>(pushLongString);
            double pushLong = rallies.Where(func4).Count() + 0.08;

            string topspinDiagonalString = (string)values[5];
            topspinDiagonalString = ReplaceExpression(topspinDiagonalString); ;
            Func<Rally, bool> func5 = ExpressionParser.Compile<Func<Rally, bool>>(topspinDiagonalString);
            double topspinDiagonal = rallies.Where(func5).Count() + 0.07;

            string topspinMiddleString = (string)values[6];
            topspinMiddleString = ReplaceExpression(topspinMiddleString);
            Func<Rally, bool> func6 = ExpressionParser.Compile<Func<Rally, bool>>(topspinMiddleString);
            double topspinMiddle = rallies.Where(func6).Count() + 0.06;

            string topspinParallelString = (string)values[7];
            topspinParallelString = ReplaceExpression(topspinParallelString);
            Func<Rally, bool> func7 = ExpressionParser.Compile<Func<Rally, bool>>(topspinParallelString);
            double topspinParallel = rallies.Where(func7).Count() + 0.05;

            string blockDiagonalString = (string)values[8];
            blockDiagonalString = ReplaceExpression(blockDiagonalString); ;
            Func<Rally, bool> func8 = ExpressionParser.Compile<Func<Rally, bool>>(blockDiagonalString);
            double blockDiagonal = rallies.Where(func8).Count() + 0.04;

            string blockMiddleString = (string)values[9];
            blockMiddleString = ReplaceExpression(blockMiddleString);
            Func<Rally, bool> func9 = ExpressionParser.Compile<Func<Rally, bool>>(blockMiddleString);
            double blockMiddle = rallies.Where(func9).Count() + 0.03;

            string blockParallelString = (string)values[10];
            blockParallelString = ReplaceExpression(blockParallelString);
            Func<Rally, bool> func10 = ExpressionParser.Compile<Func<Rally, bool>>(blockParallelString);
            double blockParallel = rallies.Where(func10).Count() + 0.02;

            string chopString = (string)values[11];
            chopString = ReplaceExpression(chopString);
            Func<Rally, bool> func11 = ExpressionParser.Compile<Func<Rally, bool>>(chopString);
            double chop = rallies.Where(func11).Count() + 0.01;


            string gridName = (string)values[12];

            //int pushShort = Int32.Parse((string)values[1]);
            //int pushHalfLong = Int32.Parse((string)values[2]);
            //int pushLong = Int32.Parse((string)values[3]);

            //int [] ranking = new int[] { flip, pushshort, pushhalflong, pushlong };
            //array.sort(ranking);
            //array.reverse(ranking);

            List<double> unranked = new List<double> { flip, pushShort, pushHalfLong, pushLong, topspinDiagonal, topspinMiddle, topspinParallel, blockDiagonal, blockMiddle, blockParallel, chop };
            unranked.Sort();
            unranked.Reverse();
            switch (gridName)
            {

                case "Flip":
                    if (unranked.IndexOf(flip) < 4)
                        return 3;
                    else
                        return 8;
                case "PushShort":
                    if (unranked.IndexOf(pushShort) < 4)
                        return 3;
                    else
                        return 8;
                case "PushHalfLong":
                    if (unranked.IndexOf(pushHalfLong) < 4)
                        return 3;
                    else
                        return 8;
                case "PushLong":
                    if (unranked.IndexOf(pushLong) < 4)
                        return 3;
                    else
                        return 8;
                case "TopspinDiagonal":
                    if (unranked.IndexOf(topspinDiagonal) < 4)
                        return 3;
                    else
                        return 8;
                case "TopspinMiddle":
                    if (unranked.IndexOf(topspinMiddle) < 4)
                        return 3;
                    else
                        return 8;
                case "TopspinParallel":
                    if (unranked.IndexOf(topspinParallel) < 4)
                        return 3;
                    else
                        return 8;
                case "BlockDiagonal":
                    if (unranked.IndexOf(blockDiagonal) < 4)
                        return 3;
                    else
                        return 8;
                case "BlockMiddle":
                    if (unranked.IndexOf(blockMiddle) < 4)
                        return 3;
                    else
                        return 8;
                case "BlockParallel":
                    if (unranked.IndexOf(blockParallel) < 4)
                        return 3;
                    else
                        return 8;
                case "Chop":
                    if (unranked.IndexOf(chop) < 4)
                        return 3;
                    else
                        return 8;
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
