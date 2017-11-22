//-----------------------------------------------------------------------
// <copyright file="RallyLengthSection.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Report.Sections
{
    using System.Linq;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using TT.Models;
    using TT.Models.Statistics;
    using TT.Report.Plots;

    /// <summary>
    /// A section for rally length statistics.
    /// </summary>
    public class RallyLengthSection : IReportSection
    {
        /// <summary>
        /// The match.
        /// </summary>
        private Match match;

        /// <summary>
        /// The plot style.
        /// </summary>
        private PlotStyle style;

        /// <summary>
        /// Initializes a new instance of the <see cref="RallyLengthSection"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <param name="style">The style for the plot</param>
        public RallyLengthSection(Match match, PlotStyle style)
        {
            this.style = style;
            this.match = match;

            this.Statistics = new RallyLengthStatistics(match);
            this.Plot = this.PlotLengths();
            this.PlotByWinner = this.PlotLengthsByWinner();
        }

        /// <summary>
        /// Gets the rally length statistics.
        /// </summary>
        public RallyLengthStatistics Statistics { get; private set; }

        /// <summary>
        /// Gets a plot of the rally lengths.
        /// </summary>
        public PlotModel Plot { get; private set; }

        /// <summary>
        /// Gets a plot of the rally lengths by winner.
        /// </summary>
        public PlotModel PlotByWinner { get; private set; }

        /// <summary>
        /// Creates a plot for lengths.
        /// </summary>
        /// <returns>The plot</returns>
        private PlotModel CreateLengthPlot()
        {
            var plot = this.style.CreatePlot();

            // The axis for the rally lengths
            var lengthAxis = new CategoryAxis()
            {
                Title = Properties.Resources.section_rallylength_dist_rallylength,
                MajorStep = 1,
                TickStyle = TickStyle.None,
                IsTickCentered = true,
            };
            plot.Axes.Add(lengthAxis);

            // Add labels for the rally lengths
            for (int i = 0; i < this.Statistics.ObservedLengths.BucketCount; ++i)
            {
                // The last bucket is cumulative, so we give it a special label
                var bucket = this.Statistics.ObservedLengths[i];
                var label = i == this.Statistics.ObservedLengths.BucketCount - 1 ?
                    string.Format(">{0}", bucket.LowerBound) :
                    bucket.UpperBound.ToString();
                lengthAxis.Labels.Add(label);
            }

            // The axis for the number of matches
            var noMatches = new LinearAxis()
            {
                Title = Properties.Resources.section_rallylength_dist_ralliescount,

                AbsoluteMinimum = 0,
                MinimumPadding = 0,
                MaximumPadding = 0.06,

                // Ticks and Grid
                MajorStep = 2,
                MinorStep = 2,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = this.style.GridlineColor,
            };
            plot.Axes.Add(noMatches);

            return plot;
        }

        /// <summary>
        /// Plots the length distribution.
        /// </summary>
        /// <returns>The plot.</returns>
        private PlotModel PlotLengths()
        {
            var plot = this.CreateLengthPlot();

            var observed = new ColumnSeries()
            {
                Title = Properties.Resources.section_rallylength_dist_observed,
            };
            plot.Series.Add(observed);
            foreach (var item in this.Statistics.ObservedLengths.ColumnItems())
            {
                observed.Items.Add(item);
            }

            var expected = new LineSeries()
            {
                Title = Properties.Resources.section_rallylength_dist_expected,
                Smooth = true,
            };
            plot.Series.Add(expected);
            var points = this.Statistics.ExpectedLengths
                .Select((n, i) => new DataPoint(i, n));
            foreach (var point in points)
            {
                expected.Points.Add(point);
            }

            return plot;
        }

        /// <summary>
        /// Plots the length distribution by winner.
        /// </summary>
        /// <returns>The plot.</returns>
        private PlotModel PlotLengthsByWinner()
        {
            var plot = this.CreateLengthPlot();
            var lengths = this.Statistics.ObservedLengthsByWinner;

            foreach (var player in new MatchPlayer[] { MatchPlayer.First, MatchPlayer.Second })
            {
                var name = player == MatchPlayer.First ?
                    this.match.FirstPlayer.Name : this.match.SecondPlayer.Name;
                var color = player == MatchPlayer.First ?
                    this.style.FirstPlayerColor : this.style.SecondPlayerColor;

                var observed = new ColumnSeries()
                {
                    Title = string.Format("{0} {1}", name, Properties.Resources.section_rallylength_dist_observed.ToLower()),
                    FillColor = color,
                };
                plot.Series.Add(observed);
                if (lengths.ContainsKey(player))
                {
                    foreach (var item in lengths[player].ColumnItems())
                    {
                        observed.Items.Add(item);
                    }
                }

                var expected = new LineSeries()
                {
                    Title = string.Format("{0} {1}", name, Properties.Resources.section_rallylength_dist_expected.ToLower()),
                    Color = color,
                    Smooth = true,
                };
                plot.Series.Add(expected);

                if (this.Statistics.ExpectedLengthsByWinner.ContainsKey(player))
                {
                    var points = this.Statistics
                        .ExpectedLengthsByWinner[player]
                        .Select((n, i) => new DataPoint(i, n));
                    foreach (var point in points)
                    {
                        expected.Points.Add(point);
                    }
                }
            }

            return plot;
        }
    }
}
