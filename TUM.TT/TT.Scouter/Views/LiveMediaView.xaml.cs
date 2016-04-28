﻿using Caliburn.Micro;
using System.Windows.Controls;
using TT.Models.Events;
using System;
using TT.Models.Util.Enums;
using TT.Models.Managers;

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
        private IEventAggregator Events;
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
            Events.Unsubscribe(this);
        }

        public void Handle(MediaControlEvent message)
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

        private void LiveMediaView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            MediaPlayer.Stop();
            MediaPlayer.Close();
            MediaPlayer.Source = Manager.Match.VideoFile != null ? new Uri(Manager.Match.VideoFile) : MediaPlayer.Source;
            MediaPlayer.Play();
            MediaPlayer.Pause();
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

        #endregion
    }
}
