using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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

        private Rally _rally;
        public Rally CurrentRally
        {
            get { return _rally; }
            set
            {
                if (_rally != value)
                {
                    _rally = value;
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
                }
            }
        }

        public RemoteSchlagViewModel RemoteSchlagView;

        public RemoteViewModel() : this(null, null)
        {
        }

        public RemoteViewModel(IEventAggregator ev, IMatchManager man)
        {
            Events = ev;
            MatchManager = man;
            CurrentRally = MatchManager.ActivePlaylist.Rallies.First();
            CurrentStroke = CurrentRally.Schläge.First();
            RemoteSchlagView = new RemoteSchlagViewModel();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            RemoteSchlagView.ActivateItem(new ServiceDetailViewModel(CurrentStroke));
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

        #endregion

    }
}
