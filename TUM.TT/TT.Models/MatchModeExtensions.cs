
//-----------------------------------------------------------------------
// <copyright file="MatchModeExtensions.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models
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
    /// The Round in a Tournament.
    /// </summary>
    public enum MatchRound
    {
        /// <summary>
        /// Round.
        /// </summary>
        [Description("Round")]
        Round,
        /// <summary>
        /// Final.
        /// </summary>
        [Description("Final")]
        Final,
        /// <summary>
        /// Semifinal.
        /// </summary>
        [Description("Semifinal")]
        Semifinal,
        /// <summary>
        /// Quarterfinal.
        /// </summary>
        [Description("Quarterfinal")]
        Quarterfinal,
        /// <summary>
        /// Round of 16.
        /// </summary>
        [Description("Round of 16")]
        R16,
        /// <summary>
        /// Round of 32.
        /// </summary>
        [Description("Round of 32")]
        R32,
        /// <summary>
        /// Round of 64.
        /// </summary>
        [Description("Round of 64")]
        R64,
        /// <summary>
        /// PreRounds.
        /// </summary>
        [Description("PreRounds")]
        PreRounds,
        /// <summary>
        /// Group Stage.
        /// </summary>
        [Description("Group Stage/Qualification")]
        GroupStage,
    }

    /// <summary>
    /// The Category in a Tournament.
    /// </summary>
    public enum MatchCategory
    {
        /// <summary>
        /// Category.
        /// </summary>
        [Description("Men's Singles")]
        Category,
        /// <summary>
        /// Men's Singles.
        /// </summary>
        [Description("Men's Singles")]
        MS,
        /// <summary>
        /// Women's Singles.
        /// </summary>
        [Description("Women's Singles")]
        WS,
        /// <summary>
        /// U21 Men's Singles.
        /// </summary>
        [Description("U21 Men's Singles")]
        U21BS,
        /// <summary>
        /// U21 Women's Singles.
        /// </summary>
        [Description("U21 Women's Singles")]
        U21GS,
        /// <summary>
        /// Junior Boys' Singles.
        /// </summary>
        [Description("Junior Boys' Singles")]
        JBS,
        /// <summary>
        /// "Junior Girls' Singles".
        /// </summary>
        [Description("Junior Girls' Singles")]
        JGS,
        /// <summary>
        /// Cadet Boys' Singles.
        /// </summary>
        [Description("Cadet Boys' Singles")]
        CBS,
        /// <summary>
        /// "Cadet Girls' Singles".
        /// </summary>
        [Description("Cadet Girls' Singles")]
        CGS,
        /// <summary>
        /// Men's Doubles.
        /// </summary>
        /// 
        [Description("Men's Doubles")]
        MD,
        /// <summary>
        /// Women's Doubles.
        /// </summary>
        [Description("Women's Doubles")]
        WD,
        /// <summary>
        /// Junior Boys' Doubles.
        /// </summary>
        [Description("Junior Boys' Doubles")]
        JBD,
        /// <summary>
        /// "Junior Girls' Doubles".
        /// </summary>
        [Description("Junior Girls' Doubles")]
        JGD,
        /// <summary>
        /// Cadet Boys' Doubles.
        /// </summary>
        [Description("Cadet Boys' Doubles")]
        CBD,
        /// <summary>
        /// "Cadet Girls' Doubles".
        /// </summary>
        [Description("Cadet Girls' Doubles")]
        CGD,
        /// <summary>
        /// Men's Team.
        /// </summary>
        [Description("Men's Doubles")]
        MT,
        /// <summary>
        /// Women's Team.
        /// </summary>
        [Description("Women's Doubles")]
        WT,
        /// <summary>
        /// Junior Boys' Team.
        /// </summary>
        [Description("Junior Boys' Team")]
        JBT,
        /// <summary>
        /// "Junior Girls' Team".
        /// </summary>
        [Description("Junior Girls' Team")]
        JGT,
        /// <summary>
        /// Cadet Boys' Team.
        /// </summary>
        [Description("Cadet Boys' Team")]
        CBT,
        /// <summary>
        /// "Cadet Girls' Team".
        /// </summary>
        [Description("Cadet Girls' Team")]
        CGT,


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
