//-----------------------------------------------------------------------
// <copyright file="BasicInformation.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013  Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------
namespace TTA.Models.Report
{
    using System.Collections.Generic;

    /// <summary>
    /// The basic information of a player.
    /// </summary>
    public class BasicInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicInformation"/> class.
        /// </summary>
        public BasicInformation()
        {
            this.Results = new List<int>();
            this.TotalPoints = 0;
            this.WinningProbability = 0;
            this.WinningSets = 0;
        }

        /// <summary>
        /// Gets or sets the <see cref="Player"/>'s name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the results of the <see cref="Player"/>.
        /// </summary>
        public List<int> Results
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the total points of the <see cref="Player"/>.
        /// </summary>
        public int TotalPoints
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the number of winning sets
        /// </summary>
        public int WinningSets
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the winning probability of the <see cref="Player"/>
        /// </summary>
        public double WinningProbability
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the winning probability of the <see cref="Player"/> when he is the serving player.
        /// </summary>
        public double ServingWinningProbability
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the competition performance of the <see cref="Player"/>
        /// </summary>
        public double CompetitionPerformance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the serving frequency.
        /// </summary>
        public int ServingFrequency
        {
            get;
            set;
        }
    }
}
