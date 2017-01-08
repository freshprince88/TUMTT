using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using TT.Models;
using TT.Models.Statistics;
using TT.Report.Plots;

namespace TT.Report.Sections
{
    public class SideSection : IReportSection
    {
        public List<PlotModel> SidePlots { get; internal set; }

        public SideSection(PlotStyle plotStyle, int strokeNr, IDictionary<string, List<Rally>> sets, Match match, object player)
        {
            SidePlots = new List<PlotModel>();
            
            foreach (var set in sets.Keys)
            {
                if (sets[set].Count > 0)
                {
                    var statistics = new SideStatistics(match, player, strokeNr, sets[set]);

                    PlotModel plot = plotStyle.CreatePlot();
                    plot.LegendOrientation = LegendOrientation.Horizontal;
                    plot.LegendPlacement = LegendPlacement.Outside;
                    plot.LegendPosition = LegendPosition.BottomCenter;
                    plot.Title = set == "all" ? Properties.Resources.sets_all : (Properties.Resources.sets_one + " " + set);
                    plot.TitleFontSize = 16;
                    plot.PlotAreaBorderThickness = new OxyThickness(1, 0, 0, 1);

                    var categoryAxis1 = new CategoryAxis();
                    categoryAxis1.Position = AxisPosition.Left;
                    categoryAxis1.MinorStep = 1;

                    var linearAxis1 = new LinearAxis();
                    linearAxis1.Position = AxisPosition.Bottom;
                    linearAxis1.MinorStep = 1;
                    linearAxis1.MajorStep = 4;
                    linearAxis1.AbsoluteMinimum = 0;
                    linearAxis1.MaximumPadding = 0.06;
                    linearAxis1.MinimumPadding = 0;

                    var barSeries1 = new BarSeries();
                    var barSeries2 = new BarSeries();
                    barSeries1.IsStacked = true;
                    barSeries2.IsStacked = true;
                    barSeries1.StrokeThickness = 1;
                    barSeries2.StrokeThickness = 1;
                    barSeries1.Title = Properties.Resources.section_stroke_won;
                    barSeries2.Title = Properties.Resources.section_stroke_lost;
                    barSeries1.LabelPlacement = LabelPlacement.Inside;
                    barSeries2.LabelPlacement = LabelPlacement.Inside;
                    barSeries1.LabelMargin = 8;
                    barSeries2.LabelMargin = 8;
                    barSeries1.LabelFormatString = "{0}";
                    barSeries2.LabelFormatString = "{0}";

                    int categoryNr = 0;

                    categoryAxis1.Labels.Add(string.Format("{0} ({1})", Properties.Resources.side_forehand, statistics.Forehand));
                    barSeries1.Items.Add(new BarItem(statistics.ForehandWon, categoryNr));
                    var spinUpLost = statistics.Forehand - statistics.ForehandWon;
                    if (spinUpLost > 0) barSeries2.Items.Add(new BarItem(spinUpLost, categoryNr));
                    categoryNr++;

                    categoryAxis1.Labels.Add(string.Format("{0} ({1})", Properties.Resources.side_backhand, statistics.Backhand));
                    barSeries1.Items.Add(new BarItem(statistics.BackhandWon, categoryNr));
                    var noSpinLost = statistics.Backhand - statistics.BackhandWon;
                    if (noSpinLost > 0) barSeries2.Items.Add(new BarItem(noSpinLost, categoryNr));
                    categoryNr++;

                    if (statistics.NotAnalysed > 0)
                    {
                        categoryAxis1.Labels.Add(string.Format("{0} ({1})", Properties.Resources.section_spin_hidden, statistics.NotAnalysed));
                        barSeries1.Items.Add(new BarItem(statistics.NotAnalysedWon, categoryNr));
                        var notAnalysedLost = statistics.NotAnalysed - statistics.NotAnalysedWon;
                        if (notAnalysedLost > 0) barSeries2.Items.Add(new BarItem(notAnalysedLost, categoryNr));
                        categoryNr++;
                    }

                    plot.Series.Add(barSeries1);
                    plot.Series.Add(barSeries2);

                    plot.Axes.Add(categoryAxis1);
                    plot.Axes.Add(linearAxis1);

                    SidePlots.Add(plot);
                }
            }
        }
    }
}