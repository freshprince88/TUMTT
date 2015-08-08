using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TT.Viewer.ViewModels
{
    public class MediaViewModel : Screen

    { MediaElement myMediaElement = new MediaElement();

        public void Play(MediaElement myMediaElement)
        {
            myMediaElement.Play();
        }
        public void Pause(MediaElement mediaElement)
        {
            mediaElement.Pause();
        }
        public void Stop(MediaElement myMediaElement)
        {
            myMediaElement.Stop();
        }
        public void Previous5Frames(MediaElement mediaElement)
        {
            mediaElement.Pause();
            //mediaIsPaused = true;
            TimeSpan Position_now = mediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            mediaElement.Position = Position_now - delta_time;
            mediaElement.ScrubbingEnabled = true;
            //buttonPlay.Content = "Play";
        }
        public void PreviousFrame(MediaElement mediaElement)
        {
            mediaElement.Pause();
            //mediaIsPaused = true;
            TimeSpan Position_now = mediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            mediaElement.Position = Position_now - delta_time;
            mediaElement.ScrubbingEnabled = true;
            //buttonPlay.Content = "Play";
        }
        public void Next5Frames(MediaElement mediaElement)
        {
            mediaElement.Pause();
            //mediaIsPaused = true;
            TimeSpan Position_now = mediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            mediaElement.Position = Position_now + delta_time;
            mediaElement.ScrubbingEnabled = true;
            //buttonPlay.Content = "Play";
        }
        public void NextFrame(MediaElement mediaElement)
        {
            mediaElement.Pause();
            //mediaIsPaused = true;
            TimeSpan Position_now = mediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            mediaElement.Position = Position_now + delta_time;
            mediaElement.ScrubbingEnabled = true;
            //buttonPlay.Content = "Play";
        }
        public void PreviousRally(MediaElement mediaElement)
        {
            
        }
        public void NextRally(MediaElement mediaElement)
        {
            
        }





        //private readonly IEventAggregator _eventAggregator;

        //public enum ViewMode
        //{

        //    Paused,
        //    Played
        //}

        //private ViewMode _mode;
        //public ViewMode Mode
        //{
        //    get
        //    {
        //        return _mode;
        //    }
        //    set
        //    {
        //        if (!_mode.Equals(value))
        //            _eventAggregator.BeginPublishOnUIThread(value);

        //        _mode = value;

        //    }
        //}

        //public MediaViewModel(IEventAggregator eventAggregator)
        //{
        //    _eventAggregator = eventAggregator;
        //}


    }
}
