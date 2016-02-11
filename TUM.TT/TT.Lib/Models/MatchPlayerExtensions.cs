//-----------------------------------------------------------------------
// <copyright file="MatchPlayerExtensions.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Models
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
        Second = 2,
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
    }
}
