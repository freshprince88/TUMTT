//-----------------------------------------------------------------------
// <copyright file="ScoringProcessSection.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Report.Sections
{
    using System.Linq;
    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using TT.Lib.Models;
    using TT.Report.Plots;

    /// <summary>
    /// A section describing the scoring process.
    /// </summary>
    public class ScoringProcessSection : IReportSection
    {
        /// <summary>
        /// The match.
        /// </summary>
        private Match match;

        /// <summary>
        /// The style for the plot.
        /// </summary>
        private PlotStyle style;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoringProcessSection"/> class.
        /// </summary>
        /// <param name="match">The match</param>
        /// <param name="style">The style to use for the plot.</param>
        public ScoringProcessSection(Match match, PlotStyle style)
        {
            this.match = match;
            this.style = style;
            this.Plot = this.PlotScoringProcess();
        }

        /// <summary>
        /// Gets a plot of the scoring process.
        /// </summary>
        public PlotModel Plot { get; private set; }

        /// <summary>
        /// Plots the scoring process of the match.
        /// </summary>
        /// <returns>The plot.</returns>
        private PlotModel PlotScoringProcess()
        {
            var plot = this.style.CreatePlot();
            plot.PlotMargins = new OxyThickness(30, double.NaN, 20, double.NaN);

            var rallyNoAxis = new LinearAxis()
            {
                Minimum = 0,
                Maximum = this.match.DefaultPlaylist.FinishedRallies.Count() + 0.5,
                Position = AxisPosition.Bottom,
                MinorStep = 5,
                MajorStep = 5,
                MajorGridlineColor = this.style.GridlineColor,
                MinorGridlineColor = this.style.GridlineColor,
            };
            var scoreAxis = new LinearAxis()
            {
                Minimum = 0,
                MaximumPadding = 0.1,
                Position = AxisPosition.Left,
                MajorStep = 1,
                MinorStep = 1,
                MajorGridlineColor = this.style.GridlineColor,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineThickness = 0.5,
                MinorGridlineColor = this.style.GridlineColor,
                MinorGridlineStyle = LineStyle.Solid,
                MinorGridlineThickness = 0.5,
            };
            plot.Axes.Add(rallyNoAxis);
            plot.Axes.Add(scoreAxis);

            var firstSeries = new LineSeries()
            {
                Color = this.style.FirstPlayerColor,
                Title = this.match.FirstPlayer.Name,
            };
            var secondSeries = new LineSeries()
            {
                Color = this.style.SecondPlayerColor,
                Title = this.match.SecondPlayer.Name,
            };
            plot.Series.Add(firstSeries);
            plot.Series.Add(secondSeries);

            // Mark the standard winning score
            plot.Annotations.Add(
                new LineAnnotation()
                {
                    Y = 11,
                    Color = OxyColors.DarkGray,
                    Type = LineAnnotationType.Horizontal,
                    LineStyle = LineStyle.Solid,
                    Layer = AnnotationLayer.BelowSeries,
                });

            var rallyNo = 0;
            foreach (var rally in this.match.DefaultPlaylist.FinishedRallies)
            {
                firstSeries.Points.Add(new DataPoint(rallyNo, rally.CurrentRallyScore.First));
                secondSeries.Points.Add(new DataPoint(rallyNo, rally.CurrentRallyScore.Second));

                if (rally.IsEndOfSet)
                {
                    // Add two additional points to mark the final rally score.
                    // We increment the rally number explicitly here to make this
                    // point overlap with the start of the new rally.
                    firstSeries.Points.Add(new DataPoint(rallyNo + 1, rally.FinalRallyScore.First));
                    secondSeries.Points.Add(new DataPoint(rallyNo + 1, rally.FinalRallyScore.Second));

                    // Mark the end of this set, by interrupting the line and drawing
                    // a vertical marker
                    firstSeries.Points.Add(new DataPoint(double.NaN, double.NaN));
                    secondSeries.Points.Add(new DataPoint(double.NaN, double.NaN));
                    plot.Annotations.Add(
                        new LineAnnotation()
                        {
                            X = rallyNo + 1,
                            Color = OxyColors.DarkGray,
                            Type = LineAnnotationType.Vertical,
                            FontSize = 8,
                            TextOrientation = AnnotationTextOrientation.Horizontal,
                            TextPadding = 8,
                            LineStyle = LineStyle.Dot,
                        });
                }

                rallyNo++;
            }

            return plot;
        }
    }
}
