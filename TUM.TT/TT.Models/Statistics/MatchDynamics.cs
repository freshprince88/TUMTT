//-----------------------------------------------------------------------
// <copyright file="MatchDynamics.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Statistics
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Computes the dynamics of a match.
    /// </summary>
    public class MatchDynamics : MatchStatistics
    {
        /// <summary>
        /// Window size for match dynamics.
        /// </summary>
        private const int WindowSize = 4;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchDynamics"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        public MatchDynamics(Match match)
            : base(match)
        {
            // The overall dynamics, calculated from the point of view of the 
            // winning player.
            var winner = this.Match.IsOver ? this.Match.DefaultPlaylist.FinishedRallies.Last().Winner :
               MatchPlayer.First;
            var overallResults = this.Match.DefaultPlaylist.FinishedRallies
                .Select(r => r.Winner == winner ? 1d : 0d)
                .ToArray();
            this.Overall = MovingBackwardForwardAverage(overallResults, WindowSize);

            // Dynamics by serving player.
            this.ByServer = new Dictionary<MatchPlayer, IEnumerable<double>>();
            foreach (var player in new MatchPlayer[] { MatchPlayer.First, MatchPlayer.Second })
            {
                var playerResults = this.Match.DefaultPlaylist.FinishedRallies
                    .Where(r => r.Server == player)
                    .Select(r => r.Winner == player ? 1d : 0d)
                    .ToArray();
                this.ByServer[player] = MovingBackwardForwardAverage(playerResults, WindowSize);
            }
        }

        /// <summary>
        /// Gets the overall match dynamics.
        /// </summary>
        /// <remarks>
        /// The returned enumerable provides the dynamics for each rally, 
        /// calculated from the point of view of the winning player.  The higher
        /// the value, the stronger was the winning player in the 
        /// corresponding rally.
        /// </remarks>
        public IEnumerable<double> Overall { get; private set; }

        /// <summary>
        /// Gets the match dynamics by serving player.
        /// </summary>
        /// <remarks>
        /// The return value provides the dynamics for rallies served by the 
        /// corresponding player. The higher the value, the stronger the player
        /// in rallies which she served.
        /// </remarks>
        public IDictionary<MatchPlayer, IEnumerable<double>> ByServer
        {
            get;
            private set;
        }

        /// <summary>
        /// Computes a moving backward and forward average.
        /// </summary>
        /// <param name="data">The data</param>
        /// <param name="windowSize">The window size</param>
        /// <returns>The average.</returns>
        private static double[] MovingBackwardForwardAverage(double[] data, int windowSize)
        {
            // Compute the backward average of the results
            var backward = new double[data.Length];

            // Fill the gap at the beginning
            for (int i = 0; i < windowSize - 1; ++i)
            {
                backward[i] = double.NaN;
            }

            for (int i = windowSize - 1; i < data.Length; ++i)
            {
                // Fill the window, for simplicity in reversed order.  Doesn't
                // matter anyway, since where just taking the average
                var window = new double[windowSize];
                for (int j = 0; j < windowSize; ++j)
                {
                    window[j] = data[i - j];
                }

                backward[i] = window.Average();
            }

            // Compute the forward average.
            var forward = new double[backward.Length];

            // Fill the gap at the end
            for (int i = forward.Length - windowSize + 1; i < forward.Length; ++i)
            {
                forward[i] = double.NaN;
            }

            for (int i = 0; i < backward.Length - windowSize + 1; ++i)
            {
                var window = new double[windowSize];
                for (int j = 0; j < windowSize; ++j)
                {
                    window[j] = backward[i + j];
                }

                forward[i] = window.Average();
            }

            return forward;
        }
    }
}
