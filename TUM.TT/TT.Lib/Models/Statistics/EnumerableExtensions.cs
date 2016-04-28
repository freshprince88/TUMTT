//-----------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Models.Statistics
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// This class provides statistical extensions to enumerable objects.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Calculate the categorical median for an enumerable.
        /// </summary>        
        /// <param name="self">The enumerable.</param>
        /// <returns>The categorical median.</returns>
        /// <seealso href="http://de.wikipedia.org/wiki/Median#Median_von_gruppierten_Daten"/>
        public static double CategoricalMedian(this IEnumerable<int> self)
        {
            var cached = self.ToArray();
            var categories = cached.ToLookup(l => l);
            var n = cached.Length;

            var nhalf = n / 2.0;
            var lowerCategoriesSum = 0;
            var category = 0;
            while (lowerCategoriesSum + categories[category].Count() < nhalf)
            {
                lowerCategoriesSum += categories[category].Count();
                category++;
            }

            return category + ((nhalf - lowerCategoriesSum) / categories[category].Count());
        }
    }
}
