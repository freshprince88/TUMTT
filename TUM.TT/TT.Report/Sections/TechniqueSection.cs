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
    public class TechniqueSection : ExistingStatisticsSection
    {
        public TechniqueSection(PlotStyle plotStyle, int strokeNumber, IDictionary<string, List<Rally>> sets, Match match, object p)
        {
            Debug.WriteLine("New Technique section");

            if (strokeNumber == 1)
                base.GetImageBitmapFrames(strokeNumber, sets, match, p, typeof(ServiceTechniqueStatisticsGridView));
            else
            {
                base.GetImageBitmapFrames(strokeNumber, sets, match, p, typeof(TechniqueStatisticsGridView));
                Debug.WriteLine("Got TechniqueGridView image");

                foreach (var set in sets.Keys)
                {
                    if (sets[set].Count > 0)
                    {
                        var statistics = new TechniqueStatistics(match, p, sets[set], strokeNumber);
                        Debug.WriteLine("Got TechniqueStatistics (set={0})", args: set);

                        PlotModel plot = plotStyle.CreatePlot();
                        plot.LegendOrientation = LegendOrientation.Horizontal;
                        plot.LegendPlacement = LegendPlacement.Outside;
                        plot.LegendPosition = LegendPosition.BottomCenter;
                        plot.PlotAreaBorderThickness = new OxyThickness(1, 0, 0, 1);

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
                        barSeries1.LabelMargin = 4;
                        barSeries2.LabelMargin = 4;
                        barSeries1.LabelFormatString = "{0}";
                        barSeries2.LabelFormatString = "{0}";

                        int categoriesCount = 0;
                        foreach (var tec in statistics.GetType().GetProperties())
                        {
                            Models.Util.Enums.Stroke.TechniqueBasic tb;
                            if (Enum.TryParse(tec.Name, out tb))
                                categoriesCount += (int)tec.GetValue(statistics) > 0 ? 1 : 0;
                        }
                        bool useShortCatNames = categoriesCount > 4;

                        int categoryNr = 0;

                        if (statistics.Push > 0)
                        {
                            categoryAxis.Labels.Add(string.Format("{0} ({1})", useShortCatNames ? Properties.Resources.technique_push_short : Properties.Resources.section_technique_push, statistics.Push));
                            barSeries1.Items.Add(new ColumnItem(statistics.PushWon, categoryNr));
                            var pushLost = statistics.Push - statistics.PushWon;
                            if (pushLost > 0) barSeries2.Items.Add(new ColumnItem(pushLost, categoryNr));
                            categoryNr++;
                        }

                        if (statistics.Flip > 0)
                        {
                            categoryAxis.Labels.Add(string.Format("{0} ({1})", useShortCatNames ? Properties.Resources.technique_flip_short : Properties.Resources.section_technique_flip, statistics.Flip));
                            barSeries1.Items.Add(new ColumnItem(statistics.FlipWon, categoryNr));
                            var flipLost = statistics.Flip - statistics.FlipWon;
                            if (flipLost > 0) barSeries2.Items.Add(new ColumnItem(flipLost, categoryNr));
                            categoryNr++;
                        }

                        if (statistics.Topspin > 0)
                        {
                            categoryAxis.Labels.Add(string.Format("{0} ({1})", useShortCatNames ? Properties.Resources.technique_topspin_short : Properties.Resources.section_technique_topspin, statistics.Topspin));
                            barSeries1.Items.Add(new ColumnItem(statistics.TopspinWon, categoryNr));
                            var topspinLost = statistics.Topspin - statistics.TopspinWon;
                            if (topspinLost > 0) barSeries2.Items.Add(new ColumnItem(topspinLost, categoryNr));
                            categoryNr++;
                        }

                        if (statistics.Block > 0)
                        {
                            categoryAxis.Labels.Add(string.Format("{0} ({1})", useShortCatNames ? Properties.Resources.technique_block_short : Properties.Resources.section_technique_block, statistics.Block));
                            barSeries1.Items.Add(new ColumnItem(statistics.BlockWon, categoryNr));
                            var blockLost = statistics.Block - statistics.BlockWon;
                            if (blockLost > 0) barSeries2.Items.Add(new ColumnItem(blockLost, categoryNr));
                            categoryNr++;
                        }

                        if (statistics.Counter > 0)
                        {
                            categoryAxis.Labels.Add(string.Format("{0} ({1})", useShortCatNames ? Properties.Resources.technique_counter_short : Properties.Resources.section_technique_counter, statistics.Counter));
                            barSeries1.Items.Add(new ColumnItem(statistics.CounterWon, categoryNr));
                            var counterLost = statistics.Counter - statistics.CounterWon;
                            if (counterLost > 0) barSeries2.Items.Add(new ColumnItem(counterLost, categoryNr));
                            categoryNr++;
                        }

                        if (statistics.Smash > 0)
                        {
                            categoryAxis.Labels.Add(string.Format("{0} ({1})", useShortCatNames ? Properties.Resources.technique_smash_short : Properties.Resources.section_technique_smash, statistics.Smash));
                            barSeries1.Items.Add(new ColumnItem(statistics.SmashWon, categoryNr));
                            var smashLost = statistics.Smash - statistics.SmashWon;
                            if (smashLost > 0) barSeries2.Items.Add(new ColumnItem(smashLost, categoryNr));
                            categoryNr++;
                        }

                        if (statistics.Lob > 0)
                        {
                            categoryAxis.Labels.Add(string.Format("{0} ({1})", useShortCatNames ? Properties.Resources.technique_lob_short : Properties.Resources.section_technique_lob, statistics.Lob));
                            barSeries1.Items.Add(new ColumnItem(statistics.LobWon, categoryNr));
                            var lobLost = statistics.Lob - statistics.LobWon;
                            if (lobLost > 0) barSeries2.Items.Add(new ColumnItem(lobLost, categoryNr));
                            categoryNr++;
                        }

                        if (statistics.Chop > 0)
                        {
                            categoryAxis.Labels.Add(string.Format("{0} ({1})", useShortCatNames ? Properties.Resources.technique_chop_short : Properties.Resources.section_technique_chop, statistics.Chop));
                            barSeries1.Items.Add(new ColumnItem(statistics.ChopWon, categoryNr));
                            var chopLost = statistics.Chop - statistics.ChopWon;
                            if (chopLost > 0) barSeries2.Items.Add(new ColumnItem(chopLost, categoryNr));
                            categoryNr++;
                        }

                        if (statistics.Special > 0)
                        {
                            categoryAxis.Labels.Add(string.Format("{0} ({1})", useShortCatNames ? Properties.Resources.technique_special_short : Properties.Resources.section_technique_special, statistics.Special));
                            barSeries1.Items.Add(new ColumnItem(statistics.SpecialWon, categoryNr));
                            var specialLost = statistics.Special - statistics.SpecialWon;
                            if (specialLost > 0) barSeries2.Items.Add(new ColumnItem(specialLost, categoryNr));
                            categoryNr++;
                        }

                        if (statistics.Miscellaneous > 0)
                        {
                            categoryAxis.Labels.Add(string.Format("{0} ({1})", useShortCatNames ? Properties.Resources.technique_miscellaneous_short : Properties.Resources.section_technique_miscellaneous, statistics.Miscellaneous));
                            barSeries1.Items.Add(new ColumnItem(statistics.MiscellaneousWon, categoryNr));
                            var miscellaneousLost = statistics.Miscellaneous - statistics.MiscellaneousWon;
                            if (miscellaneousLost > 0) barSeries2.Items.Add(new ColumnItem(miscellaneousLost, categoryNr));
                            categoryNr++;
                        }

                        plot.Series.Add(barSeries1);
                        plot.Series.Add(barSeries2);

                        plot.Axes.Add(categoryAxis);
                        plot.Axes.Add(linearAxis);

                        var setTitle = GetSetTitleString(set);
                        var sectionImages = new List<object>() { plot };
                        if (ExistingStatisticsImageBitmapFrames.ContainsKey(setTitle))
                            sectionImages.Add(ExistingStatisticsImageBitmapFrames[setTitle]);
                        else
                            sectionImages.Add(null);
                        ExistingStatisticsImageBitmapFrames[setTitle] = sectionImages;

                        Debug.WriteLine("Got Technique plot (set={0})", args: setTitle);
                    }
                    Debug.WriteLine("Technique section for stroke {0} of set {1} ready.", strokeNumber, set);
                }
            }
        }
    }
}