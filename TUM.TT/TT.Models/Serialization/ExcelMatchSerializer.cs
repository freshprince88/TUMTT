//-----------------------------------------------------------------------
// <copyright file="ExcelMatchSerializer.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using OfficeOpenXml;

    using Regex = System.Text.RegularExpressions.Regex;
    using RegexOptions = System.Text.RegularExpressions.RegexOptions;
    
    /// <summary>
    /// Imports and exports matches from Excel.
    /// </summary>
    public class ExcelMatchSerializer : IMatchSerializer
    {
        /// <summary>
        /// Pattern to parse player names.
        /// </summary>
        private static readonly Regex PlayerNamePattern =
            new Regex(@"^(?<name>[^()]+) *(?:\((?<nationality>.+)\))?$");

        /// <summary>
        /// Pattern to find the tournament label.
        /// </summary>
        private static readonly Regex TournamentLabelPattern =
            new Regex(@"^\s*Tournament\s*:?\s*$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Pattern to find the round label.
        /// </summary>
        private static readonly Regex RoundLabelPattern =
            new Regex(@"^\s*Round\s*:?\s*$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Pattern to find the first player label.
        /// </summary>
        private static readonly Regex FirstPlayerLabelPattern =
            new Regex(@"^\s*Player\s*1\s*:\s*$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Pattern to find the second player label.
        /// </summary>
        private static readonly Regex SecondPlayerLabelPattern =
            new Regex(@"^\s*Player\s*2\s*:\s*$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Pattern to find the service column.
        /// </summary>
        private static readonly Regex ServiceLabelPattern =
            new Regex(@"^\s*Service\s*:?\s*$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Pattern to find the winner column.
        /// </summary>
        private static readonly Regex WinnerLabelPattern =
            new Regex(@"^\s*Winner\s*:?\s*$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Pattern to find the Strokes of Rally column.
        /// </summary>
        private static readonly Regex StrokesLabelPattern =
            new Regex(@"^\s*Strokes\s+of\s+Rally\s*:?\s*$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Pattern to find the number of the rally.
        /// </summary>
        private static readonly Regex RallyNumberLabelPattern =
            new Regex(@"^\s*Number\s+of\s+Rally\s*:?\s*$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Pattern to find the Match date and time
        /// </summary>
        private static readonly Regex DateLabelPattern = 
            new Regex(@"^\s*Date\s*:?\s*$", RegexOptions.IgnoreCase);

        /// <summary>
        /// Serializes a match to excel.
        /// </summary>
        /// <param name="stream">the stream to serialize to</param>
        /// <param name="match">The match to serialize</param>
        public void Serialize(Stream stream, Match match)
        {
            var pkg = new ExcelPackage(stream);
            var sheet = pkg.Workbook.Worksheets.Add("Raw data");

            // The match metadata
            WriteRow(sheet, 1, "Tournament:", match.Tournament);
            WriteRow(sheet, 2, "Date:", match.DateTime.ToShortDateString());
            WriteRow(sheet, 3, "Round:", match.Round);
            WriteRow(
                sheet,
                4,
                "Players",
                "Player 1:",
                FormatPlayer(match.FirstPlayer),
                "Player 2:",
                FormatPlayer(match.SecondPlayer));

            // The sets table
            WriteRow(sheet, 5, "Results", "Player 1", "Player 2");

            var row = 6;
            var sets = match.FinishedRallies
                .Where(r => r.IsEndOfSet)
                .Select(r => r.FinalSetScore)
                .Select((s, i) => new object[] { string.Format("Set {0}", i + 1), s.First, s.Second });

            foreach (var set in sets)
            {
                WriteRow(sheet, row, set);
                row++;
            }

            for (int r = row; r <= 12; r++)
            {
                WriteRow(sheet, r, string.Format("Set {0}", r - 5));
            }

            // A blank row after the sets table
            row = 14;

            // The header of the rally table
            var header = new object[]
            {
                "Number of Rally",
                "Set 1",
                "Set 2",
                "Points 1",
                "Points 2",
                "Service",
                "Strokes of Rally",
                "Winner"
            };
            WriteRow(sheet, row, header);
            row++;

            var rallies = match.FinishedRallies
                .Select((r, i) => new object[] 
                {
                    i + 1, // The rally number
                    r.CurrentSetScore.First,
                    r.CurrentSetScore.Second,
                    r.CurrentRallyScore.First,
                    r.CurrentRallyScore.Second,
                    MatchPlayerToPlayerNo(r.Server),
                    r.Length,
                    MatchPlayerToPlayerNo(r.Winner)
                });

            foreach (var rally in rallies)
            {
                WriteRow(sheet, row, rally);
                row++;
            }

            pkg.SaveAs(stream);
            stream.Close();                     
        }

        /// <summary>
        /// Deserializes from Excel data.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <returns>the match</returns>
        /// <exception cref="ExcelSerializationException">
        /// If the Excel data has an unexpected format.
        /// </exception>
        public Match Deserialize(Stream stream)
        {
            var pkg = LoadExcel(stream);
            var sheet = pkg.Workbook.Worksheets.First();

            var match = ParseMatchInformation(sheet);

            foreach (var rally in ParseRallyTable(sheet))
            {
                match.Rallies.Add(rally);
            }

            if (match.FinishedRallies.Any())
            {
                // Determine the mode of this match
                var winningScore = match.Rallies.Last().FinalSetScore.Highest;
                var modes = (MatchMode[])Enum.GetValues(typeof(MatchMode));
                match.Mode = modes
                    .Where(m => winningScore == m.RequiredSets())
                    .DefaultIfEmpty(MatchMode.BestOf5)
                    .First();
            }

            return match;
        }

        /// <summary>
        /// Writes a single row in a sheet.
        /// </summary>
        /// <param name="sheet">The target sheet.</param>
        /// <param name="row">The row.</param>
        /// <param name="cells">Values for the cells in this row.</param>
        private static void WriteRow(
            ExcelWorksheet sheet, int row, params object[] cells)
        {
            for (int i = 0; i < cells.Length; ++i)
            {
                sheet.Cells[row, i + 1].Value = cells[i];
            }
        }

        /// <summary>
        /// Parses Match information.
        /// </summary>
        /// <param name="sheet">The sheet to parse from.</param>
        /// <returns>The parsed match information.</returns>
        private static Match ParseMatchInformation(ExcelWorksheet sheet)
        {
            var searchRange = "A1:E12";
            return new Match()
            {
                Tournament = GetTextByLabelPattern(sheet.Cells[searchRange], TournamentLabelPattern),
                Round = (MatchRound) Enum.Parse(typeof(MatchRound), GetTextByLabelPattern(sheet.Cells[searchRange], RoundLabelPattern)),
                FirstPlayer = ParsePlayer(GetTextByLabelPattern(sheet.Cells[searchRange], FirstPlayerLabelPattern)),
                SecondPlayer = ParsePlayer(GetTextByLabelPattern(sheet.Cells[searchRange], SecondPlayerLabelPattern)),

                // The Excel sheet does not provide date and time, so fall back to the only reasonable value
                // Try parsing date and time from the second row
                DateTime = ParseDateTime(sheet.Cells[searchRange], DateLabelPattern)
            };
        }

        /// <summary>
        /// Parses the rallies.
        /// </summary>
        /// <param name="sheet">The sheet to parse from.</param>
        /// <returns>The rallies</returns>
        private static IEnumerable<Rally> ParseRallyTable(ExcelWorksheet sheet)
        {
            // Search the rally table
            var range = sheet.Cells["A:A"]
                .FirstOrDefault(r => RallyNumberLabelPattern.IsMatch(r.Text));
            if (range != null)
            {
                var headerRow = new ExcelCellAddress(range.Address).Row;
                if (string.IsNullOrWhiteSpace(sheet.Cells[headerRow, 2].Text))
                {
                    // If the cell to the right of the header indicator is empty,
                    // try the next row
                    headerRow += 1;
                }

                var header = sheet.Cells[headerRow, 1, headerRow, 100];

                var serviceColumn = FindColumnByNamePattern(header, ServiceLabelPattern);
                var strokesColumn = FindColumnByNamePattern(header, StrokesLabelPattern);
                var winnerColumn = FindColumnByNamePattern(header, WinnerLabelPattern);

                for (int row = headerRow + 1; true; ++row)
                {
                    var service = sheet.Cells[row, serviceColumn].Text.Trim();
                    var strokes = sheet.Cells[row, strokesColumn].Text.Trim();
                    var winner = sheet.Cells[row, winnerColumn].Text.Trim();
                    if (string.IsNullOrEmpty(service) &&
                        string.IsNullOrEmpty(strokes) &&
                        string.IsNullOrEmpty(winner))
                    {
                        // We reached the end of the table.
                        break;
                    }

                    yield return new Rally()
                    {
                        Length = Convert.ToInt32(strokes),
                        Server = PlayerNoToMatchPlayer(Convert.ToInt32(service)),
                        Winner = PlayerNoToMatchPlayer(Convert.ToInt32(winner))
                    };
                }
            }
            else
            {
                throw new ExcelSerializationException("Could not find rally table");
            }
        }

        /// <summary>
        /// Parses the Match Date
        /// </summary>
        /// <param name="cells">The cells to search in</param>
        /// <param name="labelPattern">The label pattern</param>
        /// <returns>The date of the Match, or DateTime.Now if no date was found</returns>
        /// <exception cref="ExcelSerializationException">
        /// If the label was not found.
        /// </exception>
        private static DateTime ParseDateTime(ExcelRange cells, Regex labelPattern)
        {
            string dateString = GetTextByLabelPattern(cells, labelPattern);
            DateTime date;
            if (DateTime.TryParseExact(dateString, "d", null, DateTimeStyles.None, out date))
            {
                return date;
            }
            else
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Parses a player name from a string.
        /// </summary>
        /// <remarks>
        /// Attempts to parse the nationality from parenthesis following the name of the player.
        /// </remarks>
        /// <param name="player">The player string</param>
        /// <returns>The parsed player</returns>
        private static Player ParsePlayer(string player)
        {
            var match = PlayerNamePattern.Match(player);
            if (match.Success)
            {
                return new Player()
                {
                    Name = match.Groups["name"].Value,
                    Nationality = match.Groups["nationality"].Value
                };
            }
            else
            {
                return new Player()
                {
                    Name = player
                };
            }
        }

        /// <summary>
        /// Formats the name of a player.
        /// </summary>
        /// <param name="player">The player</param>
        /// <returns>The string representing the player</returns>
        private static string FormatPlayer(Player player)
        {
            if (player != null)
            {
                var name = player.Name;
                var nationality = player.Nationality;

                if (string.IsNullOrEmpty(nationality))
                {
                    return name;
                }
                else
                {
                    return string.Format("{0} ({1})", name, nationality);
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Loads an Excel package from a stream.
        /// </summary>
        /// <param name="stream">The stream to load from.</param>
        /// <returns>The loaded Excel package</returns>
        private static ExcelPackage LoadExcel(Stream stream)
        {
            var pkg = new ExcelPackage();
            pkg.Load(stream);
            return pkg;
        }

        /// <summary>
        /// Finds a column by header name.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="namePattern">The name pattern.</param>
        /// <returns>The column</returns>
        /// <exception cref="ExcelSerializationException">
        /// If the column was not found.
        /// </exception>
        private static int FindColumnByNamePattern(ExcelRange header, Regex namePattern)
        {
            var columns = header
                .Where(r => namePattern.IsMatch(r.Text))
                .Select(r => new ExcelCellAddress(r.Address).Column);
            if (columns.Any())
            {
                return columns.First();
            }
            else
            {
                throw new ExcelSerializationException(
                    string.Format("Cannot find the {0} column", namePattern));
            }
        }

        /// <summary>
        /// Gets a cell text by label.
        /// </summary>
        /// <remarks>
        /// The text is taken from the cell right of the cell that matches the
        /// <paramref name="labelPattern"/>.
        /// </remarks>
        /// <param name="cells">The cells to search in</param>
        /// <param name="labelPattern">The label pattern</param>
        /// <returns>The text of the cell</returns>
        /// <exception cref="ExcelSerializationException">
        /// If the label was not found.
        /// </exception>
        private static string GetTextByLabelPattern(
            ExcelRange cells,
            Regex labelPattern)
        {
            var labels = cells
                .Where(r => labelPattern.IsMatch(r.Text))
                .Select(r => new ExcelCellAddress(r.Address));
            if (labels.Any())
            {
                var labelAddress = labels.First();
                return cells.Worksheet
                    .Cells[labelAddress.Row, labelAddress.Column + 1]
                    .Text.Trim();
            }
            else
            {
                throw new ExcelSerializationException(
                    string.Format("Could not find the label pattern {0}", labelPattern));
            }
        }

        /// <summary>
        /// Converts a player number to a match player.
        /// </summary>
        /// <param name="playerNo">The player no.</param>
        /// <returns>The player.</returns>
        private static MatchPlayer PlayerNoToMatchPlayer(int playerNo)
        {
            switch (playerNo)
            {
                case 1:
                    return MatchPlayer.First;
                case 2:
                    return MatchPlayer.Second;
                default:
                    return MatchPlayer.None;
            }
        }

        /// <summary>
        /// Converts a player to a player number.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The player number</returns>
        private static object MatchPlayerToPlayerNo(MatchPlayer player)
        {
            switch (player)
            {
                case MatchPlayer.First:
                    return 1;
                case MatchPlayer.Second:
                    return 2;
                default:
                    return null;
            }
        }
    }
}
