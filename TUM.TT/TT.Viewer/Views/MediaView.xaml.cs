using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TT.Viewer.Events;
using TT.Viewer.ViewModels;
using TT.Lib.Util.Enums;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für MediaView.xaml
    /// </summary>
    public partial class MediaView : UserControl,
        IHandle<VideoControlEvent>,
        IHandle<VideoLoadedEvent>
    {
        public IEventAggregator Events { get; private set; }
        public Media.Mode PlayMode { get; set; }
        private DispatcherTimer stopTimer;
        private double Start;
        private double End;
        private bool mediaIsPaused;

        public MediaView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
            myMediaElement.ScrubbingEnabled = true;
            stopTimer = new DispatcherTimer();
            stopTimer.Tick += new EventHandler(StopTimerTick);
            Start = 0;
            End = 0;
            mediaIsPaused = true;
        }

        #region Event Handlers

        public void Handle(Media.Mute message)
        {
            switch (message)
            {
                case Media.Mute.Mute:
                    myMediaElement.IsMuted = true;
                    MuteButton.Visibility = Visibility.Hidden;
                    UnmuteButton.Visibility = Visibility.Visible;
                    UnmuteButton.IsChecked = true;
                    break;
                case Media.Mute.Unmute:
                    myMediaElement.IsMuted = false;
                    MuteButton.Visibility = Visibility.Visible;
                    UnmuteButton.Visibility = Visibility.Hidden;
                    UnmuteButton.IsChecked = false;
                    break;
                default:
                    break;
            }
        }

        public void Handle(VideoLoadedEvent message)
        {
            this.myMediaElement.Source = new Uri(message.VideoFile);
        }

        public void Handle(VideoControlEvent message)
        {
            stopTimer.Stop();

            if (!message.PlaySpeed.Equals(Media.Speed.None))
                HandlePlaySpeed(message.PlaySpeed);

            Start = message.Position.CompareTo(TimeSpan.Zero) >= 0 ? message.Position.TotalMilliseconds : Start;
            End = message.Duration > 0 ? Start + message.Duration : End;
            
            if (message.Position.CompareTo(TimeSpan.Zero) >= 0)
            {
                myMediaElement.Position = message.Position; // Im Video zum Startzeitpunkt springen
            }

            stopTimer.Interval = message.Duration > 0 ? TimeSpan.FromMilliseconds(message.Duration) : stopTimer.Interval;

            if (message.Init)
            {
                slider_timeline.Minimum = Start;
                slider_timeline.Maximum = End;
                slider_timeline.SmallChange = 40;
                slider_timeline.LargeChange = 200;

                stopTimer.Start(); //Timer starten
            }
            else if (message.Restart)
            {
                stopTimer.Start(); //Timer starten
            }

            if(!message.PlayMode.Equals(Media.Mode.None))
                HandlePlayMode(message.PlayMode);
        }

        private void StopTimerTick(object sender, EventArgs e)
        {

            if (cbRepeat.IsChecked.Value)
            {
                myMediaElement.Position = new TimeSpan(0, 0, 0, 0, (int)Start);
                slider_timeline.Value = Start;
                stopTimer.Start();
                myMediaElement.Play();
                this.PlayMode = Media.Mode.Play;
            }
            else if (cbInfinite.IsChecked.Value)
            {
                stopTimer.Stop();
                Events.PublishOnUIThread(new ResultListControlEvent(Media.Control.Previous));
            }
            else
            {
                stopTimer.Stop();
                if (mediaIsPaused)
                {
                    stopTimer.Interval = TimeSpan.FromMilliseconds(0xffffff);
                    slider_timeline.Value = 0;
                    TimeSpan ts = new TimeSpan(0, 0, 0, 0, (int)Start);
                    myMediaElement.Position = ts;

                }
                else
                {
                    double dauer = End;
                    TimeSpan dauer_ts = TimeSpan.FromMilliseconds(dauer + 1500);
                    stopTimer.Interval = dauer_ts - myMediaElement.Position;
                    stopTimer.Start();
                }
            }
        }

        #endregion

        #region Helper Methods

        private void HandlePlayMode(Media.Mode mode)
        {
            switch (mode)
            {
                case Media.Mode.Play:
                    myMediaElement.Play();
                    PlayButton.Visibility = Visibility.Hidden;
                    PauseButton.Visibility = Visibility.Visible;
                    PauseButton.IsChecked = true;
                    mediaIsPaused = false;
                    break;
                case Media.Mode.Pause:
                    myMediaElement.Pause();
                    PlayButton.Visibility = Visibility.Visible;
                    PauseButton.Visibility = Visibility.Hidden;
                    PauseButton.IsChecked = false;
                    mediaIsPaused = true;
                    break;
                case Media.Mode.Stop:
                    myMediaElement.Stop();
                    slider_timeline.Minimum = 0;
                    slider_timeline.Maximum = myMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
                    slider_timeline.Value = 0;
                    PlayButton.Visibility = Visibility.Visible;
                    PauseButton.Visibility = Visibility.Hidden;
                    PauseButton.IsChecked = false;
                    mediaIsPaused = true;
                    break;
                default:
                    break;
            }
        }

        private void HandlePlaySpeed(Media.Speed speed)
        {
            switch (speed)
            {
                case Media.Speed.Quarter:
                    myMediaElement.SpeedRatio = 0.25;
                    Slow50PercentButton.IsChecked = false;
                    Slow75PercentButton.IsChecked = false;
                    break;
                case Media.Speed.Half:
                    myMediaElement.SpeedRatio = 0.50;
                    Slow25PercentButton.IsChecked = false;
                    Slow75PercentButton.IsChecked = false;
                    break;
                case Media.Speed.Third:
                    myMediaElement.SpeedRatio = 0.75;
                    Slow25PercentButton.IsChecked = false;
                    Slow50PercentButton.IsChecked = false;
                    break;
                case Media.Speed.Full:
                    myMediaElement.SpeedRatio = 1;
                    Slow25PercentButton.IsChecked = false;
                    Slow50PercentButton.IsChecked = false;
                    Slow75PercentButton.IsChecked = false;
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
