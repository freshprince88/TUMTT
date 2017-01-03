using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using TT.Models;
using TT.Models.Statistics;
using TT.Report.Plots;

namespace TT.Report.Sections
{
    public class SideSection : IReportSection
    {
        private Match match;
        private object player;

        public List<PlotModel> SidePlots { get; internal set; }

        public SideSection(PlotStyle plotStyle, int strokeNr, IDictionary<string, List<Rally>> sets, Match match, object player)
        {
            this.SidePlots = new List<PlotModel>();
            this.match = match;
            this.player = player;

            foreach (var set in sets.Keys)
            {
                if (sets[set].Count > 0)
                {
                    var statistics = new SideStatistics(match, player, strokeNr, sets[set]);

                    PlotModel plot = plotStyle.CreatePlot();
                    plot.Title = set == "all" ? Properties.Resources.sets_all : (Properties.Resources.sets_one + " " + set);
                    plot.TitleFontSize = 16;
                    plot.LegendPlacement = LegendPlacement.Outside;
                    plot.LegendPosition = LegendPosition.RightMiddle;
                    dynamic series = new PieSeries()
                    {
                        StrokeThickness = 2.0,
                        InsideLabelPosition = 0.7,
                        AngleSpan = 360,
                        StartAngle = 0,
                        FontSize = sets.Count > 1 ? 12 : 16,
                        InsideLabelFormat = "{1} ({0})",
                    };

                    if (statistics.Forehand > 0)
                        series.Slices.Add(new PieSlice("Forehand", statistics.Forehand) { IsExploded = true });
                    if (statistics.Backhand > 0)
                        series.Slices.Add(new PieSlice("Backhand", statistics.Backhand) { IsExploded = false });
                    if (statistics.NotAnalysed > 0)
                        series.Slices.Add(new PieSlice("N/A", statistics.NotAnalysed));
                    plot.Series.Add(series);

                    SidePlots.Add(plot);
                }
            }
        }
    }
}