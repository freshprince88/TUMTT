//-----------------------------------------------------------------------
// <copyright file="RelevanceOfStrokeSection.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Report.Sections
{
    using System.Linq;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using TT.Lib.Models.Statistics;
    using TT.Report.Plots;

    /// <summary>
    /// A section for the relevance of stroke, computed by Markov simulation.
    /// </summary>
    public class RelevanceOfStrokeSection : IReportSection
    {
        /// <summary>
        /// The style for our plots.
        /// </summary>
        private PlotStyle style;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelevanceOfStrokeSection"/> class.
        /// </summary>
        /// <param name="transitions">The transitions</param>
        /// <param name="style">The style for the plots in this section</param>
        public RelevanceOfStrokeSection(Transitions transitions, PlotStyle style)
        {
            this.RelevanceOfStroke = new RelevanceOfStroke(transitions);

            this.style = style;

            this.PlotByStriker = this.PlotRelevanceOfStroke();
        }

        /// <summary>
        /// Gets the results of the match simulation.
        /// </summary>
        public RelevanceOfStroke RelevanceOfStroke { get; private set; }

        /// <summary>
        /// Gets a plot of the stroke relevance.
        /// </summary>
        public PlotModel PlotByStriker { get; private set; }

        /// <summary>
        /// Plots the relevance of stroke.
        /// </summary>
        /// <returns>The relevance of stroke.</returns>
        private PlotModel PlotRelevanceOfStroke()
        {
            var relevance = this.RelevanceOfStroke.ByStriker;
            var match = this.RelevanceOfStroke.Match;

            var plot = this.style.CreatePlot();
            plot.PlotMargins = new OxyThickness(30, double.NaN, 20, double.NaN);

            var strokeAxis = new CategoryAxis()
            {
                MajorStep = 1,
                IsTickCentered = true,
                TickStyle = TickStyle.None,
                GapWidth = 0.5,
            };
            plot.Axes.Add(strokeAxis);

            // Add labels for all strokes.  Ignore zero strokes
            for (int m = 2; m < relevance.RowCount; ++m)
            {
                var stroke = m < relevance.RowCount - 2 ?
                    (m / 2).ToString() : string.Format(">{0}", (m / 2) - 1);
                var label = string.Format(
                    "{0}{1}",
                    stroke,
                    (m % 2) == 0 ? "Win" : "Err");
                strokeAxis.Labels.Add(label);
            }

            for (int n = 0; n < relevance.ColumnCount; ++n)
            {
                var axis = new CategoryAxis()
                {
                    TextColor = n == 0 ? this.style.FirstPlayerColor :
                        this.style.SecondPlayerColor,
                    TickStyle = TickStyle.None,
                    PositionTier = n + 1,
                    Position = AxisPosition.Bottom,
                    TitlePosition = -0.1,
                };
                plot.Axes.Add(axis);

                for (int m = 2; m < relevance.RowCount; ++m)
                {
                    axis.Labels.Add((relevance[m, n] * 100).ToString("F2"));
                }
            }

            // The axis for the relevance of stroke
            var relevanceAxis = new LinearAxis()
            {
                Title = "Relevance of Stroke",
                MinimumPadding = 0.05,
                MaximumPadding = 0.05,

                // Ticks and Grid
                MajorStep = 0.5,
                MinorStep = 0.5,
                MinorTickSize = 0,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = this.style.GridlineColor,
            };
            plot.Axes.Add(relevanceAxis);

            // Plot the first and the second player            
            for (int n = 0; n < relevance.ColumnCount; ++n)
            {
                var name = match.Players.ElementAt(n).Name;

                var series = new ColumnSeries()
                {
                    Title = name,
                    FillColor = n == 0 ?
                        this.style.FirstPlayerColor : this.style.SecondPlayerColor,
                };
                plot.Series.Add(series);

                for (int m = 2; m < relevance.RowCount; ++m)
                {
                    series.Items.Add(new ColumnItem(relevance[m, n] * 100));
                }
            }

            return plot;
        }
    }
}
