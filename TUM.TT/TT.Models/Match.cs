//-----------------------------------------------------------------------
// <copyright file="Match.cs" company="Fakultät für Sport- und Gesundheitswissenschaft">
//    Copyright © 2013, 2014 Fakultät für Sport- und Gesundheitswissenschaft
// </copyright>
//-----------------------------------------------------------------------

namespace TT.Models
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Xml.Serialization;
    using TT.Models.Util;

    /// <summary>
    /// The data model which represents a single match.
    /// </summary>
    public class Match : PropertyChangedBase
    {
        /// <summary>
        /// Backs the GUID of the Match
        /// </summary>
        private Guid id;

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
        /// Backs the <see cref="Category"/> property.
        /// </summary>
        private MatchCategory category = MatchCategory.Category;

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
        /// Backs the <see cref="Synchro"/> property.
        /// </summary>
        private double synchro;
        
        /// <summary>
        /// Backs the <see cref="Rallies"/> property.
        /// </summary>
        private ObservableCollectionEx<Rally> rallies = new ObservableCollectionEx<Rally>();

        /// <summary>
        /// Backs the <see cref="Playlists"/> property.
        /// </summary>
        private ObservableCollection<Playlist> playlists = new ObservableCollection<Playlist>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Match"/> class.
        /// </summary>
        public Match()
        {
            this.id = new Guid();
            this.tournament = Properties.Resources.tournament_title_default;
            this.playlists.CollectionChanged += this.OnPlaylistsChanged;
            this.rallies.CollectionChanged += this.OnRalliesChanged;
        }

        /// <summary>
        ///  Gets the Unique ID of this match
        /// </summary>
        public Guid ID
        {
            get { return this.id; }
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
        /// Gets all rallies of this match.
        /// </summary>
        public ObservableCollectionEx<Rally> Rallies
        {
            get { return this.rallies; }
        }

        /// <summary>
        /// Gets the first serving player.
        /// </summary>
        /// <remarks>
        /// This is simply the server of the first rally.
        /// </remarks>
        [XmlIgnore]
        public MatchPlayer FirstServer
        {
            get
            {
                return this.Rallies
                    .Select(r => r.Server)
                    .DefaultIfEmpty(MatchPlayer.None)
                    .First();
            }
        }

        /// <summary>
        /// Gets the winner of the match.
        /// </summary>
        /// <remarks>
        /// This is simply the last winner of all rallies.
        /// </remarks>
        [XmlIgnore]
        public MatchPlayer Winner
        {
            get
            {
                return this.Rallies
                    .Reverse()
                    .Select(r => r.Winner)
                    .FirstOrDefault(w => w != MatchPlayer.None);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the match is over.
        /// </summary>
        [XmlIgnore]
        public bool IsOver
        {
            get
            {
                var rally = this.Rallies.LastOrDefault();
                return rally != null ?
                    rally.FinalSetScore.Highest >= this.Mode.RequiredSets() :
                    false;
            }
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
        /// Gets or sets the category of the match.
        /// </summary>
        [XmlAttribute]
        public MatchCategory Category
        {
            get { return this.category; }
            set { this.RaiseAndSetIfChanged(ref this.category, value); }
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
        /// Gets or sets the video offset in milliseconds.
        /// </summary>
        [XmlAttribute]
        public double Synchro
        {
            get { return this.synchro; }
            set
            {
                var diff = this.synchro - value;
                this.RaiseAndSetIfChanged(ref this.synchro, value);

                foreach (var r in this.Rallies)
                {
                    r.Start -= diff;
                    r.End -= diff;
                }
            }
        }
        /// <summary>
        /// Sets video offset for Start of the Rally 
        /// </summary>
        public void StartOffset(double offset)
        {
            foreach (var r in this.Rallies)
            {
                r.Start += offset;

            }
        }
        /// <summary>
        /// Sets video offset for End of the Rally 
        /// </summary>
        public void EndOffset(double offset)
        {
            foreach (var r in this.Rallies)
            {

                r.End += offset;
            }

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
        }

        /// <summary>
        /// Finds the previous rally.
        /// </summary>
        /// <param name="rally">The next rally.</param>
        /// <returns>The previous rally, or <c>null</c> if there is no previous rally.</returns>
        public Rally FindPreviousRally(Rally rally)
        {
            var index = this.Rallies.IndexOf(rally);
            return index >= 0 ? this.rallies.ElementAtOrDefault(index - 1) : null;
        }

        private void OnPlaylistsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //TODO: See Playlist.RalliesChanged
            //throw new NotImplementedException();
        }

        private void OnRalliesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Playlist list = playlists.Where<Playlist>(p => p.Name == "Alle").FirstOrDefault<Playlist>();
            if (list != null)
            {
                if (e.NewItems != null)
                {
                    foreach (Rally newRally in e.NewItems)
                    {
                        list.Add(newRally);
                    }
                }

                if (e.OldItems != null)
                {
                    foreach(Rally oldRally in e.OldItems)
                    {
                        list.Remove(oldRally);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the finished rallies of this match.
        /// </summary>
        [XmlIgnore]
        public IEnumerable<Rally> FinishedRallies
        {
            get
            {
                return this.Rallies.Where(r => r.Length > 0 && r.Winner != MatchPlayer.None);
            }
        }
    }
}
