using OxyPlot;
using TT.Converters;
using TT.Models;
using TT.Report.Plots;
using TT.Report.Sections;
using System.Windows.Media;
using System.Collections.Generic;

namespace TT.Report.Generators
{
    /// <summary>
    /// Generates a customized report.
    /// </summary>
    public class CustomizedReportGenerator : IReportGenerator
    {

        private MatchPlayerToColorConverter matchPlayerToColorConverter;
        public Dictionary<string, object> Customization { get; set; }

        public CustomizedReportGenerator()
        {
            this.matchPlayerToColorConverter = new MatchPlayerToColorConverter();
        }

        /// <summary>
        /// Generates a customized report for a match.
        /// </summary>
        /// <param name="match">The match to generate the report for.</param>
        /// <returns>The generated report.</returns>
        public Report GenerateReport(Match match)
        {
            var firstPlayerColor = matchPlayerToColorConverter.Convert(MatchPlayer.First, typeof(Color), null, System.Globalization.CultureInfo.CurrentCulture);
            var secondPlayerColor = matchPlayerToColorConverter.Convert(MatchPlayer.Second, typeof(Color), null, System.Globalization.CultureInfo.CurrentCulture);

            var plotStyle = new PlotStyle()
            {
                TextFont = "Calibri",
                TextSize = 10,
                TextColor = OxyColors.Black,
                BorderColor = OxyColors.DarkGray,
                GridlineColor = OxyColors.LightGray,
                FirstPlayerColor = OxyColor.Parse(firstPlayerColor.ToString()),
                SecondPlayerColor = OxyColor.Parse(secondPlayerColor.ToString())
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
                Author = "TUM - Fakultät für Sport- und Gesundheitswissenschaft"
            });

            report.Sections.Add(
                 new HeaderSection()
                 {
                     Headline = "Table Tennis Performance Report",
                     Round = match.Round,
                     Tournament = match.Tournament,
                     Date = match.DateTime
                 });

            report.Sections.Add(new PartSection(Properties.Resources.section_part_general));

            report.Sections.Add(new BasicInformationSection(match));
            report.Sections.Add(new ScoringProcessSection(match, plotStyle));

            var transitionSection = new TransitionsSection(match);

            if (((List<string>)Customization["general"]).Contains("rallylength"))
                report.Sections.Add(new RallyLengthSection(match, plotStyle));
            if (((List<string>)Customization["general"]).Contains("matchdynamics"))
                report.Sections.Add(new MatchDynamicsSection(match, plotStyle));
            if (((List<string>)Customization["general"]).Contains("transitionmatrix"))
                report.Sections.Add(transitionSection);
            if (((List<string>)Customization["general"]).Contains("techefficiency"))
                report.Sections.Add(new TechnicalEfficiencySection(transitionSection.Transitions));

            foreach (var p in (List<object>)Customization["players"])
            {
                if (p is Player)
                {
                    // add sections for requested players (1/2)
                    report.Sections.Add(new PartSection(Properties.Resources.section_part_player, p as Player));
                    AddStrokeSections(report);
                }
                else if (p is List<Player>)
                {
                    // aggregate all stats for both players
                }
            }

            return report;
        }

        private void AddStrokeSections(Report report)
        {
            var statsNames = new string[] { "service_stats", "return_stats", "third_stats", "fourth_stats", "last_stats", "all_stats" };
            foreach (var n in statsNames)
            {
                var strokeStats = (List<string>)Customization[n];
                if (strokeStats.Count > 0)
                {
                    report.Sections.Add(new StrokeStatsHeadingSection(GetStrokeStatsHeadingSectionName(n)));
                    foreach (var s in strokeStats)
                    {
                        report.Sections.Add(GetStrokeSection(s));
                    }

                }
            }
        }

        private string GetStrokeStatsHeadingSectionName(string statsType)
        {
            switch (statsType)
            {
                case "service_stats": return Properties.Resources.section_stroke_name_service;
                case "return_stats": return Properties.Resources.section_stroke_name_return;
                case "third_stats": return Properties.Resources.section_stroke_name_third;
                case "fourth_stats": return Properties.Resources.section_stroke_name_fourth;
                case "last_stats": return Properties.Resources.section_stroke_name_last;
                case "all_stats": return Properties.Resources.section_stroke_name_all;
            }
            return null;
        }

        private IReportSection GetStrokeSection(string sectionName)
        {
            switch (sectionName)
            {
                case "side": return new SideSection();
                case "steparound": return new StepAroundSection();
                case "spin": return new SpinSection();
                case "technique": return new TechniqueSection();
                case "placement": return new PlacementSection();
                case "table": return new LargeTableSection();
                case "service": return new LastStrokeServiceSection();
                case "number": return new LastStrokeNumberSection();
            }
            return null;
        }
    }
}
