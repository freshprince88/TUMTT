using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using TT.Models.Util.Enums;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;
using System.Windows.Controls.Primitives;

namespace TT.Viewer.ViewModels
{
    public class MediaViewModel : Screen,
        IHandle<ResultsChangedEvent>,
        IHandle<VideoPlayEvent>,
        IHandle<MediaControlEvent>,
        IHandle<MatchOpenedEvent>
    {
        private IEventAggregator events;
        private IMatchManager Manager;

        public LinkedList<Rally> Playlist { get; set; }
        public LinkedListNode<Rally> CurrentRally { get; set; }

        private Media.Mute _muted;
        public Media.Mute Muted
        {
            get
            {
                return _muted;
            }
            set
            {
                if (!_mode.Equals(value))

                    _muted = value;

            }
        }

        private Media.Speed _speed;
        public Media.Speed Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                if (!_mode.Equals(value))

                    _speed = value;

            }
        }

        private Media.Control _mode;
        public Media.Control Control
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
            }
        }

        private Media.Fullscreen _fullscreen;
        public Media.Fullscreen Fullscreen
        {
            get
            {
                return _fullscreen;
            }
            set
            {
                _fullscreen = value;
            }
        }

        private Media.Repeat _repeat;
        public Media.Repeat Repeat
        {
            get
            {
                return _repeat;
            }
            set
            {
                _repeat = value;
            }
        }

        private Media.Infinite _infinite;
        public Media.Infinite Infinite
        {
            get
            {
                return _infinite;
            }
            set
            {
                _infinite = value;
            }
        }

        public MediaViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            events = eventAggregator;
            Manager = man;
            _speed = Media.Speed.Full;
            _muted = Media.Mute.Unmute;
            _mode = Media.Control.Stop;
            _repeat = Media.Repeat.Off;
            _infinite = Media.Infinite.Off;
        }

        #region View Methods
        public void PlayPause()
        {
            if (this.Control == Media.Control.Pause || this.Control == Media.Control.Stop)
            {
                Play();
            }
            else
                Pause();
        }
        public void Play()
        {
            if (CurrentRally != null)
            {
                events.PublishOnUIThread(new VideoControlEvent()
                {
                    Start = this.Control == Media.Control.Pause ? -100 : Convert.ToDouble(CurrentRally.Value.Anfang),
                    End = Convert.ToDouble(CurrentRally.Value.Ende),
                    PlayMode = Media.Control.Play,
                    PlaySpeed = this.Speed,
                    Restart = true,
                    Init = this.Control == Media.Control.Pause ? false : true
                });
            }

            this.Control = Media.Control.Play;
        }

        public void Pause()
        {
            this.Control = Media.Control.Pause;

            events.PublishOnUIThread(new VideoControlEvent()
            {
                PlayMode = this.Control,
                PlaySpeed = this.Speed
            });

        }

        public void Stop()
        {
            this.Control = Media.Control.Stop;
            events.PublishOnUIThread(new VideoControlEvent()
            {
                PlayMode = this.Control
            });
        }

        public void Previous5Frames(MediaElement myMediaElement)
        {
            this.Control = Media.Control.Pause;
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            events.PublishOnUIThread(new VideoControlEvent()
            {
                PlayMode = this.Control,
                PlaySpeed = this.Speed,
                Position = Position_now - delta_time
            });

        }

        public void PreviousFrame(MediaElement myMediaElement)
        {
            this.Control = Media.Control.Pause;
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            events.PublishOnUIThread(new VideoControlEvent()
            {
                PlayMode = this.Control,
                PlaySpeed = this.Speed,
                Position = Position_now - delta_time
            });
        }

        public void Next5Frames(MediaElement myMediaElement)
        {
            this.Control = Media.Control.Pause;
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            events.PublishOnUIThread(new VideoControlEvent()
            {
                PlayMode = this.Control,
                PlaySpeed = this.Speed,
                Position = Position_now + delta_time
            });
        }

        public void NextFrame(MediaElement myMediaElement)
        {
            this.Control = Media.Control.Pause;
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            events.PublishOnUIThread(new VideoControlEvent()
            {
                PlayMode = this.Control,
                PlaySpeed = this.Speed,
                Position = Position_now + delta_time
            });
        }

        public void PreviousRally()
        {

            CurrentRally = CurrentRally == Playlist.First ? Playlist.Last : CurrentRally.Previous;

            if (CurrentRally == null && Playlist.Count == 0)
                return;

            events.PublishOnUIThread(new ResultListControlEvent(CurrentRally.Value));

        }

        public void NextRally()
        {
            CurrentRally = CurrentRally == Playlist.Last ? Playlist.First : CurrentRally.Next;

            if (CurrentRally == null && Playlist.Count == 0)
                return;

            events.PublishOnUIThread(new ResultListControlEvent(CurrentRally.Value));
        }

        public void JumpToEndOfRally(MediaElement myMediaElement)
        {
            this.Control = Media.Control.Pause;
            TimeSpan Position_now = TimeSpan.FromMilliseconds(CurrentRally.Value.Anfang);
            TimeSpan jump = TimeSpan.FromMilliseconds(CurrentRally.Value.Ende - CurrentRally.Value.Anfang);
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 2, 0);
            if ((jump - delta_time).TotalMilliseconds > 0) { 
            jump = jump - delta_time;
            }
            this.Control = Media.Control.Play;
            events.PublishOnUIThread(new VideoControlEvent()
            {
                PlayMode = this.Control,
                PlaySpeed = this.Speed,
                Restart=true,
                Position = Position_now + jump
            });

        }

        public void Slow75Percent(bool isChecked)
        {
            if (isChecked)
                this.Speed = Media.Speed.Third;
            else
                this.Speed = Media.Speed.Full;

            events.PublishOnUIThread(new VideoControlEvent()
            {
                PlaySpeed = this.Speed,
                PlayMode = this.Control,
                Restart = this.Control == Media.Control.Play ? true : false
            });
        }

        public void Slow50Percent(bool isChecked)
        {
            if (isChecked)
                this.Speed = Media.Speed.Half;
            else
                this.Speed = Media.Speed.Full;

            events.PublishOnUIThread(new VideoControlEvent()
            {
                PlaySpeed = this.Speed,
                PlayMode = this.Control,
                Restart = this.Control == Media.Control.Play ? true : false
            });
        }

        public void Slow25Percent(bool isChecked)
        {
            if (isChecked)
                this.Speed = Media.Speed.Quarter;
            else
                this.Speed = Media.Speed.Full;

            events.PublishOnUIThread(new VideoControlEvent()
            {
                PlaySpeed = this.Speed,
                PlayMode = this.Control,
                Restart = this.Control == Media.Control.Play ? true : false
            });
        }
        public void MuteUnmute()
        {
            if (this.Muted == Media.Mute.Unmute)
            {
                Mute();
            }
            else if (this.Muted == Media.Mute.Mute)
            {
                Unmute();
            }
        }

        public void Mute()
        {
            
            this.Muted = Media.Mute.Mute;
            events.PublishOnUIThread(this.Muted);
        }

        public void Unmute()
        {
            
            this.Muted = Media.Mute.Unmute;
            events.PublishOnUIThread(this.Muted);
        }

      
        public void toFullscreen()
        {
            if (this.Fullscreen == Media.Fullscreen.Off)
            {
                this.Fullscreen = Media.Fullscreen.On;
                events.PublishOnUIThread(new FullscreenEvent(false));
            }
            else if (this.Fullscreen == Media.Fullscreen.On)
            {
                this.Fullscreen = Media.Fullscreen.Off;
                events.PublishOnUIThread(new FullscreenEvent(true));
            }
            events.PublishOnUIThread(this.Fullscreen);
        }
        public void toRepeat()
        {
            if (this.Repeat == Media.Repeat.Off)
            {
                this.Repeat = Media.Repeat.On;
            }
            else if (this.Repeat == Media.Repeat.On)
            {
                this.Repeat = Media.Repeat.Off;
            }
            events.PublishOnUIThread(this.Repeat);
        }
        public void toInfinite()
        {
            if (this.Infinite == Media.Infinite.Off)
            {
                
                this.Infinite = Media.Infinite.On;
            }
            else if (this.Infinite == Media.Infinite.On)
            {
                
                this.Infinite = Media.Infinite.Off;
            }
            events.PublishOnUIThread(this.Infinite);
        }



