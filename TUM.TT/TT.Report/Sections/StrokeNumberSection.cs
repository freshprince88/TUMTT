using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TT.Models.Util.Enums;
using TT.Models.Statistics;
using TT.Report.Plots;

namespace TT.Report.Sections
{
    public class StrokeNumberSection : BaseSection
    {
        public List<PlotModel> NumberPlots { get; internal set; }
        private IDictionary<Stroke.TechniqueBasic, string> techniqueNames = new Dictionary<Stroke.TechniqueBasic, string>() {
            { Stroke.TechniqueBasic.Push, Properties.Resources.section_technique_push },
            { Stroke.TechniqueBasic.Flip, Properties.Resources.section_technique_flip },
            { Stroke.TechniqueBasic.Topspin, Properties.Resources.section_technique_topspin },
            { Stroke.TechniqueBasic.Block, Properties.Resources.section_technique_block },
            { Stroke.TechniqueBasic.Counter, Properties.Resources.section_technique_counter },
            { Stroke.TechniqueBasic.Smash, Properties.Resources.section_technique_smash },
            { Stroke.TechniqueBasic.Lob, Properties.Resources.section_technique_lob },
            { Stroke.TechniqueBasic.Chop, Properties.Resources.section_technique_chop },
            { Stroke.TechniqueBasic.Special, Properties.Resources.section_technique_special },
            { Stroke.TechniqueBasic.Miscellaneous, Properties.Resources.section_technique_miscellaneous },
        };

        public StrokeNumberSection(PlotStyle plotStyle, int strokeNumber, IDictionary<string, List<TT.Models.Rally>> sets, TT.Models.Match match, object p)
        {
            NumberPlots = new List<PlotModel>();

            foreach (var set in sets.Keys)
            {
                if (sets[set].Count > 0)
                {
                    var statistics = new TechniqueStatistics(match, p, sets[set], strokeNumber);

                    PlotModel plot = plotStyle.CreatePlot();
                    plot.Title = GetSetTitleString(set);
                    plot.TitleFontSize = 16;
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

                    var techniqueToSeries = new Dictionary<string, ColumnSeries>();
                    
                    int index = 0;
                    foreach (var number in statistics.NumberToTechniqueCountDict.Keys)
                    {
                        var numberCount = 0;
                        foreach (var count in statistics.NumberToTechniqueCountDict[number].Values)
                            numberCount += count;

                        if (numberCount > 0)
                        {
                            categoryAxis.Labels.Add(string.Format("{0} ({1})", number, numberCount));

                            foreach (var technique in statistics.NumberToTechniqueCountDict[number].Keys)
                            {
                                var techniqueCount = statistics.NumberToTechniqueCountDict[number][technique];
                                if (techniqueCount > 0)
                                {
                                    ColumnSeries series;
                                    if (techniqueToSeries.ContainsKey(technique))
                                        series = techniqueToSeries[technique];
                                    else
                                    {
                                        series = GetNewSeries(technique);
                                        techniqueToSeries[technique] = series;
                                    }
                                    series.Items.Add(new ColumnItem(techniqueCount, categoryIndex: index));
                                }
                            }
                            index++;
                        }
                    }

                    foreach (var series in techniqueToSeries.Values)
                        plot.Series.Add(series);

                    plot.Axes.Add(categoryAxis);
                    plot.Axes.Add(linearAxis);

                    NumberPlots.Add(plot);                    
                }
                Debug.WriteLine("Number section for stroke {0} of set {1} ready.", strokeNumber, set);
            }
        }

        private ColumnSeries GetNewSeries(string technique)
        {
            return new ColumnSeries()
            {
                IsStacked = true,
                StrokeThickness = 1,
                Title = GetSeriesTitleForTechnique(technique),
                LabelPlacement = LabelPlacement.Inside,
                LabelMargin = 4,
                LabelFormatString = "{0}"
            };
        }

        private string GetSeriesTitleForTechnique(string technique)
        {
            if (technique.Equals("N/A"))
                return Properties.Resources.section_technique_na;
            return techniqueNames[(Stroke.TechniqueBasic)Enum.Parse(typeof(Stroke.TechniqueBasic), technique)];
        }

    }
}