//-----------------------------------------------------------------------
// <copyright file="MatchStatistics.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

using System.Diagnostics;

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

        public virtual bool CountStroke(Stroke stroke, MatchPlayer player, int strokeNumber, string stat = null)
        {
            bool retVal;
            if (stroke.Player == player)
            {
                switch (strokeNumber)
                {
                    case int.MaxValue:
                        {
                            var lastWinnerStroke = stroke.Rally.LastWinnerStroke();
                            retVal = lastWinnerStroke != null && stroke.Number == lastWinnerStroke.Number;
                            break;
                        }
                    case -1: retVal = true; break;
                    default: retVal = stroke.Number == strokeNumber; break;
                }
            }
            else
                retVal = false;

            if (stat != null && retVal)
            {
                Debug.WriteLine("Counting {4}-stat of stroke {0} of rally {1} - technique={2} lastWinnerStroke={3}", stroke.Number, stroke.Rally.Number, (stroke.Stroketechnique != null ? stroke.Stroketechnique.Type : "null"), (stroke.Rally.LastWinnerStroke() != null ? stroke.Rally.LastWinnerStroke().Number.ToString() : "null"), stat);
            }
            return retVal;
        }
    }
}
