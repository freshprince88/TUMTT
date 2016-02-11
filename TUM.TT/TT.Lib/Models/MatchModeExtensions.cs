//-----------------------------------------------------------------------
// <copyright file="MatchModeExtensions.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Models
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// The mode of a match.
    /// </summary>
    public enum MatchMode
    {
        /// <summary>
        /// Best of 5 mode.
        /// </summary>
        /// <remarks>
        /// A player must win three sets to win the match.
        /// </remarks>
        [Description("Best of 5")]
        BestOf5,

        /// <summary>
        /// Best of 7 mode.
        /// </summary>
        /// <remarks>
        /// A player must win four sets to win the match.
        /// </remarks>
        [Description("Best of 7")]
        BestOf7,
    }

    /// <summary>
    /// Extensions for <see cref="MatchMode"/>.
    /// </summary>
    public static class MatchModeExtensions
    {
        /// <summary>
        /// Gets the minimum number of sets a player has to win.
        /// </summary>
        /// <param name="mode">The match mode.</param>
        /// <returns>The minimum number of sets.</returns>
        public static int RequiredSets(this MatchMode mode)
        {
            switch (mode)
            {
                case MatchMode.BestOf5:
                    return 3;
                case MatchMode.BestOf7:
                    return 4;
                default:
                    throw new ArgumentException("Unsupported match mode");
            }
        }
    }
}
