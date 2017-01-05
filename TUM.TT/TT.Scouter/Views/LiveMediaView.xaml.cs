using Caliburn.Micro;
using System.Windows.Controls;
using TT.Lib.Events;
using System;
using TT.Models.Util.Enums;
using TT.Lib.Managers;

namespace TT.Scouter.Views
{
    /// <summary>
    /// Interaction logic for LiveMediaView.xaml
    /// </summary>
    public partial class LiveMediaView : UserControl,
        IHandle<MediaControlEvent>,
        IHandle<MediaSpeedEvent>,
        IHandle<MediaMuteEvent>
    {
        public IEventAggregator Events { get; set; }

        private IMatchManager Manager;

        public LiveMediaView()
        {
            InitializeComponent();
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
            //Events.Unsubscribe(this);
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
            MediaPlayer.StopWithState();
            MediaPlayer.Close();
            MediaPlayer.Source = Manager.Match.VideoFile != null ? new Uri(Manager.Match.VideoFile) : MediaPlayer.Source;
            MediaPlayer.PlayWithState();
            MediaPlayer.PauseWithState();
            PlayButton.Visibility = System.Windows.Visibility.Visible;
            
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
                case Media.Speed.Faster:
                    MediaPlayer.SpeedRatio = 1.5;
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

        #endregion
    }
}
