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

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für MediaView.xaml
    /// </summary>
    public partial class MediaView : UserControl, 
        IHandle<MediaViewModel.PlayPause>, 
        IHandle<MediaViewModel.PlaySpeed>, 
        IHandle<MediaViewModel.MuteUnmute>,
        IHandle<VideoLoadedEvent>,
        IHandle<VideoPlayEvent>
    {
        public IEventAggregator Events { get; private set; }
        public MediaViewModel.PlayPause PlayMode { get; set; }
        private DispatcherTimer stopTimer;

        private int RallyStart;
        private int RallyEnd;

        public MediaView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
            myMediaElement.ScrubbingEnabled = true;
            PlayMode = MediaViewModel.PlayPause.Pause;
            stopTimer = new DispatcherTimer();
            RallyStart = 0;
            RallyEnd = 0;
            stopTimer.Tick += new EventHandler(StopTimerTick);
        }

        #region Event Handlers

        public void Handle(MediaViewModel.MuteUnmute message)
        {
            switch (message)
            {
                case MediaViewModel.MuteUnmute.Mute:
                    myMediaElement.IsMuted = true;
                    MuteButton.Visibility = Visibility.Hidden;
                    UnmuteButton.Visibility = Visibility.Visible;
                    UnmuteButton.IsChecked = true;
                    break;
                case MediaViewModel.MuteUnmute.Unmute:
                    myMediaElement.IsMuted = false;
                    MuteButton.Visibility = Visibility.Visible;
                    UnmuteButton.Visibility = Visibility.Hidden;
                    UnmuteButton.IsChecked = false;
                    break;
                default:
                    break;
            }
        }

        public void Handle(MediaViewModel.PlayPause message)
        {
            switch (message)
            {
                case MediaViewModel.PlayPause.Play:
                    myMediaElement.Play();
                    PlayButton.Visibility = Visibility.Hidden;
                    PauseButton.Visibility = Visibility.Visible;
                    PauseButton.IsChecked = true;
                    this.PlayMode = MediaViewModel.PlayPause.Play;
                    break;
                case MediaViewModel.PlayPause.Pause:
                    myMediaElement.Pause();
                    PlayButton.Visibility = Visibility.Visible;
                    PauseButton.Visibility = Visibility.Hidden;
                    PauseButton.IsChecked = false;
                    this.PlayMode = MediaViewModel.PlayPause.Pause;
                    break;
                case MediaViewModel.PlayPause.Stop:
                    myMediaElement.Stop();
                    PlayButton.Visibility = Visibility.Visible;
                    PauseButton.Visibility = Visibility.Hidden;
                    PauseButton.IsChecked = false;
                    this.PlayMode = MediaViewModel.PlayPause.Stop;
                    break;
                default:
                    break;
            }
        }

        public void Handle(MediaViewModel.PlaySpeed message)
        {
            switch (message)
            {
                case MediaViewModel.PlaySpeed.Quarter:
                    myMediaElement.SpeedRatio = 0.25;
                    Slow50PercentButton.IsChecked = false;
                    Slow75PercentButton.IsChecked = false;
                    break;
                case MediaViewModel.PlaySpeed.Half:
                    myMediaElement.SpeedRatio = 0.50;
                    Slow25PercentButton.IsChecked = false;
                    Slow75PercentButton.IsChecked = false;
                    break;
                case MediaViewModel.PlaySpeed.Third:
                    myMediaElement.SpeedRatio = 0.75;
                    Slow25PercentButton.IsChecked = false;
                    Slow50PercentButton.IsChecked = false;
                    break;
                case MediaViewModel.PlaySpeed.Full:
                    myMediaElement.SpeedRatio = 1;
                    Slow25PercentButton.IsChecked = false;
                    Slow50PercentButton.IsChecked = false;
                    Slow75PercentButton.IsChecked = false;
                    break;
                default:
                    break;
            }
        }

        public void Handle(VideoLoadedEvent message)
        {
            this.myMediaElement.Source = new Uri(message.VideoFile);
        }

        public void Handle(VideoPlayEvent message)
        {
            RallyStart = message.Start;
            RallyEnd = message.End;

             // Neuen Timer erstellen 
            double dauer = (RallyEnd - RallyStart) * (1 / myMediaElement.SpeedRatio); // Spieldauer des Video ermitteln
            if (dauer > 0)
            {
                stopTimer.Interval = TimeSpan.FromMilliseconds(dauer + 1500);
                stopTimer.Start(); //Timer starten
                myMediaElement.Position = new TimeSpan(0, 0, 0, 0, message.Start);
                Events.PublishOnUIThread(MediaViewModel.PlayPause.Play);
            }
            else
            {
                Events.PublishOnUIThread(MediaViewModel.PlayPause.Pause);
            }
        }

        #endregion

        private void StopTimerTick(object sender, EventArgs e)
        {
            MediaViewModel.PlayPause temp = this.PlayMode;
            Events.PublishOnUIThread(MediaViewModel.PlayPause.Pause);

            //if ((bool)buttonLoop.IsChecked)
            //{
            //    myMediaElement.Position = new TimeSpan(0, 0, 0, 0, loopingStart);
            //    //slider_timeline.Value = ((myMediaElement.Position.TotalMilliseconds - loopingStart) / 4);
            //    stopTimer.Start();
            //    myMediaElement.Play();
            //    this.PlayMode = MediaViewModel.PlayPause.Play;
            //}
            //else
            //{
                stopTimer.Stop();
                if (temp == MediaViewModel.PlayPause.Play)
                {
                    stopTimer.Interval = TimeSpan.FromMilliseconds(0xffffff);
                    //slider_timeline.Value = 0;
                    TimeSpan ts = new TimeSpan(0, 0, 0, 0, RallyStart);
                    myMediaElement.Position = ts;
                }
                else
                {
                    double dauer = RallyEnd;
                    TimeSpan dauer_ts = TimeSpan.FromMilliseconds(dauer + 1500);
                    stopTimer.Interval = dauer_ts - myMediaElement.Position;
                    stopTimer.Start();
                }


            //}
        }
        
    }
}
