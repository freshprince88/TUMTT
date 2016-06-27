using System.Linq;
using Caliburn.Micro;
using TT.Lib.Managers;
using TT.Models;
using System.Collections.ObjectModel;
using TT.Lib.Interfaces;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;
using TT.Lib.Util;
using TT.Lib.Results;
using System.Collections.Generic;

namespace TT.Scouter.ViewModels
{
    public class LiveScoutingViewModel : Screen
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;
        public int LengthHelper
        {
            get
            {
                return CurrentRally.Length;
            }
            set
            {
                if (CurrentRally.Length != value)
                {
                    var diff = value - CurrentRally.Length;
                    if (CurrentRally.Length < value)
                    {
                        for (int i = 0; i < diff; i++)
                        {
                            CurrentRally.Strokes.Add(new Stroke());
                        }

                    }
                    else if (CurrentRally.Length > value)
                    {
                        diff = -diff;
                        for (int i = 0; i < diff; i++)
                        {
                            CurrentRally.Strokes.Remove(CurrentRally.Strokes.Last());
                        }
                    }

                    CurrentRally.Length = value;
                    NotifyOfPropertyChange();
                    NotifyOfPropertyChange("CurrentRally");
                }
            }
        }
        public IMediaPosition MediaPlayer { get; set; }

        public MatchPlayer Server { get; set; }
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

                Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
                bool mark = marked != null && marked.Rallies != null && marked.Rallies.Contains(CurrentRally);

                if (mark != Markiert)
                {
                    Markiert = mark;
                    //NotifyOfPropertyChange();
                    NotifyOfPropertyChange("Markiert");
                }
                NotifyOfPropertyChange();
                NotifyOfPropertyChange("LengthHelper");
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
        private bool _winnerEnabled;
        /// <summary>
        /// Determines whether the "Winner of the Rally" Buttons are shown
        /// </summary>
        public bool IsWinnerEnabled
        {
            get { return _winnerEnabled; }
            set
            {
                if (_winnerEnabled != value)
                {
                    _winnerEnabled = value;
                    NotifyOfPropertyChange();
                }

            }
        }
        public bool Markiert { get; set; }
        public LiveViewModel LiveView { get; private set; }

        public LiveScoutingViewModel (IEventAggregator ev, IMatchManager man, IMediaPosition mp, LiveViewModel live)
        {
            Events = ev;
            MatchManager = man;
            MediaPlayer = mp;
            LiveView = live;
            CurrentRally = MatchManager.ActivePlaylist.Rallies.FirstOrDefault();

        }


        #region View Methods
        public void SetRallyLength(int length)
        {
            LengthHelper = length;
            NotifyOfPropertyChange("LengthHelper");
            MatchManager.MatchModified = true;

        }

        public void RallyWon(int player, int length)
        {

            if (!IsNewRally)
            {
                // Add CurrentRally to Playlists (Alle und Markiert, falls Checkbox)
                //   -> CurrentRally neu setzen mit Bindings
                if (player == 1)
                    CurrentRally.Winner = MatchPlayer.First;
                else
                    CurrentRally.Winner = MatchPlayer.Second;

                CurrentRally.End = MediaPlayer.MediaPosition.TotalMilliseconds + Match.Synchro;

                if (Markiert)
                {
                    Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
                    marked.Rallies.Add(CurrentRally);
                }

                CurrentRally = new Rally();
                Rallies.Add(CurrentRally);
                CurrentRally.UpdateServerAndScore();
                Server = CurrentRally.Server;
                //LengthHelper = 0;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange("Server");
                IsNewRally = true;
                IsWinnerEnabled = false;
                MatchManager.MatchModified = true;

            }
        }

        public void StartRally()
        {
            if (IsNewRally)
            {
                CurrentRally.Start = MediaPlayer.MediaPosition.TotalMilliseconds + Match.Synchro;
                IsNewRally = false;
                IsWinnerEnabled = true;
                MediaPlayer.Play();
                MatchManager.MatchModified = true;

            }
        }
        #endregion
    }
}
