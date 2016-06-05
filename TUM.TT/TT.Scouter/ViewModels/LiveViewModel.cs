﻿using System.Linq;
using Caliburn.Micro;
using TT.Lib.Managers;
using TT.Models;
using System.Collections.ObjectModel;
using TT.Scouter.Interfaces;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;
using TT.Lib.Util;
using TT.Lib.Results;
using System.Collections.Generic;

namespace TT.Scouter.ViewModels
{
    public class LiveViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;
        public bool FirstServerSet
        {
            get
            {
                return Match.FirstServer != MatchPlayer.None;
                 
            }

            
        }
       
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
                            CurrentRally.Schläge.Add(new Schlag());
                        }

                    }
                    else if (CurrentRally.Length > value)
                    {
                        diff = -diff;
                        for (int i = 0; i < diff; i++)
                        {
                            CurrentRally.Schläge.Remove(CurrentRally.Schläge.Last());
                        }
                    }

                    CurrentRally.Length = value;
                    NotifyOfPropertyChange();
                    NotifyOfPropertyChange("CurrentRally");
                }
            }
        }


        public enum TimeMode
        {
            Video,
            Timer
        }

        private TimeMode _mode;
        public TimeMode ViewMode
        {
            get
            {
                return _mode;
            }
            set
            {
                if (_mode != value)
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

        public LiveViewModel(IEventAggregator ev, IMatchManager man)
        {
          
            Events = ev;
            MatchManager = man;
            IsNewRally = true;
            IsWinnerEnabled = false;
            CurrentRally = MatchManager.ActivePlaylist.Rallies.FirstOrDefault();
            //Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
            //Markiert = marked != null && marked.Rallies != null && marked.Rallies.Contains(CurrentRally);
            MediaPlayer = new LiveMediaViewModel(Events, MatchManager);
            
        }

        #region Caliburn Hooks

        protected override void OnActivate()
        {
            base.OnActivate();
            this.ActivateItem(MediaPlayer);
        }

        protected override void OnViewReady(object view)
        {
            //CoroutineExecutionContext context = new CoroutineExecutionContext() { Target = this, Source = view, View = view };
            //if (Rallies.Count < 2)
            //{
            //    Coroutine.ExecuteAsync(ShowServer().GetEnumerator(), context);
            //}
        }

        #endregion

        #region View Methods

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

            //CurrentRally.Length = length;
            //NotifyOfPropertyChange("CurrentRally");
            LengthHelper = length;
            NotifyOfPropertyChange("LengthHelper");
        }

        public void RallyWon(int player, int length)
        {
            //SetRallyLength(length);
            //LengthHelper = length;
            if (!IsNewRally)
            {
                // Add CurrentRally to Playlists (Alle und Markiert, falls Checkbox)
                //   -> CurrentRally neu setzen mit Bindings
                if (player == 1)
                    CurrentRally.Winner = MatchPlayer.First;
                else
                    CurrentRally.Winner = MatchPlayer.Second;

                CurrentRally.Ende = MediaPlayer.MediaPosition.TotalMilliseconds + Match.Synchro;

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
            }
        }

        public void StartRally()
        {
            if (IsNewRally)
            {
                CurrentRally.Anfang = MediaPlayer.MediaPosition.TotalMilliseconds + Match.Synchro;

                IsNewRally = false;
                IsWinnerEnabled = true;


                MediaPlayer.Play();
            }
        }

        public IEnumerable<IResult> ShowServer()
        {
            var dialog = new CustomClosableComboDialog<Player>();
            dialog.CloseButton = new Button() { Content = "OK" };
            dialog.Combo = new ComboBox() { ItemsSource = Match.Players, SelectedIndex = 0, DisplayMemberPath = "Name" };
            StackPanel panel = new StackPanel() { Orientation = Orientation.Vertical };
            panel.Margin = new System.Windows.Thickness(10);
            TextBlock message = new TextBlock() { Text = "Bitte Aufschlagspieler auswählen" };
            panel.Children.Add(message);
            panel.Children.Add(dialog.Combo);
            panel.Children.Add(dialog.CloseButton);

            dialog.Content = panel;
            var result = new CustomDialogResult<Player>() { Title = "Server", DialogContent = dialog };
            yield return result;

          
            NotifyOfPropertyChange("FirstServerSet");
            this.Server = MatchManager.ConvertPlayer(result.Result);
            CurrentRally.Server = this.Server;
            NotifyOfPropertyChange("Server");
        }
        public void SetFirstServer(Player p)
        {
            
            this.Server = MatchManager.ConvertPlayer(p);
            CurrentRally.Server = this.Server;
            NotifyOfPropertyChange("Server");
            NotifyOfPropertyChange("FirstServerSet");

        }

        public void DeleteLastRally()
        {
            if (Rallies.Count > 1)
            {
                Rally lastRally = Rallies.Last();
                if (lastRally.Winner == MatchPlayer.None)
                {
                    Rallies.Remove(lastRally);
                    lastRally = Rallies.Last();
                }
                
                Rallies.Remove(lastRally);
                Rallies.Add(new Rally());
                CurrentRally = Rallies.Last();
                CurrentRally.UpdateServerAndScore();
                NotifyOfPropertyChange("FirstServerSet");

            }
        }

        //public void UpdateScoreAndServer()
        //{
        //    CurrentRally = Rallies.Last();
        //    CurrentRally = new Rally();
        //    Rallies.Add(CurrentRally);
        //    CurrentRally.UpdateServerAndScore();
        //    Server = CurrentRally.Server;
        //    NotifyOfPropertyChange("Server");
        //    IsNewRally = true;
        //    IsWinnerEnabled = false;
        //    CurrentRally.UpdateServerAndScore();
        //}

        public void FinalizeLiveMode()
        {
            if (Rallies.Last().Winner == MatchPlayer.None && Rallies.Last().Length == 0)
            {
                Rallies.Remove(Rallies.Last());
                Rallies.Last().UpdateServerAndScore();
            }

        }

        #endregion
    }
}
