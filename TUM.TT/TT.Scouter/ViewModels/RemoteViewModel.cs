using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Managers;
using TT.Lib.Models;

namespace TT.Scouter.ViewModels
{
    public class RemoteViewModel : Conductor<IScreen>.Collection.AllActive 
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;
        public Match Match { get { return MatchManager.Match;} }
        public IEnumerable<Rally> Rallies { get { return MatchManager.Match.Playlists[0].Rallies; } }
        public Rally CurrentRally { get; set; }
        public Schlag CurrentStroke { get; set; }
        public RemoteSchlagViewModel RemoteSchlagView;

        public RemoteViewModel() : this(null, null)
        {
        }

        public RemoteViewModel(IEventAggregator ev, IMatchManager man)
        {
            Events = ev;
            MatchManager = man;
            RemoteSchlagView = new RemoteSchlagViewModel();            
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            RemoteSchlagView.ActivateItem(new ServiceDetailViewModel(CurrentStroke));
        }

    }
}
