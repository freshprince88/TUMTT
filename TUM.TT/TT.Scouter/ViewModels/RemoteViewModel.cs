﻿using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;
using TT.Scouter.Util.Model;
using TT.Lib.Interfaces;
using System.Reflection;
using System.Windows.Input;
using TT.Lib.Util;

namespace TT.Scouter.ViewModels
{
    public class RemoteViewModel : Conductor<IScreen>.Collection.AllActive, IHandle<PlayModeEvent>
    {
        #region Properties
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
        private Calibration calibration = new Calibration();

        public IMediaPosition MediaPlayer { get; set; }

        public Match Match { get { return MatchManager.Match; } }
        public IEnumerable<Rally> Rallies { get { return MatchManager.Match.Rallies; } }
        public RemoteStrokeViewModel SchlagView { get; set; }

        private bool _service;
        public bool ServiceChecked
        {
            get
            {
                return _service;
            }
            set
            {
                if (_service != value)
                    _service = value;
                NotifyOfPropertyChange("ServiceChecked");
            }
        }

        private bool _receive;
        public bool ReceiveChecked
        {
            get
            {
                return _receive;
            }
            set
            {
                if (_receive != value)
                    _receive = value;
                NotifyOfPropertyChange("ReceiveChecked");
            }
        }

        private bool _third;
        public bool ThirdChecked
        {
            get
            {
                return _third;
            }
            set
            {
                if (_third != value)
                    _third = value;
                NotifyOfPropertyChange("ThirdChecked");
            }
        }

        private bool _fourth;
        public bool FourthChecked
        {
            get
            {
                return _fourth;
            }
            set
            {
                if (_fourth != value)
                    _fourth = value;
                NotifyOfPropertyChange("FourthChecked");
            }
        }

        private bool _last;
        public bool LastChecked
        {
            get
            {
                return _last;
            }
            set
            {
                if (_last != value)
                    _last = value;
                NotifyOfPropertyChange("LastChecked");
            }
        }

        public RemotePositionsRallyViewModel PositionsRallyView { get; set; }

        public bool HasLength
        {
            get
            {
                return LengthHelper > 0;
            }
        }

