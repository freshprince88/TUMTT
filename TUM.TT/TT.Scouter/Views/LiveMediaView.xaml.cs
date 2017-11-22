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
    /// Interaction logic for LiveMediaView.xaml
    /// </summary>
    public partial class LiveMediaView : ControlWithBindableKeyGestures,
        IHandle<MediaControlEvent>,
        IHandle<VideoLoadedEvent>,
        IHandle<MediaLiveScouterSpeedEvent>,
        IHandle<MediaLiveScouterMuteEvent>
    {
        public IEventAggregator Events { get; set; }
        TimeSpan currentTime;
        private IMatchManager Manager;

        public LiveMediaView()
        {
            InitializeComponent();
            currentTime = TimeSpan.Zero;
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
            Manager = IoC.Get<IMatchManager>();
            this.Loaded += LiveMediaView_Loaded;
            this.Unloaded += ExtendedMediaView_Unloaded;       
        }


        #region Media Methods

        #endregion

        #region Event Handlers

        private void ExtendedMediaView_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Events.Unsubscribe(this);
            currentTime = MediaPlayer.Position;
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

        public void Handle(MediaControlEvent message)
        {
            if (message.Source == Media.Source.LiveScouter)
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

        private void LiveMediaView_Loaded(object sender, System.Windows.RoutedEventArgs e)
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

        public void Handle(MediaLiveScouterSpeedEvent message)
        {
            switch (message.LiveScouterSpeed)
            {
                case Media.LiveScouterSpeed.Quarter:
                    MediaPlayer.SpeedRatio = 0.25;
                    break;
                case Media.LiveScouterSpeed.Half:
                    MediaPlayer.SpeedRatio = 0.5;
                    break;
                case Media.LiveScouterSpeed.Third:
                    MediaPlayer.SpeedRatio = 0.75;
                    break;
                case Media.LiveScouterSpeed.Full:
                    MediaPlayer.SpeedRatio = 1;
                    break;
                case Media.LiveScouterSpeed.Faster:
                    MediaPlayer.SpeedRatio = 1.5;
                    break;

                default:
                    break;
            }

        }

        public void Handle(MediaLiveScouterMuteEvent message)
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

        #endregion
    }
}
