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

        private Rally _currentRally;
        public Rally CurrentRally
        {
            get { return _currentRally; }
            set
            {
                _currentRally = value;
                NotifyOfPropertyChange("CurrentRally");

                Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
                bool mark = marked != null && marked.Rallies != null && marked.Rallies.Contains(CurrentRally);
                
                if(mark != Markiert)
                {
                    Markiert = mark;
                }            
            }
        }
        public bool Markiert { get; set; }


        public LiveViewModel() : this(null, null, new Rally())
        {
        }

        public LiveViewModel(IEventAggregator ev, IMatchManager man, Rally r)
        {
            Events = ev;
            MatchManager = man;
            CurrentRally = r == null ? new Rally() : r;
            Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
            Markiert = marked != null && marked.Rallies != null && marked.Rallies.Contains(CurrentRally);
        }

        #region View Methods

        public void RallyWon(int player)
        {
            //TODO: Add CurrentRally to Playlists (Alle und Markiert, falls Checkbox)
            //   -> CurrentRally neu setzen mit Bindings
            // NextRally Methode vielleicht im MatchManager, dann sind alle Infos des Spiels verfügbar
        }

        #endregion
    }
}
