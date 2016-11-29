using OxyPlot;
using TT.Lib.Converters;
using TT.Models;
using TT.Report.Plots;
using TT.Report.Sections;

namespace TT.Report.Generators
{
    /// <summary>
    /// Generates a customized report.
    /// </summary>
    public class CustomizedReportGenerator : IReportGenerator
    {

        private MatchPlayerToColorConverter conv;

        public CustomizedReportGenerator()
        {
            this.conv = new MatchPlayerToColorConverter();
        }

        /// <summary>
        /// Generates a customized report for a match.
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
            
            var report = new Report();
            report.Sections.Add(new MetadataSection()
            {
                Subject = string.Format(
                         "{0} {1}, {2} vs. {3}",
                         match.Tournament,
                         match.Round,
                         match.FirstPlayer.Name,
                         match.SecondPlayer.Name),
                Title = "Table Tennis Performance Report",
                Author = "Tom"
            });
            report.Sections.Add(
                 new HeaderSection()
                 {
                     Headline = "Tom's Table Tennis Performance Report",
                     Round = match.Round,
                     Tournament = match.Tournament,
                     Date = match.DateTime
                 });
            report.Sections.Add(new BasicInformationSection(match));
            report.Sections.Add(new RallyLengthSection(match, plotStyle));
            report.Sections.Add(new ScoringProcessSection(match, plotStyle));
            report.Sections.Add(new MatchDynamicsSection(match, plotStyle));
            var transitionSection = new TransitionsSection(match);
            report.Sections.Add(transitionSection);
            //report.Sections.Add(new TechnicalEfficiencySection(transitionSection.Transitions));
            //report.Sections.Add(new RelevanceOfStrokeSection(transitionSection.Transitions, plotStyle));

            return report;
        }
    }
}
