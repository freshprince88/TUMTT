using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using TT.Models.Util;

namespace TT.Models
{
    public class Playlist : PropertyChangedBase
    {
        /// <summary>
        /// Backs the <see cref="Rallies"/> property.
        /// </summary>
        private ObservableCollectionEx<Guid> rallyIDs = new ObservableCollectionEx<Guid>();

        private string name;

        private Match match;

        /// <summary>
        /// Initializes a new instance of the <see cref="Playlist"/> class.
        /// </summary>
        public Playlist(Match m)
        {
            this.match = m;
        }

        /// <summary>
        /// PLEASE DON'T CALL WITHOUT INITIALIZING MATCH
        /// </summary>
        public Playlist()
        {

        }

        [XmlArray]
        public ObservableCollectionEx<Guid> RallyIDs
        {
            get { return this.rallyIDs; }
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
        [XmlIgnore]
        public ReadOnlyCollection<Rally> Rallies
        {
            get
            {
                return new ReadOnlyCollection<Rally>(match.Rallies.Where<Rally>((item) => rallyIDs.Contains(item.ID)).ToList<Rally>());
            }
        }

        /// <summary>
        /// Gets or sets the name of the Playlist.
        /// </summary>
        [XmlAttribute]
        public string Name
        {
            get { return this.name; }
            set { this.RaiseAndSetIfChanged(ref this.name, value); }
        }

        /// <summary>
        /// Finds the next rally.
        /// </summary>
        /// <param name="rally">The previous rally.</param>
        /// <returns>The rally, or <c>null</c> if there is no next rally.</returns>
        public Rally FindNextRally(Rally rally)
        {
            var index = this.Rallies.IndexOf(rally);
            return index >= 0 ? this.match.Rallies.ElementAtOrDefault(index + 1) : null;
        }

        /// <summary>
        /// Finds the previous rally.
        /// </summary>
        /// <param name="rally">The next rally.</param>
        /// <returns>The previous rally, or <c>null</c> if there is no previous rally.</returns>
        public Rally FindPreviousRally(Rally rally)
        {
            var index = this.Rallies.IndexOf(rally);
            return index >= 0 ? this.match.Rallies.ElementAtOrDefault(index - 1) : null;
        }

        /// <summary>
        /// Adds Rally to Playlist if not already in Playlist
        /// </summary>
        /// <param name="r">Rally which should be added to the playlist</param>
        public void Add(Rally r)
        {
            if (!rallyIDs.Contains(r.ID))
                rallyIDs.Add(r.ID);
        }

        /// <summary>
        /// Adds Range of Rallies to Playlist
        /// </summary>
        /// <param name="itemsToAdd">Range of Rallies</param>
        public void AddRange(IEnumerable<Rally> itemsToAdd)
        {
            foreach(Rally item in itemsToAdd){
                Add(item);
            }
        }

        /// <summary>
        /// Remove Rally from Playlist
        /// </summary>
        /// <param name="r">Rally to Remove</param>
        public void Remove(Rally r)
        {
            rallyIDs.Remove(r.ID);
        }

        /// <summary>
        /// Sort Playlist by Rally-Number
        /// </summary>
        //public void Sort()
        //{
        //    List<Guid> sorted = rallyIDs.OrderBy(x => match.Rallies.Where<Rally>(r => r.ID == x).FirstOrDefault<Rally>().Number).ToList();
        //    int ptr = 0;
        //    while (ptr < sorted.Count)
        //    {
        //        if (!rallyIDs[ptr].Equals(sorted[ptr]))
        //        {
        //            Guid t = rallyIDs[ptr];
        //            rallyIDs.RemoveAt(ptr);
        //            rallyIDs.Insert(sorted.IndexOf(t), t);
        //        }
        //        else
        //        {
        //            ptr++;
        //        }
        //    }
        //}

    }
}
