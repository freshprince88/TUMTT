using Caliburn.Micro;
using System.Collections.Generic;
using TT.Lib.Managers;
using TT.Lib.Results;

namespace TT.Scouter.ViewModels
{
    public class VideoSourceViewModel : Screen
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;

        public VideoSourceViewModel(IEventAggregator ev, IMatchManager man)
        {
            Events = ev;
            MatchManager = man;
        }

        #region View Methods

        //Load Video File and go to next scene
        public IEnumerable<IResult> WithVideo()
        {
            var nextScreen = ShowScreenResult.Of<MainViewModel>();
            nextScreen.Properties.Add("SelectedTab", MainViewModel.Tabs.Live);
            nextScreen.Properties.Add("LiveMode", LiveViewModel.TimeMode.Video);
            foreach (var result in MatchManager.LoadVideo())
                yield return result;
            yield return nextScreen;
        }

        public IEnumerable<IResult> NoVideo()
        {
            //Go to next Scene Without Video
            var nextScreen = ShowScreenResult.Of<MainViewModel>();
            nextScreen.Properties.Add("SelectedTab", MainViewModel.Tabs.Live);
            nextScreen.Properties.Add("LiveMode", LiveViewModel.TimeMode.Timer);
            yield return nextScreen;
        }

        #endregion

    }
}
