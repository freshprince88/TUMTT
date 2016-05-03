//-----------------------------------------------------------------------
// <copyright file="RallyLengthStatistics.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013  Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TTA.Models.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The winning statistics of all<see cref="Rally"/> records.
    /// </summary>
    public class RallyLengthStatistics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RallyLengthStatistics"/> class.
        /// </summary>
        public RallyLengthStatistics()
        {
            this.ServiceA = new RallyStatistics();
            this.ServiceB = new RallyStatistics();
            this.WinnerA = new RallyStatistics();
            this.WinnerB = new RallyStatistics();
            this.WinnerAWithServiceA = new RallyStatistics();
            this.WinnerAWithServiceB = new RallyStatistics();
            this.WinnerBWithServiceA = new RallyStatistics();
            this.WinnerBWithServiceB = new RallyStatistics();
        }

        /// <summary>
        /// Gets or sets the <see cref="RallyStatistics"/> when <see cref="Player"/> A was the serving player.
        /// </summary>
        public RallyStatistics ServiceA
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="RallyStatistics"/> when <see cref="Player"/> B was the serving player.
        /// </summary>
        public RallyStatistics ServiceB
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="RallyStatistics"/> when <see cref="Player"/> A was the winning player.
        /// </summary>
        public RallyStatistics WinnerA
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="RallyStatistics"/> when <see cref="Player"/> B was the winning player.
        /// </summary>
        public RallyStatistics WinnerB
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="RallyStatistics"/> when <see cref="Player"/> A was the serving player and won.
        /// </summary>
        public RallyStatistics WinnerAWithServiceA
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="RallyStatistics"/> when <see cref="Player"/> A was the serving player and player B won.
        /// </summary>
        public RallyStatistics WinnerBWithServiceA
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="RallyStatistics"/> when <see cref="Player"/> B was the serving player and player A won.
        /// </summary>
        public RallyStatistics WinnerAWithServiceB
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="RallyStatistics"/> when <see cref="Player"/> B was the serving player and won.
        /// </summary>
        public RallyStatistics WinnerBWithServiceB
        {
            get;
            set;
        }

        /// <summary>
        /// Holds the mean and median.
        /// </summary>
        public class RallyStatistics
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RallyStatistics"/> class.
            /// </summary>
            public RallyStatistics()
            {
            }

            /// <summary>
            /// Gets or sets the mean value.
            /// </summary>
            public double Mean
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the median value;
            /// </summary>
            public int Median
            {
                get;
                set;
            }
        }
    }
}
