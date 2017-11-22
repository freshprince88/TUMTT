//-----------------------------------------------------------------------
// <copyright file="MatchPlayerExtensions.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.ComponentModel;

namespace TT.Models
{
    /// <summary>
    /// Refers to a player in match.
    /// </summary>
    public enum MatchPlayer
    {
        /// <summary>
        /// Refers to no player.
        /// </summary>
        None = 0,

        /// <summary>
        /// Refers to the <see cref="Match.FirstPlayer"/> in a match.
        /// </summary>
        First = 1,

        /// <summary>
        /// Refers to the <see cref="Match.SecondPlayer"/> in a match.
        /// </summary>
        Second = 2
    }

    public enum Handedness
    {
        None = 0,

        Right  = 1,

        Left = 2
    }

    public enum Grip
    {
        None = 0,

        Penhold = 1,

        Shakehand = 2
    }

    public enum PlayingStyle
    {
        None = 0,

        Offensive = 1,

        Defensive = 2
    }
    public enum StartingTableEnd
    {
        None=0,

        Top=1,

        Bottom=2
    }
    public enum MaterialFH
    {
        None=0,
        Normal=1,
        [Description("Short Pimples")]
        ShortPimples =2,
        [Description("Halflong Pimples")]
        HalfLongPimples = 3,
        [Description("Long Pimples")]
        LongPimples =4,
        [Description("Anti Top")]
        AntiTop =5
    }
    public enum MaterialBH
    {
        None = 0,
        Normal = 1,
        [Description("Short Pimples")]
        ShortPimples = 2,
        [Description("Halflong Pimples")]
        HalfLongPimples = 3,
        [Description("Long Pimples")]
        LongPimples = 4,
        [Description("Anti Top")]
        AntiTop = 5
    }

    public enum CurrentTableEnd
    {
        None=0,

        Top=1,

        Bottom=2
    }

    /// <summary>
    /// Extensions for <see cref="MatchPlayer"/>.
    /// </summary>
    public static class MatchPlayerExtensions
    {
        /// <summary>
        /// Gets the other player.
        /// </summary>
        /// <param name="self">This player.</param>
        /// <returns>The other player.</returns>
        public static MatchPlayer Other(this MatchPlayer self)
        {
            switch (self)
            {
                case MatchPlayer.First:
                    return MatchPlayer.Second;
                case MatchPlayer.Second:
                    return MatchPlayer.First;
                default:
                    return MatchPlayer.None;
            }
        }

        public static bool Equals(this MatchPlayer self, int i)
        {
            return self.Equals(Enum.GetName(typeof(MatchPlayer), i));
        }
    }
}
