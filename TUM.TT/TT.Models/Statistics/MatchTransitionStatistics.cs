//-----------------------------------------------------------------------
// <copyright file="MatchTransitionStatistics.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Statistics
{
    /// <summary>
    /// Base class for match transition statistics
    /// </summary>
    public abstract class MatchTransitionStatistics : IMatchTransitionStatistics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchTransitionStatistics"/> class.
        /// </summary>
        /// <param name="transitions">The transitions.</param>
        protected MatchTransitionStatistics(Transitions transitions)
        {
            this.Transitions = transitions;
        }

        /// <summary>
        /// Gets the transitions these statistics are based on.
        /// </summary>
        public Transitions Transitions { get; private set; }

        /// <summary>
        /// Gets the match the statistics were calculated from.
        /// </summary>
        public Match Match
        {
            get { return this.Transitions.Match; }
        }
    }
}
