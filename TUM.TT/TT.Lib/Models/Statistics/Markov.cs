//-----------------------------------------------------------------------
// <copyright file="Markov.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Models.Statistics
{
    using System;
    using MathNet.Numerics.LinearAlgebra.Double;
    using MathNet.Numerics.LinearAlgebra;

    /// <summary>
    /// Markov simulation
    /// </summary>
    public static class Markov
    {
        /// <summary>
        /// Perform a Markov simulation over a state matrix with the given start vector.
        /// </summary>
        /// <param name="input">The state matrix</param>
        /// <param name="start">The start vector</param>
        /// <param name="iterations">The number of iterations</param>
        /// <returns>The result of the last iteration</returns>
        public static Vector<double> Simulate(
            Matrix<double> input,
            Vector<double> start,
            int iterations)
        {
            if (input.ColumnCount != start.Count)
            {
                throw new ArgumentException("start value as insufficient items");
            }

            var last = start;
            for (int i = 1; i <= iterations; ++i)
            {
                var current = new DenseVector(last.Count);

                foreach (var column in input.EnumerateColumnsIndexed())
                {
                    current[column.Item1] = column.Item2 * last;
                }

                last = current;
            }

            return last;
        }
    }
}
