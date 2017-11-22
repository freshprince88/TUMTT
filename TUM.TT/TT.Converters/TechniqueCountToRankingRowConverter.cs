using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Zhucai.LambdaParser;
using System.Linq;
using TT.Models;

namespace TT.Converters
{
    [ValueConversion(typeof(object), typeof(int))]
    public class TechniqueCountToRankingRowConverter : BaseConverter, IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<Rally> rallies = (IEnumerable<Rally>)values[0];
            MatchPlayer? p = values.Length >= 14 ? (MatchPlayer?)values[13] : null;
            int? strokeNumber = values.Length >= 15 ? (int?)values[14] : null;

            if (strokeNumber != null)
                rallies = new List<Rally>(rallies.Where(r => strokeNumber.Value == int.MaxValue || r.Strokes.Count >= strokeNumber.Value));

            string flipString = ReplaceExpression((string)values[1], param: parameter, player: p, strokeNumber: strokeNumber);
            string pushShortString = ReplaceExpression((string)values[2], param: parameter, player: p, strokeNumber: strokeNumber);
            string pushHalfLongString = ReplaceExpression((string)values[3], param: parameter, player: p, strokeNumber: strokeNumber);
            string pushLongString = ReplaceExpression((string)values[4], param: parameter, player: p, strokeNumber: strokeNumber);
            string topspinDiagonalString = ReplaceExpression((string)values[5], param: parameter, player: p, strokeNumber: strokeNumber);
            string topspinMiddleString = ReplaceExpression((string)values[6], param: parameter, player: p, strokeNumber: strokeNumber);
            string topspinParallelString = ReplaceExpression((string)values[7], param: parameter, player: p, strokeNumber: strokeNumber);
            string blockDiagonalString = ReplaceExpression((string)values[8], param: parameter, player: p, strokeNumber: strokeNumber);
            string blockParallelString = ReplaceExpression((string)values[9], param: parameter, player: p, strokeNumber: strokeNumber);
            string blockMiddleString = ReplaceExpression((string)values[10], param: parameter, player: p, strokeNumber: strokeNumber);
            string chopString = ReplaceExpression((string)values[11], param: parameter, player: p, strokeNumber: strokeNumber);

            Func<Rally, bool> func1 = ExpressionParser.Compile<Func<Rally, bool>>(flipString);
            Func<Rally, bool> func2 = ExpressionParser.Compile<Func<Rally, bool>>(pushShortString);
            Func<Rally, bool> func3 = ExpressionParser.Compile<Func<Rally, bool>>(pushHalfLongString);
            Func<Rally, bool> func4 = ExpressionParser.Compile<Func<Rally, bool>>(pushLongString);
            Func<Rally, bool> func5 = ExpressionParser.Compile<Func<Rally, bool>>(topspinDiagonalString);
            Func<Rally, bool> func6 = ExpressionParser.Compile<Func<Rally, bool>>(topspinMiddleString);
            Func<Rally, bool> func7 = ExpressionParser.Compile<Func<Rally, bool>>(topspinParallelString);
            Func<Rally, bool> func8 = ExpressionParser.Compile<Func<Rally, bool>>(blockDiagonalString);
            Func<Rally, bool> func9 = ExpressionParser.Compile<Func<Rally, bool>>(blockParallelString);
            Func<Rally, bool> func10 = ExpressionParser.Compile<Func<Rally, bool>>(blockMiddleString);
            Func<Rally, bool> func11 = ExpressionParser.Compile<Func<Rally, bool>>(chopString);

            double flip, pushShort, pushHalfLong, pushLong, topspinDiagonal, topspinMiddle, topspinParallel, blockDiagonal, blockParallel, blockMiddle, chop;
            flip = pushShort = pushHalfLong = pushLong = topspinDiagonal = topspinMiddle = topspinParallel = blockDiagonal = blockParallel = blockMiddle = chop = 0;

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
                    if (originalStrokeNumber != null && strokeNumber == 1)
                        continue;

                    if (originalStrokeNumber != null && lastUsedStrokeNumber != strokeNumber)
                    {
                        flipString = ReplaceExpression((string)values[1], param: parameter, player: p, strokeNumber: strokeNumber);
                        func1 = ExpressionParser.Compile<Func<Rally, bool>>(flipString);
                    }
                    flip += func1.Invoke(r) ? 1 : 0;

