//-----------------------------------------------------------------------
// <copyright file="MatchExtensions.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Util
{
    using System;
    using System.Linq;
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

        public static MatchMeta MatchMetaFromMagicFilename(string filename)
        {
            string[] playerFields;
            string[] matchFields;

            var slices = filename.Split('.')[0].Split('_');
            var length = slices.Length;
            // Double
            if (length == 17 || length == 18)
            {
                playerFields = new string[]
                {
                  slices[1] + ' ' + slices[2] + '+' + slices[4] + ' ' + slices[5], slices[3],
                  slices[7] + ' ' + slices[8] + '+' + slices[10] + ' ' + slices[11], slices[9]
                };
                matchFields = slices.Skip(13).ToArray();
            }
            // Single
            else if (length == 11 || length == 12)
            {
                playerFields = new string[]
{
                  slices[1] + ' ' + slices[2], slices[3],
                  slices[4] + ' ' + slices[5], slices[6]
};
                matchFields = slices.Skip(7).ToArray();
            }
            else
            {
                throw new ArgumentException();
            }

            var year = int.Parse("20" + slices[0].Substring(0, 2));
            var month = int.Parse(slices[0].Substring(2, 2));
            var day = int.Parse(slices[0].Substring(4, 2));

            return new MatchMeta()
            {
                Guid = Guid.NewGuid(),
                Date = new DateTime(year, month, day),
                Tournament = matchFields[0] + ' ' + matchFields[1],
                Category = matchFields[2],
                Round = matchFields.Length == 5 ? matchFields[4] : matchFields[3],
                DisabilityClass = matchFields.Length == 5 ? matchFields[2] : "NoClass",

                FirstPlayer = new PlayerMeta() {
                    Name = playerFields[0],
                    Nationality = playerFields[1]
                },
                SecondPlayer = new PlayerMeta()
                {
                    Name = playerFields[2],
                    Nationality = playerFields[3]
                }
            };
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
