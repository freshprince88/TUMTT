using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TT.Models;
using TT.Models.Statistics;
using TT.Report.Plots;

namespace TT.Report.Sections
{
    public class StrokeNumberSection : IReportSection
    {
        public List<PlotModel> NumberPlots { get; internal set; }

        public StrokeNumberSection(PlotStyle plotStyle, int strokeNumber, IDictionary<string, List<Rally>> sets, Match match, object p)
        {
            NumberPlots = new List<PlotModel>();

            foreach (var set in sets.Keys)
            {
                if (sets[set].Count > 0)
                {
                    var statistics = new TechniqueStatistics(match, p, sets[set], strokeNumber);

                    PlotModel plot = plotStyle.CreatePlot();
                    plot.Title = set == "all" ? Properties.Resources.sets_all : (Properties.Resources.sets_one + " " + set);
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
            switch (technique)
            {
                case "Push": return Properties.Resources.section_technique_push;
                case "Flip": return Properties.Resources.section_technique_flip;
                case "Topspin": return Properties.Resources.section_technique_topspin;
                case "Block": return Properties.Resources.section_technique_block;
                case "Counter": return Properties.Resources.section_technique_counter;
                case "Smash": return Properties.Resources.section_technique_smash;
                case "Lob": return Properties.Resources.section_technique_lob;
                case "Chop": return Properties.Resources.section_technique_chop;
                case "Special": return Properties.Resources.section_technique_special;
                case "Miscellaneous": return Properties.Resources.section_technique_miscellaneous;
                case "N/A": return Properties.Resources.section_technique_na;
            }
            throw new ArgumentException("no technique '" + technique + "' defined");
        }

    }
}