                    if (originalStrokeNumber != null && lastUsedStrokeNumber != strokeNumber)
                    {
                        pushShortString = ReplaceExpression((string)values[2], param: parameter, player: p, strokeNumber: strokeNumber);
                        func2 = ExpressionParser.Compile<Func<Rally, bool>>(pushShortString);
                    }
                    pushShort += func2.Invoke(r) ? 1 : 0;

                    if (originalStrokeNumber != null && lastUsedStrokeNumber != strokeNumber)
                    {
                        pushHalfLongString = ReplaceExpression((string)values[3], param: parameter, player: p, strokeNumber: strokeNumber);
                        func3 = ExpressionParser.Compile<Func<Rally, bool>>(pushHalfLongString);
                    }
                    pushHalfLong += func3.Invoke(r) ? 1 : 0;

                    if (originalStrokeNumber != null && lastUsedStrokeNumber != strokeNumber)
                    {
                        pushLongString = ReplaceExpression((string)values[4], param: parameter, player: p, strokeNumber: strokeNumber);
                        func4 = ExpressionParser.Compile<Func<Rally, bool>>(pushLongString);
                    }
                    pushLong += func4.Invoke(r) ? 1 : 0;

                    if (originalStrokeNumber != null && lastUsedStrokeNumber != strokeNumber)
                    {
                        topspinDiagonalString = ReplaceExpression((string)values[5], param: parameter, player: p, strokeNumber: strokeNumber);
                        func5 = ExpressionParser.Compile<Func<Rally, bool>>(topspinDiagonalString);
                    }
                    topspinDiagonal += func5.Invoke(r) ? 1 : 0;

                    if (originalStrokeNumber != null && lastUsedStrokeNumber != strokeNumber)
                    {
                        topspinMiddleString = ReplaceExpression((string)values[6], param: parameter, player: p, strokeNumber: strokeNumber);
                        func6 = ExpressionParser.Compile<Func<Rally, bool>>(topspinMiddleString);
                    }
                    topspinMiddle += func6.Invoke(r) ? 1 : 0;

                    if (originalStrokeNumber != null && lastUsedStrokeNumber != strokeNumber)
                    {
                        topspinParallelString = ReplaceExpression((string)values[7], param: parameter, player: p, strokeNumber: strokeNumber);
                        func7 = ExpressionParser.Compile<Func<Rally, bool>>(topspinParallelString);
                    }
                    topspinParallel += func7.Invoke(r) ? 1 : 0;

                    if (originalStrokeNumber != null && lastUsedStrokeNumber != strokeNumber)
                    {
                        blockDiagonalString = ReplaceExpression((string)values[8], param: parameter, player: p, strokeNumber: strokeNumber);
                        func8 = ExpressionParser.Compile<Func<Rally, bool>>(blockDiagonalString);
                    }
                    blockDiagonal += func8.Invoke(r) ? 1 : 0;

                    if (originalStrokeNumber != null && lastUsedStrokeNumber != strokeNumber)
                    {
                        blockParallelString = ReplaceExpression((string)values[9], param: parameter, player: p, strokeNumber: strokeNumber);
                        func9 = ExpressionParser.Compile<Func<Rally, bool>>(blockParallelString);
                    }
                    blockParallel += func9.Invoke(r) ? 1 : 0;

                    if (originalStrokeNumber != null && lastUsedStrokeNumber != strokeNumber)
                    {
                        blockMiddleString = ReplaceExpression((string)values[10], param: parameter, player: p, strokeNumber: strokeNumber);
                        func10 = ExpressionParser.Compile<Func<Rally, bool>>(blockMiddleString);
                    }
                    blockMiddle += func10.Invoke(r) ? 1 : 0;

                    if (originalStrokeNumber != null && lastUsedStrokeNumber != strokeNumber)
                    {
                        chopString = ReplaceExpression((string)values[11], param: parameter, player: p, strokeNumber: strokeNumber);
                        func11 = ExpressionParser.Compile<Func<Rally, bool>>(chopString);
                    }
                    chop += func11.Invoke(r) ? 1 : 0;

                    lastUsedStrokeNumber = strokeNumber;
                }
            }
            flip += 0.11; pushShort += 0.10; pushHalfLong += 0.09; pushLong += 0.08; topspinDiagonal += 0.07; topspinMiddle += 0.06;
            topspinParallel += 0.05; blockDiagonal += 0.04; blockParallel += 0.02; blockMiddle += 0.03; chop += 0.01;

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
