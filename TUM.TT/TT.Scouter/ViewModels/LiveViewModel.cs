using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using TT.Lib.Managers;

namespace TT.Scouter.ViewModels
{
    public class LiveViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;

        public LiveViewModel() : this(null, null)
        {
        }

        public LiveViewModel(IEventAggregator ev, IMatchManager man) 
        {
            Events = ev;
            MatchManager = man;
        }
    }
}
