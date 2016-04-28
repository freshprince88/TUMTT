//-----------------------------------------------------------------------
// <copyright file="IReportGenerator.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Report.Generators
{
    using TT.Models;

    /// <summary>
    /// Interface for report generators.
    /// </summary>
    public interface IReportGenerator
    {
        /// <summary>
        /// Generates a report for a match.
        /// </summary>
        /// <param name="match">The match to generate the report for.</param>
        /// <returns>The generated report.</returns>
        Report GenerateReport(Match match);
    }
}