#endregion

        #region Caliburn Hooks

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            events.Subscribe(this);
        }

        /// <summary>
        /// Handles deactivation of this view model.
        /// </summary>
        /// <param name="close">Whether the view model is closed</param>
        protected override void OnDeactivate(bool close)
        {
            events.Unsubscribe(this);
            base.OnDeactivate(close);
        }
            #endregion

        #region Event Handlers

        public void Handle(ResultsChangedEvent message)
        {
            this.Playlist = new LinkedList<Rally>(message.Rallies);
            CurrentRally = Playlist.First;
        }

        public void Handle(MediaControlEvent message)
        {
            switch (message.Ctrl)
            {
                case Media.Control.Previous:
                    this.PreviousRally();
                    break;
                case Media.Control.Next:
                    this.NextRally();
                    break;
                case Media.Control.Pause:
                    this.Pause();
                    break;
                default:
                    break;
            }
        }

        public void Handle(VideoPlayEvent message)
        {
            Rally r = message.Current;
            CurrentRally = Playlist.Find(r);
            this.Control = Media.Control.Play;
            Play();
        }

        public void Handle(MatchOpenedEvent message)
        {
            this.Speed = Media.Speed.Full;
            this.Muted = Media.Mute.Unmute;
            this.Control = Media.Control.Stop;
            this.Repeat = Media.Repeat.Off;
            this.Infinite = Media.Infinite.Off;
            events.PublishOnUIThread(this.Infinite);
            events.PublishOnUIThread(this.Repeat);
            events.PublishOnUIThread(this.Muted);
            events.PublishOnUIThread(this.Speed);
            events.PublishOnUIThread(this.Control);
        }

        #endregion

        #region Helper Methods

        #endregion
    }
}
