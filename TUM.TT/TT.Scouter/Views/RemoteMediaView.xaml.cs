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
using TT.Models.Util.Enums;

namespace TT.Scouter.Views
{
    /// <summary>
    /// Interaction logic for RemoteMediaView.xaml
    /// </summary>
    public partial class RemoteMediaView : UserControl,
        IHandle<MediaControlEvent>,
        IHandle<MediaSpeedEvent>,
        IHandle<MediaMuteEvent>,
        IHandle<VideoLoadedEvent>
    {
        private IEventAggregator Events;
        private IMatchManager Manager;
        TimeSpan currentTime;

        public RemoteMediaView()
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
            if (message.Source == Media.Source.RemoteScouter)
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
