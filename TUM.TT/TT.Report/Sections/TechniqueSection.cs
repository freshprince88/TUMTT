using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TT.Models;
using TT.Models.Statistics;
using TT.Report.Plots;
using TT.Report.Views;

namespace TT.Report.Sections
{
    public class TechniqueSection : ExistingStatisticsSection, IReportSection
    {
        public TechniqueSection(PlotStyle plotStyle, int strokeNumber, IDictionary<string, List<Rally>> sets, Match match, object p)
        {
            if (strokeNumber == 1)
                base.GetImageBitmapFrames(strokeNumber, sets, match, p, new ServiceTechniqueGridView());
            else
            {
                // Dispatcher is needed here because TechniqueGridView modifies the Grid (Column/Rows) based on ordering of counts
                // TODO fix this causing the (real) ServiceStatisticsView not being able to load due to thread object ownership
                //Application.Current.Dispatcher.Invoke(() =>
                Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    base.GetImageBitmapFrames(strokeNumber, sets, match, p, new TechniqueGridView());

                    foreach (var set in sets.Keys)
                    {
                        if (sets[set].Count > 0)
                        {
                            var statistics = new TechniqueStatistics(match, p, sets[set], strokeNumber);

                            PlotModel plot = plotStyle.CreatePlot();
                            plot.LegendOrientation = LegendOrientation.Horizontal;
                            plot.LegendPlacement = LegendPlacement.Outside;
                            plot.LegendPosition = LegendPosition.BottomCenter;

                            var categoryAxis = new CategoryAxis();
                            categoryAxis.Position = AxisPosition.Bottom;
                            categoryAxis.MinorStep = 1;

                            var linearAxis = new LinearAxis();
                            linearAxis.Position = AxisPosition.Left;
                            linearAxis.MinorStep = 1;
                            linearAxis.MajorStep = 4;
                            linearAxis.AbsoluteMinimum = 0;
                            linearAxis.MaximumPadding = 0.06;
                            linearAxis.MinimumPadding = 0;

                            var barSeries1 = new ColumnSeries();
                            var barSeries2 = new ColumnSeries();
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

                            categoryAxis.Labels.Add(string.Format("{0} ({1})", Properties.Resources.section_technique_topspin, statistics.Topspin));
                            barSeries1.Items.Add(new ColumnItem(statistics.TopspinWon, categoryNr));
                            var topspinLost = statistics.Topspin - statistics.TopspinWon;
                            if (topspinLost > 0) barSeries2.Items.Add(new ColumnItem(topspinLost, categoryNr));
                            categoryNr++;

                            categoryAxis.Labels.Add(string.Format("{0} ({1})", Properties.Resources.section_technique_push, statistics.Push));
                            barSeries1.Items.Add(new ColumnItem(statistics.PushWon, categoryNr));
                            var pushLost = statistics.Push - statistics.PushWon;
                            if (pushLost > 0) barSeries2.Items.Add(new ColumnItem(pushLost, categoryNr));
                            categoryNr++;

                            categoryAxis.Labels.Add(string.Format("{0} ({1})", Properties.Resources.section_technique_flip, statistics.Flip));
                            barSeries1.Items.Add(new ColumnItem(statistics.FlipWon, categoryNr));
                            var flipLost = statistics.Flip - statistics.FlipWon;
                            if (flipLost > 0) barSeries2.Items.Add(new ColumnItem(flipLost, categoryNr));
                            categoryNr++;

                            categoryAxis.Labels.Add(string.Format("{0} ({1})", Properties.Resources.section_technique_smash, statistics.Smash));
                            barSeries1.Items.Add(new ColumnItem(statistics.SmashWon, categoryNr));
                            var smashLost = statistics.Smash - statistics.SmashWon;
                            if (smashLost > 0) barSeries2.Items.Add(new ColumnItem(smashLost, categoryNr));
                            categoryNr++;

                            //categoryAxis1.Labels.Add(string.Format("{0}{2}({1})", Properties.Resources.section_technique_misc, statistics.NotAnalysed, Environment.NewLine));
                            //barSeries1.Items.Add(new BarItem(statistics.NotAnalysedWon, categoryNr));
                            //var notAnalysedLost = statistics.NotAnalysed - statistics.NotAnalysedWon;
                            //if (notAnalysedLost > 0) barSeries2.Items.Add(new BarItem(notAnalysedLost, categoryNr));
                            //categoryNr++;

                            plot.Series.Add(barSeries1);
                            plot.Series.Add(barSeries2);

                            plot.Axes.Add(categoryAxis);
                            plot.Axes.Add(linearAxis);

                            ExistingStatisticsImageBitmapFrames[set] = new List<object>() { plot, ExistingStatisticsImageBitmapFrames[set] };
                        }
                        Debug.WriteLine("Technique section for stroke {0} of set {1} ready.", strokeNumber, set);
                    }
                });
            }
        }
    }
}