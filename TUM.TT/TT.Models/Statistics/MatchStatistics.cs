//-----------------------------------------------------------------------
// <copyright file="MatchStatistics.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Statistics
{
    /// <summary>
    /// Base class for match statistics.
    /// </summary>
    public class MatchStatistics : IMatchStatistics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchStatistics"/> class.
        /// </summary>
        /// <param name="match">The match</param>
        public MatchStatistics(Match match)
        {
            this.Match = match;
        }

        /// <summary>
        /// Gets the match these statistics are calculated from.
        /// </summary>
        public Match Match { get; private set; }

        public virtual bool CountStroke(Stroke stroke, MatchPlayer player, int strokeNumber)
        {
            if (stroke.Player == player)
            {
                switch (strokeNumber)
                {
                    case int.MaxValue:
                        {
                            var lastWinnerStroke = stroke.Rally.LastWinnerStroke();
                            return lastWinnerStroke != null && stroke.Number == lastWinnerStroke.Number;
                        }
                    case -1: return true;
                    default: return stroke.Number == strokeNumber;
                }
            }
            return false;
        }
    }
}
