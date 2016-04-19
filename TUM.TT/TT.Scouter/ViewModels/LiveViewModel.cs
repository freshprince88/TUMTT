﻿using System.Linq;
using Caliburn.Micro;
using TT.Lib.Managers;
using TT.Lib.Models;
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

        private bool _rallyEnded;
        /// <summary>
        /// Determines whether the "Finish Rally" Button is shown
        /// </summary>
        public bool RallyEnded
        {
            get { return _rallyEnded; }
            set
            {
                if (_rallyEnded != value)
                {
                    _rallyEnded = value;
                    NotifyOfPropertyChange();
                }

            }
        }

        private bool _rallyActive;
        /// <summary>
        /// Determines whether the "Start Rally" Button is shown
        /// </summary>
        public bool IsRallyActive
        {
            get { return _rallyActive; }
            set
            {
                if (_rallyActive != value)
                {
                    _rallyActive = value;
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
            RallyEnded = false;
            CurrentRally = MatchManager.ActivePlaylist.Rallies.First();
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
            CoroutineExecutionContext context = new CoroutineExecutionContext() { Target = this, Source = view, View = view };
            Coroutine.ExecuteAsync(ShowServer().GetEnumerator(), context);
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

            CurrentRally.Length = length;
            //NotifyOfPropertyChange("CurrentRally");
        }

        public void RallyWon(int player)
        {
            // Add CurrentRally to Playlists (Alle und Markiert, falls Checkbox)
            //   -> CurrentRally neu setzen mit Bindings
            if (player == 1)
                CurrentRally.Winner = MatchPlayer.First;
            else
                CurrentRally.Winner = MatchPlayer.Second;

            CurrentRally.Ende = MediaPlayer.MediaPosition.TotalMilliseconds;


            IsNewRally = false;
            IsRallyActive = false;
            RallyEnded = true;
        }

        public void StartRally()
        {           
            CurrentRally.Anfang = MediaPlayer.MediaPosition.TotalMilliseconds;

            IsNewRally = false;
            IsRallyActive = true;
            RallyEnded = false;
            MediaPlayer.Play();
        }

        public void FinishRally()
        {
            if (Markiert)
            {
                Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
                marked.Rallies.Add(CurrentRally);
            }

            CurrentRally = new Rally();
            Rallies.Add(CurrentRally);
            CurrentRally.UpdateServerAndScore();
            Server = CurrentRally.Server;
            NotifyOfPropertyChange("Server");

            IsNewRally = true;
            IsRallyActive = false;
            RallyEnded = false;
        }

        public IEnumerable<IResult> ShowServer()
        {
            var dialog = new CustomClosableComboDialog<Player>();
            dialog.CloseButton = new Button() { Content = "OK" };
            dialog.Combo = new ComboBox() { ItemsSource = Match.Players, SelectedIndex = 0, DisplayMemberPath = "Name" };
            StackPanel panel = new StackPanel() { Orientation = Orientation.Vertical };
            panel.Margin = new System.Windows.Thickness(10);
            TextBlock message = new TextBlock() { Text = "Bitte Aufschlagspieler Auswählen" };
            panel.Children.Add(message);
            panel.Children.Add(dialog.Combo);
            panel.Children.Add(dialog.CloseButton);

            dialog.Content = panel;
            var result = new CustomDialogResult<Player>() { Title = "Server", DialogContent = dialog };
            yield return result;

            this.Server = MatchManager.ConvertPlayer(result.Result);
            CurrentRally.Server = this.Server;
            NotifyOfPropertyChange("Server");
        }

        #endregion
    }
}
 