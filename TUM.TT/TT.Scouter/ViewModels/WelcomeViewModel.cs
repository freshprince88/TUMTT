using Caliburn.Micro;
using System.Collections.Generic;
using TT.Lib.Managers;
using TT.Lib.Results;

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

        public IEnumerable<IResult> OpenNewMatch()
        {
            MatchManager.CreateNewMatch();
            var next = ShowScreenResult.Of<NewMatchViewModel>();
            yield return next;
        }

        public IEnumerable<IResult> OpenMatch()
        {
            //      Load Match
            //      Open RemoteView in Shell
            foreach (IResult result in MatchManager.OpenMatch())
            {
                yield return result;
            }
            var next = ShowScreenResult.Of<MainViewModel>();
            next.Properties.Add("SelectedTab", MainViewModel.Tabs.Remote);
            yield return next;
        }
        public IEnumerable<IResult> OpenMatchWithoutVideo()
        {
            foreach (IResult result in MatchManager.OpenLiveMatch())
            {
                yield return result;
            }
            var next = ShowScreenResult.Of<NewMatchViewModel>();
            yield return next;
        }

        #endregion
    }

}
