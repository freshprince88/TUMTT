using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace TT.Lib.Models
{
    public class Playlist : PropertyChangedBase
    {
        /// <summary>
        /// Backs the <see cref="Rallies"/> property.
        /// </summary>
        private ObservableCollection<Rally> rallies = new ObservableCollection<Rally>();

        private string name;

        private Match match;

        /// <summary>
        /// Initializes a new instance of the <see cref="Playlist"/> class.
        /// </summary>
        public Playlist()
        {
            this.rallies.CollectionChanged += this.OnRalliesChanged;
        }

        /// <summary>
        /// Gets or sets the match this rally.
        /// </summary>
        [XmlIgnore]
        public Match Match
        {
            get { return this.match; }
            set { this.RaiseAndSetIfChanged(ref this.match, value); }
        }

        /// <summary>
        /// Gets all rallies of this match.
        /// </summary>
        public ObservableCollection<Rally> Rallies
        {
            get { return this.rallies; }
        }

        /// <summary>
        /// Gets or sets the round of the match.
        /// </summary>
        [XmlAttribute]
        public string Name
        {
            get { return this.name; }
            set { this.RaiseAndSetIfChanged(ref this.name, value); }
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
                    rally.FinalSetScore.Highest >= this.Match.Mode.RequiredSets() :
                    false;
            }
        }

        /// <summary>
        /// Finds the next rally.
        /// </summary>
        /// <param name="rally">The previous rally.</param>
        /// <returns>The rally, or <c>null</c> if there is no next rally.</returns>
        public Rally FindNextRally(Rally rally)
        {
            var index = this.rallies.IndexOf(rally);
            return index >= 0 ? this.rallies.ElementAtOrDefault(index + 1) : null;
        }

        /// <summary>
        /// Finds the previous rally.
        /// </summary>
        /// <param name="rally">The next rally.</param>
        /// <returns>The previous rally, or <c>null</c> if there is no previous rally.</returns>
        public Rally FindPreviousRally(Rally rally)
        {
            var index = this.rallies.IndexOf(rally);
            return index >= 0 ? this.rallies.ElementAtOrDefault(index - 1) : null;
        }

        /// <summary>
        /// Gets the finished rallies of this match.
        /// </summary>
        [XmlIgnore]
        public IEnumerable<Rally> FinishedRallies
        {
            get
            {
                return this.rallies.Where(r => r.Length > 0 && r.Winner != MatchPlayer.None);
            }
        }

        /// <summary>
        /// Handles changes to the list of rallies.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="args">The event arguments.</param>
        private void OnRalliesChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems != null)
            {
                foreach (var rally in args.NewItems.Cast<Rally>())
                {
                    // Connect to each new rally, and update its data.
                    rally.Playlist = this;
                    rally.PropertyChanged += this.OnRallyChanged;
                    rally.UpdateServerAndScore();
                }
            }

            // Update the rally after the new one
            this.rallies[args.NewStartingIndex].UpdateServerAndScore();

            if (args.OldItems != null)
            {
                foreach (var rally in args.OldItems.Cast<Rally>())
                {
                    // Disconnect from each removed rally.
                    rally.Playlist = null;
                    rally.PropertyChanged -= this.OnRallyChanged;
                }

                // Update the rally after the removed one.
                if (args.OldStartingIndex < this.rallies.Count)
                {
                    this.rallies[args.OldStartingIndex].UpdateServerAndScore();
                }
            }
        }

        /// <summary>
        /// Handles a change of a rally.
        /// </summary>
        /// <param name="sender">The changed rally.</param>
        /// <param name="args">The arguments describing the change.</param>
        private void OnRallyChanged(object sender, PropertyChangedEventArgs args)
        {
            var rally = (Rally)sender;
            if (this.rallies.Contains(rally))
            {
                var nextRally = this.FindNextRally(rally);
                if (nextRally != null)
                {
                    nextRally.UpdateServerAndScore();
                }
            }
        }
    }
}
