//-----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Util
{
    /// <summary>
    /// Extensions for the <see cref="System.String"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a default value, if a string is null or empty.
        /// </summary>
        /// <param name="s">The string to test</param>
        /// <param name="defaultString">The default string.</param>
        /// <returns>
        /// The string <paramref name="s"/>, or the <paramref name="defaultString"/>, 
        /// if <paramref name="s"/> is null or empty.
        /// </returns>
        public static string WhenNullOrEmpty(this string s, string defaultString)
        {
            return !string.IsNullOrWhiteSpace(s) ? s : defaultString;
        }
    }
}
