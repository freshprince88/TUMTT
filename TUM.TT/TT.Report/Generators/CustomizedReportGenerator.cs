using OxyPlot;
using TT.Converters;
using TT.Models;
using TT.Report.Plots;
using TT.Report.Sections;
using System.Windows.Media;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace TT.Report.Generators
{
    /// <summary>
    /// Generates a customized report.
    /// </summary>
    public class CustomizedReportGenerator : IReportGenerator
    {

        public class SectionsAddedEventArgs
        {
            public Report Report { get; }

            public SectionsAddedEventArgs(Report report)
            {
                Report = report;
            }
        }

        private readonly MatchPlayerToColorConverter _matchPlayerToColorConverter;

        public bool Abort { get; set; }
        public Match Match { get; set; }
        public string CustomizationId { get; private set; }
        public EventHandler<SectionsAddedEventArgs> SectionsAdded;

        private Dictionary<string, object> _customization;
        public Dictionary<string, object> Customization {
            get
            {
                return _customization;
            }
            set
            {
                _customization = value;
                CustomizationId = (string)value["id"];
            }
        }
        
        public CustomizedReportGenerator()
        {
            this._matchPlayerToColorConverter = new MatchPlayerToColorConverter();
        }

        public void GenerateReport()
        {
            if (Match == null)
                throw new Exception("Match is null.");

            var report = GenerateReport(Match);
            if (!Abort)
            {
                SectionsAdded?.Invoke(this, new SectionsAddedEventArgs(report));
                Debug.WriteLine("Thread '{1}' done.", GetHashCode(), Thread.CurrentThread.Name);
            }
            else
                Debug.WriteLine("CustomizedReportGenerator {0}: report generation aborted! (Thread: {1})", GetHashCode(), Thread.CurrentThread.Name);
        }

        /// <summary>
        /// Generates a customized report for a match.
        /// </summary>
        /// <param name="match">The match to generate the report for.</param>
        /// <returns>The generated report.</returns>
        public Report GenerateReport(Match match)
        {
            var firstPlayerColor = _matchPlayerToColorConverter.Convert(MatchPlayer.First, typeof(Color), null, System.Globalization.CultureInfo.CurrentCulture);
            var secondPlayerColor = _matchPlayerToColorConverter.Convert(MatchPlayer.Second, typeof(Color), null, System.Globalization.CultureInfo.CurrentCulture);

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

            if (Abort) return null;

            report.Sections.Add(new MetadataSection()
            {
                Subject = $"{match.Tournament} {match.Round}, {match.FirstPlayer.Name} vs. {match.SecondPlayer.Name}",
                Title = Properties.Resources.report_title,
                Author = "TUM - Fakultät für Sport- und Gesundheitswissenschaft"
            });

            report.Sections.Add(
                 new HeaderSection()
                 {
                     Headline = Properties.Resources.report_header_headline,
                     Round = match.Round,
                     Tournament = match.Tournament,
                     Date = match.DateTime
                 });

            report.Sections.Add(new PartSection(Properties.Resources.section_part_general));

            report.Sections.Add(new BasicInformationSection(match));
            report.Sections.Add(new ScoringProcessSection(match, plotStyle));

            if (Abort) return null;

            var transitionSection = new TransitionsSection(match);

            if (((List<string>)Customization["general"]).Contains("rallylength"))
                report.Sections.Add(new RallyLengthSection(match, plotStyle));
            if (((List<string>)Customization["general"]).Contains("matchdynamics"))
                report.Sections.Add(new MatchDynamicsSection(match, plotStyle));
            if (((List<string>)Customization["general"]).Contains("transitionmatrix"))
                report.Sections.Add(transitionSection);
            if (((List<string>)Customization["general"]).Contains("techefficiency"))
                report.Sections.Add(new TechnicalEfficiencySection(transitionSection.Transitions));

            if (Abort) return null;

            foreach (var p in (List<object>)Customization["players"])
            {
                if (Abort) return null;
                if (p is Player)
                {
                    // add sections for requested players (1/2)
                    report.Sections.Add(new PartSection(Properties.Resources.section_part_player, p as Player));
                    AddStrokeSections(report, plotStyle, match, p);
                }
                else if (p is List<Player>)
                {
                    // aggregate all stats for both players
                }
            }

            return report;
        }

        private void AddStrokeSections(Report report, PlotStyle plotStyle, Match match, object player)
        {
            var statsNames = new string[] { "service_stats", "return_stats", "third_stats", "fourth_stats", "last_stats", "all_stats" };
            foreach (var n in statsNames)
            {
                var strokeStats = (List<string>)Customization[n];
                if (strokeStats.Count > 0)
                {
                    var headingNameAndNumber = GetStrokeStatsHeadingNameAndStrokeNr(n);
                    report.Sections.Add(new StrokeStatsHeadingSection((string)headingNameAndNumber[0]));

                    bool stepAround = strokeStats.Contains("steparound");
                    bool hasSideSection = false;
                    foreach (var s in strokeStats)
                    {
                        var sec = GetStrokeSection(s, plotStyle, match, player, stepAround, (int)headingNameAndNumber[1]);
                        if (sec is SideSection)
                        {
                            if (!hasSideSection)
                                report.Sections.Add(sec);
                            hasSideSection = true;
                        }
                        else
                        {
                            if (sec != null)
                                report.Sections.Add(sec);
                        }
                    }                    
                }
            }
        }

        private object[] GetStrokeStatsHeadingNameAndStrokeNr(string statsType)
        {
            switch (statsType)
            {
                case "service_stats": return new object[] { Properties.Resources.section_stroke_name_service, 1 };
                case "return_stats": return new object[] { Properties.Resources.section_stroke_name_return, 2 };
                case "third_stats": return new object[] { Properties.Resources.section_stroke_name_third, 3 };
                case "fourth_stats": return new object[] { Properties.Resources.section_stroke_name_fourth, 4 };
                case "last_stats": return new object[] { Properties.Resources.section_stroke_name_last, int.MaxValue };
                case "all_stats": return new object[] { Properties.Resources.section_stroke_name_all, -1 };
            }
            return null;
        }

        private IReportSection GetStrokeSection(string sectionName, PlotStyle plotStyle, Match match, object player, bool stepAround, int strokeNumber)
        {
            var sets = (IDictionary<string, List<Rally>>)Customization["sets"];
            switch (sectionName)
            {
                case "steparound":
                case "side": return new SideSection(plotStyle, strokeNumber, stepAround, sets, match, player);
                case "spin": return new SpinSection(plotStyle, sets, match, player);
                case "technique": return new TechniqueSection(plotStyle, strokeNumber, sets, match, player);
                case "placement": return new PlacementSection(strokeNumber, sets, match, player);
                case "table": return new LargeTableSection(strokeNumber, sets, match, player);
                case "service": return new LastStrokeServiceSection();
                case "number": return new StrokeNumberSection(plotStyle, strokeNumber, sets, match, player);
            }
            return null;
        }
    }
}
