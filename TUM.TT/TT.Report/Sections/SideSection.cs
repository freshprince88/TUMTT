using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using TT.Models;
using TT.Models.Statistics;
using TT.Report.Plots;

namespace TT.Report.Sections
{
    public class SideSection : IReportSection
    {
        public List<PlotModel> SidePlots { get; internal set; }
        public bool HasStepAround { get; private set; }

        public SideSection(PlotStyle plotStyle, int strokeNr, bool stepAround, IDictionary<string, List<Rally>> sets, Match match, object player)
        {
            SidePlots = new List<PlotModel>();
            HasStepAround = stepAround;

            bool multipleSets = sets.Count > 1;
            
            foreach (var set in sets.Keys)
            {
                if (sets[set].Count > 0)
                {
                    var statistics = new SideStatistics(match, player, strokeNr, sets[set]);
                    int dataMax = Enumerable.Max(new int[] { statistics.Backhand, statistics.BackhandStepAround, statistics.Forehand, statistics.ForehandStepAround, statistics.NotAnalysed});

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
                    linearAxis1.MajorStep = Math.Ceiling(dataMax / 4d);
                    linearAxis1.MinorStep = linearAxis1.MajorStep / 4d;
                    linearAxis1.AbsoluteMinimum = 0;
                    linearAxis1.MaximumPadding = 0.06;
                    linearAxis1.MinimumPadding = 0;
                    
                    var wonSeries = new BarSeries();
                    var lostSeries = new BarSeries();
                    wonSeries.IsStacked = true;
                    lostSeries.IsStacked = true;
                    wonSeries.StrokeThickness = 1;
                    lostSeries.StrokeThickness = 1;
                    wonSeries.Title = Properties.Resources.section_stroke_won;
                    lostSeries.Title = Properties.Resources.section_stroke_lost;
                    wonSeries.LabelPlacement = LabelPlacement.Inside;
                    lostSeries.LabelPlacement = LabelPlacement.Inside;
                    wonSeries.LabelMargin = 8;
                    lostSeries.LabelMargin = 8;
                    wonSeries.LabelFormatString = "{0}";
                    lostSeries.LabelFormatString = "{0}";

                    int categoryNr = 0;

                    if (statistics.NotAnalysed > 0)
                    {
                        categoryAxis1.Labels.Add(string.Format("{0} ({1})", Properties.Resources.stat_not_analysed, statistics.NotAnalysed));
                        wonSeries.Items.Add(new BarItem(statistics.NotAnalysedWon, categoryNr));
                        var notAnalysedLost = statistics.NotAnalysed - statistics.NotAnalysedWon;
                        if (notAnalysedLost > 0) lostSeries.Items.Add(new BarItem(notAnalysedLost, categoryNr));
                        categoryNr++;
                    }

                    if (stepAround)
                    {
                        categoryAxis1.Labels.Add(string.Format("{0} ({1})", multipleSets ? Properties.Resources.side_backhand_steparound_short : Properties.Resources.side_backhand_steparound, statistics.BackhandStepAround));
                        wonSeries.Items.Add(new BarItem(statistics.BackhandStepAroundWon, categoryNr));
                        var bhSaLost = statistics.BackhandStepAround - statistics.BackhandStepAroundWon;
                        if (bhSaLost > 0) lostSeries.Items.Add(new BarItem(bhSaLost, categoryNr));
                        categoryNr++;
                    }

                    categoryAxis1.Labels.Add(string.Format("{0} ({1})", multipleSets ? Properties.Resources.side_backhand_short : Properties.Resources.side_backhand, statistics.Backhand));
                    wonSeries.Items.Add(new BarItem(statistics.BackhandWon, categoryNr));
                    var noSpinLost = statistics.Backhand - statistics.BackhandWon;
                    if (noSpinLost > 0) lostSeries.Items.Add(new BarItem(noSpinLost, categoryNr));
                    categoryNr++;

                    if (stepAround)
                    {
                        categoryAxis1.Labels.Add(string.Format("{0} ({1})", multipleSets ? Properties.Resources.side_forehand_steparound_short : Properties.Resources.side_forehand_steparound, statistics.ForehandStepAround));
                        wonSeries.Items.Add(new BarItem(statistics.ForehandStepAroundWon, categoryNr));
                        var fhSaLost = statistics.ForehandStepAround - statistics.ForehandStepAroundWon;
                        if (fhSaLost > 0) lostSeries.Items.Add(new BarItem(fhSaLost, categoryNr));
                        categoryNr++;
                    }

                    categoryAxis1.Labels.Add(string.Format("{0} ({1})", multipleSets ? Properties.Resources.side_forehand_short : Properties.Resources.side_forehand, statistics.Forehand));
                    wonSeries.Items.Add(new BarItem(statistics.ForehandWon, categoryNr));
                    var spinUpLost = statistics.Forehand - statistics.ForehandWon;
                    if (spinUpLost > 0) lostSeries.Items.Add(new BarItem(spinUpLost, categoryNr));
                    categoryNr++;

                    plot.Series.Add(wonSeries);
                    plot.Series.Add(lostSeries);

                    plot.Axes.Add(categoryAxis1);
                    plot.Axes.Add(linearAxis1);

                    SidePlots.Add(plot);
                }
            }
        }
    }
}