        public int LengthHelper
        {
            get
            {
                return CurrentRally?.Length ?? 0;
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
                        SchlagView.Strokes = CurrentRally.Strokes;
                        Events.PublishOnUIThread(new RalliesStrokesAddedEvent());
                        PositionsRallyView.OnNewStrokes();
                        //SchlagView.CurrentStroke = CurrentStroke;

                    }
                    else if (CurrentRally.Length > value && value != 0)
                    {
                        diff = -diff;
                        for (int i = 0; i < diff; i++)
                        {
                            CurrentRally.Strokes.Remove(CurrentRally.Strokes.Last());
                        }
                        SchlagView.Strokes = CurrentRally.Strokes;
                        SchlagView.CurrentStroke = CurrentRally.Strokes.Last();
                        PositionsRallyView.OnNewStrokes();
                    }
                    else if (value == 0)
                    {
                        diff = -diff;
                        for (int i = 0; i < diff; i++)
                        {
                            CurrentRally.Strokes.Remove(CurrentRally.Strokes.Last());
                        }
                        PositionsRallyView.OnNewStrokes();
                    }

                    CurrentRally.Length = value;
                    NotifyOfPropertyChange();
                    NotifyOfPropertyChange("SchlagView");
                    NotifyOfPropertyChange("SchlagView.CurrentRally");
                    NotifyOfPropertyChange("LengthHelper");
                    NotifyOfPropertyChange("CurrentRally");
                    NotifyOfPropertyChange("HasLength");
                }
            }
        }


        private Rally _rally;
        public Rally CurrentRally
        {
            get { return _rally; }
            set
            {
                if (value != null && (fromConstructor || _rally != value))
                {
                    MatchManager.ActiveRally = value;

                    Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
                    bool mark = marked != null && marked.Rallies != null && marked.Rallies.Contains(MatchManager.ActiveRally);

                    if (mark != IsMarked)
                    {
                        IsMarked = mark;
                        //NotifyOfPropertyChange();
                                          
                    }

                    if (SchlagView == null || PositionsRallyView == null)
                    {
                        // Positioning IMPORTANT - this way PositionsRallyView gets notified about a calculated Stroke before SchlagView and PositionRallyView depends on the Schlagviewstate
                        if (PositionsRallyView == null) PositionsRallyView = new RemotePositionsRallyViewModel(this, calibration);
                        if (SchlagView == null) SchlagView = new RemoteStrokeViewModel(this, MatchManager, value, calibration);
                    }
                    else
                    {
                        //TODO Hier kommt er nicht rein, wenn man die Länge verändert -> keine neuen Schläge werden erstellt!!!!
                        SchlagView.Strokes = value.Strokes;
                        SchlagView.CurrentRally = value;
                        PositionsRallyView.OnNewStrokes();
                    }
                    _rally = value;

                    if (_rally.Length > 0)
                    {
                        SchlagView.CurrentStroke = _rally.Strokes.FirstOrDefault();
                        if (ServiceChecked)
                        {
                            SchlagView.CurrentStroke = _rally.Strokes[0];
                        }
                        if (ReceiveChecked)
                        {
                            if (_rally.Length > 1)
                            {
                                SchlagView.CurrentStroke = _rally.Strokes[1];
                            }
                            else
                            {
                                SchlagView.CurrentStroke = _rally.Strokes.Last();
                            }
                        }

                        if (ThirdChecked)
                        {
                            if (_rally.Length > 2)
                            {
                                SchlagView.CurrentStroke = _rally.Strokes[2];
                            }
                            else
                            {
                                SchlagView.CurrentStroke = _rally.Strokes.Last();
                            }
                        }
                        if (FourthChecked)
                        {
                            if (_rally.Length > 3)
                            {
                                SchlagView.CurrentStroke = _rally.Strokes[3];
                            }
                            else
                            {
                                SchlagView.CurrentStroke = _rally.Strokes.Last();
                            }
                        }
                        if (LastChecked)
                        {
                            if (_rally.Winner == _rally.Strokes[_rally.Strokes.Count - 1].Player)
                            {
                                SchlagView.CurrentStroke = _rally.Strokes[_rally.Strokes.Count - 1];
                            }
                            else
                            {
                                if (_rally.Length > 1)
                                {
                                    SchlagView.CurrentStroke = _rally.Strokes[_rally.Strokes.Count - 2];
                                }
                                else
                                    SchlagView.CurrentStroke = _rally.Strokes[0];
                            }

                        }
                    }

                    NotifyOfPropertyChange("CurrentRally");
                    NotifyOfPropertyChange("CurrentStroke");
                    NotifyOfPropertyChange("LengthHelper");
                    NotifyOfPropertyChange("SchlagView.Strokes");
                    NotifyOfPropertyChange("SchlagView.CurrentRally");
                    NotifyOfPropertyChange("HasLength");
                    NotifyOfPropertyChange("IsMarked");
                }
            }
        }

        private Stroke _stroke;
        public Stroke CurrentStroke
        {
            get { return _stroke; }
            set
            {
                if (_stroke != value && value != null && CurrentRally != null)
                {
                    _stroke = value;
                    NotifyOfPropertyChange("CurrentStroke");

                    if (_stroke.Number == 1)
                    {
                        SchlagView.ActivateItem(new ServiceDetailViewModel(CurrentStroke, MatchManager, CurrentRally));
                    }
                    else
                    {
                        SchlagView.ActivateItem(new StrokeDetailViewModel(CurrentStroke, MatchManager, CurrentRally));
                    }

                    PositionsRallyView.OnCurrentStrokeChanged();
                }
            }
        }

        bool fromConstructor;

        public IDialogCoordinator Dialogs { get; set; }

        private bool _marked;
        public bool IsMarked
        {
            get
            {
                return _marked;
            }
            set
            {
                if (_marked != value)
                    _marked = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        public RemoteViewModel(IEventAggregator ev, IMatchManager man, IDialogCoordinator dia)
        {
            Events = ev;
            MatchManager = man;
            Dialogs = dia;
            fromConstructor = true;
            CurrentRally = MatchManager.Match.Rallies.FirstOrDefault();
            ServiceChecked = true;
            ReceiveChecked = false;
            ThirdChecked = false;
            FourthChecked = false;
            LastChecked = false;
            IsMarked = false;
        }
        protected override void OnActivate()
        {
            base.OnActivate();
            this.Events.Subscribe(this);

            if (MediaPlayer == null)
            {
                MediaPlayer = new RemoteMediaViewModel(Events, MatchManager, Dialogs, calibration);
                NotifyOfPropertyChange("MediaPlayer");
            }

            this.ActivateItem(MediaPlayer);
            this.ActivateItem(SchlagView);
            this.ActivateItem(PositionsRallyView);

            if (fromConstructor)
                CurrentRally = MatchManager.Match.Rallies.FirstOrDefault();

            fromConstructor = false;
        }

        protected override void OnDeactivate(bool close)
        {
            Events.Unsubscribe(this);
            base.OnDeactivate(close);
        }

        #region View Methods

        public void RallySelected(SelectionChangedEventArgs e)
        {
            Rally item = e.AddedItems.Count > 0 ? (Rally)e.AddedItems[0] : null;

            if (item != null)
            {
                if (MediaPlayer == null)
                    return;

                if (MediaPlayer.toRallyStart == true)
                {
                    CurrentRally = item;
                    TimeSpan anfangRally = TimeSpan.FromMilliseconds(item.Start);
                    TimeSpan endeRally = TimeSpan.FromMilliseconds(item.End);
                    MediaPlayer.MediaPosition = anfangRally;
                    MediaPlayer.EndPosition = endeRally;
                    MediaPlayer.Play();
                }
                else if (MediaPlayer.toRallyStart != true)
                {
                    CurrentRally = item;
                    TimeSpan anfangRally = TimeSpan.FromMilliseconds(item.Start);
                    TimeSpan vorEndeRally = TimeSpan.FromMilliseconds(item.End - 1000);
                    TimeSpan endeRally = TimeSpan.FromMilliseconds(item.End);
                    MediaPlayer.MediaPosition = vorEndeRally;
                    MediaPlayer.EndPosition = endeRally;
                    MediaPlayer.Play();
                }
            }
        }

        public void NextRally()
        {
            if (Rallies.Where(r => r.Number == CurrentRally.Number + 1).FirstOrDefault() != null)
            {
                var rally = Rallies.Where(r => r.Number == CurrentRally.Number + 1).FirstOrDefault();
                CurrentRally = rally;
            }
        }

        public void PreviousRally()
        {
            if (Rallies.Where(r => r.Number == CurrentRally.Number - 1).FirstOrDefault() != null)
            {
                var rally = Rallies.Where(r => r.Number == CurrentRally.Number - 1).FirstOrDefault();
                CurrentRally = rally;
            }
        }

        public void StartRallyAtBeginning()
        {
            MediaPlayer.MediaPosition = TimeSpan.FromMilliseconds(CurrentRally.Start);

        }

        public void PlusSecond(int i)
        {
            if (i == 1)
            {
                CurrentRally.Start = CurrentRally.Start + 500;

            }
            else if (i == 2)
            {
                CurrentRally.End = CurrentRally.End + 500;
                MediaPlayer.EndPosition = TimeSpan.FromMilliseconds(CurrentRally.End);
            }

        }

        public void MinusSecond(int i)
        {
            if (i == 1)
            {
                CurrentRally.Start = CurrentRally.Start - 500;
            }
            else if (i == 2)
            {
                CurrentRally.End = CurrentRally.End - 500;
                MediaPlayer.EndPosition = TimeSpan.FromMilliseconds(CurrentRally.End);
            }
        }

        public void SetDefaultStroke(MenuItem m)
        {
            switch (m.Header.ToString())
            {
                case "Service":
                    ServiceChecked = true;
                    ReceiveChecked = false;
                    ThirdChecked = false;
                    FourthChecked = false;
                    LastChecked = false;
                    break;
                case "Receive":
                    ServiceChecked = false;
                    ReceiveChecked = true;
                    ThirdChecked = false;
                    FourthChecked = false;
                    LastChecked = false;
                    break;
                case "3rd Stroke":
                    ServiceChecked = false;
                    ReceiveChecked = false;
                    ThirdChecked = true;
                    FourthChecked = false;
                    LastChecked = false;
                    break;
                case "4th Stroke":
                    ServiceChecked = false;
                    ReceiveChecked = false;
                    ThirdChecked = false;
                    FourthChecked = true;
                    LastChecked = false;
                    break;
                case "Last Stroke":
                    ServiceChecked = false;
                    ReceiveChecked = false;
                    ThirdChecked = false;
                    FourthChecked = false;
                    LastChecked = true;
                    break;
            }

        }

        public void AddToMarkedPlaylist()
        {

            if (IsMarked)
            {
                Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
                marked.Add(CurrentRally);
            }

            else
            {
                Playlist marked = Match.Playlists.Where(p => p.Name == "Markiert").FirstOrDefault();
                marked.Remove(CurrentRally);

            }
            MatchManager.MatchModified = true;
            NotifyOfPropertyChange("MatchManager.MatchModified");
            NotifyOfPropertyChange();
            NotifyOfPropertyChange("IsMarked");
            NotifyOfPropertyChange("CurrentRally");



        }

        #endregion




        private void SetMatchModified(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            MatchManager.MatchModified = true;

        }

        #region Events

        public void Handle(PlayModeEvent message)
        {
            switch (message.PlayMode)
            {
                case null:
                    NextRally();
                    break;
                case false:
                    break;
                case true:
                    StartRallyAtBeginning();
                    break;

                default:
                    break;
            }
        }

        public void CalibrateTable()
        {
            ((RemoteMediaViewModel)MediaPlayer).CalibrateTable();
        }

        public void ToogleCalibration()
        {
            ((RemoteMediaViewModel)MediaPlayer).ToogleCalibration();
        }

        #endregion
    }
}
