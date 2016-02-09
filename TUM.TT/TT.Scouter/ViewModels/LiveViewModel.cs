using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using TT.Lib.Managers;
using TT.Lib.Models;

namespace TT.Scouter.ViewModels
{
    public class LiveViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;
        public Match Match { get { return MatchManager.Match; } }
        public Rally CurrentRally { get; set; }
        public bool Markiert { get; set; }


        public LiveViewModel() : this(null, null, new Rally())
        {
        }

        public LiveViewModel(IEventAggregator ev, IMatchManager man, Rally r)
        {
            Events = ev;
            MatchManager = man;
            CurrentRally = r;
            Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
            Markiert = marked != null && marked.Rallies != null && marked.Rallies.Contains(CurrentRally);
        }

        #region View Methods

        public IEnumerable<IResult> RallyWon(int player)
        {
            //TODO: Add CurrentRally to Playlists (Alle und Markiert, falls Checkbox)
            //   -> Neuen View mit nächster Rally einladen (Oder reicht es CurrentRally neu zu setzen wegen Bindings??)
            switch (player)
            {
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }
            return null;
        }

        #endregion
    }
}
