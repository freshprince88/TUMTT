//-----------------------------------------------------------------------
// <copyright file="TechnicalEfficiency.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Statistics
{
    using System;
    using System.Linq;
    using MathNet.Numerics;

    /// <summary>
    /// Computes the technical efficiency of players.
    /// </summary>
    public class TechnicalEfficiency : MatchTransitionStatistics
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TechnicalEfficiency"/> class.
        /// </summary>
        /// <param name="transitions">The state transitions.</param>
        public TechnicalEfficiency(Transitions transitions)
            : base(transitions)
        {
        }

        /// <summary>
        /// Gets the number of errors of <paramref name="player"/> in rallies
        /// of length <paramref name="n"/>.
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="n">The rally length</param>
        /// <returns>The errors of <paramref name="player"/> in rallies of length <paramref name="n"/></returns>
        public double ErrorsAtLength(MatchPlayer player, int n)
        {
            var errors = this.Transitions.ErrorsAtStrokeByPlayer[player];

            if (n < 1 || n >= errors.Count)
            {
                throw new ArgumentOutOfRangeException("Cannot compute errors for stroke " + n);
            }

            var otherPoints = this.Transitions.PointsAtStrokeByPlayer[player.Other()];

            if (n == 0)
            {
                return errors[n];
            }
            else if (n == errors.Count - 1)
            {
                return errors[n] + otherPoints[n - 1] + otherPoints[n] + this.ErrorsAtLength(player, n - 1);
            }
            else
            {
                return errors[n] + otherPoints[n - 1];
            }
        }

        /// <summary>
        /// Gets the number of scores of <paramref name="player"/> in rallies
        /// of length <paramref name="n"/>.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="n">The rally length.</param>
        /// <returns>The scores of <paramref name="player"/> in rallies of length <paramref name="n"/></returns>
        public double ScoresAtLength(MatchPlayer player, int n)
        {
            var points = this.Transitions.PointsAtStrokeByPlayer[player];

            if (n < 0 || n >= points.Count)
            {
                throw new ArgumentOutOfRangeException("Cannot compute errors for stroke " + n);
            }

            var otherErrors = this.Transitions.ErrorsAtStrokeByPlayer[player.Other()];

            if (n == points.Count - 1)
            {
                return points[n] + otherErrors[n] + points[n - 1];
            }
            else if (n == points.Count - 2)
            {
                return points[n];
            }
            else
            {
                return points[n] + otherErrors[n + 1];
            }
        }

        /// <summary>
        /// Computes the technical efficiency.
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="n">The rally length</param>
        /// <returns>
        /// The technical efficiency of <paramref name="player"/> in rallies of length <paramref name="n"/>
        /// </returns>
        public TE TechnicalEfficiencyAtLength(MatchPlayer player, int n)
        {
            return new TE(
                this.ScoringRateAtLength(player, n),
                this.UsageRateAtLength(player, n));
        }

        /// <summary>
        /// Computes the technical efficiency at service.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The technical efficiency of <paramref name="player"/> at service.</returns>
        public TE ServiceTechnicalEfficiency(MatchPlayer player)
        {
            return this.AggregateTechnicalEfficiency(player, 1, 3);
        }

        /// <summary>
        /// Computes the technical efficiency at return.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The technical efficiency of <paramref name="player"/> at return.</returns>
        public TE ReturnTechnicalEfficiency(MatchPlayer player)
        {
            return this.AggregateTechnicalEfficiency(player, 2, 4);
        }

        /// <summary>
        /// Computes the technical efficiency in long rallies.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The technical efficiency of <paramref name="player"/> in long rallies.</returns>
        public TE LongRallyTechnicalEfficiency(MatchPlayer player)
        {
            return this.AggregateTechnicalEfficiency(player, 5, 7);
        }

        /// <summary>
        /// Computes an aggregate technical efficiency.
        /// </summary>
        /// <param name="p">The player</param>
        /// <param name="low">The lower rally length</param>
        /// <param name="high">The higher rally length</param>
        /// <returns>The aggregate technical efficiency.</returns>
        private TE AggregateTechnicalEfficiency(MatchPlayer p, int low, int high)
        {
            var usage = this.ScoresAtLength(p, low) + this.ErrorsAtLength(p, low)
                + this.ScoresAtLength(p, high) + this.ErrorsAtLength(p, high);
            var sr = (this.ScoresAtLength(p, low) + this.ScoresAtLength(p, high)) / usage;
            var ur = usage / this.Match.DefaultPlaylist.FinishedRallies.Count();
            return new TE(sr, ur);
        }

        /// <summary>
        /// Gets the scoring rate of rallies of length <paramref name="n"/>.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="n">The rally length</param>
        /// <returns>
        /// The scoring rate of <paramref name="player"/> in rallies of length <paramref name="n"/>
        /// </returns>
        private double ScoringRateAtLength(MatchPlayer player, int n)
        {
            var scores = this.ScoresAtLength(player, n);
            return scores / (scores + this.ErrorsAtLength(player, n));
        }

        /// <summary>
        /// Gets the usage rate of rallies of length <paramref name="n"/>.
        /// </summary>
        /// <param name="player">The player</param>
        /// <param name="n">The rally length</param>
        /// <returns>
        /// The usage rate of <paramref name="player"/> in rallies of length <paramref name="n"/>
        /// </returns>
        private double UsageRateAtLength(MatchPlayer player, int n)
        {
            return (this.ScoresAtLength(player, n) + this.ErrorsAtLength(player, n)) /
                this.Match.DefaultPlaylist.FinishedRallies.Count();
        }

        /// <summary>
        /// A technical efficiency aggregate
        /// </summary>
        public class TE
        {
            /// <summary>
            /// The coefficients for the TE polynomial.
            /// </summary>
            private static readonly double[] TECoefficients = new double[] 
            {
                -(1 + (1 / Math.Sqrt(2))),
                1.5 + Math.Sqrt(2),
                (-1) / Math.Sqrt(2)
            };

            /// <summary>
            /// Initializes a new instance of the <see cref="TE"/> class.
            /// </summary>
            /// <param name="scoringRate">The scoring rate.</param>
            /// <param name="usageRate">The usage rate.</param>
            public TE(double scoringRate, double usageRate)
            {
                this.ScoringRate = scoringRate;
                this.UsageRate = usageRate;
            }

            /// <summary>
            /// Gets the scoring rate.
            /// </summary>
            public double ScoringRate { get; private set; }

            /// <summary>
            /// Gets the usage rate.
            /// </summary>
            public double UsageRate { get; private set; }

            /// <summary>
            /// Gets the simple technical efficiency.
            /// </summary>
            /// <remarks>
            /// The simple technical efficiency is computed by a simple linear
            /// calculation from scoring rate and usage rate.
            /// </remarks>
            public double Simple
            {
                get
                {
                    return .5 - ((.5 - this.ScoringRate) * this.UsageRate);
                }
            }

            /// <summary>
            /// Gets the polynomial technical efficiency.
            /// </summary>
            /// <remarks>
            /// The technical efficiency is computed as polynomial of 
            /// usage rate and scoring rate.
            /// </remarks>
            public double Polynomial
            {
                get
                {
                    var x = Math.Pow(1 + this.UsageRate, this.ScoringRate - .5);
                    return Evaluate.Polynomial(x, TECoefficients);
                }
            }
        }
    }
}
