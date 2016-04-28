//-----------------------------------------------------------------------
// <copyright file="IMatchStatistics.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Models.Statistics
{
    /// <summary>
    /// Statistics for a match.
    /// </summary>
    public interface IMatchStatistics
    {
        /// <summary>
        /// Gets the match the statistics were calculated from.
        /// </summary>
        Match Match { get; }
    }
}
