using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Events;
using TT.Lib.Managers;

namespace TT.Scouter.ViewModels
{
    public class WelcomeViewModel : Screen
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;

        public WelcomeViewModel(IEventAggregator ev, IMatchManager man)
        {
            Events = ev;
            MatchManager = man;
        }

        #region View Methods

        public void OpenNewMatch()
        {
            var newMatch = new NewMatchViewModel(Events, MatchManager);
            Events.PublishOnUIThread(new SetShellContentEvent(newMatch));
        }

        public void OpenMatch()
        {
            //TODO: Open DetailView in Shell
        }

        #endregion
    }

}
