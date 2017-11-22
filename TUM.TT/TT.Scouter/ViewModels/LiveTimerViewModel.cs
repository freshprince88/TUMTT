using Caliburn.Micro;
using System;
using System.Diagnostics;
using System.Timers;
using TT.Lib.Interfaces;

namespace TT.Scouter.ViewModels
{
    public class LiveTimerViewModel : Screen, IMediaPosition
    {
        Stopwatch watch;
        Timer reportTimer;

        public TimeSpan MediaPosition
        {
            get
            {
                return watch.Elapsed;
            }
            set
            {
                throw new NotImplementedException();
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

        private double _min;
        public double Minimum
        {
            get
            {
                return _min;
            }
            set
            {
                if (_min != value)
                    _min = value;
                NotifyOfPropertyChange();
            }
        }

        private double _max;
        public double Maximum
        {
            get
            {
                return _max;
            }
            set
            {
                if (_max != value)
                    _max = value;
                NotifyOfPropertyChange();
            }
        }

        public bool toRallyStart { get; set; }

        public LiveTimerViewModel()
        {
            watch = new Stopwatch();
            reportTimer = new Timer(200);
            reportTimer.AutoReset = true;
            reportTimer.Elapsed += ReportTimer_Elapsed;
            reportTimer.Start();       
        }

        #region Caliburn Hooks

        #endregion


        #region View Methods

        #endregion

        #region Helper Methods

        public void Play()
        {
            if (!watch.IsRunning)
            {
                watch.Start();
            }
        }

        public void Pause()
        {
            watch.Stop();
        }

        public void Stop()
        {
            Pause();
            watch.Reset();
        }
        public void SkipForward()
        {

        }
        public void SkipBackwards()
        {

        }
        public void PlayPause() { }
        private void ReportTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(watch.IsRunning)
                NotifyOfPropertyChange("MediaPosition");
        }
        
        #endregion
    }
}
