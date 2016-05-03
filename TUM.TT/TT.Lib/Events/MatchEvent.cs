//-----------------------------------------------------------------------
// <copyright file="MatchEvent.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Events
{
    using TT.Models;

    /// <summary>
    /// An event on a match.
    /// </summary>
    public abstract class MatchEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchEvent"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        protected MatchEvent(Match match)
        {
            this.Match = match;
        }

        /// <summary>
        /// Gets the match affected by this event.
        /// </summary>
        public Match Match { get; private set; }
    }
}
