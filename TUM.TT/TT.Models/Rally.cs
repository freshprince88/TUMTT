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

namespace TT.Models
{

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
        /// Backs the <see cref="Strokes"/> property.
        /// </summary>
        private ObservableCollection<Stroke> schläge = new ObservableCollection<Stroke>();

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
        /// Backs the <see cref="Number"/> property.
        /// </summary>
        private int nummer = 1;

        /// <summary>
        /// Backs the <see cref="Length"/> property.
        /// </summary>
        private int length;

        /// <summary>
        /// Backs the <see cref="Start"/> property.
        /// </summary>
        private double anfang;

        /// <summary>
        /// Backs the <see cref="End"/> property.
        /// </summary>
        private double ende;

        /// <summary>
        /// Backs the <see cref="Comment"/> property.
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

        public ObservableCollection<Stroke> Strokes //Wenn das funktioniert, müsste es klappen... aber geht nicht
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
        public int Number
        {
            get
            {
                return this.nummer;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.nummer, value);
                this.NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets video start of the Rally.
        /// </summary>
        [XmlAttribute]
        public double Start
        {
            get
            {
                return this.anfang;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.anfang, value);
                this.NotifyPropertyChanged("Start");
                this.NotifyPropertyChanged();


            }
        }

        /// <summary>
        /// Gets or sets video end of the Rally.
        /// </summary>
        [XmlAttribute]
        public double End
        {
            get
            {
                return this.ende;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.ende, value);
                this.NotifyPropertyChanged("End");
                this.NotifyPropertyChanged();

            }
        }

        /// <summary>
        /// Gets or sets the comment of the Rally.
        /// </summary>
        [XmlAttribute]
        public string Comment
        {
            get
            {
                return this.kommentar;
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.kommentar, value);
                this.NotifyPropertyChanged("Comment");
                this.NotifyPropertyChanged();


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
                this.NotifyPropertyChanged("Length");
                this.NotifyPropertyChanged();

            }
        }

        /// <summary>
        /// Gets or sets the serving player of this rally.
        /// </summary>
        [XmlAttribute]
        public MatchPlayer Server
        {
            get { return this.server; }
            set
            {
                this.RaiseAndSetIfChanged(ref this.server, value);
                this.NotifyPropertyChanged("Server");
                this.NotifyPropertyChanged();


            }
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
                    this.NotifyPropertyChanged();

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
                    this.NotifyPropertyChanged();

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
                Number = previousRally.Number + 1;
            }
            else
            {
                Number = 1;
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
        public Stroke LastWinnerStroke()
        {
            int StrokeNumber;
            if (Strokes[Convert.ToInt32(Length) - 1].Player == Winner)
            {
                StrokeNumber = Convert.ToInt32(Length) - 1;
            }
            else
            {
                StrokeNumber = Convert.ToInt32(Length) - 2;
            }
            return Strokes[StrokeNumber];
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
                foreach (var schlag in args.NewItems.Cast<Stroke>())
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
        public Stroke FindPreviousStroke(Stroke stroke)
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

                if (this.Strokes[now].Course == "Net/Out")
                    return false;

                if (Double.IsNaN(this.Strokes[now].Placement.WX))
                    return false;

                return Math.Abs(Convert.ToInt32(this.Strokes[prev].Placement.WX) - Convert.ToInt32(this.Strokes[now].Placement.WX)) > 80;
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

                if (this.Strokes[now].Course == "Net/Out")
                    return false;

                return this.Strokes[now].IsBotMid() || this.Strokes[now].IsMidMid() || this.Strokes[now].IsTopMid();
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
                if (this.Strokes[now].Course == "Net/Out")
                    return false;
                if (Double.IsNaN(this.Strokes[now].Placement.WX))
                    return false;

                return Math.Abs(Convert.ToInt32(this.Strokes[prev].Placement.WX) - Convert.ToInt32(this.Strokes[now].Placement.WX)) <= 40;
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
                case "TotalServicesCount":
                    return Convert.ToInt32(this.Length) >= minlegth;
                case "TotalServicesCountPointPlayer1":
                    return Convert.ToInt32(this.Length) >= minlegth && this.Winner == MatchPlayer.First;
                case "TotalServicesCountPointPlayer2":
                    return Convert.ToInt32(this.Length) >= minlegth && this.Winner == MatchPlayer.Second;
                case "TotalReceivesCount":
                    return Convert.ToInt32(this.Length) >= minlegth;
                case "TotalReceivesCountPointPlayer1":
                    return Convert.ToInt32(this.Length) >= minlegth && this.Winner == MatchPlayer.First;
                case "TotalReceivesCountPointPlayer2":
                    return Convert.ToInt32(this.Length) >= minlegth && this.Winner == MatchPlayer.Second;                
                case "TotalThirdBallsCount":
                    return Convert.ToInt32(this.Length) >= minlegth;
                case "TotalThirdBallsCountPointPlayer1":
                    return Convert.ToInt32(this.Length) >= minlegth && this.Winner == MatchPlayer.First;
                case "TotalThirdBallsCountPointPlayer2":
                    return Convert.ToInt32(this.Length) >= minlegth && this.Winner == MatchPlayer.Second;
                case "TotalFourthBallsCount":
                    return Convert.ToInt32(this.Length) >= minlegth;
                case "TotalFourthBallsCountPointPlayer1":
                    return Convert.ToInt32(this.Length) >= minlegth && this.Winner == MatchPlayer.First;
                case "TotalFourthBallsCountPointPlayer2":
                    return Convert.ToInt32(this.Length) >= minlegth && this.Winner == MatchPlayer.Second;

                default:
                    return true;

            }
        }
        public bool HasServiceStatistics(string name)
        {
            switch (name)

            {
                case "":
                    return true;
                #region Pendulum
                case "ForehandPendulumTotalButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Pendulum";
                case "ForehandPendulumPointsWonButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Pendulum" && this.Strokes[0].Player == this.Winner;
                case "ForehandPendulumDirectPointsWonButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Pendulum" && this.Strokes[0].Player == this.Winner && this.Length < 3;
                case "ForehandPendulumPointsLostButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Pendulum" && this.Strokes[0].Player != this.Winner;
                case "BackhandPendulumTotalButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Pendulum";
                case "BackhandPendulumPointsWonButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Pendulum" && this.Strokes[0].Player == this.Winner;
                case "BackhandPendulumDirectPointsWonButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Pendulum" && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "BackhandPendulumPointsLostButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Pendulum" && this.Strokes[0].Player != this.Winner;
                case "AllPendulumTotalButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Pendulum";
                case "AllPendulumPointsWonButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Pendulum" && this.Strokes[0].Player == this.Winner;
                case "AllPendulumDirectPointsWonButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Pendulum" && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "AllPendulumPointsLostButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Pendulum" && this.Strokes[0].Player != this.Winner;

                #endregion

                #region ReversePendulum
                case "ForehandReversePendulumTotalButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Reverse";
                case "ForehandReversePendulumPointsWonButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Reverse" && this.Strokes[0].Player == this.Winner;
                case "ForehandReversePendulumDirectPointsWonButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Reverse" && this.Strokes[0].Player == this.Winner && this.Length < 3;
                case "ForehandReversePendulumPointsLostButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Reverse" && this.Strokes[0].Player != this.Winner;
                case "BackhandReversePendulumTotalButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Reverse";
                case "BackhandReversePendulumPointsWonButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Reverse" && this.Strokes[0].Player == this.Winner;
                case "BackhandReversePendulumDirectPointsWonButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Reverse" && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "BackhandReversePendulumPointsLostButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Reverse" && this.Strokes[0].Player != this.Winner;
                case "AllReversePendulumTotalButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Reverse";
                case "AllReversePendulumPointsWonButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Reverse" && this.Strokes[0].Player == this.Winner;
                case "AllReversePendulumDirectPointsWonButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Reverse" && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "AllReversePendulumPointsLostButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Reverse" && this.Strokes[0].Player != this.Winner;

                #endregion

                #region Tomahawk
                case "ForehandTomahawkTotalButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Tomahawk";
                case "ForehandTomahawkPointsWonButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Tomahawk" && this.Strokes[0].Player == this.Winner;
                case "ForehandTomahawkDirectPointsWonButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Tomahawk" && this.Strokes[0].Player == this.Winner && this.Length < 3;
                case "ForehandTomahawkPointsLostButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Tomahawk" && this.Strokes[0].Player != this.Winner;
                case "BackhandTomahawkTotalButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Tomahawk";
                case "BackhandTomahawkPointsWonButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Tomahawk" && this.Strokes[0].Player == this.Winner;
                case "BackhandTomahawkDirectPointsWonButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Tomahawk" && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "BackhandTomahawkPointsLostButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Tomahawk" && this.Strokes[0].Player != this.Winner;
                case "AllTomahawkTotalButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Tomahawk";
                case "AllTomahawkPointsWonButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Tomahawk" && this.Strokes[0].Player == this.Winner;
                case "AllTomahawkDirectPointsWonButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Tomahawk" && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "AllTomahawkPointsLostButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Tomahawk" && this.Strokes[0].Player != this.Winner;

                #endregion

                #region Special
                case "ForehandSpecialTotalButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Special";
                case "ForehandSpecialPointsWonButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Special" && this.Strokes[0].Player == this.Winner;
                case "ForehandSpecialDirectPointsWonButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Special" && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "ForehandSpecialPointsLostButton":
                    return this.Strokes[0].Side == "Forehand" && this.Strokes[0].Servicetechnique == "Special" && this.Strokes[0].Player != this.Winner;
                case "BackhandSpecialTotalButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Special";
                case "BackhandSpecialPointsWonButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Special" && this.Strokes[0].Player == this.Winner;
                case "BackhandSpecialDirectPointsWonButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Special" && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "BackhandSpecialPointsLostButton":
                    return this.Strokes[0].Side == "Backhand" && this.Strokes[0].Servicetechnique == "Special" && this.Strokes[0].Player != this.Winner;
                case "AllSpecialTotalButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Special";
                case "AllSpecialPointsWonButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Special" && this.Strokes[0].Player == this.Winner;
                case "AllSpecialDirectPointsWonButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Special" && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "AllSpecialPointsLostButton":
                    return (this.Strokes[0].Side == "Forehand" || this.Strokes[0].Side == "Backhand") && this.Strokes[0].Servicetechnique == "Special" && this.Strokes[0].Player != this.Winner;

                #endregion

                #region All Forehand Services
                case "ForehandAllTotalButton":
                    return this.Strokes[0].Side == "Forehand" && (this.Strokes[0].Servicetechnique == "Pendulum" || this.Strokes[0].Servicetechnique == "Reverse" || this.Strokes[0].Servicetechnique == "Tomahawk" || this.Strokes[0].Servicetechnique == "Special");
                case "ForehandAllPointsWonButton":
                    return this.Strokes[0].Side == "Forehand" && (this.Strokes[0].Servicetechnique == "Pendulum" || this.Strokes[0].Servicetechnique == "Reverse" || this.Strokes[0].Servicetechnique == "Tomahawk" || this.Strokes[0].Servicetechnique == "Special") && this.Strokes[0].Player == this.Winner;
                case "ForehandAllDirectPointsWonButton":
                    return this.Strokes[0].Side == "Forehand" && (this.Strokes[0].Servicetechnique == "Pendulum" || this.Strokes[0].Servicetechnique == "Reverse" || this.Strokes[0].Servicetechnique == "Tomahawk" || this.Strokes[0].Servicetechnique == "Special") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "ForehandAllPointsLostButton":
                    return this.Strokes[0].Side == "Forehand" && (this.Strokes[0].Servicetechnique == "Pendulum" || this.Strokes[0].Servicetechnique == "Reverse" || this.Strokes[0].Servicetechnique == "Tomahawk" || this.Strokes[0].Servicetechnique == "Special") && this.Strokes[0].Player != this.Winner;
                #endregion

                #region All Backhand Services
                case "BackhandAllTotalButton":
                    return this.Strokes[0].Side == "Backhand" && (this.Strokes[0].Servicetechnique == "Pendulum" || this.Strokes[0].Servicetechnique == "Reverse" || this.Strokes[0].Servicetechnique == "Tomahawk" || this.Strokes[0].Servicetechnique == "Special");
                case "BackhandAllPointsWonButton":
                    return this.Strokes[0].Side == "Backhand" && (this.Strokes[0].Servicetechnique == "Pendulum" || this.Strokes[0].Servicetechnique == "Reverse" || this.Strokes[0].Servicetechnique == "Tomahawk" || this.Strokes[0].Servicetechnique == "Special") && this.Strokes[0].Player == this.Winner;
                case "BackhandAllDirectPointsWonButton":
                    return this.Strokes[0].Side == "Backhand" && (this.Strokes[0].Servicetechnique == "Pendulum" || this.Strokes[0].Servicetechnique == "Reverse" || this.Strokes[0].Servicetechnique == "Tomahawk" || this.Strokes[0].Servicetechnique == "Special") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "BackhandAllPointsLostButton":
                    return this.Strokes[0].Side == "Backhand" && (this.Strokes[0].Servicetechnique == "Pendulum" || this.Strokes[0].Servicetechnique == "Reverse" || this.Strokes[0].Servicetechnique == "Tomahawk" || this.Strokes[0].Servicetechnique == "Special") && this.Strokes[0].Player != this.Winner;
                #endregion


                #region All Services
                case "AllServicesTotalButton":
                    return (this.Strokes[0].Side == "Backhand" || this.Strokes[0].Side == "Forehand") && (this.Strokes[0].Servicetechnique == "Pendulum" || this.Strokes[0].Servicetechnique == "Reverse" || this.Strokes[0].Servicetechnique == "Tomahawk" || this.Strokes[0].Servicetechnique == "Special");
                case "AllServicesPointsWonButton":
                    return (this.Strokes[0].Side == "Backhand" || this.Strokes[0].Side == "Forehand") && (this.Strokes[0].Servicetechnique == "Pendulum" || this.Strokes[0].Servicetechnique == "Reverse" || this.Strokes[0].Servicetechnique == "Tomahawk" || this.Strokes[0].Servicetechnique == "Special") && this.Strokes[0].Player == this.Winner;
                case "AllServicesDirectPointsWonButton":
                    return (this.Strokes[0].Side == "Backhand" || this.Strokes[0].Side == "Forehand") && (this.Strokes[0].Servicetechnique == "Pendulum" || this.Strokes[0].Servicetechnique == "Reverse" || this.Strokes[0].Servicetechnique == "Tomahawk" || this.Strokes[0].Servicetechnique == "Special") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "AllServicesPointsLostButton":
                    return (this.Strokes[0].Side == "Backhand" || this.Strokes[0].Side == "Forehand") && (this.Strokes[0].Servicetechnique == "Pendulum" || this.Strokes[0].Servicetechnique == "Reverse" || this.Strokes[0].Servicetechnique == "Tomahawk" || this.Strokes[0].Servicetechnique == "Special") && this.Strokes[0].Player != this.Winner;
                #endregion
                default:
                    return true;
            }
        }


        public bool HasServerPositionStatistics(string name)
        {
            switch (name)
            {
                case "":
                    return true;

                #region Position Left
                case "PositionLeftTotalButton":
                    return this.Strokes[0].IsLeftServicePosition();
                case "PositionLeftPointsWonButton":
                    return this.Strokes[0].IsLeftServicePosition() && this.Strokes[0].Player == this.Winner;
                case "PositionLeftDirectPointsWonButton":
                    return this.Strokes[0].IsLeftServicePosition() && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "PositionLeftPointsLostButton":
                    return this.Strokes[0].IsLeftServicePosition() && this.Strokes[0].Player != this.Winner;
                #endregion
                #region Position Middle
                case "PositionMiddleTotalButton":
                    return this.Strokes[0].IsMiddleServicePosition();
                case "PositionMiddlePointsWonButton":
                    return this.Strokes[0].IsMiddleServicePosition() && this.Strokes[0].Player == this.Winner;
                case "PositionMiddleDirectPointsWonButton":
                    return this.Strokes[0].IsMiddleServicePosition() && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "PositionMiddlePointsLostButton":
                    return this.Strokes[0].IsMiddleServicePosition() && this.Strokes[0].Player != this.Winner;
                #endregion
                #region Position Right
                case "PositionRightTotalButton":
                    return this.Strokes[0].IsRightServicePosition();
                case "PositionRightPointsWonButton":
                    return this.Strokes[0].IsRightServicePosition() && this.Strokes[0].Player == this.Winner;
                case "PositionRightDirectPointsWonButton":
                    return this.Strokes[0].IsRightServicePosition() && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "PositionRightPointsLostButton":
                    return this.Strokes[0].IsRightServicePosition() && this.Strokes[0].Player != this.Winner;
                #endregion

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
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Flip";
                case "ForehandFlipPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Flip" && this.Strokes[stroke].Player == this.Winner;
                case "ForehandFlipDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Flip" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < stroke + 3;
                case "ForehandFlipPointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Flip" && this.Strokes[stroke].Player != this.Winner;

                case "BackhandFlipTotalButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Flip";
                case "BackhandFlipPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Flip" && this.Strokes[stroke].Player == this.Winner;
                case "BackhandFlipDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Flip" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandFlipPointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Flip" && this.Strokes[stroke].Player != this.Winner;

                case "AllFlipTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].Stroketechnique.Type == "Flip";
                case "AllFlipPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].Stroketechnique.Type == "Flip" && this.Strokes[stroke].Player == this.Winner;
                case "AllFlipDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].Stroketechnique.Type == "Flip" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllFlipPointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].Stroketechnique.Type == "Flip" && this.Strokes[stroke].Player != this.Winner;

                #endregion

                #region Push short

                case "ForehandPushShortTotalButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsShort();
                case "ForehandPushShortPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsShort() && this.Strokes[stroke].Player == this.Winner;
                case "ForehandPushShortDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsShort() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandPushShortPointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsShort() && this.Strokes[stroke].Player != this.Winner;

                case "BackhandPushShortTotalButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsShort();
                case "BackhandPushShortPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsShort() && this.Strokes[stroke].Player == this.Winner;
                case "BackhandPushShortDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsShort() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandPushShortPointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsShort() && this.Strokes[stroke].Player != this.Winner;

                case "AllPushShortTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].IsShort() && this.Strokes[stroke].Stroketechnique.Type == "Push";
                case "AllPushShortPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].IsShort() && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].Player == this.Winner;
                case "AllPushShortDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].IsShort() && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllPushShortPointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].IsShort() && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].Player != this.Winner;

                #endregion

                #region Push halflong

                case "ForehandPushHalfLongTotalButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsHalfLong();
                case "ForehandPushHalfLongPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsHalfLong() && this.Strokes[stroke].Player == this.Winner;
                case "ForehandPushHalfLongDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsHalfLong() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandPushHalfLongPointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsHalfLong() && this.Strokes[stroke].Player != this.Winner;

                case "BackhandPushHalfLongTotalButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsHalfLong();
                case "BackhandPushHalfLongPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsHalfLong() && this.Strokes[stroke].Player == this.Winner;
                case "BackhandPushHalfLongDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsHalfLong() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandPushHalfLongPointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsHalfLong() && this.Strokes[stroke].Player != this.Winner;

                case "AllPushHalfLongTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].IsHalfLong() && this.Strokes[stroke].Stroketechnique.Type == "Push";
                case "AllPushHalfLongPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].IsHalfLong() && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].Player == this.Winner;
                case "AllPushHalfLongDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].IsHalfLong() && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllPushHalfLongPointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].IsHalfLong() && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].Player != this.Winner;


                #endregion

                #region Push long

                case "ForehandPushLongTotalButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsLong();
                case "ForehandPushLongPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsLong() && this.Strokes[stroke].Player == this.Winner;
                case "ForehandPushLongDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsLong() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandPushLongPointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsLong() && this.Strokes[stroke].Player != this.Winner;

                case "BackhandPushLongTotalButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsLong();
                case "BackhandPushLongPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsLong() && this.Strokes[stroke].Player == this.Winner;
                case "BackhandPushLongDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsLong() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandPushLongPointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].IsLong() && this.Strokes[stroke].Player != this.Winner;

                case "AllPushLongTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].IsLong() && this.Strokes[stroke].Stroketechnique.Type == "Push";
                case "AllPushLongPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].IsLong() && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].Player == this.Winner;
                case "AllPushLongDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].IsLong() && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllPushLongPointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].IsLong() && this.Strokes[stroke].Stroketechnique.Type == "Push" && this.Strokes[stroke].Player != this.Winner;


                #endregion

                #region Topspin diagonal

                case "ForehandTopspinDiagonalTotalButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsDiagonal(stroke);
                case "ForehandTopspinDiagonalPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsDiagonal(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "ForehandTopspinDiagonalDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsDiagonal(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandTopspinDiagonalPointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsDiagonal(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "BackhandTopspinDiagonalTotalButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsDiagonal(stroke);
                case "BackhandTopspinDiagonalPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsDiagonal(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "BackhandTopspinDiagonalDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsDiagonal(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandTopspinDiagonalPointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsDiagonal(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "AllTopspinDiagonalTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsDiagonal(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Topspin";
                case "AllTopspinDiagonalPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsDiagonal(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.Strokes[stroke].Player == this.Winner;
                case "AllTopspinDiagonalDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsDiagonal(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllTopspinDiagonalPointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsDiagonal(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.Strokes[stroke].Player != this.Winner;

                #endregion

                #region Topspin Middle

                case "ForehandTopspinMiddleTotalButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsMiddle(stroke);
                case "ForehandTopspinMiddlePointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsMiddle(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "ForehandTopspinMiddleDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsMiddle(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandTopspinMiddlePointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsMiddle(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "BackhandTopspinMiddleTotalButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsMiddle(stroke);
                case "BackhandTopspinMiddlePointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsMiddle(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "BackhandTopspinMiddleDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsMiddle(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandTopspinMiddlePointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsMiddle(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "AllTopspinMiddleTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsMiddle(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Topspin";
                case "AllTopspinMiddlePointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsMiddle(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.Strokes[stroke].Player == this.Winner;
                case "AllTopspinMiddleDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsMiddle(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllTopspinMiddlePointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsMiddle(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.Strokes[stroke].Player != this.Winner;

                #endregion

                #region Topspin parallel

                case "ForehandTopspinParallelTotalButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsParallel(stroke);
                case "ForehandTopspinParallelPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsParallel(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "ForehandTopspinParallelDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsParallel(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandTopspinParallelPointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsParallel(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "BackhandTopspinParallelTotalButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsParallel(stroke);
                case "BackhandTopspinParallelPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsParallel(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "BackhandTopspinParallelDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsParallel(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandTopspinParallelPointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.IsParallel(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "AllTopspinParallelTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsParallel(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Topspin";
                case "AllTopspinParallelPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsParallel(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.Strokes[stroke].Player == this.Winner;
                case "AllTopspinParallelDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsParallel(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllTopspinParallelPointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsParallel(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Topspin" && this.Strokes[stroke].Player != this.Winner;

                #endregion

                #region Block diagonal

                case "ForehandBlockDiagonalTotalButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsDiagonal(stroke);
                case "ForehandBlockDiagonalPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsDiagonal(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "ForehandBlockDiagonalDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsDiagonal(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandBlockDiagonalPointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsDiagonal(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "BackhandBlockDiagonalTotalButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsDiagonal(stroke);
                case "BackhandBlockDiagonalPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsDiagonal(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "BackhandBlockDiagonalDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsDiagonal(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandBlockDiagonalPointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsDiagonal(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "AllBlockDiagonalTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsDiagonal(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Block";
                case "AllBlockDiagonalPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsDiagonal(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.Strokes[stroke].Player == this.Winner;
                case "AllBlockDiagonalDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsDiagonal(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllBlockDiagonalPointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsDiagonal(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.Strokes[stroke].Player != this.Winner;

                #endregion

                #region Block Middle

                case "ForehandBlockMiddleTotalButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsMiddle(stroke);
                case "ForehandBlockMiddlePointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsMiddle(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "ForehandBlockMiddleDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsMiddle(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandBlockMiddlePointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsMiddle(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "BackhandBlockMiddleTotalButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsMiddle(stroke);
                case "BackhandBlockMiddlePointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsMiddle(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "BackhandBlockMiddleDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsMiddle(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandBlockMiddlePointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsMiddle(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "AllBlockMiddleTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsMiddle(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Block";
                case "AllBlockMiddlePointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsMiddle(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.Strokes[stroke].Player == this.Winner;
                case "AllBlockMiddleDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsMiddle(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllBlockMiddlePointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsMiddle(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.Strokes[stroke].Player != this.Winner;

                #endregion

                #region Block parallel

                case "ForehandBlockParallelTotalButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsParallel(stroke);
                case "ForehandBlockParallelPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsParallel(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "ForehandBlockParallelDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsParallel(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandBlockParallelPointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsParallel(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "BackhandBlockParallelTotalButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsParallel(stroke);
                case "BackhandBlockParallelPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsParallel(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "BackhandBlockParallelDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsParallel(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandBlockParallelPointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.IsParallel(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "AllBlockParallelTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsParallel(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Block";
                case "AllBlockParallelPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsParallel(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.Strokes[stroke].Player == this.Winner;
                case "AllBlockParallelDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsParallel(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllBlockParallelPointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsParallel(stroke) && this.Strokes[stroke].Stroketechnique.Type == "Block" && this.Strokes[stroke].Player != this.Winner;

                #endregion

                #region Chop 
                case "ForehandChopTotalButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Chop";
                case "ForehandChopPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Chop" && this.Strokes[stroke].Player == this.Winner;
                case "ForehandChopDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Chop" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < stroke + 3;
                case "ForehandChopPointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Stroketechnique.Type == "Chop" && this.Strokes[stroke].Player != this.Winner;

                case "BackhandChopTotalButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Chop";
                case "BackhandChopPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Chop" && this.Strokes[stroke].Player == this.Winner;
                case "BackhandChopDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Chop" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandChopPointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Stroketechnique.Type == "Chop" && this.Strokes[stroke].Player != this.Winner;

                case "AllChopTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].Stroketechnique.Type == "Chop";
                case "AllChopPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].Stroketechnique.Type == "Chop" && this.Strokes[stroke].Player == this.Winner;
                case "AllChopDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].Stroketechnique.Type == "Chop" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllChopPointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].Stroketechnique.Type == "Chop" && this.Strokes[stroke].Player != this.Winner;
                #endregion

                #region All Receives 
                case "ForehandReceiveAllTotalButton":
                    return this.Strokes[stroke].Side == "Forehand";
                case "ForehandReceiveAllPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Player == this.Winner;
                case "ForehandReceiveAllDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < stroke + 3;
                case "ForehandReceiveAllPointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].Player != this.Winner;

                case "BackhandReceiveAllTotalButton":
                    return this.Strokes[stroke].Side == "Backhand";
                case "BackhandReceiveAllPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Player == this.Winner;
                case "BackhandReceiveAllDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandReceiveAllPointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].Player != this.Winner;

                case "AllReceiveAllTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand");
                case "AllReceiveAllPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].Player == this.Winner;
                case "AllReceiveAllDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllReceiveAllPointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].Player != this.Winner;
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
                    return this.Strokes[stroke].PointOfContact == "over";
                case "OverTheTablePointsWonButton":
                    return this.Strokes[stroke].PointOfContact == "over" && this.Strokes[stroke].Player == this.Winner;
                case "OverTheTableDirectPointsWonButton":
                    return this.Strokes[stroke].PointOfContact == "over" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "OverTheTablePointsLostButton":
                    return this.Strokes[stroke].PointOfContact == "over" && this.Strokes[stroke].Player != this.Winner;

                #endregion

                #region at the table
                case "AtTheTableTotalButton":
                    return this.Strokes[stroke].PointOfContact == "behind";
                case "AtTheTablePointsWonButton":
                    return this.Strokes[stroke].PointOfContact == "behind" && this.Strokes[stroke].Player == this.Winner;
                case "AtTheTableDirectPointsWonButton":
                    return this.Strokes[stroke].PointOfContact == "behind" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AtTheTablePointsLostButton":
                    return this.Strokes[stroke].PointOfContact == "behind" && this.Strokes[stroke].Player != this.Winner;

                #endregion

                #region half distance
                case "HalfDistanceTotalButton":
                    return this.Strokes[stroke].PointOfContact == "half-distance";
                case "HalfDistancePointsWonButton":
                    return this.Strokes[stroke].PointOfContact == "half-distance" && this.Strokes[stroke].Player == this.Winner;
                case "HalfDistanceDirectPointsWonButton":
                    return this.Strokes[stroke].PointOfContact == "half-distance" && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "HalfDistancePointsLostButton":
                    return this.Strokes[stroke].PointOfContact == "half-distance" && this.Strokes[stroke].Player != this.Winner;

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
                    return this.Strokes[stroke].IsTopLeft() || this.Strokes[stroke].IsMidLeft() || this.Strokes[stroke].IsBotLeft();
                case "PlacementForehandAllPointsWonButton":
                    return (this.Strokes[stroke].IsTopLeft() || this.Strokes[stroke].IsMidLeft() || this.Strokes[stroke].IsBotLeft()) && this.Strokes[stroke].Player == this.Winner;
                case "PlacementForehandAllDirectPointsWonButton":
                    return (this.Strokes[stroke].IsTopLeft() || this.Strokes[stroke].IsMidLeft() || this.Strokes[stroke].IsBotLeft()) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "PlacementForehandAllPointsLostButton":
                    return (this.Strokes[stroke].IsTopLeft() || this.Strokes[stroke].IsMidLeft() || this.Strokes[stroke].IsBotLeft()) && this.Strokes[stroke].Player != this.Winner;
                #endregion
                #region ForehandLong
                case "PlacementForehandLongTotalButton":
                    return this.Strokes[stroke].IsTopLeft();
                case "PlacementForehandLongPointsWonButton":
                    return this.Strokes[stroke].IsTopLeft() && this.Strokes[stroke].Player == this.Winner;
                case "PlacementForehandLongDirectPointsWonButton":
                    return this.Strokes[stroke].IsTopLeft() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "PlacementForehandLongPointsLostButton":
                    return this.Strokes[stroke].IsTopLeft() && this.Strokes[stroke].Player != this.Winner;
                #endregion
                #region ForehandHalfLong
                case "PlacementForehandHalfLongTotalButton":
                    return this.Strokes[stroke].IsMidLeft();
                case "PlacementForehandHalfLongPointsWonButton":
                    return this.Strokes[stroke].IsMidLeft() && this.Strokes[stroke].Player == this.Winner;
                case "PlacementForehandHalfLongDirectPointsWonButton":
                    return this.Strokes[stroke].IsMidLeft() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "PlacementForehandHalfLongPointsLostButton":
                    return this.Strokes[stroke].IsMidLeft() && this.Strokes[stroke].Player != this.Winner;
                #endregion
                #region ForehandShort
                case "PlacementForehandShortTotalButton":
                    return this.Strokes[stroke].IsBotLeft();
                case "PlacementForehandShortPointsWonButton":
                    return this.Strokes[stroke].IsBotLeft() && this.Strokes[stroke].Player == this.Winner;
                case "PlacementForehandShortDirectPointsWonButton":
                    return this.Strokes[stroke].IsBotLeft() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "PlacementForehandShortPointsLostButton":
                    return this.Strokes[stroke].IsBotLeft() && this.Strokes[stroke].Player != this.Winner;
                #endregion
                #region MiddleAll
                case "PlacementMiddleAllTotalButton":
                    return this.Strokes[stroke].IsTopMid() || this.Strokes[stroke].IsMidMid() || this.Strokes[stroke].IsBotMid();
                case "PlacementMiddleAllPointsWonButton":
                    return (this.Strokes[stroke].IsTopMid() || this.Strokes[stroke].IsMidMid() || this.Strokes[stroke].IsBotMid()) && this.Strokes[stroke].Player == this.Winner;
                case "PlacementMiddleAllDirectPointsWonButton":
                    return (this.Strokes[stroke].IsTopMid() || this.Strokes[stroke].IsMidMid() || this.Strokes[stroke].IsBotMid()) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "PlacementMiddleAllPointsLostButton":
                    return (this.Strokes[stroke].IsTopMid() || this.Strokes[stroke].IsMidMid() || this.Strokes[stroke].IsBotMid()) && this.Strokes[stroke].Player != this.Winner;
                #endregion
                #region MiddleLong
                case "PlacementMiddleLongTotalButton":
                    return this.Strokes[stroke].IsTopMid();
                case "PlacementMiddleLongPointsWonButton":
                    return this.Strokes[stroke].IsTopMid() && this.Strokes[stroke].Player == this.Winner;
                case "PlacementMiddleLongDirectPointsWonButton":
                    return this.Strokes[stroke].IsTopMid() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "PlacementMiddleLongPointsLostButton":
                    return this.Strokes[stroke].IsTopMid() && this.Strokes[stroke].Player != this.Winner;
                #endregion
                #region MiddleHalfLong
                case "PlacementMiddleHalfLongTotalButton":
                    return this.Strokes[stroke].IsMidMid();
                case "PlacementMiddleHalfLongPointsWonButton":
                    return this.Strokes[stroke].IsMidMid() && this.Strokes[stroke].Player == this.Winner;
                case "PlacementMiddleHalfLongDirectPointsWonButton":
                    return this.Strokes[stroke].IsMidMid() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "PlacementMiddleHalfLongPointsLostButton":
                    return this.Strokes[stroke].IsMidMid() && this.Strokes[stroke].Player != this.Winner;
                #endregion
                #region MiddleShort
                case "PlacementMiddleShortTotalButton":
                    return this.Strokes[stroke].IsBotMid();
                case "PlacementMiddleShortPointsWonButton":
                    return this.Strokes[stroke].IsBotMid() && this.Strokes[stroke].Player == this.Winner;
                case "PlacementMiddleShortDirectPointsWonButton":
                    return this.Strokes[stroke].IsBotMid() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "PlacementMiddleShortPointsLostButton":
                    return this.Strokes[stroke].IsBotMid() && this.Strokes[stroke].Player != this.Winner;
                #endregion
                #region BackhandAll
                case "PlacementBackhandAllTotalButton":
                    return this.Strokes[stroke].IsTopRight() || this.Strokes[stroke].IsMidRight() || this.Strokes[stroke].IsBotRight();
                case "PlacementBackhandAllPointsWonButton":
                    return (this.Strokes[stroke].IsTopRight() || this.Strokes[stroke].IsMidRight() || this.Strokes[stroke].IsBotRight()) && this.Strokes[stroke].Player == this.Winner;
                case "PlacementBackhandAllDirectPointsWonButton":
                    return (this.Strokes[stroke].IsTopRight() || this.Strokes[stroke].IsMidRight() || this.Strokes[stroke].IsBotRight()) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "PlacementBackhandAllPointsLostButton":
                    return (this.Strokes[stroke].IsTopRight() || this.Strokes[stroke].IsMidRight() || this.Strokes[stroke].IsBotRight()) && this.Strokes[stroke].Player != this.Winner;
                #endregion
                #region BackhandLong
                case "PlacementBackhandLongTotalButton":
                    return this.Strokes[stroke].IsTopRight();
                case "PlacementBackhandLongPointsWonButton":
                    return this.Strokes[stroke].IsTopRight() && this.Strokes[stroke].Player == this.Winner;
                case "PlacementBackhandLongDirectPointsWonButton":
                    return this.Strokes[stroke].IsTopRight() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "PlacementBackhandLongPointsLostButton":
                    return this.Strokes[stroke].IsTopRight() && this.Strokes[stroke].Player != this.Winner;
                #endregion
                #region BackhandHalfLong
                case "PlacementBackhandHalfLongTotalButton":
                    return this.Strokes[stroke].IsMidRight();
                case "PlacementBackhandHalfLongPointsWonButton":
                    return this.Strokes[stroke].IsMidRight() && this.Strokes[stroke].Player == this.Winner;
                case "PlacementBackhandHalfLongDirectPointsWonButton":
                    return this.Strokes[stroke].IsMidRight() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "PlacementBackhandHalfLongPointsLostButton":
                    return this.Strokes[stroke].IsMidRight() && this.Strokes[stroke].Player != this.Winner;
                #endregion
                #region BackhandShort
                case "PlacementBackhandShortTotalButton":
                    return this.Strokes[stroke].IsBotRight();
                case "PlacementBackhandShortPointsWonButton":
                    return this.Strokes[stroke].IsBotRight() && this.Strokes[stroke].Player == this.Winner;
                case "PlacementBackhandShortDirectPointsWonButton":
                    return this.Strokes[stroke].IsBotRight() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "PlacementBackhandShortPointsLostButton":
                    return this.Strokes[stroke].IsBotRight() && this.Strokes[stroke].Player != this.Winner;
                #endregion
                #region AllLong
                case "PlacementAllLongTotalButton":
                    return this.Strokes[stroke].IsLong();
                case "PlacementAllLongPointsWonButton":
                    return this.Strokes[stroke].IsLong() && this.Strokes[stroke].Player == this.Winner;
                case "PlacementAllLongDirectPointsWonButton":
                    return this.Strokes[stroke].IsLong() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "PlacementAllLongPointsLostButton":
                    return this.Strokes[stroke].IsLong() && this.Strokes[stroke].Player != this.Winner;
                #endregion
                #region AllHalfLong
                case "PlacementAllHalfLongTotalButton":
                    return this.Strokes[stroke].IsHalfLong();
                case "PlacementAllHalfLongPointsWonButton":
                    return this.Strokes[stroke].IsHalfLong() && this.Strokes[stroke].Player == this.Winner;
                case "PlacementAllHalfLongDirectPointsWonButton":
                    return this.Strokes[stroke].IsHalfLong() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "PlacementAllHalfLongPointsLostButton":
                    return this.Strokes[stroke].IsHalfLong() && this.Strokes[stroke].Player != this.Winner;
                #endregion
                #region AllShort
                case "PlacementAllShortTotalButton":
                    return this.Strokes[stroke].IsShort();
                case "PlacementAllShortPointsWonButton":
                    return this.Strokes[stroke].IsShort() && this.Strokes[stroke].Player == this.Winner;
                case "PlacementAllShortDirectPointsWonButton":
                    return this.Strokes[stroke].IsShort() && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "PlacementAllShortPointsLostButton":
                    return this.Strokes[stroke].IsShort() && this.Strokes[stroke].Player != this.Winner;
                #endregion

                #region ReceiveErrors
                case "PlacementAllServiceErrorsTotalButton":
                    if ((stroke + 1) % 2 == 1)
                    {
                        return this.Server != this.Winner && this.Length == (stroke + 1);
                    }
                    else if ((stroke + 1) % 2 == 0)
                    {
                        return this.Server == this.Winner && this.Length == (stroke + 1);
                    }
                    else
                        return true;
                #endregion
                default:
                    return true;
            }
        }
        public bool HasSpinStatistics(string name)
        {
            switch (name)
            {
                case "":
                    return true;
                #region UpSpin

                case "UpSideLeftTotalButton":
                    return (this.Strokes[0].Spin.SL == "1" && this.Strokes[0].Spin.TS == "1");
                case "UpSideLeftPointsWonButton":
                    return (this.Strokes[0].Spin.SL == "1" && this.Strokes[0].Spin.TS == "1") && this.Strokes[0].Player == this.Winner;
                case "UpSideLeftDirectPointsWonButton":
                    return (this.Strokes[0].Spin.SL == "1" && this.Strokes[0].Spin.TS == "1") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "UpSideLeftPointsLostButton":
                    return (this.Strokes[0].Spin.SL == "1" && this.Strokes[0].Spin.TS == "1") && this.Strokes[0].Player != this.Winner;

                case "UpTotalButton":
                    return (this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0" && this.Strokes[0].Spin.TS == "1");
                case "UpPointsWonButton":
                    return (this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0" && this.Strokes[0].Spin.TS == "1") && this.Strokes[0].Player == this.Winner;
                case "UpDirectPointsWonButton":
                    return (this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0" && this.Strokes[0].Spin.TS == "1") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "UpPointsLostButton":
                    return (this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0" && this.Strokes[0].Spin.TS == "1") && this.Strokes[0].Player != this.Winner;

                case "UpSideRightTotalButton":
                    return (this.Strokes[0].Spin.SR == "1" && this.Strokes[0].Spin.TS == "1");
                case "UpSideRightPointsWonButton":
                    return (this.Strokes[0].Spin.SR == "1" && this.Strokes[0].Spin.TS == "1") && this.Strokes[0].Player == this.Winner;
                case "UpSideRightDirectPointsWonButton":
                    return (this.Strokes[0].Spin.SR == "1" && this.Strokes[0].Spin.TS == "1") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "UpSideRightPointsLostButton":
                    return (this.Strokes[0].Spin.SR == "1" && this.Strokes[0].Spin.TS == "1") && this.Strokes[0].Player != this.Winner;

                case "UpAllTotalButton":
                    return this.Strokes[0].Spin.TS == "1";
                case "UpAllPointsWonButton":
                    return this.Strokes[0].Spin.TS == "1" && this.Strokes[0].Player == this.Winner;
                case "UpAllDirectPointsWonButton":
                    return this.Strokes[0].Spin.TS == "1" && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "UpAllPointsLostButton":
                    return this.Strokes[0].Spin.TS == "1" && this.Strokes[0].Player != this.Winner;

                #endregion

                #region No UpDown Spin

                case "SideLeftTotalButton":
                    return (this.Strokes[0].Spin.SL == "1" && this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0");
                case "SideLeftPointsWonButton":
                    return (this.Strokes[0].Spin.SL == "1" && this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0") && this.Strokes[0].Player == this.Winner;
                case "SideLeftDirectPointsWonButton":
                    return (this.Strokes[0].Spin.SL == "1" && this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "SideLeftPointsLostButton":
                    return (this.Strokes[0].Spin.SL == "1" && this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0") && this.Strokes[0].Player != this.Winner;

                case "NoSpinTotalButton":
                    return (this.Strokes[0].Spin.No == "1" && this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0" && this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0");
                case "NoSpinPointsWonButton":
                    return (this.Strokes[0].Spin.No == "1" && this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0" && this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0") && this.Strokes[0].Player == this.Winner;
                case "NoSpinDirectPointsWonButton":
                    return (this.Strokes[0].Spin.No == "1" && this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0" && this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "NoSpinPointsLostButton":
                    return (this.Strokes[0].Spin.No == "1" && this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0" && this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0") && this.Strokes[0].Player != this.Winner;

                case "SideRightTotalButton":
                    return (this.Strokes[0].Spin.SR == "1" && this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0");
                case "SideRightPointsWonButton":
                    return (this.Strokes[0].Spin.SR == "1" && this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0") && this.Strokes[0].Player == this.Winner;
                case "SideRightDirectPointsWonButton":
                    return (this.Strokes[0].Spin.SR == "1" && this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "SideRightPointsLostButton":
                    return (this.Strokes[0].Spin.SR == "1" && this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0") && this.Strokes[0].Player != this.Winner;

                case "NoUpDownAllTotalButton":
                    return (this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0");
                case "NoUpDownAllPointsWonButton":
                    return (this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0") && this.Strokes[0].Player == this.Winner;
                case "NoUpDownAllDirectPointsWonButton":
                    return (this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "NoUpDownAllPointsLostButton":
                    return (this.Strokes[0].Spin.TS == "0" && this.Strokes[0].Spin.US == "0") && this.Strokes[0].Player != this.Winner;

                #endregion

                #region DownSpin

                case "DownSideLeftTotalButton":
                    return (this.Strokes[0].Spin.SL == "1" && this.Strokes[0].Spin.US == "1");
                case "DownSideLeftPointsWonButton":
                    return (this.Strokes[0].Spin.SL == "1" && this.Strokes[0].Spin.US == "1") && this.Strokes[0].Player == this.Winner;
                case "DownSideLeftDirectPointsWonButton":
                    return (this.Strokes[0].Spin.SL == "1" && this.Strokes[0].Spin.US == "1") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "DownSideLeftPointsLostButton":
                    return (this.Strokes[0].Spin.SL == "1" && this.Strokes[0].Spin.US == "1") && this.Strokes[0].Player != this.Winner;

                case "DownTotalButton":
                    return (this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0" && this.Strokes[0].Spin.US == "1");
                case "DownPointsWonButton":
                    return (this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0" && this.Strokes[0].Spin.US == "1") && this.Strokes[0].Player == this.Winner;
                case "DownDirectPointsWonButton":
                    return (this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0" && this.Strokes[0].Spin.US == "1") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "DownPointsLostButton":
                    return (this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0" && this.Strokes[0].Spin.US == "1") && this.Strokes[0].Player != this.Winner;

                case "DownSideRightTotalButton":
                    return (this.Strokes[0].Spin.SR == "1" && this.Strokes[0].Spin.US == "1");
                case "DownSideRightPointsWonButton":
                    return (this.Strokes[0].Spin.SR == "1" && this.Strokes[0].Spin.US == "1") && this.Strokes[0].Player == this.Winner;
                case "DownSideRightDirectPointsWonButton":
                    return (this.Strokes[0].Spin.SR == "1" && this.Strokes[0].Spin.US == "1") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "DownSideRightPointsLostButton":
                    return (this.Strokes[0].Spin.SR == "1" && this.Strokes[0].Spin.US == "1") && this.Strokes[0].Player != this.Winner;

                case "DownAllTotalButton":
                    return this.Strokes[0].Spin.US == "1";
                case "DownAllPointsWonButton":
                    return this.Strokes[0].Spin.US == "1" && this.Strokes[0].Player == this.Winner;
                case "DownAllDirectPointsWonButton":
                    return this.Strokes[0].Spin.US == "1" && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "DownAllPointsLostButton":
                    return this.Strokes[0].Spin.US == "1" && this.Strokes[0].Player != this.Winner;

                #endregion

                #region SideLeft All

                case "SideLeftAllTotalButton":
                    return (this.Strokes[0].Spin.SL == "1");
                case "SideLeftAllPointsWonButton":
                    return (this.Strokes[0].Spin.SL == "1") && this.Strokes[0].Player == this.Winner;
                case "SideLeftAllDirectPointsWonButton":
                    return (this.Strokes[0].Spin.SL == "1") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "SideLeftAllPointsLostButton":
                    return (this.Strokes[0].Spin.SL == "1") && this.Strokes[0].Player != this.Winner;

                #endregion

                #region No SideSpin All

                case "NoSideAllTotalButton":
                    return (this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0");
                case "NoSideAllPointsWonButton":
                    return (this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0") && this.Strokes[0].Player == this.Winner;
                case "NoSideAllDirectPointsWonButton":
                    return (this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "NoSideAllPointsLostButton":
                    return (this.Strokes[0].Spin.SL == "0" && this.Strokes[0].Spin.SR == "0") && this.Strokes[0].Player != this.Winner;


                #endregion

                #region SideRight All

                case "SideRightAllTotalButton":
                    return (this.Strokes[0].Spin.SR == "1");
                case "SideRightAllPointsWonButton":
                    return (this.Strokes[0].Spin.SR == "1") && this.Strokes[0].Player == this.Winner;
                case "SideRightAllDirectPointsWonButton":
                    return (this.Strokes[0].Spin.SR == "1") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "SideRightAllPointsLostButton":
                    return (this.Strokes[0].Spin.SR == "1") && this.Strokes[0].Player != this.Winner;


                #endregion

                #region All Spins Total

                case "AllSpinTotalButton":
                    return (this.Strokes[0].Spin.SR == "1" || this.Strokes[0].Spin.SL == "1" || this.Strokes[0].Spin.US == "1" || this.Strokes[0].Spin.TS == "1" || this.Strokes[0].Spin.No == "1");
                case "AllSpinPointsWonButton":
                    return (this.Strokes[0].Spin.SR == "1" || this.Strokes[0].Spin.SL == "1" || this.Strokes[0].Spin.US == "1" || this.Strokes[0].Spin.TS == "1" || this.Strokes[0].Spin.No == "1") && this.Strokes[0].Player == this.Winner;
                case "AllSpinDirectPointsWonButton":
                    return (this.Strokes[0].Spin.SR == "1" || this.Strokes[0].Spin.SL == "1" || this.Strokes[0].Spin.US == "1" || this.Strokes[0].Spin.TS == "1" || this.Strokes[0].Spin.No == "1") && this.Strokes[0].Player == this.Winner && Convert.ToInt32(this.Length) < 3;
                case "AllSpinPointsLostButton":
                    return (this.Strokes[0].Spin.SR == "1" || this.Strokes[0].Spin.SL == "1" || this.Strokes[0].Spin.US == "1" || this.Strokes[0].Spin.TS == "1" || this.Strokes[0].Spin.No == "1") && this.Strokes[0].Player != this.Winner;


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
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround && this.IsDiagonal(stroke);
                case "ForehandStepAroundInsideOutPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround && this.IsDiagonal(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "ForehandStepAroundInsideOutDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround && this.IsDiagonal(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandStepAroundInsideOutPointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround && this.IsDiagonal(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "BackhandStepAroundInsideOutTotalButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround && this.IsDiagonal(stroke);
                case "BackhandStepAroundInsideOutPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround && this.IsDiagonal(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "BackhandStepAroundInsideOutDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround && this.IsDiagonal(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandStepAroundInsideOutPointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround && this.IsDiagonal(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "AllStepAroundInsideOutTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsDiagonal(stroke) && this.Strokes[stroke].StepAround;
                case "AllStepAroundInsideOutPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsDiagonal(stroke) && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player == this.Winner;
                case "AllStepAroundInsideOutDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsDiagonal(stroke) && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllStepAroundInsideOutPointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsDiagonal(stroke) && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player != this.Winner;

                #endregion

                #region StepAround Middle

                case "ForehandStepAroundMiddleTotalButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround && this.IsMiddle(stroke);
                case "ForehandStepAroundMiddlePointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround && this.IsMiddle(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "ForehandStepAroundMiddleDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround && this.IsMiddle(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandStepAroundMiddlePointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround && this.IsMiddle(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "BackhandStepAroundMiddleTotalButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround && this.IsMiddle(stroke);
                case "BackhandStepAroundMiddlePointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround && this.IsMiddle(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "BackhandStepAroundMiddleDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround && this.IsMiddle(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandStepAroundMiddlePointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround && this.IsMiddle(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "AllStepAroundMiddleTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsMiddle(stroke) && this.Strokes[stroke].StepAround;
                case "AllStepAroundMiddlePointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsMiddle(stroke) && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player == this.Winner;
                case "AllStepAroundMiddleDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsMiddle(stroke) && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllStepAroundMiddlePointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsMiddle(stroke) && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player != this.Winner;

                #endregion

                #region StepAround parallel

                case "ForehandStepAroundParallelTotalButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround && this.IsParallel(stroke);
                case "ForehandStepAroundParallelPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround && this.IsParallel(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "ForehandStepAroundParallelDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround && this.IsParallel(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandStepAroundParallelPointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround && this.IsParallel(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "BackhandStepAroundParallelTotalButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround && this.IsParallel(stroke);
                case "BackhandStepAroundParallelPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround && this.IsParallel(stroke) && this.Strokes[stroke].Player == this.Winner;
                case "BackhandStepAroundParallelDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround && this.IsParallel(stroke) && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandStepAroundParallelPointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround && this.IsParallel(stroke) && this.Strokes[stroke].Player != this.Winner;

                case "AllStepAroundParallelTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsParallel(stroke) && this.Strokes[stroke].StepAround;
                case "AllStepAroundParallelPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsParallel(stroke) && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player == this.Winner;
                case "AllStepAroundParallelDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsParallel(stroke) && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllStepAroundParallelPointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.IsParallel(stroke) && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player != this.Winner;

                #endregion

                #region StepAround all Directions

                case "ForehandStepAroundAllTotalButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround;
                case "ForehandStepAroundAllPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player == this.Winner;
                case "ForehandStepAroundAllDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "ForehandStepAroundAllPointsLostButton":
                    return this.Strokes[stroke].Side == "Forehand" && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player != this.Winner;

                case "BackhandStepAroundAllTotalButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround;
                case "BackhandStepAroundAllPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player == this.Winner;
                case "BackhandStepAroundAllDirectPointsWonButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "BackhandStepAroundAllPointsLostButton":
                    return this.Strokes[stroke].Side == "Backhand" && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player != this.Winner;

                case "AllStepAroundAllTotalButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].StepAround;
                case "AllStepAroundAllPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player == this.Winner;
                case "AllStepAroundAllDirectPointsWonButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player == this.Winner && Convert.ToInt32(this.Length) < (stroke + 3);
                case "AllStepAroundAllPointsLostButton":
                    return (this.Strokes[stroke].Side == "Forehand" || this.Strokes[stroke].Side == "Backhand") && this.Strokes[stroke].StepAround && this.Strokes[stroke].Player != this.Winner;


                #endregion

                default:
                    return true;
            }

        }

        #endregion


    }
}
