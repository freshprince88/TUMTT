using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;
using TT.Scouter.Interfaces;

namespace TT.Scouter.ViewModels
{
    public class RemoteViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;

        public IMediaPosition MediaPlayer { get; set; }

        public Match Match { get { return MatchManager.Match; } }
        public IEnumerable<Rally> Rallies { get { return MatchManager.ActivePlaylist.Rallies; } }
        public int RallyCount { get { return Rallies.Count(); } }
        public RemoteSchlagViewModel SchlagView { get; set; }

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
                        SchlagView = new RemoteSchlagViewModel(value.Schläge);
                    else
                    {
                        SchlagView.Strokes = CurrentRally.Schläge;
                    }
                    _rally = value;

                    if(_rally.Length > 0)
                        CurrentStroke = _rally.Schläge.FirstOrDefault();

                    NotifyOfPropertyChange("CurrentRally");
                    NotifyOfPropertyChange("CurrentStroke");
                    NotifyOfPropertyChange("LengthHelper");


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
                        SchlagView.ActivateItem(new ServiceDetailViewModel(CurrentStroke));
                    }
                    else
                    {
                        SchlagView.ActivateItem(new SchlagDetailViewModel(CurrentStroke));
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
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            this.ActivateItem(MediaPlayer);
            this.ActivateItem(SchlagView);
        }

        #region View Methods

        public void RallySelected(SelectionChangedEventArgs e)
        {
            Rally item = e.AddedItems.Count > 0 ? (Rally)e.AddedItems[0] : null;

            if (item != null)
            {
                CurrentRally = item;
                TimeSpan anfangRally = TimeSpan.FromMilliseconds(item.Anfang);
                TimeSpan endeRally = TimeSpan.FromMilliseconds(item.Ende);
                MediaPlayer.MediaPosition = anfangRally;
                MediaPlayer.EndPosition = endeRally;
                MediaPlayer.Play();
                
                
               
            }
        }

        public void NextRally()
        {
            var rally = Rallies.Where(r => r.Nummer == CurrentRally.Nummer + 1).FirstOrDefault();
            Events.PublishOnUIThread(new ResultListControlEvent(rally));
            CurrentRally = rally;
        }

        public void PreviousRally()
        {
            var rally = Rallies.Where(r => r.Nummer == CurrentRally.Nummer - 1).FirstOrDefault();
            Events.PublishOnUIThread(new ResultListControlEvent(rally));
            CurrentRally = rally;
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
            }

            #endregion

        }
    }
}
