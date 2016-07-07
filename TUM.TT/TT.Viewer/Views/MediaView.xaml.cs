using Caliburn.Micro;
using System;
using System.Windows.Controls;
using TT.Lib.Events;
using TT.Models.Util.Enums;
using TT.Lib.Managers;
using System.Windows.Forms;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System.Windows.Media;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für MediaView.xaml
    /// </summary>
    public partial class MediaView : System.Windows.Controls.UserControl,
        IHandle<MediaControlEvent>,
        IHandle<MediaSpeedEvent>,
        IHandle<MediaMuteEvent>,
        IHandle<VideoLoadedEvent>,
        IHandle<FullscreenEvent>
    {

        private IEventAggregator Events;
        private IMatchManager Manager;
        TimeSpan currentTime;

        public TimeSpan TimeoutToHide { get; private set; }
        public DateTime LastMouseMove { get; private set; }
        public bool IsHidden { get; private set; }
        Timer timer = new Timer();
        System.Drawing.Point mousePosition { get; set; }




        public MediaView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            //Events.Subscribe(this);
            Manager = IoC.Get<IMatchManager>();
            this.Loaded += RemoteMediaView_Loaded;
            this.Unloaded += ExtendedMediaView_Unloaded;
            currentTime = TimeSpan.Zero;
            TimeoutToHide = TimeSpan.FromSeconds(2);
            this.MouseMove += MediaView_MouseMove;
            timer.Tick += new EventHandler(timer1_Tick);
            timer.Interval = (1000) * (3);
            IsHidden = false;
            mousePosition = System.Windows.Forms.Control.MousePosition;


        }




        #region Media Methods


        #endregion

        #region Event Handlers
        private void MediaView_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (FullscreenButton.IsChecked == true)
            {
                if (mousePosition != System.Windows.Forms.Control.MousePosition)
                {
                    Mouse.OverrideCursor = null;

                    LastMouseMove = DateTime.Now;

                    if (IsHidden)
                    {
                        SliderRow.Height = new GridLength(1, GridUnitType.Auto);
                        PlayerRow1.Height = new GridLength(25);
                        PlayerRow2.Height = new GridLength(25);
                        IsHidden = false;
                        Events.PublishOnUIThread(new FullscreenHideAllEvent(false));
                    }

                    timer.Enabled = true;
                    timer.Stop();
                    timer.Start();
                    mousePosition = System.Windows.Forms.Control.MousePosition;
                }
            }
        }

        private void ExtendedMediaView_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Events.Unsubscribe(this);
            currentTime = MediaPlayer.Position;
        }

        public void Handle(MediaControlEvent message)
        {
            if (message.Source == Media.Source.Viewer)
            {
                switch (message.Ctrl)
                {
                    case Media.Control.Stop:
                        MediaPlayer.StopWithState();
                        break;
                    case Media.Control.Pause:
                        MediaPlayer.PauseWithState();
                        break;
                    case Media.Control.Play:
                        MediaPlayer.PlayWithState();
                        break;
                    default:
                        break;
                }
            }
        }

        private void RemoteMediaView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Events.Subscribe(this);

            if (Manager.Match.VideoFile != null && Manager.Match.VideoFile != string.Empty)
            {
                MediaPlayer.StopWithState();
                MediaPlayer.Close();
                MediaPlayer.Source = new Uri(Manager.Match.VideoFile);
                MediaPlayer.MediaPosition = currentTime;
                MediaPlayer.PlayWithState();
                MediaPlayer.PauseWithState();
                PlayButton.Visibility = System.Windows.Visibility.Visible;
            }
        }

        public void Handle(MediaSpeedEvent message)
        {
            switch (message.Speed)
            {
                case Media.Speed.Quarter:
                    MediaPlayer.SpeedRatio = 0.25;
                    break;
                case Media.Speed.Half:
                    MediaPlayer.SpeedRatio = 0.5;
                    break;
                case Media.Speed.Third:
                    MediaPlayer.SpeedRatio = 0.75;
                    break;
                case Media.Speed.Full:
                    MediaPlayer.SpeedRatio = 1;
                    break;
                default:
                    break;
            }

        }

        public void Handle(MediaMuteEvent message)
        {
            switch (message.Mute)
            {
                case Media.Mute.Mute:
                    MediaPlayer.IsMuted = true;
                    break;
                case Media.Mute.Unmute:
                    MediaPlayer.IsMuted = false;
                    break;
                default:
                    break;
            }
        }

        public void Handle(VideoLoadedEvent message)
        {
            MediaPlayer.StopWithState();
            MediaPlayer.Close();
            MediaPlayer.Source = Manager.Match.VideoFile != null ? new Uri(Manager.Match.VideoFile) : MediaPlayer.Source;
            MediaPlayer.PlayWithState();
            MediaPlayer.PauseWithState();
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            if (FullscreenButton.IsChecked == true)
            {
                timer.Stop();
                {
                    SliderRow.Height = new GridLength(0);
                    PlayerRow1.Height = new GridLength(0);
                    PlayerRow2.Height = new GridLength(0);
                    IsHidden = true;
                    Events.PublishOnUIThread(new FullscreenHideAllEvent(true));
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.None;
                }

            }
        }

        public void Handle(FullscreenEvent message)
        {
            switch (message.Fullscreen)
            {
                case true:
                   
                        MediaPlayer.SetValue(Grid.RowSpanProperty, 4);
                    //SolidColorBrush bg = new SolidColorBrush(Colors.Black);
                    //bg.Opacity = 0.5;
                    //MediaControl1.Background = bg;
                    //MediaControl2.Background = bg;

                    //MediaControl1.ClearValue(BackgroundProperty);
                        

                    //// For each screen, add the screen properties to a list box.
                    //foreach (var screen in System.Windows.Forms.Screen.AllScreens)
                    //{

                    //    listBox1.Items.Add("Device Name: " + screen.DeviceName);
                    //    listBox1.Items.Add("Bounds: " +
                    //        screen.Bounds.ToString());
                    //    listBox1.Items.Add("Type: " +
                    //        screen.GetType().ToString());
                    //    listBox1.Items.Add("Working Area: " +
                    //        screen.WorkingArea.ToString());
                    //    listBox1.Items.Add("Primary Screen: " +
                    //        screen.Primary.ToString());
                    //};
                    break;
                case false:
                        MediaPlayer.SetValue(Grid.RowSpanProperty, 1);
                    break;
                default:
                    break;
            }
        }


        #endregion
    }
}
