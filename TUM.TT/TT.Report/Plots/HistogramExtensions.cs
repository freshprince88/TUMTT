//-----------------------------------------------------------------------
// <copyright file="HistogramExtensions.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Report.Plots
{
    using System.Collections.Generic;
    using MathNet.Numerics.Statistics;
    using OxyPlot.Series;

    /// <summary>
    /// Plotting extensions for <see cref="Histogram"/>.
    /// </summary>
    public static class HistogramExtensions
    {
        /// <summary>
        /// Creates items for all buckets in a histogram.
        /// </summary>
        /// <param name="histogram">The histogram.</param>
        /// <returns>The column items.</returns>
        public static IEnumerable<ColumnItem> ColumnItems(this Histogram histogram)
        {
            for (int i = 0; i < histogram.BucketCount; ++i)
            {
                yield return new ColumnItem(histogram[i].Count);
            }
        }
    }
}
