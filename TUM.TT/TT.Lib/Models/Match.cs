//-----------------------------------------------------------------------
// <copyright file="Match.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Lib.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// The data model which represents a single match.
    /// </summary>
    public class Match : PropertyChangedBase
    {
        /// <summary>
        /// Backs the <see cref="FirstPlayer"/> property.
        /// </summary>
        private Player firstPlayer;

        /// <summary>
        /// Backs the <see cref="SecondPlayer"/> property.
        /// </summary>
        private Player secondPlayer;

        /// <summary>
        /// Backs the <see cref="DateTime"/> property.
        /// </summary>
        private DateTime dateTime;

        /// <summary>
        /// Backs the <see cref="Tournament"/> property.
        /// </summary>
        private string tournament;

        /// <summary>
        /// Backs the <see cref="Round"/> property.
        /// </summary>
        private string round;

        /// <summary>
        /// Backs the <see cref="Mode"/> property.
        /// </summary>
        private MatchMode mode = MatchMode.BestOf5;

        /// <summary>
        /// Backs the <see cref="VideoFile"/> property.
        /// </summary>
        private string videoFile;

        /// <summary>
        /// Backs the <see cref="Playlists"/> property.
        /// </summary>
        private ObservableCollection<Playlist> playlists = new ObservableCollection<Playlist>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Match"/> class.
        /// </summary>
        public Match()
        {
            this.playlists.CollectionChanged += this.OnPlaylistsChanged;
            FirstPlayer = new Player();
            SecondPlayer = new Player();
            DateTime = DateTime.Today;
        }

        /// <summary>
        /// Gets or sets the first <see cref="Player"/> in this match.
        /// </summary>
        public Player FirstPlayer
        {
            get { return this.firstPlayer; }
            set { this.RaiseAndSetIfChanged(ref this.firstPlayer, value); }
        }

        /// <summary>
        /// Gets or sets the second <see cref="Player"/> in this match.
        /// </summary>
        public Player SecondPlayer
        {
            get { return this.secondPlayer; }
            set { this.RaiseAndSetIfChanged(ref this.secondPlayer, value); }
        }

        /// <summary>
        /// Gets all playlists of this match.
        /// </summary>
        public ObservableCollection<Playlist> Playlists
        {
            get { return this.playlists; }
        }

        /// <summary>
        /// Gets all players in this match.
        /// </summary>
        [XmlIgnore]
        public IEnumerable<Player> Players
        {
            get
            {
                if (this.firstPlayer != null)
                {
                    yield return this.firstPlayer;
                }

                if (this.secondPlayer != null)
                {
                    yield return this.secondPlayer;
                }
            }
        }

        /// <summary>
        /// Gets or sets the tournament the match is part of.
        /// </summary>
        [XmlAttribute]
        public string Tournament
        {
            get { return this.tournament; }
            set { this.RaiseAndSetIfChanged(ref this.tournament, value); }
        }

        /// <summary>
        /// Gets or sets the Video location the match is part of.
        /// </summary>
        [XmlAttribute]
        public string VideoFile
        {
            get { return this.videoFile; }
            set { this.RaiseAndSetIfChanged(ref this.videoFile, value); }
        }

        /// <summary>
        /// Gets or sets the round of the match.
        /// </summary>
        [XmlAttribute]
        public string Round
        {
            get { return this.round; }
            set { this.RaiseAndSetIfChanged(ref this.round, value); }
        }

        /// <summary>
        /// Gets or sets the mode of this match.
        /// </summary>
        [XmlAttribute]
        public MatchMode Mode
        {
            get { return this.mode; }
            set { this.RaiseAndSetIfChanged(ref this.mode, value); }
        }

        /// <summary>
        /// Gets or sets the date and time of the match.
        /// </summary>
        [XmlAttribute]
        public DateTime DateTime
        {
            get { return this.dateTime; }
            set { this.RaiseAndSetIfChanged(ref this.dateTime, value); }
        }      

        /// <summary>
        /// Swaps the players in this match.
        /// </summary>
        public void SwapPlayers()
        {
            var first = this.FirstPlayer;
            var second = this.SecondPlayer;

            this.FirstPlayer = second;
            this.SecondPlayer = first;

            foreach(var pl in Playlists)
            {
                // Swap the server
                pl.Rallies.First().Server = pl.FirstServer.Other();

                // Swap the winner of each rally
                foreach (var rally in pl.Rallies)
                {
                    rally.Winner = rally.Winner.Other();
                }
            }
        }

        private void OnPlaylistsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //TODO: See Playlist.RalliesChanged
            //throw new NotImplementedException();
        }
    }
}
