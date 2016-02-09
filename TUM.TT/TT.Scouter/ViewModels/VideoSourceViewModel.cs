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
            OpenFileDialogResult videoDialog = MatchManager.LoadVideo();
            yield return videoDialog;
            MatchManager.Match.VideoFile = videoDialog.Result;

            var nextScreen = ShowScreenResult.Of<LiveViewModel>();
            yield return nextScreen;
        }

        public IEnumerable<IResult> NoVideo()
        {
            //Go to next Scene Without Video
            var nextScreen = ShowScreenResult.Of<LiveViewModel>();
            yield return nextScreen;
        }

        #endregion

    }
}
