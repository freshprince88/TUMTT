//-----------------------------------------------------------------------
// <copyright file="TechnicalEfficiencySection.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Report.Sections
{
    using TT.Models.Statistics;

    /// <summary>
    /// Section for technical efficiency.
    /// </summary>
    public class TechnicalEfficiencySection : IReportSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TechnicalEfficiencySection"/> class.
        /// </summary>
        /// <param name="transitions">The transitions.</param>
        public TechnicalEfficiencySection(Transitions transitions)
        {
            this.TechnicalEfficiency = new TechnicalEfficiency(transitions);
        }

        /// <summary>
        /// Gets the technical efficiency.
        /// </summary>
        public TechnicalEfficiency TechnicalEfficiency { get; private set; }
    }
}
