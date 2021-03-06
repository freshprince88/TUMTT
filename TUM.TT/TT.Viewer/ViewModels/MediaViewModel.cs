﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using TT.Models.Util.Enums;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;
using MahApps.Metro.Controls.Dialogs;
using TT.Lib.Interfaces;
using TT.Lib.Util;

namespace TT.Viewer.ViewModels
{
    public class MediaViewModel : Screen, IMediaPosition,
        IHandle<PlayModeEvent>, IHandle<MediaControlEvent>
    {
        #region Properties

        ///// <summary>
        ///// Sets key bindings for ControlWithBindableKeyGestures
        ///// </summary>
        //public Dictionary<string, KeyBinding> KeyBindings
        //{
        //    get
        //    {
        //        //get all method names of this class
        //        var methodNames = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Select(info => info.Name);

        //        //get all existing key gestures that match the method names
        //        var keyGesture = ShortcutFactory.Instance.KeyGestures.Where(pair => methodNames.Contains(pair.Key));

        //        //return relevant key gestures
        //        return keyGesture.ToDictionary(x => x.Key, x => (KeyBinding)x.Value); // TODO
        //    }
        //    set { }
        //}

        private TimeSpan _mediaLength;
        public TimeSpan MediaLength
        {
            get
            {
                return _mediaLength;
            }
            set
            {
                if (_mediaLength != value)
                    _mediaLength = value;
                NotifyOfPropertyChange();
            }
        }

        private TimeSpan _mediaPos;
        public TimeSpan MediaPosition
        {
            get
            {
                return _mediaPos;
            }
            set
            {
                if (_mediaPos != value)
                    _mediaPos = value;
                NotifyOfPropertyChange();
            }
        }

        private TimeSpan _endPos;
        public TimeSpan EndPosition
        {
            get
            {
                return _endPos;
            }
            set
            {
                if (_endPos != value)
                    _endPos = value;
                NotifyOfPropertyChange();
            }
        }

        private bool _playing;
        public bool IsPlaying
        {
            get
            {
                return _playing;
            }
            set
            {
                if (_playing != value)
                {
                    _playing = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        private bool _muted;
        public bool IsMuted
        {
            get
            {
                return _muted;
            }
            set
            {
                if (_muted != value)
                    _muted = value;
                NotifyOfPropertyChange();
            }
        }

        private bool _fullscreen;
        public bool IsFullscreen
        {
            get
            {
                return _fullscreen;
            }
            set
            {
                if (_fullscreen != value)
                    _fullscreen = value;
                NotifyOfPropertyChange();
            }
        }

        private double _min;
        public double Minimum
        {
            get
            {
                return _min;
            }
            set
            {
                if (_min != value)
                    _min = value;
                NotifyOfPropertyChange();
            }
        }

        private double _max;
        public double Maximum
        {
            get
            {
                return _max;
            }
            set
            {
                if (_max != value)
                    _max = value;
                NotifyOfPropertyChange();
            }
        }

        public Match Match { get { return Manager.Match; } }
        private IEventAggregator Events;
        private IMatchManager Manager;
        private IDialogCoordinator Dialogs;
        public bool syncStart { get; set; }
        public bool syncEnd { get; set; }

        private bool _toRallyStart;
        public bool toRallyStart
        {
            get
            {
                return _toRallyStart;
            }
            set
            {
                if (_toRallyStart != value)
                {
                    _toRallyStart = value;
                    NotifyOfPropertyChange();
                    NotifyOfPropertyChange("roRallyStart");
                }
            }
        }

        private bool? _playMode;
        public bool? PlayMode
        {
            get
            {
                return _playMode;
            }
            set
            {
                if (_playMode != value)
                    _playMode = value;
                NotifyOfPropertyChange();
            }
        }

        #endregion

        public MediaViewModel(IEventAggregator ev, IMatchManager man, IDialogCoordinator cor)
        {
            Events = ev;
            Manager = man;
            IsFullscreen = false;
            Dialogs = cor;
            syncStart = true;
            syncEnd = true;
            toRallyStart = true;
            PlayMode = false;
        }

        #region Caliburn hooks

        protected override void OnActivate()
        {
            base.OnActivate();
            Events.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            Events.PublishOnUIThread(new DeactivationEvent(close));
            Events.Unsubscribe(this);
            base.OnDeactivate(close);
        }

        #endregion        

        public IEnumerable<IResult> Open()
        {
            return Manager.LoadVideo();
        }

        public void PlayModeHelper()
        {
            switch (PlayMode)
            {
                case null:
                    PlayMode = false;
                    break;
                case false:
                    PlayMode = true;
                    break;
                case true:
                    PlayMode = null;
                    break;
            }
        }

        #region Media Methods

        public void Pause()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.Viewer));
        }

        public void Play()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Play, Media.Source.Viewer));
        }

        public void PlayPause()
        {
            if (IsPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }

        public void Stop()
        {            
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Stop, Media.Source.Viewer));;
        }

        public void NextFrame()
        {            
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.Viewer));
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            MediaPosition = MediaPosition + delta_time;
        }

        public void Next5Frames()
        {            
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.Viewer));
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            MediaPosition = MediaPosition + delta_time;
        }

        public void PreviousFrame()
        {            
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.Viewer));
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            MediaPosition = MediaPosition - delta_time;
        }

        public void Previous5Frames()
        {            
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.Viewer));
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            MediaPosition = MediaPosition - delta_time;
        }


        public void Slow(int slow)
        {
            if (slow == 50)
                Events.PublishOnUIThread(new MediaSpeedEvent(Media.Speed.Half));
            else if (slow == 75)
                Events.PublishOnUIThread(new MediaSpeedEvent(Media.Speed.Third));
            else if (slow == 25)
                Events.PublishOnUIThread(new MediaSpeedEvent(Media.Speed.Quarter));
            else
                Events.PublishOnUIThread(new MediaSpeedEvent(Media.Speed.Full));
        }

        public void Mute()
        {
            IsMuted = true;
            Events.PublishOnUIThread(new MediaMuteEvent(Media.Mute.Mute));
        }

        public void UnMute()
        {
            IsMuted = false;
            Events.PublishOnUIThread(new MediaMuteEvent(Media.Mute.Unmute));
        }
        public void FullscreenHelper()
        {
            IsFullscreen = !IsFullscreen;
            FullscreenOnOff();
        }

        public void FullscreenOnOff()
        {
            if (IsFullscreen)
            {
                Events.PublishOnUIThread(new FullscreenEvent(true));
            }
            else
            {
                Events.PublishOnUIThread(new FullscreenHidePlayerEvent(false));
                Events.PublishOnUIThread(new FullscreenEvent(false));

            }
        }

        public void NextRally()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Next, Media.Source.Viewer));
        }

        public void PreviousRally()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Previous, Media.Source.Viewer));
        }

        public void StartRallyAtBeginning()
        {
            MediaPosition = TimeSpan.FromMilliseconds(Manager.ActiveRally.Start);
        }

        public void PauseRallyAtBeginning()
        {
            MediaPosition = TimeSpan.FromMilliseconds(Manager.ActiveRally.Start);
            Pause();
        }
        public void SkipForward()
        {

        }
        public void SkipBackwards()
        {

        }

        #endregion

        #region Event Handlers

        public void Handle(PlayModeEvent message)
        {
            switch (message.PlayMode)
            {
                case null:
                    Pause();
                    NextRally();
                    break;
                case false:
                    PauseRallyAtBeginning();
                    break;
                case true:
                    StartRallyAtBeginning();
                    break;
                default:
                    break;
            }
        }

        public void Handle(MediaControlEvent message)
        {
            if (message.Source == Media.Source.Viewer)
            {
                switch (message.Ctrl)
                {
                    case Media.Control.Stop:
                        IsPlaying = false;
                        break;
                    case Media.Control.Pause:
                        IsPlaying = false;
                        break;
                    case Media.Control.Play:
                        IsPlaying = true;
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}
