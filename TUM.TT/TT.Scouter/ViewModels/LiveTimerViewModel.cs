using Caliburn.Micro;
using System;
using System.Diagnostics;
using System.Timers;
using TT.Scouter.Interfaces;

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

        private void ReportTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(watch.IsRunning)
                NotifyOfPropertyChange("MediaPosition");
        }

        #endregion
    }
}
