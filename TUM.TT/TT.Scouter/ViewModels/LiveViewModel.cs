using System.Linq;
using Caliburn.Micro;
using TT.Lib.Managers;
using TT.Lib.Models;
using System.Collections.ObjectModel;
using System;

namespace TT.Scouter.ViewModels
{
    public class LiveViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;
        public Match Match { get { return MatchManager.Match; } }
        public ObservableCollection<Rally> Rallies { get { return MatchManager.ActivePlaylist.Rallies; } }

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
                    NotifyOfPropertyChange("Markiert");
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
            CurrentRally.Server = MatchPlayer.First;
            Rallies.Add(CurrentRally);
            CurrentRally.UpdateServerAndScore();                                 
            Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
            Markiert = marked != null && marked.Rallies != null && marked.Rallies.Contains(CurrentRally);
        }

        #region View Methods

        public void RallyWon(int player)
        {
            // Add CurrentRally to Playlists (Alle und Markiert, falls Checkbox)
            //   -> CurrentRally neu setzen mit Bindings
            if (player == 1)
                CurrentRally.Winner = MatchPlayer.First;
            else
                CurrentRally.Winner = MatchPlayer.Second;            

            if (Markiert)
            {
                Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
                marked.Rallies.Add(CurrentRally);             
            }

            CurrentRally = new Rally();
            Rallies.Add(CurrentRally);
            CurrentRally.UpdateServerAndScore();
            //NotifyOfPropertyChange("CurrentRally");            
        }

        public void SetRallyLength(int length)
        {
            CurrentRally.Length = length;
            NotifyOfPropertyChange("CurrentRally");
        }

        #endregion
    }
}
