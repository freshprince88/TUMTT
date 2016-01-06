using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using TT.Lib;
using TT.Lib.Util.Enums;
using TT.Viewer.Events;

namespace TT.Viewer.ViewModels
{
    public class MediaViewModel : Screen,
        IHandle<ResultsChangedEvent>,
        IHandle<VideoPlayEvent>
    {
        private IEventAggregator events;
        private bool isDragging;

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

        private Media.Mode _mode;
        public Media.Mode Mode
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

        public MediaViewModel(IEventAggregator eventAggregator)
        {
            events = eventAggregator;
            isDragging = false;
            _speed = Media.Speed.Full;
            _muted = Media.Mute.Unmute;
            _mode = Media.Mode.Stop;
        }

        #region View Methods

        public void Play(MediaElement myMediaElement)
        {
            this.Mode = Media.Mode.Play;
            int RallyStart = Convert.ToInt32(myMediaElement.Position.TotalMilliseconds); ;
            int RallyEnd = Convert.ToInt32(CurrentRally.Value.Ende);

            // Neuen Timer erstellen 
            double dauer = (RallyEnd - RallyStart) * (1 / ((double)this.Speed / 100)); // Spieldauer des Video ermitteln
            if (dauer > 0)
            {
                events.PublishOnUIThread(new VideoControlEvent()
                {
                    Position = new TimeSpan(0, 0, 0, 0, RallyStart),
                    PlayMode = this.Mode,
                    PlaySpeed = this.Speed,
                    Duration = dauer + 1500,
                    Restart = true
                });
            }
        }

        public void Pause(MediaElement myMediaElement)
        {
            this.Mode = Media.Mode.Pause;
            int RallyStart = Convert.ToInt32(myMediaElement.Position.TotalMilliseconds);
            int RallyEnd = Convert.ToInt32(CurrentRally.Value.Ende);

            // Neuen Timer erstellen 
            double dauer = (RallyEnd - RallyStart) * (1 / ((double)this.Speed / 100)); // Spieldauer des Video ermitteln
            if (dauer > 0)
            {
                events.PublishOnUIThread(new VideoControlEvent()
                {
                    Position = new TimeSpan(0, 0, 0, 0, RallyStart),
                    PlayMode = this.Mode,
                    PlaySpeed = this.Speed,
                    Duration = dauer + 1500
                });
            }
        }

        public void Stop()
        {
            this.Mode = Media.Mode.Stop;
            events.PublishOnUIThread(new VideoControlEvent()
            {
                PlayMode = this.Mode
            });
        }

        public void Previous5Frames(MediaElement myMediaElement)
        {
            this.Mode = Media.Mode.Pause;
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            myMediaElement.Position = Position_now - delta_time;            
            events.PublishOnUIThread(this.Mode);

        }

        public void PreviousFrame(MediaElement myMediaElement)
        {
            this.Mode = Media.Mode.Pause;
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            myMediaElement.Position = Position_now - delta_time;
            events.PublishOnUIThread(this.Mode);
        }

        public void Next5Frames(MediaElement myMediaElement)
        {
            this.Mode = Media.Mode.Pause;
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            myMediaElement.Position = Position_now + delta_time;
            events.PublishOnUIThread(this.Mode);
        }

        public void NextFrame(MediaElement myMediaElement)
        {
            this.Mode = Media.Mode.Pause;
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            myMediaElement.Position = Position_now + delta_time;
            events.PublishOnUIThread(this.Mode);
        }

        public void PreviousRally(MediaElement myMediaElement)
        {
            events.PublishOnUIThread(new ResultListControlEvent(Media.Control.Previous));
        }

        public void NextRally(MediaElement myMediaElement)
        {
            events.PublishOnUIThread(new ResultListControlEvent(Media.Control.Next));
        }

        public void Slow75Percent(bool isChecked)
        {
            if (isChecked)
                this.Speed = Media.Speed.Third;         
            else
                this.Speed = Media.Speed.Full;

            events.PublishOnUIThread(new VideoControlEvent()
            {
                PlaySpeed = this.Speed
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
                PlaySpeed = this.Speed
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
                PlaySpeed = this.Speed
            });
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

        public void Fullscreen(MediaElement myMediaElement)
        {
        }

        //public void SeekToMediaPosition(int sliderValue)
        //{
        //    if (!isDragging)
        //    {
        //        int RallyStart = sliderValue;
        //        int RallyEnd = Convert.ToInt32(CurrentRally.Value.Ende);

        //        // Neuen Timer erstellen 
        //        double dauer = (RallyEnd - RallyStart) * (1 / ((double)this.Speed / 100)); // Spieldauer des Video ermitteln
        //        if (dauer > 0)
        //        {
        //            events.PublishOnUIThread(new VideoControlEvent()
        //            {
        //                Position = new TimeSpan(0, 0, 0, 0, RallyStart),
        //                PlayMode = Media.Mode.None,
        //                PlaySpeed = Media.Speed.None,
        //                Duration = dauer + 1500,
        //                Restart = this.Mode == Media.Mode.Play ? true : false
        //            });
        //        }
        //    }
        //}

        private void SliderDragStarted()
        {
            isDragging = true;
        }

        private void SliderDragCompleted(int sliderValue)
        {
            if (isDragging)
            {
                int RallyStart = sliderValue;
                int RallyEnd = Convert.ToInt32(CurrentRally.Value.Ende);

                // Neuen Timer erstellen 
                double dauer = (RallyEnd - RallyStart) * (1 / ((double)this.Speed / 100)); // Spieldauer des Video ermitteln
                if (dauer > 0)
                {
                    events.PublishOnUIThread(new VideoControlEvent()
                    {
                        Position = new TimeSpan(0, 0, 0, 0, RallyStart),
                        PlayMode = Media.Mode.None,
                        PlaySpeed = Media.Speed.None,
                        Duration = dauer + 1500,
                        Restart = this.Mode == Media.Mode.Play ? true : false
                    });
                }
            }
            isDragging = false;
        }

        #endregion

        #region Caliburn Hooks

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
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

        public void Handle(VideoPlayEvent message)
        {
            Rally r = message.Current;

            CurrentRally = Playlist.Find(r);

            int RallyStart = Convert.ToInt32(CurrentRally.Value.Anfang);
            int RallyEnd = Convert.ToInt32(CurrentRally.Value.Ende);

            // Neuen Timer erstellen 
            double dauer = (RallyEnd - RallyStart) * (1 / ((double)this.Speed/100)); // Spieldauer des Video ermitteln
            if (dauer > 0)
            {
                events.PublishOnUIThread(new VideoControlEvent()
                {
                    Position = new TimeSpan(0, 0, 0, 0, RallyStart),
                    PlayMode = Media.Mode.Play,
                    PlaySpeed = this.Speed,
                    Duration = dauer + 1500,
                    Init = true
                });
            }
        } 

        #endregion

        #region Helper Methods

        private void InitVideo()
        {

        }

        #endregion
    }
}
