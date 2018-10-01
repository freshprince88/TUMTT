//-----------------------------------------------------------------------
// <copyright file="MatchExtensions.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Util
{
    using System;
    using TT.Models.Api;
    using TT.Models;

    /// <summary>
    /// Extends the match class with some useful additions.
    /// </summary>
    public static class MatchMetaExtensions
    {
        /// <summary>
        /// Gets a default file name for a match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns>The default file name.</returns>
        public static string DefaultFilename(this MatchMeta match)
        {
            var first = match.FirstPlayer ?? new PlayerMeta();
            var second = match.SecondPlayer ?? new PlayerMeta();

            return string.Format(
                "{0:yyyy-MM-dd} - {1} vs {2} - {3} - {4} - {5}",
                match.Date,
                first.Name.WhenNullOrEmpty("A"),
                second.Name.WhenNullOrEmpty("B"),
                match.Tournament.WhenNullOrEmpty("Unknown tournament"),
                match.Category.ToString().WhenNullOrEmpty("Unknown Category"),
                match.Round.ToString().WhenNullOrEmpty("Unknown round")
            );
        }

        public static void UpdateMatchWithMetaInfo(Match match, MatchMeta meta)
        {
            match.ID = meta.Guid;
            match.Tournament = meta.Tournament;
            match.DateTime = meta.Date;
            try
            {
                match.Category = (MatchCategory)Enum.Parse(typeof(MatchCategory), meta.Category);
                match.Round = (MatchRound)Enum.Parse(typeof(MatchRound), meta.Round);
                match.Mode = (MatchMode)Enum.Parse(typeof(MatchMode), meta.Mode);
                match.DisabilityClass = (DisabilityClass)Enum.Parse(typeof(DisabilityClass), meta.DisabilityClass);
            }
            catch { }
            UpdatePlayerWithMeta(match.FirstPlayer, meta.FirstPlayer);
            UpdatePlayerWithMeta(match.SecondPlayer, meta.SecondPlayer);
        }

        private static void UpdatePlayerWithMeta(Player player, PlayerMeta meta)
        {
            player.Name = meta.Name;
            player.Nationality = meta.Nationality;
            player.Material = meta.Material;
            try
            {
                player.PlayingStyle = (PlayingStyle)Enum.Parse(typeof(PlayingStyle), meta.PlayingStyle);
                player.Handedness = (Handedness)Enum.Parse(typeof(Handedness), meta.Handedness);
                player.Grip = (Grip)Enum.Parse(typeof(Grip), meta.Grip);
                player.MaterialBH = (MaterialBH)Enum.Parse(typeof(MaterialBH), meta.MaterialBackhand);
                player.MaterialFH = (MaterialFH)Enum.Parse(typeof(MaterialFH), meta.MaterialForehand);
            }
            catch { }
        }
    }
}
