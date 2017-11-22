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
        protected sealed override string SectionName => "Stroke Number section";

        public List<PlotModel> NumberPlots { get; }
        private readonly IDictionary<Stroke.TechniqueBasic, object[]> _techniqueDisplayables = new Dictionary<Stroke.TechniqueBasic, object[]>() {
            { Stroke.TechniqueBasic.Push, new object []{ Properties.Resources.section_technique_push, OxyColor.Parse("#000000") }},
            { Stroke.TechniqueBasic.Flip, new object []{ Properties.Resources.section_technique_flip, OxyColor.Parse("#000000") }},
            { Stroke.TechniqueBasic.Topspin, new object []{ Properties.Resources.section_technique_topspin, OxyColor.Parse("#000000") }},
            { Stroke.TechniqueBasic.Block, new object []{ Properties.Resources.section_technique_block, OxyColor.Parse("#000000") }},
            { Stroke.TechniqueBasic.Counter, new object []{ Properties.Resources.section_technique_counter, OxyColor.Parse("#000000") }},
            { Stroke.TechniqueBasic.Smash, new object []{ Properties.Resources.section_technique_smash, OxyColor.Parse("#000000") }},
            { Stroke.TechniqueBasic.Lob, new object []{ Properties.Resources.section_technique_lob, OxyColor.Parse("#000000") }},
            { Stroke.TechniqueBasic.Chop, new object []{ Properties.Resources.section_technique_chop, OxyColor.Parse("#000000") }},
            { Stroke.TechniqueBasic.Special, new object []{ Properties.Resources.section_technique_special, OxyColor.Parse("#000000") }},
            { Stroke.TechniqueBasic.Miscellaneous, new object []{  Properties.Resources.section_technique_miscellaneous, OxyColor.Parse("#000000") }},
        };

        public StrokeNumberSection(PlotStyle plotStyle, int strokeNumber, IDictionary<string, List<Models.Rally>> sets, Models.Match match, object p)
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

                    Debug.WriteLine("{2} for stroke {0} of set {1} ready.", GetStrokeNumberString(strokeNumber), set, SectionName);
                }
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
                LabelFormatString = "{0}",
                FillColor = GetSeriesColorForTechnique(technique)
            };
        }

        private OxyColor GetSeriesColorForTechnique(string technique)
        {
            if (technique.Equals("N/A"))
                return OxyColors.OrangeRed;
            var col = (OxyColor)_techniqueDisplayables[(Stroke.TechniqueBasic)Enum.Parse(typeof(Stroke.TechniqueBasic), technique)][1];
            if (!col.Equals(OxyColors.Black))
                return col;
            var ran = new Random(technique.GetHashCode());
            return OxyColor.FromRgb((byte) ran.Next(255), (byte) ran.Next(255), (byte) ran.Next(255));
        }

        private string GetSeriesTitleForTechnique(string technique)
        {
            if (technique.Equals("N/A"))
                return Properties.Resources.section_technique_na;
            return (string)_techniqueDisplayables[(Stroke.TechniqueBasic)Enum.Parse(typeof(Stroke.TechniqueBasic), technique)][0];
        }

    }
}