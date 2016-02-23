using Caliburn.Micro;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Lib.Models;

namespace TT.Scouter.ViewModels
{
    public class RemoteViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;
        public Match Match { get { return MatchManager.Match; } }
        public IEnumerable<Rally> Rallies { get { return MatchManager.ActivePlaylist.Rallies; } }
        public int RallyCount { get { return Rallies.Count(); } }

        private Rally _rally;
        public Rally CurrentRally
        {
            get { return _rally; }
            set
            {
                if (_rally != value)
                {
                    _rally = value;
                    CurrentStroke = _rally.Schläge.First();
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

        public RemoteViewModel() : this(null, null)
        {
        }

        public RemoteViewModel(IEventAggregator ev, IMatchManager man)
        {
            Events = ev;
            MatchManager = man;
            SchlagView = new RemoteSchlagViewModel();
            CurrentRally = MatchManager.ActivePlaylist.Rallies.First();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
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

        public void NextStroke()
        {
            CurrentStroke = CurrentRally.Schläge[CurrentStroke.Nummer];
        }

        public void PreviousStroke()
        {
            var idx = CurrentStroke.Nummer - 1;
            CurrentStroke = CurrentRally.Schläge[idx - 1];
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
