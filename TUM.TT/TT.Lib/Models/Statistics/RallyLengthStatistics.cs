//-----------------------------------------------------------------------
// <copyright file="RallyLengthStatistics.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Models.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MathNet.Numerics.Distributions;
    using MathNet.Numerics.Statistics;

    /// <summary>
    /// Statistics about the length of rallies.
    /// </summary>
    public class RallyLengthStatistics : MatchStatistics
    {
        /// <summary>
        /// Number of buckets for rally length histograms.
        /// </summary>
        private const int NumberOfRallyLengthBuckets = 16;

        /// <summary>
        /// Initializes a new instance of the <see cref="RallyLengthStatistics"/> class.
        /// </summary>
        /// <param name="match">The match to calculate statistics for</param>
        public RallyLengthStatistics(Match match)
            : base(match)
        {
            this.TotalLengths = this.Match.DefaultPlaylist.FinishedRallies
                .Select(r => r.Length)
                .ToList();
            this.MaximumLength = this.TotalLengths.Max();
            this.ByServer = this.Match.DefaultPlaylist.FinishedRallies
                .ToLookup(r => r.Server, r => r.Length);
            this.ByWinner = this.Match.DefaultPlaylist.FinishedRallies
                .ToLookup(r => r.Winner, r => r.Length);
            this.ByServerAndWinner = this.Match.DefaultPlaylist.FinishedRallies
                .GroupBy(r => r.Server)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToLookup(r => r.Winner, r => r.Length));

            this.ObservedLengths = LengthHistogram(this.TotalLengths);
            this.ObservedLengthsByWinner = this.ByWinner
                .ToDictionary(g => g.Key, g => LengthHistogram(g));

            // Compute expected rally lengths
            this.ExpectedLengths = this.ExpectedRallyLengths();
            this.ExpectedLengthsByWinner = new Dictionary<MatchPlayer, double[]>() 
            {
                { MatchPlayer.First, this.ExpectedRallyLengths(MatchPlayer.First) },
                { MatchPlayer.Second, this.ExpectedRallyLengths(MatchPlayer.Second) },
            };
        }

        /// <summary>
        /// Gets the total rally lengths.
        /// </summary>
        public IList<int> TotalLengths { get; private set; }

        /// <summary>
        /// Gets the maximum rally length of the match.
        /// </summary>
        public int MaximumLength { get; private set; }

        /// <summary>
        /// Gets the lengths of rallies by server.
        /// </summary>
        public ILookup<MatchPlayer, int> ByServer { get; private set; }

        /// <summary>
        /// Gets the length of rallies by winner.
        /// </summary>
        public ILookup<MatchPlayer, int> ByWinner { get; private set; }

        /// <summary>
        /// Gets the length of rallies by winner after the first player serving.
        /// </summary>
        public IDictionary<MatchPlayer, ILookup<MatchPlayer, int>> ByServerAndWinner
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the number of rallies, distributed by length.
        /// </summary>
        public Histogram ObservedLengths { get; private set; }

        /// <summary>
        /// Gets the expected rally lengths.
        /// </summary>
        public double[] ExpectedLengths { get; private set; }

        /// <summary>
        /// Gets the number of rallies won by rally length, distributed by length.
        /// </summary>        
        public IDictionary<MatchPlayer, Histogram> ObservedLengthsByWinner
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a distribution of the expected length of won rallies, by player.
        /// </summary>
        public IDictionary<MatchPlayer, double[]> ExpectedLengthsByWinner
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a histogram for rally lengths.
        /// </summary>
        /// <param name="lengths">The rally lengths.</param>
        /// <returns>The histogram.</returns>
        private static Histogram LengthHistogram(IEnumerable<int> lengths)
        {
            return new Histogram(
                lengths.Select(l => (double)l),
                NumberOfRallyLengthBuckets,
                0,
                NumberOfRallyLengthBuckets);
        }

        /// <summary>
        /// Computes the expected lengths of rallies.
        /// </summary>
        /// <returns>The expected rally lengths</returns>
        private double[] ExpectedRallyLengths()
        {
            var total = this.Match.DefaultPlaylist.FinishedRallies.Count();
            var possion = new Poisson(this.TotalLengths.Select(l => (double)l).Mean());

            var d = new double[NumberOfRallyLengthBuckets];

            for (int i = 1; i < NumberOfRallyLengthBuckets; ++i)
            {
                d[i - 1] = total * possion.Probability(i);
            }

            d[NumberOfRallyLengthBuckets - 1] = total - d.Sum();
            return d;
        }

        /// <summary>
        /// Computes expected lengths of the rallies won by the given player.
        /// </summary>
        /// <remarks>
        /// For each <c>i</c>, <c>d[i]</c> (where <c>d</c> is the returned array)
        /// the expected amount of won rallies of length <c>i + 1</c>, with the 
        /// exception of the last item, which provides the expected value for all
        /// rallies with length <c>&gt; i</c>.
        /// </remarks>
        /// <param name="winner">the player</param>
        /// <returns>
        /// The expected lengths of the rallies won by
        /// <paramref name="winner"/>.
        /// </returns>
        private double[] ExpectedRallyLengths(MatchPlayer winner)
        {
            var total = this.Match.DefaultPlaylist.FinishedRallies
                    .Where(r => r.Winner == winner)
                    .Count();

            var d = new double[NumberOfRallyLengthBuckets];

            if (this.ByWinner[winner].Any())
            {
                var poisson = new Poisson(
                    this.ByWinner[winner].Select(l => (double)l).Mean());

                for (int i = 1; i < NumberOfRallyLengthBuckets; ++i)
                {
                    d[i - 1] = total * poisson.Probability(i);
                }

                d[NumberOfRallyLengthBuckets - 1] = total - d.Sum();
            }
            else
            {
                Array.Clear(d, 0, NumberOfRallyLengthBuckets);
            }

            return d;
        }
    }
}
