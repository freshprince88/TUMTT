//-----------------------------------------------------------------------
// <copyright file="TransitionsSection.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Report.Sections
{
    using TT.Lib.Models;
    using TT.Lib.Models.Statistics;

    /// <summary>
    /// Section for transitions.
    /// </summary>
    public class TransitionsSection : IReportSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransitionsSection"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        public TransitionsSection(Match match)
        {
            this.Transitions = new Transitions(match);
        }

        /// <summary>
        /// Gets the transitions.
        /// </summary>
        public Transitions Transitions { get; private set; }
    }
}
