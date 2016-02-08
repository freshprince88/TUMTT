using Caliburn.Micro;
using TT.Lib.Managers;

namespace TT.Scouter.ViewModels
{
    public class NewMatchViewModel : Screen
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;

        public NewMatchViewModel(IEventAggregator ev, IMatchManager man)
        {
            Events = ev;
            MatchManager = man;
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            MatchManager.CreateNewMatch();
        }
    }
}
