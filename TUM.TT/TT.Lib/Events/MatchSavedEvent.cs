//-----------------------------------------------------------------------
// <copyright file="MatchOpenedEvent.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Events
{
    using TT.Models;

    /// <summary>
    /// An event indicating that a match was saved.
    /// </summary>
    public class MatchSavedEvent : MatchEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchSavedEvent"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        public MatchSavedEvent(Match match) : base(match) { }
    }
}
