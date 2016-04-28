//-----------------------------------------------------------------------
// <copyright file="ReportGenerator.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013  Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TTA.Models.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using iTextSharp.text;

    /// <summary>
    /// A generator for <see cref="Report"/>s.
    /// </summary>
    public class ReportGenerator
    {
        /// <summary>
        /// Backs the <see cref="Match"/> property.
        /// </summary>
        private Match match;

        /// <summary>
        /// Backs the <see cref="IncludeBasicInformation"/> property.
        /// </summary>
        private bool includeBasicInformation = true;

        /// <summary>
        /// Backs the <see cref="IncludeRallyLength"/> property.
        /// </summary>
        private bool includeRallyLength = true;

        /// <summary>
        /// Backs the <see cref="IncludeRallyLength"/> property.
        /// </summary>
        private bool includeTransitions = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportGenerator"/> class.
        /// </summary>
        public ReportGenerator()
        {
        }

        /// <summary>
        /// Gets or sets report's match.
        /// </summary>
        public Match Match
        {
            get
            {
                return this.match;
            }

            set
            {
                this.match = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether basic information should be included.
        /// </summary>
        public bool IncludeBasicInformation
        {
            get
            {
                return this.includeBasicInformation;
            }

            set
            {
                this.includeBasicInformation = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether rally length should be included.
        /// </summary>
        public bool IncludeRallyLength
        {
            get
            {
                return this.includeRallyLength;
            }

            set
            {
                this.includeRallyLength = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether transition statistics should be included.
        /// </summary>
        public bool IncludeTransitions
        {
            get
            {
                return this.includeTransitions;
            }

            set
            {
                this.includeTransitions = value;
            }
        }

        /// <summary>
        /// Creates all statistics for the report
        /// </summary>
        /// <returns>The created <see cref="Report"/>.</returns>
        public Report CreateReport()
        {
            Report report = new Report();

            report.BasicInformations = this.IncludeBasicInformation ? this.CreateBasicInformations() : null;
            report.RallyLength = this.IncludeRallyLength ? this.CreateRallyLengthStatistics() : null;
            report.Transitions = this.IncludeTransitions ? this.CreateTranstionStatistics() : null;

            return report;
        }

        /// <summary>
        /// Generates a PDF report.
        /// </summary>
        /// <param name="fileName">The file name to save the PDF report to.</param>
        public void CreatePDF(string fileName)
        {
            PDFGenerator generator = new PDFGenerator(this.CreateReport());
            Document document = generator.GetPDFDocument();
        }

        /// <summary>
        /// Creates the basic information of the match.
        /// </summary>
        /// <returns>The list of <see cref="BasicInformation"/>.</returns>
        private List<BasicInformation> CreateBasicInformations()
        {
            BasicInformation playerAInfo = new BasicInformation();
            BasicInformation playerBInfo = new BasicInformation();
            List<BasicInformation> infos = new List<BasicInformation>();
            infos.Add(playerAInfo);
            infos.Add(playerBInfo);

            //// add the name
            playerAInfo.Name = this.Match.FirstPlayer.Name;
            playerBInfo.Name = this.Match.SecondPlayer.Name;

            int playerAServing = 0;
            int playerBServing = 0;

            int playerAWinningService = 0;
            int playerBWinningService = 0;

            foreach (Rally rally in this.match.Rallies)
            {
                if (rally.IsEndOfSet)
                {
                    //// add the final score of each set to the results
                    playerAInfo.Results.Add(rally.FinalRallyScore.First);
                    playerBInfo.Results.Add(rally.FinalRallyScore.Second);

                    //// add the points of each set to the total points.
                    playerAInfo.TotalPoints += rally.FinalRallyScore.First;
                    playerBInfo.TotalPoints += rally.FinalRallyScore.Second;

                    //// increase the number of winning sets
                    playerAInfo.WinningSets += rally.FinalRallyScore.First > rally.FinalRallyScore.Second ? 1 : 0;
                    playerBInfo.WinningSets += rally.FinalRallyScore.First < rally.FinalRallyScore.Second ? 1 : 0;
                }

                playerAServing += rally.Server == MatchPlayer.First ? 1 : 0;
                playerBServing += rally.Server == MatchPlayer.Second ? 1 : 0;

                playerAWinningService += rally.Server == MatchPlayer.First && rally.Winner == MatchPlayer.First ? 1 : 0;
                playerBWinningService += rally.Server == MatchPlayer.Second && rally.Winner == MatchPlayer.Second ? 1 : 0;
            }

            //// if the last rally is not the end of a set, add all missing information.
            if (!this.match.Rallies.Last().IsEndOfSet)
            {
                int finalScoreFirst = this.match.Rallies.Last().FinalRallyScore.First;
                int finalScoreSecond = this.match.Rallies.Last().FinalRallyScore.Second;

                playerAInfo.Results.Add(finalScoreFirst);
                playerBInfo.Results.Add(finalScoreSecond);

                playerAInfo.TotalPoints += this.match.Rallies.Last().FinalRallyScore.First;
                playerBInfo.TotalPoints += this.match.Rallies.Last().FinalRallyScore.Second;
            }

            //// compute the winning probability
            int totalPoints = playerAInfo.TotalPoints + playerBInfo.TotalPoints;
            playerAInfo.WinningProbability = (double)playerAInfo.TotalPoints / totalPoints;
            playerBInfo.WinningProbability = (double)playerBInfo.TotalPoints / totalPoints;

            //// compute the winning probability when the player is the serving player
            playerAInfo.ServingWinningProbability = playerAServing == 0 ? 0 : (double)playerAWinningService / (double)playerAServing;
            playerBInfo.ServingWinningProbability = playerBServing == 0 ? 0 : (double)playerBWinningService / (double)playerBServing;

            //// compute the competition performance
            //// =WENN(D2>E2;WENN(D2=11;D2-E2-11;-11+1/(D2-11));WENN(E2=11;D2-E2-11;-11-1/(E2-11)))
            //// valueA += WENN(pointsA>PointsB;
            ////     WENN(PointsA=11;
            ////        PointsA-PointsB-11;
            ////        -11+1/(PointsA-11));
            ////     WENN(PointsB=11;
            ////        PointsA-PointsB-11;
            ////        -11-1/(PointsB-11)))
            ////
            //// =WENN(D2>E2;WENN(D2=11;E2-D2-11;-11-1/(D2-11));WENN(E2=11;E2-D2-11;-11+1/(E2-11)))
            ////  valueB += WENN(PointsA>PointsB;
            ////    WENN(PointsA=11;
            ////        PointsB-PointsA-11;
            ////        -11-1/(PointsA-11));
            ////    WENN(PointsB=11;
            ////        PointsB-PointsA-11;
            ////        -11+1/(PointsB-11)))
            double valueA = 0;
            double valueB = 0;

            for (var i = 0; i < playerAInfo.Results.Count(); i++)
            {
                var pointsA = playerAInfo.Results[i];
                var pointsB = playerBInfo.Results[i];

                if (pointsA > pointsB)
                {
                    valueA += Math.Pow(pointsA == 11 ? pointsA - pointsB - 11 : -11 + (1 / (pointsA - 11)), 2);
                    valueB += Math.Pow(pointsA == 11 ? pointsB - pointsA - 11 : -11 - (1 / (pointsA - 11)), 2);
                }
                else
                {
                    valueA += Math.Pow(pointsB == 11 ? pointsA - pointsB - 11 : -11 - (1 / (pointsB - 11)), 2);
                    valueB += Math.Pow(pointsB == 11 ? pointsB - pointsA - 11 : -11 + (1 / (pointsB - 11)), 2);
                }
            }

            playerAInfo.CompetitionPerformance = 22 - Math.Sqrt(valueA / playerAInfo.Results.Count());
            playerBInfo.CompetitionPerformance = 22 - Math.Sqrt(valueB / playerBInfo.Results.Count());

            return infos;
        }

        /// <summary>
        /// Creates the rally length statistics
        /// </summary>
        /// <returns>The created  <see cref="RallyLengthStatistics"/></returns>
        private RallyLengthStatistics CreateRallyLengthStatistics()
        {
            RallyLengthStatistics stats = new RallyLengthStatistics();

            //// stats for serving player A
            var servingA = this.Match.Rallies
                .Where(r => r.Server == MatchPlayer.First)
                .OrderBy(l => l.Length)
                .ToList();
            stats.ServiceA.Mean = servingA.Select(r => r.Length).Average();
            stats.ServiceA.Median = servingA[(int)Math.Ceiling(servingA.Count / 2.0)].Length;

            //// stats for winner A
            var winnersA = this.Match.Rallies
                .Where(r => r.Winner == MatchPlayer.First)
                .OrderBy(l => l.Length)
                .ToList();
            stats.WinnerA.Mean = winnersA.Select(r => r.Length).Average();
            stats.WinnerA.Median = winnersA[(int)Math.Ceiling(winnersA.Count / 2.0)].Length;

            //// stats for winner A when A is serving player
            var winnersAByServingA = winnersA
                .Where(r => r.Server == MatchPlayer.First)
                .OrderBy(l => l.Length)
                .ToList();
            stats.WinnerAWithServiceA.Mean = winnersAByServingA.Select(r => r.Length).Average();
            stats.WinnerAWithServiceA.Median = winnersAByServingA[(int)Math.Ceiling(winnersAByServingA.Count / 2.0)].Length;

            //// stats for winner A when B is serving player
            var winnersAByServingB = winnersA
                .Where(r => r.Server == MatchPlayer.First)
                .OrderBy(l => l.Length)
                .ToList();
            stats.WinnerAWithServiceB.Mean = winnersAByServingB.Select(r => r.Length).Average();
            stats.WinnerAWithServiceB.Median = winnersAByServingB[(int)Math.Ceiling(winnersAByServingB.Count / 2.0)].Length;

            //// stats for serving player B
            var servingB = this.Match.Rallies
                .Where(r => r.Server == MatchPlayer.Second)
                .OrderBy(l => l.Length)
                .ToList();
            stats.ServiceB.Mean = servingB.Select(r => r.Length).Average();
            stats.ServiceB.Median = servingB[(int)Math.Ceiling(servingB.Count / 2.0)].Length;

            //// stats for winner B
            var winnersB = this.Match.Rallies
                .Where(r => r.Winner == MatchPlayer.Second)
                .OrderBy(l => l.Length)
                .ToList();
            stats.WinnerB.Mean = winnersB.Select(r => r.Length).Average();
            stats.WinnerB.Median = winnersB[(int)Math.Ceiling(winnersB.Count / 2.0)].Length;

            //// stats for winner B when A is serving player
            var winnersBByServingA = winnersA
                .Where(r => r.Server == MatchPlayer.First)
                .OrderBy(l => l.Length)
                .ToList();
            stats.WinnerBWithServiceA.Mean = winnersBByServingA.Select(r => r.Length).Average();
            stats.WinnerBWithServiceA.Median = winnersBByServingA[(int)Math.Ceiling(winnersBByServingA.Count / 2.0)].Length;

            //// stats for winner B when B is serving player
            var winnersBByServingB = winnersA
                .Where(r => r.Server == MatchPlayer.First)
                .OrderBy(l => l.Length)
                .ToList();
            stats.WinnerBWithServiceB.Mean = winnersBByServingB.Select(r => r.Length).Average();
            stats.WinnerBWithServiceB.Median = winnersBByServingB[(int)Math.Ceiling(winnersBByServingB.Count / 2.0)].Length;

            return stats;
        }

        /// <summary>
        /// Creates the transition statistics
        /// </summary>
        /// <returns>The created list of <see cref="AbsoluteNumberOfTransition"/></returns>
        private List<AbsoluteNumberOfTransition> CreateTranstionStatistics()
        {
            var transitionsA = new AbsoluteNumberOfTransition();

            var transitionsB = new AbsoluteNumberOfTransition();

            // for each transition lenght, determine all statistics
            for (int i = 1; i < 7; i++)
            {
                //// statistics for A
                //// server is a and length is more or equal to i
                var transitionsForA = this.Match.Rallies
               .Where(r => r.Length > i && r.Server == MatchPlayer.First)
               .ToList();
                transitionsA.Transitions.Add(transitionsForA.Count);

                //// A is server and winner and the rally length is exactly i
                var pointsForA = this.Match.Rallies
                    .Where(r => r.Length == i && r.Winner == MatchPlayer.First)
                    .ToList();
                transitionsA.PointsTransitionServer.Add(pointsForA.Count);

                //// A is server and B is winner and the rally length is exactly i
                var pointsForBServerA = this.Match.Rallies
                    .Where(r => r.Length == i && r.Winner == MatchPlayer.Second)
                    .ToList();
                transitionsA.PointsTransitionReceiver.Add(pointsForBServerA.Count);

                var transProbA = (double)transitionsForA.Count / ((double)transitionsForA.Count + (double)pointsForA.Count + (double)pointsForBServerA.Count);
                transitionsA.TransitionProbabilities.Add(transProbA);

                var transProbWinA = (double)pointsForA.Count / ((double)transitionsForA.Count + (double)pointsForA.Count + (double)pointsForBServerA.Count);
                transitionsA.PointsPropabilitiesServer.Add(transProbWinA);

                var transProbWinBServerA = (double)pointsForBServerA.Count / ((double)transitionsForA.Count + (double)pointsForA.Count + (double)pointsForBServerA.Count);
                transitionsA.PointsPropabilitiesReceiver.Add(transProbWinBServerA);

                //// -------------------------------------------------------------------
                //// statistics for B
                //// server is B and length is more or equal to i
                var transitionsForB = this.Match.Rallies
              .Where(r => r.Length > i && r.Server == MatchPlayer.Second)
              .ToList();
                transitionsB.Transitions.Add(transitionsForB.Count);

                //// B is server and winner and the rally length is exactly i
                var pointsForB = this.Match.Rallies
                    .Where(r => r.Length == i && r.Winner == MatchPlayer.Second)
                    .ToList();
                transitionsB.PointsTransitionServer.Add(pointsForB.Count);

                //// B is server and A is winner and the rally length is exactly i
                var pointsForAServerB = this.Match.Rallies
                    .Where(r => r.Length == i && r.Winner == MatchPlayer.First)
                    .ToList();
                transitionsB.PointsTransitionReceiver.Add(pointsForAServerB.Count);

                var transProbB = (double)transitionsForB.Count / ((double)transitionsForB.Count + (double)pointsForB.Count + (double)pointsForAServerB.Count);
                transitionsB.TransitionProbabilities.Add(transProbB);

                var transProbWinB = (double)pointsForB.Count / ((double)transitionsForB.Count + (double)pointsForB.Count + (double)pointsForAServerB.Count);
                transitionsB.PointsPropabilitiesServer.Add(transProbWinB);

                var transProbWinAServerB = (double)pointsForAServerB.Count / ((double)transitionsForB.Count + (double)pointsForB.Count + (double)pointsForAServerB.Count);
                transitionsB.PointsPropabilitiesReceiver.Add(transProbWinAServerB);
            }

            //// statistics for B
            //// server is B and length is greater than 6
            var transitionsForAMax = this.Match.Rallies
           .Where(r => r.Length > 6 && r.Server == MatchPlayer.First)
           .ToList();
            transitionsB.Transitions.Add(transitionsForAMax.Count);

            //// A is server and winner and the rally length is greater than 6 
            var pointsForAMax = this.Match.Rallies
                .Where(r => r.Winner == MatchPlayer.First)
                .ToList();
            transitionsA.PointsTransitionServer.Add(pointsForAMax.Count);

            //// A is server and B is winner and the rally length is greater than 6
            var pointsForBServerAMax = this.Match.Rallies
                .Where(r => r.Winner == MatchPlayer.Second)
                .ToList();
            transitionsA.PointsTransitionReceiver.Add(pointsForBServerAMax.Count);

            var transProbAMax = (double)transitionsForAMax.Count / ((double)transitionsForAMax.Count + (double)pointsForAMax.Count + (double)pointsForBServerAMax.Count);
            transitionsA.TransitionProbabilities.Add(transProbAMax);

            var transProbWinAMax = (double)pointsForAMax.Count / ((double)transitionsForAMax.Count + (double)pointsForAMax.Count + (double)pointsForBServerAMax.Count);
            transitionsA.PointsPropabilitiesServer.Add(transProbWinAMax);

            var transProbWinBServerAMax = (double)pointsForBServerAMax.Count / ((double)transitionsForAMax.Count + (double)pointsForAMax.Count + (double)pointsForBServerAMax.Count);
            transitionsA.PointsPropabilitiesReceiver.Add(transProbWinBServerAMax);

            //// statistics for B
            //// server is B and the rally length is greater than 6
            var transitionsForBMax = this.Match.Rallies
           .Where(r => r.Length > 6 && r.Server == MatchPlayer.Second)
           .ToList();
            transitionsA.Transitions.Add(transitionsForBMax.Count);

            //// B is server and winner and the rally length is greater than 6
            var pointsForBMax = this.Match.Rallies
                .Where(r => r.Winner == MatchPlayer.Second)
                .ToList();
            transitionsB.PointsTransitionServer.Add(pointsForBMax.Count);

            //// B is server and A is winner the rally length is greater than 6
            var pointsForAServerBMax = this.Match.Rallies
                .Where(r => r.Winner == MatchPlayer.First)
                .ToList();
            transitionsB.PointsTransitionReceiver.Add(pointsForAServerBMax.Count);

            var transProbBMax = (double)transitionsForBMax.Count / ((double)transitionsForBMax.Count + (double)pointsForBMax.Count + (double)pointsForAServerBMax.Count);
            transitionsB.TransitionProbabilities.Add(transProbBMax);

            var transProbWinBMax = (double)pointsForBMax.Count / ((double)transitionsForBMax.Count + (double)pointsForBMax.Count + (double)pointsForAServerBMax.Count);
            transitionsB.PointsPropabilitiesServer.Add(transProbWinBMax);

            var transProbWinAServerBMax = (double)pointsForAServerBMax.Count / ((double)transitionsForBMax.Count + (double)pointsForBMax.Count + (double)pointsForAServerBMax.Count);
            transitionsB.PointsPropabilitiesReceiver.Add(transProbWinAServerBMax);

            var list = new List<AbsoluteNumberOfTransition>();
            list.Add(transitionsA);
            list.Add(transitionsB);
            return list;
        }
    }
}