//-----------------------------------------------------------------------
// <copyright file="Rally.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace TT.Lib.Models { 

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
        /// Backs the <see cref="Schläge"/> property.
        /// </summary>
        private ObservableCollection<Schlag> schläge = new ObservableCollection<Schlag>();

        /// <summary>
        /// Backs the <see cref="Winner"/> property.
        /// </summary>
        private MatchPlayer winner = MatchPlayer.None;

        /// <summary>
        /// Backs the <see cref="Length"/> property.
        /// </summary>
        //private int length;

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
        private int nummer = 1;

        /// <summary>
        /// Backs the <see cref="Length"/> property.
        /// </summary>
        private int length;

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
            this.schläge.CollectionChanged += this.OnSchlägeChanged;
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
        
        public ObservableCollection<Schlag> Schläge //Wenn das funktioniert, müsste es klappen... aber geht nicht
        {
            get { return this.schläge; }
            set { this.RaiseAndSetIfChanged(ref this.schläge, value); }
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
            set
            {
                this.RaiseAndSetIfChanged(ref this.length, value);
                
                
                //int diff = value - schläge.Count();
                
                //if (schläge.Count < value)
                //{
                //    for (int i = 0; i < diff; i++)
                //    {
                //        schläge.Add(new Schlag());
                //    }

                //}
                //if (schläge.Count > value)
                //{
                //    diff = -diff;
                //    for (int i = 0; i < diff; i++)
                //    {
                //        schläge.Remove(schläge.Last());
                //        //schläge.RemoveAt(schläge.IndexOf(schläge.Last()));
                //    }
                //}

                //if (diff != 0)
                //{
                //    this.RaiseAndSetIfChanged(ref this.length, value);
                //    NotifyPropertyChanged("Schläge");

                //}
            }
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
                this.UpdateScore();
                this.UpdateNummer();
                this.UpdateServer();
            }
        }

        /// <summary>
        /// Updates the nummer of this rally.
        /// </summary>
        private void UpdateNummer()
        {
            var previousRally = this.Playlist.FindPreviousRally(this);

            // We don't need to update the server if there is no previous rally
            if (previousRally != null)
            {
                Nummer = previousRally.Nummer + 1;
            }
            else
            {
                Nummer = 1;
            }
        }

        /// <summary>
        /// Updates the server of this rally.
        /// </summary>
        private void UpdateServer()
        {
            MatchPlayer FirstServer = this.Playlist.Rallies[0].Server;
            var previousRally = this.Playlist.FindPreviousRally(this);

            // We don't need to update the server if there is no previous rally
            if (previousRally != null)
            {
                var prePreviousRally = this.Playlist.FindPreviousRally(previousRally);

                if (previousRally.IsEndOfSet)
                {
                   
                    // The server changes on every set, so each two sets the first server in match serves first again.
                    this.Server = (this.CurrentSetScore.Total % 2 == 0) ?
                        FirstServer : FirstServer.Other();
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
                    MatchPlayer FirstS = this.Playlist.Rallies[0].Server;
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

        /// <summary>
        /// Handles changes to the list of rallies.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private void OnSchlägeChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems != null)
            {
                foreach (var schlag in args.NewItems.Cast<Schlag>())
                {
                    // Connect to each new rally, and update its data.
                    schlag.Rally = this;
                    schlag.PropertyChanged += this.OnSchlagChanged;
                    schlag.Update();
                }
            }
        }

        /// <summary>
        /// Handles a change of a rally.
        /// </summary>
        /// <param name="sender">The changed rally.</param>
        /// <param name="args">The arguments describing the change.</param>
        private void OnSchlagChanged(object sender, PropertyChangedEventArgs args)
        {
            //var rally = (Rally)sender;
            //if (this.rallies.Contains(rally))
            //{
            //    var nextRally = this.FindNextRally(rally);
            //    if (nextRally != null)
            //    {
            //        nextRally.UpdateServerAndScore();
            //    }
            //}
        }

        /// <summary>
        /// Finds the previous stroke.
        /// </summary>
        /// <param name="stroke">The next stroke.</param>
        /// <returns>The previous stroke, or <c>null</c> if there is no previous stroke.</returns>
        public Schlag FindPreviousStroke(Schlag stroke)
        {
            var index = this.schläge.IndexOf(stroke);
            return index >= 0 ? this.schläge.ElementAtOrDefault(index - 1) : null;
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

                if (this.Schläge[now].Verlauf == "Aus")
                    return false;

                if (Double.IsNaN(this.Schläge[now].Platzierung.WX))
                    return false;

                return Math.Abs(Convert.ToInt32(this.Schläge[prev].Platzierung.WX) - Convert.ToInt32(this.Schläge[now].Platzierung.WX)) > 80;
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

                if (this.Schläge[now].Verlauf == "Aus")
                    return false;

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
                if (this.Schläge[now].Verlauf == "Aus" || this.Schläge[now].Verlauf == "Netz")
                    return false;
                if (Double.IsNaN(this.Schläge[now].Platzierung.WX))
                    return false;

                return Math.Abs(Convert.ToInt32(this.Schläge[prev].Platzierung.WX) - Convert.ToInt32(this.Schläge[now].Platzierung.WX)) <= 40;
            }

        }
        #endregion

        #region Helper Methods Statistics

        public bool HasBasisInformationStatistics(int minlegth, string name)
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

        public bool HasTechniqueStatistics(int stroke, string name)
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
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < stroke+3;
                case "ForehandFlipPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandFlipTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Flip";
                case "BackhandFlipPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandFlipDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "BackhandFlipPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler != this.Winner;

                case "AllFlipTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Schlagtechnik.Art == "Flip";
                case "AllFlipPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllFlipDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "AllFlipPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Schlagtechnik.Art == "Flip" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region Push short

                case "ForehandPushShortTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort();
                case "ForehandPushShortPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandPushShortDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "ForehandPushShortPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandPushShortTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort();
                case "BackhandPushShortPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandPushShortDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "BackhandPushShortPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Spieler != this.Winner;

                case "AllPushShortTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf";
                case "AllPushShortPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllPushShortDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "AllPushShortPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsShort() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region Push halflong

                case "ForehandPushHalfLongTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong();
                case "ForehandPushHalfLongPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandPushHalfLongDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "ForehandPushHalfLongPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandPushHalfLongTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong();
                case "BackhandPushHalfLongPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandPushHalfLongDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "BackhandPushHalfLongPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Spieler != this.Winner;

                case "AllPushHalfLongTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf";
                case "AllPushHalfLongPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllPushHalfLongDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "AllPushHalfLongPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler != this.Winner;


                #endregion

                #region Push long

                case "ForehandPushLongTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong();
                case "ForehandPushLongPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandPushLongDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "ForehandPushLongPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandPushLongTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong();
                case "BackhandPushLongPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandPushLongDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "BackhandPushLongPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Spieler != this.Winner;

                case "AllPushLongTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf";
                case "AllPushLongPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllPushLongDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "AllPushLongPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].IsLong() && this.Schläge[stroke].Schlagtechnik.Art == "Schupf" && this.Schläge[stroke].Spieler != this.Winner;


                #endregion

                #region Topspin diagonal

                case "ForehandTopspinDiagonalTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(stroke);
                case "ForehandTopspinDiagonalPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandTopspinDiagonalDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "ForehandTopspinDiagonalPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandTopspinDiagonalTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(stroke);
                case "BackhandTopspinDiagonalPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandTopspinDiagonalDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "BackhandTopspinDiagonalPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "AllTopspinDiagonalTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin";
                case "AllTopspinDiagonalPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllTopspinDiagonalDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "AllTopspinDiagonalPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region Topspin Middle

                case "ForehandTopspinMiddleTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(stroke);
                case "ForehandTopspinMiddlePointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandTopspinMiddleDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "ForehandTopspinMiddlePointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandTopspinMiddleTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(stroke);
                case "BackhandTopspinMiddlePointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandTopspinMiddleDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "BackhandTopspinMiddlePointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "AllTopspinMiddleTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin";
                case "AllTopspinMiddlePointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllTopspinMiddleDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "AllTopspinMiddlePointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region Topspin parallel

                case "ForehandTopspinParallelTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(stroke);
                case "ForehandTopspinParallelPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandTopspinParallelDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "ForehandTopspinParallelPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandTopspinParallelTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(stroke);
                case "BackhandTopspinParallelPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandTopspinParallelDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "BackhandTopspinParallelPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.IsParallel(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "AllTopspinParallelTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin";
                case "AllTopspinParallelPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllTopspinParallelDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "AllTopspinParallelPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Topspin" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region Block diagonal

                case "ForehandBlockDiagonalTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsDiagonal(stroke);
                case "ForehandBlockDiagonalPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandBlockDiagonalDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandBlockDiagonalPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandBlockDiagonalTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsDiagonal(stroke);
                case "BackhandBlockDiagonalPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandBlockDiagonalDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandBlockDiagonalPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "AllBlockDiagonalTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Block";
                case "AllBlockDiagonalPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllBlockDiagonalDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllBlockDiagonalPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region Block Middle

                case "ForehandBlockMiddleTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsMiddle(stroke);
                case "ForehandBlockMiddlePointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandBlockMiddleDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandBlockMiddlePointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandBlockMiddleTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsMiddle(stroke);
                case "BackhandBlockMiddlePointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandBlockMiddleDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandBlockMiddlePointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "AllBlockMiddleTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Block";
                case "AllBlockMiddlePointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllBlockMiddleDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllBlockMiddlePointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region Block parallel

                case "ForehandBlockParallelTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsParallel(stroke);
                case "ForehandBlockParallelPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsParallel(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandBlockParallelDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsParallel(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandBlockParallelPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsParallel(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandBlockParallelTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsParallel(stroke);
                case "BackhandBlockParallelPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsParallel(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandBlockParallelDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsParallel(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandBlockParallelPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.IsParallel(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "AllBlockParallelTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Block";
                case "AllBlockParallelPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllBlockParallelDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllBlockParallelPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(stroke) && this.Schläge[stroke].Schlagtechnik.Art == "Block" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region Chop 
                case "ForehandChopTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schnittabwehr";
                case "ForehandChopPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schnittabwehr" && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandChopDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schnittabwehr" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < stroke + 3;
                case "ForehandChopPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schnittabwehr" && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandChopTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schnittabwehr";
                case "BackhandChopPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schnittabwehr" && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandChopDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schnittabwehr" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandChopPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Schlagtechnik.Art == "Schnittabwehr" && this.Schläge[stroke].Spieler != this.Winner;

                case "AllChopTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Schlagtechnik.Art == "Schnittabwehr";
                case "AllChopPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Schlagtechnik.Art == "Schnittabwehr" && this.Schläge[stroke].Spieler == this.Winner;
                case "AllChopDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Schlagtechnik.Art == "Schnittabwehr" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllChopPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Schlagtechnik.Art == "Schnittabwehr" && this.Schläge[stroke].Spieler != this.Winner;
                #endregion

                #region All Receives 
                case "ForehandReceiveAllTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand";
                case "ForehandReceiveAllPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandReceiveAllDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < stroke + 3;
                case "ForehandReceiveAllPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandReceiveAllTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand";
                case "BackhandReceiveAllPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandReceiveAllDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandReceiveAllPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Spieler != this.Winner;

                case "AllReceiveAllTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand");
                case "AllReceiveAllPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Spieler == this.Winner;
                case "AllReceiveAllDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllReceiveAllPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Spieler != this.Winner;
                #endregion

                default:
                    return true;
            }

        }

        public bool HasContactPositionStatistics(int stroke, string name)
        {
            switch (name)
            {
                case "":
                    return true;

                #region Over the table
                case "OverTheTableTotalButton":
                    return this.Schläge[stroke].Balltreffpunkt == "über";
                case "OverTheTablePointsWonButton":
                    return this.Schläge[stroke].Balltreffpunkt == "über" && this.Schläge[stroke].Spieler == this.Winner;
                case "OverTheTableDirectPointsWonButton":
                    return this.Schläge[stroke].Balltreffpunkt == "über" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "OverTheTablePointsLostButton":
                    return this.Schläge[stroke].Balltreffpunkt == "über" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region at the table
                case "AtTheTableTotalButton":
                    return this.Schläge[stroke].Balltreffpunkt == "hinter";
                case "AtTheTablePointsWonButton":
                    return this.Schläge[stroke].Balltreffpunkt == "hinter" && this.Schläge[stroke].Spieler == this.Winner;
                case "AtTheTableDirectPointsWonButton":
                    return this.Schläge[stroke].Balltreffpunkt == "hinter" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AtTheTablePointsLostButton":
                    return this.Schläge[stroke].Balltreffpunkt == "hinter" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region half distance
                case "HalfDistanceTotalButton":
                    return this.Schläge[stroke].Balltreffpunkt == "Halbdistanz";
                case "HalfDistancePointsWonButton":
                    return this.Schläge[stroke].Balltreffpunkt == "Halbdistanz" && this.Schläge[stroke].Spieler == this.Winner;
                case "HalfDistanceDirectPointsWonButton":
                    return this.Schläge[stroke].Balltreffpunkt == "Halbdistanz" && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "HalfDistancePointsLostButton":
                    return this.Schläge[stroke].Balltreffpunkt == "Halbdistanz" && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                default:
                    return true;

            }

        }

        public bool HasPlacementStatistics(int stroke, string name)
        {
            switch (name)
            {
                case "":
                    return true;

                #region ForehandAll
                case "PlacementForehandAllTotalButton":
                    return this.Schläge[stroke].IsTopLeft() || this.Schläge[stroke].IsMidLeft() || this.Schläge[stroke].IsBotLeft();
                case "PlacementForehandAllPointsWonButton":
                    return (this.Schläge[stroke].IsTopLeft() || this.Schläge[stroke].IsMidLeft() || this.Schläge[stroke].IsBotLeft()) && this.Schläge[stroke].Spieler == this.Winner;
                case "PlacementForehandAllDirectPointsWonButton":
                    return (this.Schläge[stroke].IsTopLeft() || this.Schläge[stroke].IsMidLeft() || this.Schläge[stroke].IsBotLeft()) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "PlacementForehandAllPointsLostButton":
                    return (this.Schläge[stroke].IsTopLeft() || this.Schläge[stroke].IsMidLeft() || this.Schläge[stroke].IsBotLeft()) && this.Schläge[stroke].Spieler != this.Winner;
                #endregion
                #region ForehandLong
                case "PlacementForehandLongTotalButton":
                    return this.Schläge[stroke].IsTopLeft();
                case "PlacementForehandLongPointsWonButton":
                    return this.Schläge[stroke].IsTopLeft() && this.Schläge[stroke].Spieler == this.Winner;
                case "PlacementForehandLongDirectPointsWonButton":
                    return this.Schläge[stroke].IsTopLeft() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "PlacementForehandLongPointsLostButton":
                    return this.Schläge[stroke].IsTopLeft() && this.Schläge[stroke].Spieler != this.Winner;
                #endregion
                #region ForehandHalfLong
                case "PlacementForehandHalfLongTotalButton":
                    return this.Schläge[stroke].IsMidLeft();
                case "PlacementForehandHalfLongPointsWonButton":
                    return this.Schläge[stroke].IsMidLeft() && this.Schläge[stroke].Spieler == this.Winner;
                case "PlacementForehandHalfLongDirectPointsWonButton":
                    return this.Schläge[stroke].IsMidLeft() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "PlacementForehandHalfLongPointsLostButton":
                    return this.Schläge[stroke].IsMidLeft() && this.Schläge[stroke].Spieler != this.Winner;
                #endregion
                #region ForehandShort
                case "PlacementForehandShortTotalButton":
                    return this.Schläge[stroke].IsBotLeft();
                case "PlacementForehandShortPointsWonButton":
                    return this.Schläge[stroke].IsBotLeft() && this.Schläge[stroke].Spieler == this.Winner;
                case "PlacementForehandShortDirectPointsWonButton":
                    return this.Schläge[stroke].IsBotLeft() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "PlacementForehandShortPointsLostButton":
                    return this.Schläge[stroke].IsBotLeft() && this.Schläge[stroke].Spieler != this.Winner;
                #endregion
                #region MiddleAll
                case "PlacementMiddleAllTotalButton":
                    return this.Schläge[stroke].IsTopMid() || this.Schläge[stroke].IsMidMid() || this.Schläge[stroke].IsBotMid();
                case "PlacementMiddleAllPointsWonButton":
                    return (this.Schläge[stroke].IsTopMid() || this.Schläge[stroke].IsMidMid() || this.Schläge[stroke].IsBotMid()) && this.Schläge[stroke].Spieler == this.Winner;
                case "PlacementMiddleAllDirectPointsWonButton":
                    return (this.Schläge[stroke].IsTopMid() || this.Schläge[stroke].IsMidMid() || this.Schläge[stroke].IsBotMid()) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "PlacementMiddleAllPointsLostButton":
                    return (this.Schläge[stroke].IsTopMid() || this.Schläge[stroke].IsMidMid() || this.Schläge[stroke].IsBotMid()) && this.Schläge[stroke].Spieler != this.Winner;
                #endregion
                #region MiddleLong
                case "PlacementMiddleLongTotalButton":
                    return this.Schläge[stroke].IsTopMid();
                case "PlacementMiddleLongPointsWonButton":
                    return this.Schläge[stroke].IsTopMid() && this.Schläge[stroke].Spieler == this.Winner;
                case "PlacementMiddleLongDirectPointsWonButton":
                    return this.Schläge[stroke].IsTopMid() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "PlacementMiddleLongPointsLostButton":
                    return this.Schläge[stroke].IsTopMid() && this.Schläge[stroke].Spieler != this.Winner;
                #endregion
                #region MiddleHalfLong
                case "PlacementMiddleHalfLongTotalButton":
                    return this.Schläge[stroke].IsMidMid();
                case "PlacementMiddleHalfLongPointsWonButton":
                    return this.Schläge[stroke].IsMidMid() && this.Schläge[stroke].Spieler == this.Winner;
                case "PlacementMiddleHalfLongDirectPointsWonButton":
                    return this.Schläge[stroke].IsMidMid() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "PlacementMiddleHalfLongPointsLostButton":
                    return this.Schläge[stroke].IsMidMid() && this.Schläge[stroke].Spieler != this.Winner;
                #endregion
                #region MiddleShort
                case "PlacementMiddleShortTotalButton":
                    return this.Schläge[stroke].IsBotMid();
                case "PlacementMiddleShortPointsWonButton":
                    return this.Schläge[stroke].IsBotMid() && this.Schläge[stroke].Spieler == this.Winner;
                case "PlacementMiddleShortDirectPointsWonButton":
                    return this.Schläge[stroke].IsBotMid() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "PlacementMiddleShortPointsLostButton":
                    return this.Schläge[stroke].IsBotMid() && this.Schläge[stroke].Spieler != this.Winner;
                #endregion
                #region BackhandAll
                case "PlacementBackhandAllTotalButton":
                    return this.Schläge[stroke].IsTopRight() || this.Schläge[stroke].IsMidRight() || this.Schläge[stroke].IsBotRight();
                case "PlacementBackhandAllPointsWonButton":
                    return (this.Schläge[stroke].IsTopRight() || this.Schläge[stroke].IsMidRight() || this.Schläge[stroke].IsBotRight()) && this.Schläge[stroke].Spieler == this.Winner;
                case "PlacementBackhandAllDirectPointsWonButton":
                    return (this.Schläge[stroke].IsTopRight() || this.Schläge[stroke].IsMidRight() || this.Schläge[stroke].IsBotRight()) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "PlacementBackhandAllPointsLostButton":
                    return (this.Schläge[stroke].IsTopRight() || this.Schläge[stroke].IsMidRight() || this.Schläge[stroke].IsBotRight()) && this.Schläge[stroke].Spieler != this.Winner;
                #endregion
                #region BackhandLong
                case "PlacementBackhandLongTotalButton":
                    return this.Schläge[stroke].IsTopRight();
                case "PlacementBackhandLongPointsWonButton":
                    return this.Schläge[stroke].IsTopRight() && this.Schläge[stroke].Spieler == this.Winner;
                case "PlacementBackhandLongDirectPointsWonButton":
                    return this.Schläge[stroke].IsTopRight() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "PlacementBackhandLongPointsLostButton":
                    return this.Schläge[stroke].IsTopRight() && this.Schläge[stroke].Spieler != this.Winner;
                #endregion
                #region BackhandHalfLong
                case "PlacementBackhandHalfLongTotalButton":
                    return this.Schläge[stroke].IsMidRight();
                case "PlacementBackhandHalfLongPointsWonButton":
                    return this.Schläge[stroke].IsMidRight() && this.Schläge[stroke].Spieler == this.Winner;
                case "PlacementBackhandHalfLongDirectPointsWonButton":
                    return this.Schläge[stroke].IsMidRight() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "PlacementBackhandHalfLongPointsLostButton":
                    return this.Schläge[stroke].IsMidRight() && this.Schläge[stroke].Spieler != this.Winner;
                #endregion
                #region BackhandShort
                case "PlacementBackhandShortTotalButton":
                    return this.Schläge[stroke].IsBotRight();
                case "PlacementBackhandShortPointsWonButton":
                    return this.Schläge[stroke].IsBotRight() && this.Schläge[stroke].Spieler == this.Winner;
                case "PlacementBackhandShortDirectPointsWonButton":
                    return this.Schläge[stroke].IsBotRight() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "PlacementBackhandShortPointsLostButton":
                    return this.Schläge[stroke].IsBotRight() && this.Schläge[stroke].Spieler != this.Winner;
                #endregion
                #region AllLong
                case "PlacementAllLongTotalButton":
                    return this.Schläge[stroke].IsLong();
                case "PlacementAllLongPointsWonButton":
                    return this.Schläge[stroke].IsLong() && this.Schläge[stroke].Spieler == this.Winner;
                case "PlacementAllLongDirectPointsWonButton":
                    return this.Schläge[stroke].IsLong() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "PlacementAllLongPointsLostButton":
                    return this.Schläge[stroke].IsLong() && this.Schläge[stroke].Spieler != this.Winner;
                #endregion
                #region AllHalfLong
                case "PlacementAllHalfLongTotalButton":
                    return this.Schläge[stroke].IsHalfLong();
                case "PlacementAllHalfLongPointsWonButton":
                    return this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Spieler == this.Winner;
                case "PlacementAllHalfLongDirectPointsWonButton":
                    return this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "PlacementAllHalfLongPointsLostButton":
                    return this.Schläge[stroke].IsHalfLong() && this.Schläge[stroke].Spieler != this.Winner;
                #endregion
                #region AllShort
                case "PlacementAllShortTotalButton":
                    return this.Schläge[stroke].IsShort();
                case "PlacementAllShortPointsWonButton":
                    return this.Schläge[stroke].IsShort() && this.Schläge[stroke].Spieler == this.Winner;
                case "PlacementAllShortDirectPointsWonButton":
                    return this.Schläge[stroke].IsShort() && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke+3);
                case "PlacementAllShortPointsLostButton":
                    return this.Schläge[stroke].IsShort() && this.Schläge[stroke].Spieler != this.Winner;
                #endregion

                #region ReceiveErrors
                case "PlacementAllServiceErrorsTotalButton":
                    if ((stroke+1) % 2 == 1)
                    {
                        return this.Server != this.Winner && this.Length == (stroke+1);
                    }
                    else if ((stroke + 1) % 2 == 0)
                    {
                        return this.Server == this.Winner && this.Length == (stroke+1);
                    }
                    else
                        return true;
                #endregion
                default:
                    return true;
            }
        }

        public bool HasStepAroundStatistics(int stroke, string name)
        {
            switch (name)
            {
                case "":
                    return true;


                    #region StepAround Inside-Out
                    

                case "ForehandStepAroundInsideOutTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen && this.IsDiagonal(stroke);
                case "ForehandStepAroundInsideOutPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandStepAroundInsideOutDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandStepAroundInsideOutPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandStepAroundInsideOutTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen && this.IsDiagonal(stroke);
                case "BackhandStepAroundInsideOutPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandStepAroundInsideOutDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandStepAroundInsideOutPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen && this.IsDiagonal(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "AllStepAroundInsideOutTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(stroke) && this.Schläge[stroke].Umlaufen;
                case "AllStepAroundInsideOutPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(stroke) && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler == this.Winner;
                case "AllStepAroundInsideOutDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(stroke) && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllStepAroundInsideOutPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsDiagonal(stroke) && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region StepAround Middle

                case "ForehandStepAroundMiddleTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen && this.IsMiddle(stroke);
                case "ForehandStepAroundMiddlePointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandStepAroundMiddleDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandStepAroundMiddlePointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandStepAroundMiddleTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen && this.IsMiddle(stroke);
                case "BackhandStepAroundMiddlePointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandStepAroundMiddleDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandStepAroundMiddlePointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen && this.IsMiddle(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "AllStepAroundMiddleTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(stroke) && this.Schläge[stroke].Umlaufen;
                case "AllStepAroundMiddlePointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(stroke) && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler == this.Winner;
                case "AllStepAroundMiddleDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(stroke) && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllStepAroundMiddlePointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsMiddle(stroke) && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region StepAround parallel

                case "ForehandStepAroundParallelTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen && this.IsParallel(stroke);
                case "ForehandStepAroundParallelPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen && this.IsParallel(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandStepAroundParallelDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen && this.IsParallel(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandStepAroundParallelPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen && this.IsParallel(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandStepAroundParallelTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen && this.IsParallel(stroke);
                case "BackhandStepAroundParallelPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen && this.IsParallel(stroke) && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandStepAroundParallelDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen && this.IsParallel(stroke) && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandStepAroundParallelPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen && this.IsParallel(stroke) && this.Schläge[stroke].Spieler != this.Winner;

                case "AllStepAroundParallelTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(stroke) && this.Schläge[stroke].Umlaufen;
                case "AllStepAroundParallelPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(stroke) && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler == this.Winner;
                case "AllStepAroundParallelDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(stroke) && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllStepAroundParallelPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.IsParallel(stroke) && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler != this.Winner;

                #endregion

                #region StepAround all Directions

                case "ForehandStepAroundAllTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen;
                case "ForehandStepAroundAllPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler == this.Winner;
                case "ForehandStepAroundAllDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandStepAroundAllPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Vorhand" && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler != this.Winner;

                case "BackhandStepAroundAllTotalButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen;
                case "BackhandStepAroundAllPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler == this.Winner;
                case "BackhandStepAroundAllDirectPointsWonButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandStepAroundAllPointsLostButton":
                    return this.Schläge[stroke].Schlägerseite == "Rückhand" && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler != this.Winner;

                case "AllStepAroundAllTotalButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Umlaufen;
                case "AllStepAroundAllPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler == this.Winner;
                case "AllStepAroundAllDirectPointsWonButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllStepAroundAllPointsLostButton":
                    return (this.Schläge[stroke].Schlägerseite == "Vorhand" || this.Schläge[stroke].Schlägerseite == "Rückhand") && this.Schläge[stroke].Umlaufen && this.Schläge[stroke].Spieler != this.Winner;


                #endregion

                default:
                    return true;
            }

        }

        #endregion


    }
}
