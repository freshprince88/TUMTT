//-----------------------------------------------------------------------
// <copyright file="DefaultReportGenerator.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Report.Generators
{
    using OxyPlot;
    using TT.Models;
    using TT.Report.Plots;
    using TT.Report.Sections;

    /// <summary>
    /// Generates a default report.
    /// </summary>
    public class DefaultReportGenerator : IReportGenerator
    {
        /// <summary>
        /// Generates a default report for a match.
        /// </summary>
        /// <param name="match">The match to generate the report for.</param>
        /// <returns>The generated report.</returns>
        public Report GenerateReport(Match match)
        {
            var plotStyle = new PlotStyle()
            {
                TextFont = "Calibri",
                TextSize = 10,
                TextColor = OxyColors.Black,
                BorderColor = OxyColors.DarkGray,
                GridlineColor = OxyColors.LightGray,
                FirstPlayerColor = OxyColor.Parse("#4F81BD"),
                SecondPlayerColor = OxyColor.Parse("#C0504D"),
            };

            // TODO: Swap players!
            var report = new Report();
            report.Sections.Add(new MetadataSection()
                {
                    Subject = string.Format(
                         "{0} {1}, {2} vs. {3}",
                         match.Tournament,
                         match.Round.ToString(),
                         match.FirstPlayer.Name,
                         match.SecondPlayer.Name),
                    Title = "Table Tennis Performance Report",
                    Author = "TODO"
                });
            report.Sections.Add(
                 new HeaderSection()
                {
                    Headline = "Table Tennis Performance Report",
                     Tournament = match.Tournament,
                     Category = match.Category.ToString(),
                     DisabilityClass = match.DisabilityClass.ToString(),
                     Round = match.Round.ToString(),                    
                     Date = match.DateTime
                });
            report.Sections.Add(new BasicInformationSection(match));
            report.Sections.Add(new RallyLengthSection(match, plotStyle));
            report.Sections.Add(new ScoringProcessSection(match, plotStyle));
            report.Sections.Add(new MatchDynamicsSection(match, plotStyle));
            var transitionSection = new TransitionsSection(match);
            report.Sections.Add(transitionSection);
            report.Sections.Add(new TechnicalEfficiencySection(transitionSection.Transitions));
            report.Sections.Add(new RelevanceOfStrokeSection(transitionSection.Transitions, plotStyle));

            return report;
        }
    }
}
