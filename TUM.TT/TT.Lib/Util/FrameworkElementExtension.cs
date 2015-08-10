//-----------------------------------------------------------------------
// <copyright file="FrameworkElementExtension.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Util
{
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Extensions for <see cref="FrameworkElement"/>.
    /// </summary>
    public static class FrameworkElementExtension
    {
        /// <summary>
        /// Traverse all logical parents of <paramref name="element"/>, including <paramref name="element"/> itself.
        /// </summary>
        /// <param name="element">The element whose parents to traverse.</param>
        /// <returns>An enumerable of all parents with <paramref name="element"/> is first item.</returns>
        public static IEnumerable<FrameworkElement> TraverseParents(this FrameworkElement element)
        {
            while (element != null)
            {
                yield return element;
                element = element.Parent as FrameworkElement;
            }
        }
    }
}
