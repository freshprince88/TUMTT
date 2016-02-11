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
    }
}
