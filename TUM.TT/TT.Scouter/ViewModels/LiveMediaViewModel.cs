using Caliburn.Micro;
using System;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;
using TT.Models.Util.Enums;
using TT.Scouter.Interfaces;

namespace TT.Scouter.ViewModels
{
    public class LiveMediaViewModel : Screen, IMediaPosition
    {
        private TimeSpan _mediaLength;
        public TimeSpan MediaLength {
            get
            {
                return _mediaLength;
            }
            set
            {
                if (_mediaLength != value)
                    _mediaLength = value;
                NotifyOfPropertyChange();
            }
        }

        private TimeSpan _mediaPos;
        public TimeSpan MediaPosition
        {
            get
            {
                return _mediaPos;
            }
            set
            {
                if (_mediaPos != value)
                    _mediaPos = value;
                NotifyOfPropertyChange();
            }
        }

        private TimeSpan _endPos;
        public TimeSpan EndPosition
        {
            get
            {
                return _endPos;
            }
            set
            {
                if (_endPos != value)
                    _endPos = value;
                NotifyOfPropertyChange();
            }
        }

        private bool _playing;
        public bool IsPlaying
        {
            get
            {
                return _playing;
            }
            set
            {
                if (_playing != value)
                    _playing = value;
                NotifyOfPropertyChange();
            }
        }

        private bool _muted;
        public bool IsMuted
        {
            get
            {
                return _muted;
            }
            set
            {
                if (_muted != value)
                    _muted = value;
                NotifyOfPropertyChange();
            }
        }
        public bool toRallyStart { get; set; }
       

        public Match Match { get { return Manager.Match; } }

        private IEventAggregator Events;
        private IMatchManager Manager;

        public LiveMediaViewModel(IEventAggregator ev, IMatchManager man)
        {
            Events = ev;
            Manager = man;
            IsPlaying = false;
        }

        #region  Caliburn Hooks

        //protected override void OnActivate()
        //{
        //    base.OnActivate();
        //    //MediaPosition = TimeSpan.Zero;
        //}
        

        #endregion

        #region View Methods

        public void Play()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Play,Media.Source.LiveScouter));
            IsPlaying = true;
        }

        public void Pause()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.LiveScouter));
            IsPlaying = false;
        }

        public void Stop()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.LiveScouter));
            IsPlaying = false;
            MediaPosition = TimeSpan.Zero;
        }

        public void Slow(int slow)
        {
            if(slow == 50)
                Events.PublishOnUIThread(new MediaSpeedEvent(Media.Speed.Half));
            else if(slow == 75)
                Events.PublishOnUIThread(new MediaSpeedEvent(Media.Speed.Third));
            else if(slow == 25)
                Events.PublishOnUIThread(new MediaSpeedEvent(Media.Speed.Quarter));
            else if (slow == 150)
                Events.PublishOnUIThread(new MediaSpeedEvent(Media.Speed.Faster));
            else
                Events.PublishOnUIThread(new MediaSpeedEvent(Media.Speed.Full));
        }

        public void Mute()
        {
            IsMuted = true;
            Events.PublishOnUIThread(new MediaMuteEvent(Media.Mute.Mute));            
        }

        public void UnMute()
        {
            IsMuted = false;
            Events.PublishOnUIThread(new MediaMuteEvent(Media.Mute.Unmute));
        }

        #endregion
    }
}
