//-----------------------------------------------------------------------
// <copyright file="MatchStatistics.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Models.Statistics
{
    /// <summary>
    /// Base class for match statistics.
    /// </summary>
    public abstract class MatchStatistics : IMatchStatistics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchStatistics"/> class.
        /// </summary>
        /// <param name="match">The match</param>
        protected MatchStatistics(Match match)
        {
            this.Match = match;
        }

        /// <summary>
        /// Gets the match these statistics are calculated from.
        /// </summary>
        public Match Match { get; private set; }
    }
}
