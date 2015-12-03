using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Lib;
using TT.Viewer.Events;

namespace TT.Viewer.ViewModels
{
    public class MediaViewModel : Screen
    {
        private IEventAggregator events;

        public enum MuteUnmute
        {

            Mute,
            Unmute
        }
        private MuteUnmute _sound;
        public MuteUnmute Sound
        {
            get
            {
                return _sound;
            }
            set
            {
                if (!_mode.Equals(value))
                    events.PublishOnUIThread(value);

                _sound = value;

            }
        }

        public enum PlaySpeed
        {
            Quarter,
            Half,
            Third,
            Full
        }

        private PlaySpeed _speed;
        public PlaySpeed Speed
        {
            get
            {
                return _speed;
            }
            set
            {
                if (!_mode.Equals(value))
                    events.PublishOnUIThread(value);

                _speed = value;

            }
        }
        public enum PlayPause
        {
            Stop,
            Pause,
            Play
        }
        private PlayPause _mode;
        public PlayPause Mode
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
        }

        #region View Methods

        public void Play()
        {
            this.Mode = PlayPause.Play;
            events.PublishOnUIThread(this.Mode);
        }

        public void Pause()
        {
            this.Mode = PlayPause.Pause;
            events.PublishOnUIThread(this.Mode);
        }

        public void Stop()
        {
            this.Mode = PlayPause.Stop;
            events.PublishOnUIThread(this.Mode);
        }

        public void Previous5Frames(MediaElement myMediaElement)
        {
            this.Mode = PlayPause.Pause;
            events.PublishOnUIThread(this.Mode);
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            myMediaElement.Position = Position_now - delta_time;
            //myMediaElement.ScrubbingEnabled = true;

        }

        public void PreviousFrame(MediaElement myMediaElement)
        {
            this.Mode = PlayPause.Pause;
            events.PublishOnUIThread(this.Mode);
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            myMediaElement.Position = Position_now - delta_time;
            //myMediaElement.ScrubbingEnabled = true;
        }

        public void Next5Frames(MediaElement myMediaElement)
        {
            this.Mode = PlayPause.Pause;
            events.PublishOnUIThread(this.Mode);
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            myMediaElement.Position = Position_now + delta_time;
            //myMediaElement.ScrubbingEnabled = true;
        }

        public void NextFrame(MediaElement myMediaElement)
        {
            this.Mode = PlayPause.Pause;
            events.PublishOnUIThread(this.Mode);
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            myMediaElement.Position = Position_now + delta_time;
        }

        public void PreviousRally(MediaElement myMediaElement)
        {

        }

        public void NextRally(MediaElement myMediaElement)
        {

        }

        public void Slow75Percent(bool isChecked)
        {
            if (isChecked)
                this.Speed = PlaySpeed.Third;
            else
                this.Speed = PlaySpeed.Full;
        }

        public void Slow50Percent(bool isChecked)
        {
            if (isChecked)
                this.Speed = PlaySpeed.Half;
            else
                this.Speed = PlaySpeed.Full;
        }

        public void Slow25Percent(bool isChecked)
        {
            if (isChecked)
                this.Speed = PlaySpeed.Quarter;
            else
                this.Speed = PlaySpeed.Full;
        }

        public void Mute()
        {
            this.Sound = MuteUnmute.Mute;
        }

        public void Unmute()
        {
            this.Sound = MuteUnmute.Unmute;
        }

        public void Fullscreen(MediaElement myMediaElement)
        {
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

        #endregion
    }
}
