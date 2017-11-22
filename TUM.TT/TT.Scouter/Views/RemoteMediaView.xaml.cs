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
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Lib.Views;
using TT.Models.Util.Enums;

namespace TT.Scouter.Views
{
    /// <summary>
    /// Interaction logic for RemoteMediaView.xaml
    /// </summary>
    public partial class RemoteMediaView : ControlWithBindableKeyGestures,
        IHandle<MediaControlEvent>,
        IHandle<MediaSpeedEvent>,
        IHandle<MediaMuteEvent>,
        IHandle<VideoLoadedEvent>,
        IHandle<DrawLineEvent>,
        IHandle<DeleteLinesEvent>,
        IHandle<FollowMouseEvent>
    {
        public IEventAggregator Events { get; set; }

        private IMatchManager Manager;
        TimeSpan currentTime;

        private bool lineIsDisplayed = false;

        public RemoteMediaView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
            Manager = IoC.Get<IMatchManager>();
            this.Loaded += RemoteMediaView_Loaded;
            this.Unloaded += ExtendedMediaView_Unloaded;
            currentTime = TimeSpan.Zero;
        }


        #region Media Methods

        #endregion

        #region Event Handlers

        private void ExtendedMediaView_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Events.Unsubscribe(this);
            currentTime = MediaPlayer.Position;
        }

        public void Handle(MediaControlEvent message)
        {
            if (message.Source == Media.Source.RemoteScouter)
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

            PlayButton.Visibility = System.Windows.Visibility.Visible;
        }

        public void Handle(DrawLineEvent message)
        {
            MediaContainer.Children.Add(message.Line);
        }

        public void Handle(DeleteLinesEvent message)
        {
            foreach(Line l in message.Lines)
            {
                MediaContainer.Children.Remove(l);
            }
        }

        public void Handle(FollowMouseEvent message)
        {
            if (message.LastPosition.X == -1 && message.LastPosition.Y == -1)
            {
                interactiveLine.X1 = 0;
                interactiveLine.Y1 = 0;
                interactiveLine.Visibility = Visibility.Hidden;
                lineIsDisplayed = false;
            }else
            {
                interactiveLine.X1 = message.LastPosition.X;
                interactiveLine.Y1 = message.LastPosition.Y;
                interactiveLine.Visibility = Visibility.Visible;
                lineIsDisplayed = true;
            }
        }

        private void ContentControl_MouseMove(object sender, MouseEventArgs e)
        {
            // Get the x and y coordinates of the mouse pointer.
            System.Windows.Point position = e.GetPosition(MediaContainer);
            interactiveLine.X2 = position.X;
            interactiveLine.Y2 = position.Y;
        }

        #endregion

        private void ContentControl_MouseLeave(object sender, MouseEventArgs e)
        {
            interactiveLine.Visibility = Visibility.Hidden;
        }

        private void ContentControl_MouseEnter(object sender, MouseEventArgs e)
        {
            if (lineIsDisplayed)
                interactiveLine.Visibility = Visibility.Visible;
            else
                interactiveLine.Visibility = Visibility.Hidden;
        }
    }
}
