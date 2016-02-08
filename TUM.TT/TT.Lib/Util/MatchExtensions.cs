//-----------------------------------------------------------------------
// <copyright file="MatchExtensions.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Util
{
    using TT.Lib.Models;

    /// <summary>
    /// Extends the match class with some useful additions.
    /// </summary>
    public static class MatchExtensions
    {
        /// <summary>
        /// Gets a human-readable title of a match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns>A title for the match.</returns>
        public static string Title(this Match match)
        {
            return string.Format(
                "{0} - {1}",
                match.Tournament.WhenNullOrEmpty("Unknown tournament"),
                match.Round.WhenNullOrEmpty("Unknown round"));
        }

        /// <summary>
        /// Gets a default file name for a match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns>The default file name.</returns>
        public static string DefaultFilename(this Match match)
        {
            var first = match.FirstPlayer ?? new Player();
            var second = match.SecondPlayer ?? new Player();
            return string.Format(
                "{0:yyyy-MM-dd} - {1} vs {2} - {3} - {4}",
                match.DateTime,
                first.Name.WhenNullOrEmpty("A"),
                second.Name.WhenNullOrEmpty("B"),
                match.Round.WhenNullOrEmpty("Unknown round"),
                match.Tournament.WhenNullOrEmpty("Unknown tournament"));
        }
    }
}
