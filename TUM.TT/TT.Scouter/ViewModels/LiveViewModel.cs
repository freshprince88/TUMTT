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
using System.Windows.Input;
using System.Reflection;
using System;

namespace TT.Scouter.ViewModels
{
    public class LiveViewModel : Conductor<IScreen>.Collection.AllActive
    {
        /// <summary>
        /// Sets key bindings for ControlWithBindableKeyGestures
        /// </summary>
        public Dictionary<string, KeyBinding> KeyBindings
        {
            get
            {
                //get all method names of this class
                var methodNames = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Select(info => info.Name);

                //get all existing key gestures that match the method names
                var keyGesture = ShortcutFactory.Instance.KeyGestures.Where(pair => methodNames.Contains(pair.Key));

                //return relevant key gestures
                return keyGesture.ToDictionary(x => x.Key, x => (KeyBinding)x.Value); // TODO
            }
            set { }
        }

        private IEventAggregator Events;
        private IMatchManager MatchManager;
        private IDialogCoordinator Dialogs;

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


        public enum TimeMode
        {
            None,
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
                    //this.DeactivateItem(MediaPlayer, true);
                    _mode = value;
                    switch (_mode)
                    {
                        case TimeMode.Video:
                            MediaPlayer = new LiveMediaViewModel(Events, MatchManager, Dialogs);
                            break;
                        case TimeMode.Timer:
                            MediaPlayer = new LiveTimerViewModel();
                            break;
                        default:
                            break;
                    }
                    //this.ActivateItem(MediaPlayer);
                    NotifyOfPropertyChange();
                }
            }
        }

        public IMediaPosition MediaPlayer { get; set; }

        public MatchPlayer Server { get; set; }
        public MatchPlayer firstServerBackup { get; set; }

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
                NotifyOfPropertyChange("CurrentRally");

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
        public ChoiceOfEndsViewModel ChoiceOfEnds { get; private set; }
        public ChoiceOfServiceReceiveViewModel ChoiceOfServiceReceive { get; private set; }
        public LiveScoutingViewModel LiveScouting { get; private set; }

        private Screen _trans;
        public Screen TransitioningContent
        {
            get { return _trans; }
            set
            {
                if (_trans != value)
                {
                    _trans = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public Screen CurrentScreen { get; set; }

        public LiveViewModel(IEventAggregator ev, IMatchManager man,IDialogCoordinator cor)
        {
            Events = ev;
            MatchManager = man;
            Dialogs = cor;
            IsNewRally = true;
            IsWinnerEnabled = false;
            CurrentRally = MatchManager.ActivePlaylist.Rallies.FirstOrDefault();
            //Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
            //Markiert = marked != null && marked.Rallies != null && marked.Rallies.Contains(CurrentRally);
            //MediaPlayer = new LiveMediaViewModel(Events, MatchManager,Dialogs);
            ChoiceOfEnds = new ChoiceOfEndsViewModel(Events, MatchManager, this);
            ChoiceOfServiceReceive = new ChoiceOfServiceReceiveViewModel(Events, MatchManager, this);
            LiveScouting = new LiveScoutingViewModel(Events, MatchManager, MediaPlayer, this);

            if (MatchManager.Match.FirstPlayer.StartingTableEnd == StartingTableEnd.None || MatchManager.Match.SecondPlayer.StartingTableEnd == StartingTableEnd.None)
            {
                CurrentScreen = ChoiceOfEnds;
            }
            else
            {
                if (!FirstServerSet)
                {
                    CurrentScreen = ChoiceOfEnds;
                }
                else
                {
                    CurrentScreen = LiveScouting;
                }
            }
        }

        #region Caliburn Hooks
        protected override void OnActivate()
        {
            base.OnActivate();
            this.Events.Subscribe(this);

            if (ViewMode == TimeMode.None && !String.IsNullOrWhiteSpace(MatchManager.Match.VideoFile))
            {
                ViewMode = TimeMode.Video;
                NotifyOfPropertyChange("MediaPlayer");
            }


            this.ActivateItem(MediaPlayer);
            this.ActivateItem(ChoiceOfEnds);
            this.ActivateItem(ChoiceOfServiceReceive);
            this.ActivateItem(LiveScouting);
            TransitioningContent = CurrentScreen;
        }
        protected override void OnDeactivate(bool close)
        {
            Events.Unsubscribe(this);
            base.OnDeactivate(close);
        }


        #endregion

        #region View Methods

        public void SetRallyLength(int length)
        {
            LengthHelper = length;
            NotifyOfPropertyChange("LengthHelper");
            MatchManager.MatchModified = true;

        }

        public void RallyWon(int player)
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
                NotifyOfPropertyChange("CurrentRally");
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

        public void SetNewStart()
        {
            if (!IsNewRally)
            {
                CurrentRally.Start = MediaPlayer.MediaPosition.TotalMilliseconds + Match.Synchro;
                MatchManager.MatchModified = true;
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
            MatchManager.MatchModified = true;
        }
        public void DeleteLastRally()
        {
            if (Rallies.Count == 2)
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
                this.Server = firstServerBackup;
                CurrentRally.Server = this.Server;
                NotifyOfPropertyChange("LiveView.Server");
                NotifyOfPropertyChange("LiveView.FirstServerSet");
                MatchManager.MatchModified = true;
                CurrentRally.UpdateServerAndScore();
                IsNewRally = true;
                IsWinnerEnabled = false;
                MatchManager.MatchModified = true;
                NotifyOfPropertyChange("FirstServerSet");
                NotifyOfPropertyChange("CurrentRally");

            }
            if (Rallies.Count > 2)
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
                IsNewRally = true;
                IsWinnerEnabled = false;
                MatchManager.MatchModified = true;
                NotifyOfPropertyChange("FirstServerSet");
                NotifyOfPropertyChange("CurrentRally");

            }

        }

        public void FinalizeLiveMode()
        {
            if (Rallies.Last().Winner == MatchPlayer.None && Rallies.Last().Length == 0)
            {
                Rallies.Remove(Rallies.Last());
                Rallies.Last().UpdateServerAndScore();
            }
        }

        public void ChangeTransitioningContent()
        {
            TransitioningContent = CurrentScreen;
        }

        #endregion

        #region Helper Methods for Shortcuts

        public void DeleteLastRallyLiveMode()
        {
            DeleteLastRally();
        }
     
        

    


        #endregion
    }
}
