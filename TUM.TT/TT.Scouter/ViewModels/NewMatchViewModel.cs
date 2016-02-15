using Caliburn.Micro;
using System.Collections.Generic;
using TT.Lib.Managers;
using TT.Lib.Models;
using TT.Lib.Results;

namespace TT.Scouter.ViewModels
{
    public class NewMatchViewModel : Screen
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;
        public Match Match { get { return MatchManager.Match; } }

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

        #region View Methods

        public void AddNewPlayer(int num)
        {
            //TODO: Show new Player Screen, afterwards come back here
        }

        public IEnumerable<IResult> SaveMatchDetails()
        {
            //TODO: Save Match Details in MatchManager and go to Video Choice
            var nextScreen = ShowScreenResult.Of<VideoSourceViewModel>();
            yield return nextScreen;
        }

        #endregion
    }
}
