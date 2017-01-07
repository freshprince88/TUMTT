﻿using System;
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
                rallies = rallies.Where(r => r.Strokes.Count >= strokeNumber.Value);

            double flip, pushShort, pushHalfLong, pushLong, topspinDiagonal, topspinMiddle, topspinParallel, blockDiagonal, blockParallel, blockMiddle, chop;
            flip = pushShort = pushHalfLong = pushLong = topspinDiagonal = topspinMiddle = topspinParallel = blockDiagonal = blockParallel = blockMiddle = chop = 0;
            foreach (var r in rallies)
            {
                var start = 0;
                var limit = 1;
                if (strokeNumber != null)
                {
                    if (strokeNumber.Value == -1)
                        limit = r.Strokes.Count;
                    else if (strokeNumber.Value == int.MaxValue)
                    {
                        start = r.LastWinnerStroke().Number - 1;
                        limit = r.LastWinnerStroke().Number;
                    }
                    else
                    {
                        start = strokeNumber.Value - 1;
                        limit = strokeNumber.Value;
                    }
                }
                for (int i = start; i < limit; i++)
                {
                    strokeNumber = i + 1;

                    string flipString = (string)values[1];
                    flipString = ReplaceExpression(flipString, param: parameter, player: p, strokeNumber: strokeNumber);
                    Func<Rally, bool> func1 = ExpressionParser.Compile<Func<Rally, bool>>(flipString);
                    flip += func1.Invoke(r) ? 1 : 0; //double flip = rallies.Where(func1).Count() + 0.11;

                    string pushShortString = (string)values[2];
                    pushShortString = ReplaceExpression(pushShortString, param: parameter, player: p, strokeNumber: strokeNumber);
                    Func<Rally, bool> func2 = ExpressionParser.Compile<Func<Rally, bool>>(pushShortString);
                    pushShort += func2.Invoke(r) ? 1 : 0;  //double pushShort = rallies.Where(func2).Count() + 0.10;

                    string pushHalfLongString = (string)values[3];
                    pushHalfLongString = ReplaceExpression(pushHalfLongString, param: parameter, player: p, strokeNumber: strokeNumber);
                    Func<Rally, bool> func3 = ExpressionParser.Compile<Func<Rally, bool>>(pushHalfLongString);
                    pushHalfLong += func3.Invoke(r) ? 1 : 0;  //double pushHalfLong = rallies.Where(func3).Count() + 0.09;

                    string pushLongString = (string)values[4];
                    pushLongString = ReplaceExpression(pushLongString, param: parameter, player: p, strokeNumber: strokeNumber);
                    Func<Rally, bool> func4 = ExpressionParser.Compile<Func<Rally, bool>>(pushLongString);
                    pushLong += func4.Invoke(r) ? 1 : 0;  //double pushLong = rallies.Where(func4).Count() + 0.08;

                    string topspinDiagonalString = (string)values[5];
                    topspinDiagonalString = ReplaceExpression(topspinDiagonalString, param: parameter, player: p, strokeNumber: strokeNumber);
                    Func<Rally, bool> func5 = ExpressionParser.Compile<Func<Rally, bool>>(topspinDiagonalString);
                    topspinDiagonal += func5.Invoke(r) ? 1 : 0;  //double topspinDiagonal = rallies.Where(func5).Count() + 0.07;

                    string topspinMiddleString = (string)values[6];
                    topspinMiddleString = ReplaceExpression(topspinMiddleString, param: parameter, player: p, strokeNumber: strokeNumber);
                    Func<Rally, bool> func6 = ExpressionParser.Compile<Func<Rally, bool>>(topspinMiddleString);
                    topspinMiddle += func6.Invoke(r) ? 1 : 0;  //double topspinMiddle = rallies.Where(func6).Count() + 0.06;

                    string topspinParallelString = (string)values[7];
                    topspinParallelString = ReplaceExpression(topspinParallelString, param: parameter, player: p, strokeNumber: strokeNumber);
                    Func<Rally, bool> func7 = ExpressionParser.Compile<Func<Rally, bool>>(topspinParallelString);
                    topspinParallel += func7.Invoke(r) ? 1 : 0; //double topspinParallel = rallies.Where(func7).Count() + 0.05;

                    string blockDiagonalString = (string)values[8];
                    blockDiagonalString = ReplaceExpression(blockDiagonalString, param: parameter, player: p, strokeNumber: strokeNumber);
                    Func<Rally, bool> func8 = ExpressionParser.Compile<Func<Rally, bool>>(blockDiagonalString);
                    blockDiagonal += func8.Invoke(r) ? 1 : 0; //double blockDiagonal = rallies.Where(func8).Count() + 0.04;

                    string blockParallelString = (string)values[9];
                    blockParallelString = ReplaceExpression(blockParallelString, param: parameter, player: p, strokeNumber: strokeNumber);
                    Func<Rally, bool> func9 = ExpressionParser.Compile<Func<Rally, bool>>(blockParallelString);
                    blockParallel += func9.Invoke(r) ? 1 : 0; //double blockParallel = rallies.Where(func10).Count() + 0.02;

                    string blockMiddleString = (string)values[10];
                    blockMiddleString = ReplaceExpression(blockMiddleString, param: parameter, player: p, strokeNumber: strokeNumber);
                    Func<Rally, bool> func10 = ExpressionParser.Compile<Func<Rally, bool>>(blockMiddleString);
                    blockMiddle += func10.Invoke(r) ? 1 : 0; //double blockMiddle = rallies.Where(func9).Count() + 0.03;

                    string chopString = (string)values[11];
                    chopString = ReplaceExpression(chopString, param: parameter, player: p, strokeNumber: strokeNumber);
                    Func<Rally, bool> func11 = ExpressionParser.Compile<Func<Rally, bool>>(chopString);
                    chop += func11.Invoke(r) ? 1 : 0; //double chop = rallies.Where(func11).Count() + 0.01;
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
