using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TT.Models.Events;
using TT.Models.Managers;
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

        public bool HasLength
        {
            get
            {
                return RallyLength > 0;
            }
        }

        public int RallyLength
        {
            get { return CurrentRally.Length; }
            set
            {
                if(value != CurrentRally.Length)
                {
                    CurrentRally.Length = value;
                    NotifyOfPropertyChange();
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
                if (_rally != value)
                {
                    _rally = value;
                    CurrentStroke = _rally.Schläge.FirstOrDefault();

                    if (SchlagView == null)
                        SchlagView = new RemoteSchlagViewModel(value.Schläge);
                    else
                        SchlagView.Strokes = CurrentRally.Schläge;

                    NotifyOfPropertyChange("CurrentRally");
                }
            }
        }

        private Schlag _stroke;
        public Schlag CurrentStroke
        {
            get { return _stroke; }
            set
            {
                if (_stroke != value)
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

        public RemoteSchlagViewModel SchlagView { get; set; }

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

        #endregion

    }
}
