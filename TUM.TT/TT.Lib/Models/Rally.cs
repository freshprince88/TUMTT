//-----------------------------------------------------------------------
// <copyright file="Rally.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Models
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    /// <summary>
    /// A rally in a <see cref="Match"/>.
    /// </summary>
    public class Rally : PropertyChangedBase
    {
        /// <summary>
        /// Backs the <see cref="Playlist"/> property.
        /// </summary>
        private Playlist playlist;

        /// <summary>
        /// Backs the <see cref="Rallies"/> property.
        /// </summary>
        private ObservableCollection<Schlag> schläge = new ObservableCollection<Schlag>();

        /// <summary>
        /// Backs the <see cref="Winner"/> property.
        /// </summary>
        private MatchPlayer winner = MatchPlayer.None;

        /// <summary>
        /// Backs the <see cref="Length"/> property.
        /// </summary>
        private int length;

        /// <summary>
        /// Backs the <see cref="CurrentRallyScore"/> property.
        /// </summary>
        private Score currentRallyScore = new Score(0, 0);

        /// <summary>
        /// Backs the <see cref="CurrentSetScore"/> property.
        /// </summary>
        private Score currentSetScore = new Score(0, 0);

        /// <summary>
        /// Backs the <see cref="Server"/> property.
        /// </summary>
        private MatchPlayer server;

        /// <summary>
        /// Backs the <see cref="Nummer"/> property.
        /// </summary>
        private int nummer;

        /// <summary>
        /// Backs the <see cref="Anfang"/> property.
        /// </summary>
        private double anfang;

        /// <summary>
        /// Backs the <see cref="Ende"/> property.
        /// </summary>
        private double ende;

        /// <summary>
        /// Backs the <see cref="Kommentar"/> property.
        /// </summary>
        private string kommentar;

        /// <summary>
        /// Initializes a new instance of the <see cref="Rally"/> class.
        /// </summary>
        public Rally()
        {
        }

        /// <summary>
        /// Gets or sets the match this rally.
        /// </summary>
        [XmlIgnore]
        public Playlist Playlist
        {
            get { return this.playlist; }
            set { this.RaiseAndSetIfChanged(ref this.playlist, value); }
        }

        /// <summary>
        /// Gets all strokes of this rally.
        /// </summary>
        public ObservableCollection<Schlag> Schläge
        {
            get { return this.schläge; }
        }

        /// <summary>
        /// Gets or sets the winner <see cref="Player"/> of the Rally.
        /// </summary>
        [XmlAttribute]
        public MatchPlayer Winner
        {
            get
            {
                return this.winner;
            }

            set
            {
                if (this.RaiseAndSetIfChanged(ref this.winner, value))
                {
                    // If the winner changes, the final score and end of set state changes, too.
                    this.NotifyPropertyChanged("FinalRallyScore");
                    this.NotifyPropertyChanged("FinalSetScore");
                    this.NotifyPropertyChanged("IsEndOfSet");
                }
            }
        }

        /// <summary>
        /// Gets or sets Number of the Rally.
        /// </summary>
        [XmlAttribute]
        public int Nummer
        {
            get
            {
                return this.nummer;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.nummer, value);
            }
        }

        /// <summary>
        /// Gets or sets video start of the Rally.
        /// </summary>
        [XmlAttribute]
        public double Anfang
        {
            get
            {
                return this.anfang;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.anfang, value);
            }
        }

        /// <summary>
        /// Gets or sets video end of the Rally.
        /// </summary>
        [XmlAttribute]
        public double Ende
        {
            get
            {
                return this.ende;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.ende, value);
            }
        }

        /// <summary>
        /// Gets or sets the comment of the Rally.
        /// </summary>
        [XmlAttribute]
        public string Kommentar
        {
            get
            {
                return this.kommentar;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.kommentar, value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the set is finished
        /// </summary>
        [XmlIgnore]
        public bool IsEndOfSet
        {
            get { return this.FinalRallyScore.WinsRally; }
        }

        /// <summary>
        /// Gets or sets the length of the rally.
        /// </summary>
        [XmlAttribute]
        public int Length
        {
            get { return this.length; }
            set { this.RaiseAndSetIfChanged(ref this.length, value); }
        }

        /// <summary>
        /// Gets or sets the serving player of this rally.
        /// </summary>
        [XmlAttribute]
        public MatchPlayer Server
        {
            get { return this.server; }
            set { this.RaiseAndSetIfChanged(ref this.server, value); }
        }

        /// <summary>
        /// Gets or sets the rally score during this rally.
        /// </summary>
        public Score CurrentRallyScore
        {
            get
            {
                return this.currentRallyScore;
            }

            set
            {
                if (this.RaiseAndSetIfChanged(ref this.currentRallyScore, value))
                {
                    this.NotifyPropertyChanged("FinalRallyScore");
                    this.NotifyPropertyChanged("FinalSetScore");
                    this.NotifyPropertyChanged("IsEndOfSet");
                }
            }
        }

        /// <summary>
        /// Gets the rally score after this rally.
        /// </summary>
        [XmlIgnore]
        public Score FinalRallyScore
        {
            get { return this.CurrentRallyScore.WinningScore(this.Winner); }
        }

        /// <summary>
        /// Gets or sets the set score during this rally.
        /// </summary>
        public Score CurrentSetScore
        {
            get
            {
                return this.currentSetScore;
            }

            set
            {
                if (this.RaiseAndSetIfChanged(ref this.currentSetScore, value))
                {
                    this.NotifyPropertyChanged("FinalSetScore");
                }
            }
        }

        /// <summary>
        /// Gets the set score after this rally.
        /// </summary>
        [XmlIgnore]
        public Score FinalSetScore
        {
            get
            {
                return this.IsEndOfSet ? this.CurrentSetScore.WinningScore(this.Winner) : this.CurrentSetScore;
            }
        }

        /// <summary>
        /// Updates the server and score of this rally.
        /// </summary>
        public void UpdateServerAndScore()
        {
            if (this.Playlist == null)
            {
                throw new InvalidOperationException("Rally not part of a Playlist");
            }
            else
            {
                this.UpdateServer();
                this.UpdateScore();
            }
        }

        /// <summary>
        /// Updates the server of this rally.
        /// </summary>
        private void UpdateServer()
        {
            var previousRally = this.Playlist.FindPreviousRally(this);

            // We don't need to update the server if there is no previous rally
            if (previousRally != null)
            {
                var prePreviousRally = this.Playlist.FindPreviousRally(previousRally);

                if (previousRally.IsEndOfSet)
                {
                    // The server changes on every set, so each two sets the first server in match serves first again.
                    this.Server = (this.CurrentSetScore.Total % 2 == 0) ?
                        this.Playlist.FirstServer : this.Playlist.FirstServer.Other();
                }
                else if (this.CurrentRallyScore.Lowest >= 10)
                {
                    // If the set extends beyond 10:10, server changes on every rally
                    this.Server = previousRally.Server.Other();
                }
                else if (prePreviousRally != null
                    && previousRally.Server == prePreviousRally.Server
                    && !prePreviousRally.IsEndOfSet)
                {
                    // If the last two rallies in *this* set were served by the same player, 
                    // change the serving player for this rally
                    this.Server = previousRally.Server.Other();
                }
                else
                {
                    // Otherwise the same player serves again
                    this.Server = previousRally.Server;
                }
            }
        }

        /// <summary>
        /// Updates the score of this rally.
        /// </summary>
        private void UpdateScore()
        {
            var previousRally = this.Playlist.FindPreviousRally(this);

            // If there is no previous rally, start fresh with zero score.  If the previous rally
            // wins the set, also start with a fresh 
            this.CurrentRallyScore = previousRally != null && !previousRally.IsEndOfSet ?
                previousRally.FinalRallyScore : new Score(0, 0);
            this.CurrentSetScore = previousRally != null ? previousRally.FinalSetScore : new Score(0, 0);
        }

        /// <summary>
        /// Returns the last stroke of the Winner of this rally
        /// </summary>
        public Schlag LastWinnerStroke()
        {
            int StrokeNumber;
            if (Schläge[Convert.ToInt32(Length) - 1].Spieler == Winner)
            {
                StrokeNumber = Convert.ToInt32(Length) - 1;
            }
            else
            {
                StrokeNumber = Convert.ToInt32(Length) - 2;
            }
            return Schläge[StrokeNumber];
        }

        #region Helper Methods
        public Boolean IsDiagonal(int i)
        {
            if (i <= 0)
            {
                return false;
            }
            else
            {
                int now = i;
                int prev = i - 1;

                return Math.Abs(Convert.ToInt32(this.Schläge[prev].Platzierung.WX) - Convert.ToInt32(this.Schläge[now].Platzierung.WX)) > 100;
            }
            
        }

        public Boolean IsMiddle(int i)
        {
            if (i <= 0)
            {
                return false;
            }
            else
            {
                int now = i;
                int prev = i - 1;

                return this.Schläge[now].IsBotMid() || this.Schläge[now].IsMidMid() || this.Schläge[now].IsTopMid();
            }

        }

        public Boolean IsParallel(int i)
        {
            if (i <= 0)
            {
                return false;
            }
            else
            {
                int now = i;
                int prev = i - 1;

                return Math.Abs(Convert.ToInt32(this.Schläge[prev].Platzierung.WX) - Convert.ToInt32(this.Schläge[now].Platzierung.WX)) <= 100;
            }

        }
        #region Helper Methods Statistics

        public Boolean HasBasisInformationStatistics(int minlegth, string name)
        {
            switch (name)
            {
                case "":
                    return true;
                case "TotalReceivesCount":
                    return Convert.ToInt32(this.Length) >= minlegth;
                case "TotalReceivesCountPointPlayer1":
                    return Convert.ToInt32(this.Length) >= minlegth && this.Winner == MatchPlayer.First;
                case "TotalReceivesCountPointPlayer2":
                    return Convert.ToInt32(this.Length) >= minlegth && this.Winner == MatchPlayer.Second;
                default:
                    return true;

            }
        }

        public Boolean HasTechniqueStatistics(int stroke, string name)
        {
            switch (name)
            {
                case "":
                    return true;

                #region Flip 
                case "ForehandFlipTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Flip";
                case "ForehandFlipPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandFlipDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "ForehandFlipPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandFlipTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Flip";
                case "BackhandFlipPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandFlipDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "BackhandFlipPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler != this.Winner;

                case "AllFlipTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Schlagtechnik.Art == "Flip";
                case "AllFlipPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllFlipDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "AllFlipPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region Push short

                case "ForehandPushShortTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort();
                case "ForehandPushShortPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandPushShortDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "ForehandPushShortPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandPushShortTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort();
                case "BackhandPushShortPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandPushShortDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "BackhandPushShortPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Spieler != this.Winner;

                case "AllPushShortTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf";
                case "AllPushShortPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllPushShortDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "AllPushShortPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region Push halflong

                case "ForehandPushHalfLongTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong();
                case "ForehandPushHalfLongPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandPushHalfLongDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "ForehandPushHalfLongPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandPushHalfLongTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong();
                case "BackhandPushHalfLongPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandPushHalfLongDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "BackhandPushHalfLongPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Spieler != this.Winner;

                case "AllPushHalfLongTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf";
                case "AllPushHalfLongPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllPushHalfLongDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "AllPushHalfLongPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler != this.Winner;


                #endregion

                #region Push long

                case "ForehandPushLongTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong();
                case "ForehandPushLongPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandPushLongDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "ForehandPushLongPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandPushLongTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong();
                case "BackhandPushLongPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandPushLongDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "BackhandPushLongPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Spieler != this.Winner;

                case "AllPushLongTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf";
                case "AllPushLongPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllPushLongDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "AllPushLongPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler != this.Winner;


                #endregion

                #region Topspin diagonal

                case "ForehandTopspinDiagonalTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1);
                case "ForehandTopspinDiagonalPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1) && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandTopspinDiagonalDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "ForehandTopspinDiagonalPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1) && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandTopspinDiagonalTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1);
                case "BackhandTopspinDiagonalPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1) && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandTopspinDiagonalDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "BackhandTopspinDiagonalPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(1) && this.Schläge[stroke].Spieler != this.Winner;

                case "AllTopspinDiagonalTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(1) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin";
                case "AllTopspinDiagonalPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(1) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllTopspinDiagonalDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(1) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "AllTopspinDiagonalPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(1) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region Topspin Middle

                case "ForehandTopspinMiddleTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1);
                case "ForehandTopspinMiddlePointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1) && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandTopspinMiddleDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "ForehandTopspinMiddlePointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1) && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandTopspinMiddleTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1);
                case "BackhandTopspinMiddlePointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1) && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandTopspinMiddleDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "BackhandTopspinMiddlePointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(1) && this.Schläge[stroke].Spieler != this.Winner;

                case "AllTopspinMiddleTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(1) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin";
                case "AllTopspinMiddlePointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(1) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllTopspinMiddleDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(1) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "AllTopspinMiddlePointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(1) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region Topspin parallel

                case "ForehandTopspinParallelTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1);
                case "ForehandTopspinParallelPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1) && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandTopspinParallelDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "ForehandTopspinParallelPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1) && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandTopspinParallelTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1);
                case "BackhandTopspinParallelPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1) && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandTopspinParallelDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "BackhandTopspinParallelPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(1) && this.Schläge[stroke].Spieler != this.Winner;

                case "AllTopspinParallelTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(1) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin";
                case "AllTopspinParallelPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(1) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllTopspinParallelDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(1) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < 4;
                case "AllTopspinParallelPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(1) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion





                default:
                    return true;
            }

        }



        #endregion
        #endregion
    }
}
