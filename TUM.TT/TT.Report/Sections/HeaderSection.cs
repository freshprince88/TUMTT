//-----------------------------------------------------------------------
// <copyright file="HeaderSection.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//----------------------------------------------------------------------

namespace TT.Report.Sections
{
    using System;

    /// <summary>
    /// The header of a report.
    /// </summary>
    public class HeaderSection : IReportSection
    {
        /// <summary>
        /// Gets or sets the report headline
        /// </summary>
        public string Headline { get; set; }

        /// <summary>
        /// Gets or sets the tournament.
        /// </summary>
        public string Tournament { get; set; }

        /// <summary>
        /// Gets or sets the round.
        /// </summary>
        public string Round { get; set; }

        /// <summary>
        /// Gets or sets the match date
        /// </summary>
        public DateTime Date { get; set; }
    }
}
