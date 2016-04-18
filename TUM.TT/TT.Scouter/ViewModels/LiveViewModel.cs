using System.Linq;
using Caliburn.Micro;
using TT.Lib.Managers;
using TT.Lib.Models;
using System.Collections.ObjectModel;
using TT.Scouter.Interfaces;

namespace TT.Scouter.ViewModels
{
    public class LiveViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;

        public enum TimeMode
        {
            Video,
            Timer
        }

        private TimeMode _mode;
        public TimeMode ViewMode {
            get
            {
                return _mode;
            }
            set
            {
                if(_mode != value)
                {
                    this.DeactivateItem(MediaPlayer, true);
                    _mode = value;
                    switch (_mode)
                    {
                        case TimeMode.Video:
                            MediaPlayer = new LiveMediaViewModel(Events, MatchManager);
                            break;
                        case TimeMode.Timer:
                            MediaPlayer = new LiveTimerViewModel();
                            break;
                        default:
                            break;
                    }
                    this.ActivateItem(MediaPlayer);
                    NotifyOfPropertyChange();
                }
            }
        }

        public IMediaPosition MediaPlayer { get; set; }

        public Match Match { get { return MatchManager.Match; } }
        public ObservableCollection<Rally> Rallies { get { return MatchManager.ActivePlaylist.Rallies; } }

        private Rally _currentRally;
        /// <summary>
        /// Currently displayed Rally
        /// </summary>
        public Rally CurrentRally
        {
            get { return _currentRally; }
            set
            {
                _currentRally = value;
                NotifyOfPropertyChange();

                Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
                bool mark = marked != null && marked.Rallies != null && marked.Rallies.Contains(CurrentRally);

                if (mark != Markiert)
                {
                    Markiert = mark;
                    NotifyOfPropertyChange("Markiert");
                }
            }
        }


        private bool _newRally;
        /// <summary>
        /// Determines whether the "Start Rally" Button is shown
        /// </summary>
        public bool IsNewRally
        {
            get { return _newRally; }
            set
            {
                if (_newRally != value)
                {
                    _newRally = value;
                    NotifyOfPropertyChange();
                }

            }
        }

        public bool Markiert { get; set; }

        public LiveViewModel(IEventAggregator ev, IMatchManager man)
        {
            Events = ev;
            MatchManager = man;           
            IsNewRally = true;
            CurrentRally = MatchManager.ActivePlaylist.Rallies.First();
            Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
            Markiert = marked != null && marked.Rallies != null && marked.Rallies.Contains(CurrentRally);

            MediaPlayer = new LiveMediaViewModel(Events, MatchManager);
        }

        #region Caliburn Hooks

        protected override void OnActivate()
        {
            base.OnActivate();
            this.ActivateItem(MediaPlayer);            
        }

        #endregion

        #region View Methods

        public void RallyWon(int player)
        {
            // Add CurrentRally to Playlists (Alle und Markiert, falls Checkbox)
            //   -> CurrentRally neu setzen mit Bindings
            if (player == 1)
                CurrentRally.Winner = MatchPlayer.First;
            else
                CurrentRally.Winner = MatchPlayer.Second;

            CurrentRally.Ende = MediaPlayer.MediaPosition.TotalMilliseconds;

            //TODO: Dummy Klasse für MediaPlayer bauen falls kein Video geladen wurde
            //      Timer läuft, der die MediaPosition simuliert
            //CurrentRally.Ende = MediaPlayer.MediaPosition.TotalMilliseconds;

            //if (CurrentRally.Length == 0)
            //    CurrentRally.Length = 1;

            if (Markiert)
            {
                Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
                marked.Rallies.Add(CurrentRally);
            }

            CurrentRally = new Rally();
            Rallies.Add(CurrentRally);
            CurrentRally.UpdateServerAndScore();
            IsNewRally = true;
            CurrentRally.Anfang = MediaPlayer.MediaPosition.TotalMilliseconds;
            //NotifyOfPropertyChange("CurrentRally");            
        }

        public void SetRallyLength(int length)
        {
            //var diff = length - CurrentRally.Length;
            //if (CurrentRally.Length < length)
            //{
            //    for (int i = 0; i < diff; i++)
            //    {
            //        CurrentRally.Schläge.Add(new Schlag());
            //    }

            //}
            //else if (CurrentRally.Length > length)
            //{
            //    diff = -diff;
            //    for (int i = 0; i < diff; i++)
            //    {
            //        CurrentRally.Schläge.Remove(CurrentRally.Schläge.Last());
            //    }
            //}

            CurrentRally.Length = length;
            //NotifyOfPropertyChange("CurrentRally");
        }

        public void StartRally()
        {
            //TODO: Dummy Klasse für MediaPlayer bauen falls kein Video geladen wurde
            //      Timer läuft, der die MediaPosition simuliert
            //CurrentRally.Anfang = MediaPlayer.MediaPosition.TotalMilliseconds;
            IsNewRally = false;
            MediaPlayer.Play();
        }

        #endregion
    }
}
