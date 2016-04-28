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
using TT.Models.Events;
using TT.Viewer.ViewModels;
using TT.Models.Util.Enums;
using System.Windows.Controls.Primitives;
using System.Timers;
using TT.Lib.Managers;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für MediaView.xaml
    /// </summary>
    public partial class MediaView : UserControl,
        IHandle<VideoControlEvent>,
        IHandle<Media.Mute>,
        IHandle<VideoLoadedEvent>,
        IHandle<Media.Fullscreen>,
        IHandle<Media.Repeat>,
        IHandle<Media.Infinite>
    {
        public IEventAggregator Events { get; private set; }
        private IMatchManager Manager;
        private DispatcherTimer stopTimer;
        private DispatcherTimer sliderTimer;
        private double Start;
        private double End;
        private double slider_tick;
        private bool mediaIsPaused;
        private bool isDragging;

        public MediaView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
            Manager = IoC.Get<IMatchManager>();
            myMediaElement.ScrubbingEnabled = true;         
            stopTimer = new DispatcherTimer();
            stopTimer.Tick += new EventHandler(StopTimerTick);
            slider_tick = 100;
            sliderTimer = new DispatcherTimer();
            sliderTimer.Tick += new EventHandler(SliderTimerTick); ;
            sliderTimer.Interval = TimeSpan.FromMilliseconds(slider_tick);
            Start = 0;
            End = 0;
            mediaIsPaused = true;
            isDragging = false;

            //this.Loaded += MediaView_Loaded;

            //myMediaElement.MediaOpened += (s, args) =>
            //{
            //    var test = myMediaElement.Position;
            //};
        }

        private void MediaView_Loaded(object sender, RoutedEventArgs e)
        {
            myMediaElement.Stop();
            myMediaElement.Close();
            myMediaElement.Source = Manager.Match.VideoFile != null ? new Uri(Manager.Match.VideoFile) : myMediaElement.Source;
            myMediaElement.Play();
            myMediaElement.Pause();
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
        public void Handle(Media.Fullscreen message)
        {
            switch (message)
            {
                case Media.Fullscreen.Off:
                    FullscreenButton.IsChecked = true;
                    break;
                case Media.Fullscreen.On:
                    FullscreenButton.IsChecked = false;
                    break;
                default:
                    break;               
            }
        }
        public void Handle(Media.Repeat message)
        {
            switch (message)
            {
                case Media.Repeat.On:
                    cbRepeat.IsChecked = true;
                    break;
                case Media.Repeat.Off:
                    cbRepeat.IsChecked = false;
                    break;
                default:
                    break;
            }
        }
        public void Handle(Media.Infinite message)
        {
            switch (message)
            {
                case Media.Infinite.On:
                    cbInfinite.IsChecked = true;
                    break;
                case Media.Infinite.Off:
                    cbInfinite.IsChecked = false;
                    break;
                default:
                    break;
            }
        }

        public void Handle(VideoLoadedEvent message)
        {
            
            myMediaElement.Stop();
            myMediaElement.Close();
            this.myMediaElement.Source = message.VideoFile != null ? new Uri(message.VideoFile) : myMediaElement.Source;
            myMediaElement.Play();
            myMediaElement.Pause();
        }

        public void Handle(VideoControlEvent message)
        {
            stopTimer.Stop();
            sliderTimer.Stop();
            myMediaElement.Pause();

            Start = message.Start >= 0 ? message.Start : Start;
            End = message.End >= 0 ? message.End : End;

            if (!message.PlaySpeed.Equals(Media.Speed.None))
                HandlePlaySpeed(message.PlaySpeed);

            if (message.Position.CompareTo(TimeSpan.Zero) < 0 && message.Start >= 0)
            {
                myMediaElement.Position = TimeSpan.FromMilliseconds(Start); // Im Video zum Startzeitpunkt springen
            }
            else if (message.Position.CompareTo(TimeSpan.Zero) >= 0)
            {
                if (message.Position.TotalMilliseconds > End)
                {
                    if (cbRepeat.IsChecked.Value)
                    {
                        myMediaElement.Position = TimeSpan.FromMilliseconds(Start + (message.Position.TotalMilliseconds - End));
                        slider_timeline.Value = Start + (message.Position.TotalMilliseconds - End);
                    }
                    else
                    {
                        stopTimer.Stop();
                        myMediaElement.Pause();
                        Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Next, Media.Source.Viewer));
                    }
                }
                else
                {
                    myMediaElement.Position = message.Position;
                    slider_timeline.Value = message.Position.TotalMilliseconds;
                }
            }

            // Neuen Timer erstellen
            double Pos = message.Start < 0 ? myMediaElement.Position.TotalMilliseconds : Start;

            double dauer = (End - Pos) * (1 / (myMediaElement.SpeedRatio)); // Spieldauer des Video ermitteln
            stopTimer.Interval = dauer > 0 ? TimeSpan.FromMilliseconds(dauer) : stopTimer.Interval;

            if (message.Init)
            {
                slider_timeline.Minimum = Start;
                slider_timeline.Maximum = End - 100;
                slider_timeline.SmallChange = 40;
                slider_timeline.LargeChange = 200;
                slider_timeline.Value = slider_timeline.Minimum;

                stopTimer.Start(); //Timer starten
                sliderTimer.Start();
            }
            else if (message.Restart)
            {
                stopTimer.Start(); //Timer starten
                sliderTimer.Start();
            }

            if (!message.PlayMode.Equals(Media.Control.None))
                HandlePlayMode(message.PlayMode);
        }

        private void StopTimerTick(object sender, EventArgs e)
        {
            if (cbRepeat.IsChecked.Value)
            {
                TimeSpan pos = TimeSpan.FromMilliseconds(Start);
                Events.PublishOnUIThread(new VideoControlEvent()
                {
                    Position = pos,
                    PlayMode = mediaIsPaused ? Media.Control.Pause : Media.Control.Play,
                    Restart = mediaIsPaused ? false : true
                });
            }
            else if (cbInfinite.IsChecked.Value)
            {
                stopTimer.Stop();
                myMediaElement.Pause();
                Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Next, Media.Source.Viewer));
            }
            else
            {
                myMediaElement.Position = TimeSpan.FromMilliseconds(Start);
                slider_timeline.Value = slider_timeline.Minimum;
                Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.Viewer));
            }
        }

        private void SliderTimerTick(object sender, EventArgs e)
        {
            double distance = slider_timeline.Maximum - slider_timeline.Minimum;
            double ticks = distance / slider_tick;
            double add = distance / (0.9 * ticks);

            Dispatcher.Invoke(() => slider_timeline.Value += add);

        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isDragging)
                myMediaElement.Position = TimeSpan.FromMilliseconds(e.NewValue);
        }

        private void Slider_DragStarted(object sender, DragStartedEventArgs e)
        {
            if (myMediaElement.Source == null)
                return;

            sliderTimer.Stop();
            myMediaElement.Pause();
            isDragging = true;
        }

        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (myMediaElement.Source == null)
                return;

            double newValue = ((Slider)sender).Value;

            TimeSpan pos = TimeSpan.FromMilliseconds(newValue);
            Events.PublishOnUIThread(new VideoControlEvent()
            {
                Position = pos,
                PlayMode = mediaIsPaused ? Media.Control.Pause : Media.Control.Play,
                Restart = mediaIsPaused ? false : true

            });
            isDragging = false;
        }

        #endregion

        #region Helper Methods
        private void HandlePlayMode(Media.Control mode)
        {
            switch (mode)
            {
                case Media.Control.Play:
                    myMediaElement.Play();
                    PlayButton.Visibility = Visibility.Hidden;
                    PauseButton.Visibility = Visibility.Visible;
                    PauseButton.IsChecked = true;
                    mediaIsPaused = false;
                    sliderTimer.Start();
                    break;
                case Media.Control.Pause:
                    myMediaElement.Pause();
                    PlayButton.Visibility = Visibility.Visible;
                    PauseButton.Visibility = Visibility.Hidden;
                    PauseButton.IsChecked = false;
                    mediaIsPaused = true;
                    sliderTimer.Stop();
                    break;
                case Media.Control.Stop:
                    myMediaElement.Stop();
                    //slider_timeline.Minimum = 0;
                    //slider_timeline.Maximum = myMediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
                    //slider_timeline.Value = 0;
                    PlayButton.Visibility = Visibility.Visible;
                    PauseButton.Visibility = Visibility.Hidden;
                    PauseButton.IsChecked = false;
                    mediaIsPaused = true;
                    sliderTimer.Stop();
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
                    sliderTimer.Interval = TimeSpan.FromMilliseconds(4 * slider_tick);
                    Slow50PercentButton.IsChecked = false;
                    Slow75PercentButton.IsChecked = false;
                    break;
                case Media.Speed.Half:
                    myMediaElement.SpeedRatio = 0.50;
                    sliderTimer.Interval = TimeSpan.FromMilliseconds(2 * slider_tick);
                    Slow25PercentButton.IsChecked = false;
                    Slow75PercentButton.IsChecked = false;
                    break;
                case Media.Speed.Third:
                    myMediaElement.SpeedRatio = 0.75;
                    sliderTimer.Interval = TimeSpan.FromMilliseconds(1.33 * slider_tick);
                    Slow25PercentButton.IsChecked = false;
                    Slow50PercentButton.IsChecked = false;
                    break;
                case Media.Speed.Full:
                    myMediaElement.SpeedRatio = 1;
                    sliderTimer.Interval = TimeSpan.FromMilliseconds(slider_tick);
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
