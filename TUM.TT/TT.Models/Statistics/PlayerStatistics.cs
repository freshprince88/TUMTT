//-----------------------------------------------------------------------
// <copyright file="PlayerStatistics.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Computes statistics about a player in a match.
    /// </summary>
    public class PlayerStatistics : MatchStatistics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerStatistics"/> class.
        /// </summary>
        /// <param name="match">The match to calculate the statistics for.</param>
        /// <param name="player">The player in the match</param>
        public PlayerStatistics(Match match, MatchPlayer player)
            : base(match)
        {
            this.Player = player;

            this.TotalSets = this.Match.DefaultPlaylist.FinishedRallies
                    .Where(r => r.IsEndOfSet && r.Winner == this.Player)
                    .Count();
            this.TotalPoints = this.Match.DefaultPlaylist.FinishedRallies
                    .Where(r => r.Winner == this.Player)
                    .Count();
            this.WinningProbability = this.TotalPoints / (double)this.Match.DefaultPlaylist.FinishedRallies
                    .Where(r => r.Winner != MatchPlayer.None)
                    .Count();
            this.ServiceFrequency = this.Match.DefaultPlaylist.FinishedRallies
                    .Where(r => r.Server == this.Player)
                    .Count();
            this.ProbabilityOfWinningAfterService = this.Match.DefaultPlaylist.FinishedRallies
                    .Where(r => r.Server == this.Player && r.Winner == this.Player)
                    .Count() / (double)this.ServiceFrequency;

            var sets = this.Match.DefaultPlaylist.FinishedRallies
                    .Where(r => r.IsEndOfSet)
                    .ToArray();
            var rallyPerformance = sets
                .Sum(r => this.Performance(r));
            this.CompetitionPerformance = 22 - Math.Sqrt(rallyPerformance / sets.Length);

            var scoringProcess = new List<List<int>>() { new List<int>() };

            foreach (var rally in this.Match.DefaultPlaylist.FinishedRallies)
            {
                scoringProcess.Last().Add(rally.FinalRallyScore.Of(this.Player));
                if (rally.IsEndOfSet)
                {
                    scoringProcess.Add(new List<int>());
                }
            }

            this.ScoringProcess = scoringProcess;
        }

        /// <summary>
        /// Gets the player these statistics are for.
        /// </summary>
        public MatchPlayer Player { get; private set; }

        /// <summary>
        /// Gets the total sets the player scored.
        /// </summary>
        public int TotalSets { get; private set; }

        /// <summary>
        /// Gets the total points of the player.
        /// </summary>
        public int TotalPoints { get; private set; }

        /// <summary>
        /// Gets the probability of this player winning the match.
        /// </summary>
        public double WinningProbability { get; private set; }

        /// <summary>
        /// Gets the frequency of services of this player.
        /// </summary>
        public int ServiceFrequency { get; private set; }

        /// <summary>
        /// Gets the probability of the player winning after having served.
        /// </summary>
        public double ProbabilityOfWinningAfterService { get; private set; }

        /// <summary>
        /// Gets the competition performance of this player.
        /// </summary>
        public double CompetitionPerformance { get; private set; }

        /// <summary>
        /// Gets the scoring process of this player.
        /// </summary>
        /// <value>
        /// The score in each rally, of each set.
        /// </value>
        public IEnumerable<IEnumerable<int>> ScoringProcess { get; private set; }

        /// <summary>
        /// Gets the performance in a rally.
        /// </summary>
        /// <param name="rally">The rally whose performance to compute</param>
        /// <returns>The performance</returns>
        private double Performance(Rally rally)
        {
            var score = rally.FinalRallyScore;
            var winningScore = score.Highest;

            double bias = rally.Winner == this.Player ? +1 : -1;
            var @base = winningScore == 11 ?
                score.Of(this.Player) - score.Of(this.Player.Other()) - 11 :
                -11 + (bias / (double)(winningScore - 11));

            return Math.Pow(@base, 2);
        }
    }
}
