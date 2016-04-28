//-----------------------------------------------------------------------
// <copyright file="BasicInformationSection.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Report.Sections
{
    using System.Collections.Generic;
    using System.Linq;
    using TT.Lib.Models;
    using TT.Lib.Models.Statistics;

    /// <summary>
    /// A section with basic information about a match.
    /// </summary>
    public class BasicInformationSection : IReportSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicInformationSection"/> class.
        /// </summary>
        /// <param name="match">The match.</param>
        public BasicInformationSection(Match match)
        {
            this.FirstPlayer = match.FirstPlayer;
            this.SecondPlayer = match.SecondPlayer;
            this.FinalSetScore = match.DefaultPlaylist.FinishedRallies.Last().FinalSetScore;
            this.FinalRallyScores = match.DefaultPlaylist.FinishedRallies
                .Where(r => r.IsEndOfSet)
                .Select(r => r.FinalRallyScore)
                .ToList();

            this.FirstPlayerStats = new PlayerStatistics(match, MatchPlayer.First);
            this.SecondPlayerStats = new PlayerStatistics(match, MatchPlayer.Second);
        }

        /// <summary>
        /// Gets the first player.
        /// </summary>
        public Player FirstPlayer { get; private set; }

        /// <summary>
        /// Gets the second player.
        /// </summary>
        public Player SecondPlayer { get; private set; }

        /// <summary>
        /// Gets the final set score.
        /// </summary>
        public Score FinalSetScore { get; private set; }

        /// <summary>
        /// Gets the final rally scores.
        /// </summary>
        public IEnumerable<Score> FinalRallyScores { get; private set; }

        /// <summary>
        /// Gets the statistics of the first player.
        /// </summary>
        public PlayerStatistics FirstPlayerStats { get; private set; }

        /// <summary>
        /// Gets the statistics of the second player.
        /// </summary>
        public PlayerStatistics SecondPlayerStats { get; private set; }
    }
}
