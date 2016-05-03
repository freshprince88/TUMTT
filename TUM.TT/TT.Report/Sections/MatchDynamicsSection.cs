//-----------------------------------------------------------------------
// <copyright file="MatchDynamicsSection.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
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
    using TT.Models;
    using TT.Models.Statistics;
    using TT.Report.Plots;

    /// <summary>
    /// A section representing the match dynamics.
    /// </summary>
    public class MatchDynamicsSection : IReportSection
    {
        /// <summary>
        /// The plot style.
        /// </summary>
        private PlotStyle style;

        /// <summary>
        /// The match.
        /// </summary>
        private Match match;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchDynamicsSection"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <param name="style">The plot style.</param>
        public MatchDynamicsSection(Match match, PlotStyle style)
        {
            this.match = match;
            this.style = style;
            this.Dynamics = new MatchDynamics(match);

            this.OverallPlot = this.PlotOverallDynamics();
            this.ByServerPlot = this.PlotByServerDynamics();
        }

        /// <summary>
        /// Gets the match dynamics.
        /// </summary>
        public MatchDynamics Dynamics { get; private set; }

        /// <summary>
        /// Gets a plot for the overall dynamics of the match.
        /// </summary>
        public PlotModel OverallPlot { get; private set; }

        /// <summary>
        /// Gets a plot for the match dynamics by serving player.
        /// </summary>
        public PlotModel ByServerPlot { get; private set; }

        /// <summary>
        /// Creates a plot for match dynamics.
        /// </summary>
        /// <returns>The plot model.</returns>
        private PlotModel CreateDynamicsPlot()
        {
            var plot = this.style.CreatePlot();
            plot.PlotMargins = new OxyThickness(30, double.NaN, 20, double.NaN);

            var rallyNoAxis = new LinearAxis()
            {
                Minimum = 0,
                Maximum = this.match.DefaultPlaylist.FinishedRallies.Count() + 0.5,
                MinorStep = 5,
                MajorStep = 5,
                MinorTickSize = 0,
                Position = AxisPosition.Bottom,
            };
            var dynamicsAxis = new LinearAxis()
            {
                Minimum = 0,
                Maximum = 1.05,
                MajorStep = 0.2,
                MinorStep = 0.1,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = this.style.GridlineColor,
                MajorGridlineThickness = 0.5,
                MinorGridlineStyle = LineStyle.Solid,
                MinorGridlineColor = this.style.GridlineColor,
                MinorGridlineThickness = 0.5,
            };
            plot.Axes.Add(rallyNoAxis);
            plot.Axes.Add(dynamicsAxis);

            // Mark the maximum dynamics value.  We use an annotation instead of
            // just limiting the Y axis to 1.0, because annotations can be drawn
            // below series, hence the series is properly visible even if the
            // dynamics is 1 for a longer time.
            plot.Annotations.Add(
                new LineAnnotation()
                {
                    Y = 1,
                    Color = OxyColors.DarkGray,
                    Type = LineAnnotationType.Horizontal,
                    LineStyle = LineStyle.Solid,
                    Layer = AnnotationLayer.BelowSeries,
                });

            return plot;
        }

        /// <summary>
        /// Plots the overall dynamics.
        /// </summary>
        /// <returns>The plot.</returns>
        private PlotModel PlotOverallDynamics()
        {
            var plot = this.CreateDynamicsPlot();

            var series = new LineSeries()
            {
                Smooth = true,
                Color = this.style.FirstPlayerColor,
            };
            plot.Series.Add(series);

            var points = this.Dynamics.Overall
                .Select((d, i) => new DataPoint(i + 1, d));
            foreach (var point in points)
            {
                series.Points.Add(point);
            }

            return plot;
        }

        /// <summary>
        /// Plots the server dynamics
        /// </summary>
        /// <returns>The plot.</returns>
        private PlotModel PlotByServerDynamics()
        {
            var plot = this.CreateDynamicsPlot();

            var players = new MatchPlayer[] { MatchPlayer.First, MatchPlayer.Second };
            foreach (var player in players)
            {
                var name = player == MatchPlayer.First ?
                    this.match.FirstPlayer.Name : this.match.SecondPlayer.Name;
                var color = player == MatchPlayer.First ?
                    this.style.FirstPlayerColor : this.style.SecondPlayerColor;
                var series = new LineSeries()
                {
                    Title = name,
                    Color = color,
                    Smooth = true,
                };
                plot.Series.Add(series);

                // Join the dynamics with the corresponding rally index.
                var points = this.match.DefaultPlaylist.FinishedRallies
                    .Select((r, i) => r.Server == player ? i + 1 : -1)
                    .Where(i => i != -1)
                    .Zip(this.Dynamics.ByServer[player], (n, d) => new DataPoint(n, d));
                foreach (var point in points)
                {
                    series.Points.Add(point);
                }
            }

            return plot;
        }
    }
}
