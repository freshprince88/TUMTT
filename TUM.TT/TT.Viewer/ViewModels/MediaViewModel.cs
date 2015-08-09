using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Lib;

namespace TT.Viewer.ViewModels
{
    public class MediaViewModel : Screen

    {
        private IEventAggregator events;

        public enum PlayPause
        {

            Pause,
            Play
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

        private PlayPause _mode;
        public PlayPause Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                if (!_mode.Equals(value))
                    events.PublishOnUIThread(value);

                _mode = value;

            }
        }

        public MediaViewModel(IEventAggregator eventAggregator)
        {
            events = eventAggregator;
        }



        public void Play(MediaElement myMediaElement)
        {
            myMediaElement.Play();
            this.Mode = PlayPause.Play;          
        }

        public void Pause(MediaElement myMediaElement)
        {
            myMediaElement.Pause();
            this.Mode = PlayPause.Pause;
        }
       
        public void Stop(MediaElement myMediaElement)
        {
            myMediaElement.Stop();
            this.Mode = PlayPause.Pause;

        }
        public void Previous5Frames(MediaElement myMediaElement)
        {
            myMediaElement.Pause();
            //mediaIsPaused = true;
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            myMediaElement.Position = Position_now - delta_time;
            myMediaElement.ScrubbingEnabled = true;
            this.Mode = PlayPause.Pause;
            //buttonPlay.Content = "Play";
        }
        public void PreviousFrame(MediaElement myMediaElement)
        {
            myMediaElement.Pause();
            //mediaIsPaused = true;
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            myMediaElement.Position = Position_now - delta_time;
            myMediaElement.ScrubbingEnabled = true;
            this.Mode = PlayPause.Pause;
            //buttonPlay.Content = "Play";
        }
        public void Next5Frames(MediaElement myMediaElement)
        {
            myMediaElement.Pause();
            //mediaIsPaused = true;
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            myMediaElement.Position = Position_now + delta_time;
            myMediaElement.ScrubbingEnabled = true;
            this.Mode = PlayPause.Pause;
            //buttonPlay.Content = "Play";
        }
        public void NextFrame(MediaElement myMediaElement)
        {
            myMediaElement.Pause();
            //mediaIsPaused = true;
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            myMediaElement.Position = Position_now + delta_time;
            myMediaElement.ScrubbingEnabled = true;
            this.Mode = PlayPause.Pause;
            //buttonPlay.Content = "Play";
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

        
        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        /// <summary>
        /// Handles deactivation of this view model.
        /// </summary>
        /// <param name="close">Whether the view model is closed</param>
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
        }


    }
}
