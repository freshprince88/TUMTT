//-----------------------------------------------------------------------
// <copyright file="MatchOpenedEvent.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Viewer.Events
{
    using TT.Lib.Models;

    /// <summary>
    /// An event indicating that a match was opened.
    /// </summary>
    public class MatchInformationEvent : MatchEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchOpenedEvent"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <param name="fileName">The file name the match was opened from</param>
        public MatchInformationEvent(Match match)
            : base(match)
        {
        }

        /// <summary>
        /// Gets the name of the file this match was opened from.
        /// </summary>
    }
}
