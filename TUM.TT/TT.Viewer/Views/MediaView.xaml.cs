using Caliburn.Micro;
using System;
using System.Windows.Controls;
using TT.Lib.Events;
using TT.Models.Util.Enums;
using TT.Lib.Managers;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für MediaView.xaml
    /// </summary>
    public partial class MediaView : UserControl,
        IHandle<MediaControlEvent>,
        IHandle<MediaSpeedEvent>,
        IHandle<MediaMuteEvent>,
        IHandle<VideoLoadedEvent>
    {
        private IEventAggregator Events;
        private IMatchManager Manager;
        TimeSpan currentTime;

        public MediaView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            //Events.Subscribe(this);
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
            if (message.Source == Media.Source.Viewer)
            {
                switch (message.Ctrl)
                {
                    case Media.Control.Stop:
                        MediaPlayer.Stop();
                        break;
                    case Media.Control.Pause:
                        MediaPlayer.Pause();
                        break;
                    case Media.Control.Play:
                        MediaPlayer.Play();
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
                MediaPlayer.Stop();
                MediaPlayer.Close();
                MediaPlayer.Source = new Uri(Manager.Match.VideoFile);
                MediaPlayer.MediaPosition = currentTime;
                MediaPlayer.Play();
                MediaPlayer.Pause();
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
            MediaPlayer.Stop();
            MediaPlayer.Close();
            MediaPlayer.Source = Manager.Match.VideoFile != null ? new Uri(Manager.Match.VideoFile) : MediaPlayer.Source;
            MediaPlayer.Play();
            MediaPlayer.Pause();
        }

        #endregion
    }
}
