using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;
using TT.Lib.Interfaces;

namespace TT.Scouter.ViewModels
{
    public class RemoteViewModel : Conductor<IScreen>.Collection.AllActive, IHandle<PlayModeEvent>
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;

        public IMediaPosition MediaPlayer { get; set; }

        public Match Match { get { return MatchManager.Match; } }
        public IEnumerable<Rally> Rallies { get { return MatchManager.ActivePlaylist.Rallies; } }
        public int RallyCount { get { return Rallies.Count(); } }
        public RemoteSchlagViewModel SchlagView { get; set; }
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
                        SchlagView.CurrentStroke = CurrentRally.Schläge.Last();
                    }

                    CurrentRally.Length = value;
                    NotifyOfPropertyChange();
                    NotifyOfPropertyChange("CurrentRally");
                }
            }
        }


        private Rally _rally;
        public Rally CurrentRally
        {
            get { return _rally; }
            set
            {
                if (_rally != value)
                {


                    if (SchlagView == null)
                        SchlagView = new RemoteSchlagViewModel(MatchManager, value);
                    else
                    {
                        SchlagView.Strokes = value.Schläge;
                        SchlagView.CurrentRally = value;
                    }
                    _rally = value;

                    if (_rally.Length > 0)
                    {
                        CurrentStroke = _rally.Schläge.FirstOrDefault();
                        if (ServiceChecked)
                        {
                            CurrentStroke = _rally.Schläge[0];
                        }
                        if (ReceiveChecked)
                        {
                            if (_rally.Length > 1)
                            {
                                CurrentStroke = _rally.Schläge[1];
                            }
                            else
                            {
                                CurrentStroke = _rally.Schläge.Last();
                            }
                        }

                        if (ThirdChecked)
                        {
                            if (_rally.Length > 2)
                            {
                                CurrentStroke = _rally.Schläge[2];
                            }
                            else
                            {
                                CurrentStroke = _rally.Schläge.Last();
                            }
                        }
                        if (FourthChecked)
                        {
                            if (_rally.Length > 3)
                            {
                                CurrentStroke = _rally.Schläge[3];
                            }
                            else
                            {
                                CurrentStroke = _rally.Schläge.Last();
                            }
                        }
                        if (LastChecked)
                        {
                            CurrentStroke = _rally.Schläge.Last();
                        }



                    }


                    NotifyOfPropertyChange("CurrentRally");
                    NotifyOfPropertyChange("CurrentStroke");
                    NotifyOfPropertyChange("LengthHelper");
                    NotifyOfPropertyChange("SchlagView.Strokes");
                    NotifyOfPropertyChange("SchlagView.CurrentRally");



                }
            }
        }

        private Schlag _stroke;
        public Schlag CurrentStroke
        {
            get { return _stroke; }
            set
            {
                if (_stroke != value && value != null)
                {
                    _stroke = value;
                    NotifyOfPropertyChange("CurrentStroke");

                    if (_stroke.Nummer == 1)
                    {
                        SchlagView.ActivateItem(new ServiceDetailViewModel(CurrentStroke, MatchManager));
                    }
                    else
                    {
                        SchlagView.ActivateItem(new SchlagDetailViewModel(CurrentStroke, MatchManager));
                    }
                }
            }
        }



        public RemoteViewModel(IEventAggregator ev, IMatchManager man, IDialogCoordinator dia)
        {
            Events = ev;
            MatchManager = man;
            CurrentRally = MatchManager.ActivePlaylist.Rallies.First();
            MediaPlayer = new RemoteMediaViewModel(Events, MatchManager, dia);
            ServiceChecked = true;
            ReceiveChecked = false;
            ThirdChecked = false;
            FourthChecked = false;
            LastChecked = false;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            this.Events.Subscribe(this);
            this.ActivateItem(MediaPlayer);
            this.ActivateItem(SchlagView);
        }

        #region View Methods

        public void RallySelected(SelectionChangedEventArgs e)
        {
            Rally item = e.AddedItems.Count > 0 ? (Rally)e.AddedItems[0] : null;

            if (item != null)
            {
                if (MediaPlayer.toRallyStart == true)
                {
                    CurrentRally = item;
                    TimeSpan anfangRally = TimeSpan.FromMilliseconds(item.Anfang);
                    TimeSpan endeRally = TimeSpan.FromMilliseconds(item.Ende);
                    MediaPlayer.MediaPosition = anfangRally;
                    MediaPlayer.EndPosition = endeRally;
                    MediaPlayer.Play();
                }
                else if (MediaPlayer.toRallyStart != true)
                {
                    CurrentRally = item;
                    TimeSpan anfangRally = TimeSpan.FromMilliseconds(item.Anfang);
                    TimeSpan vorEndeRally = TimeSpan.FromMilliseconds(item.Ende - 1000);
                    TimeSpan endeRally = TimeSpan.FromMilliseconds(item.Ende);
                    MediaPlayer.MediaPosition = vorEndeRally;
                    MediaPlayer.EndPosition = endeRally;
                    MediaPlayer.Play();
                }
            }
        }

        public void NextRally()
        {
            if (Rallies.Where(r => r.Nummer == CurrentRally.Nummer + 1).FirstOrDefault() != null)
            {
                var rally = Rallies.Where(r => r.Nummer == CurrentRally.Nummer + 1).FirstOrDefault();
                Events.PublishOnUIThread(new ResultListControlEvent(rally));
                CurrentRally = rally;
            }
        }

        public void PreviousRally()
        {
            if (Rallies.Where(r => r.Nummer == CurrentRally.Nummer - 1).FirstOrDefault() != null)
            {
                var rally = Rallies.Where(r => r.Nummer == CurrentRally.Nummer - 1).FirstOrDefault();
                Events.PublishOnUIThread(new ResultListControlEvent(rally));
                CurrentRally = rally;
            }
        }
        public void StartRallyAtBeginning()
        {
            MediaPlayer.MediaPosition = TimeSpan.FromMilliseconds(CurrentRally.Anfang);

        }
        public void PlusSecond(int i)
        {
            if (i == 1)
            {
                CurrentRally.Anfang = CurrentRally.Anfang + 500;

            }
            else if (i == 2)
            {
                CurrentRally.Ende = CurrentRally.Ende + 500;
                MediaPlayer.EndPosition = TimeSpan.FromMilliseconds(CurrentRally.Ende);
            }

        }
        public void MinusSecond(int i)
        {
            if (i == 1)
            {
                CurrentRally.Anfang = CurrentRally.Anfang - 500;
            }
            else if (i == 2)
            {
                CurrentRally.Ende = CurrentRally.Ende - 500;
                MediaPlayer.EndPosition = TimeSpan.FromMilliseconds(CurrentRally.Ende);
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


        #endregion

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
        #endregion
    }
}
