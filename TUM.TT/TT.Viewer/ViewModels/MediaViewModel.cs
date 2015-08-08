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

    { public MediaViewModel()
        {
        }



        public void Play(MediaElement myMediaElement)
        {
            myMediaElement.Play();
            
        }
        public void Pause(MediaElement myMediaElement)
        {
            myMediaElement.Pause();
        }
       
        public void Stop(MediaElement myMediaElement)
        {
            myMediaElement.Stop();
            
        }
        public void Previous5Frames(MediaElement myMediaElement)
        {
            myMediaElement.Pause();
            //mediaIsPaused = true;
            TimeSpan Position_now = myMediaElement.Position;
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            myMediaElement.Position = Position_now - delta_time;
            myMediaElement.ScrubbingEnabled = true;
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
            //buttonPlay.Content = "Play";
        }
       


        public void PreviousRally(MediaElement myMediaElement)
        {
            
        }
        public void NextRally(MediaElement myMediaElement)
        {
            
        }
        public void Slow75Percent(MediaElement myMediaElement)
        {
            if (myMediaElement.SpeedRatio == 0.75)
            {
                myMediaElement.SpeedRatio = 1;
            }
            else
                myMediaElement.SpeedRatio = 0.75; 

        }
        public void Slow50Percent(MediaElement myMediaElement)
        {
            myMediaElement.SpeedRatio = 0.50;
        }
        public void Slow25Percent(MediaElement myMediaElement)
        {
            myMediaElement.SpeedRatio = 0.25;
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
