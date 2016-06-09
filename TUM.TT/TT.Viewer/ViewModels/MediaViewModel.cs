﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using TT.Models.Util.Enums;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;
using MahApps.Metro.Controls.Dialogs;
using TT.Lib.Interfaces;

namespace TT.Viewer.ViewModels
{
    public class MediaViewModel : Screen, IMediaPosition
    {
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
                    _playing = value;
                NotifyOfPropertyChange();
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

        public MediaViewModel(IEventAggregator ev, IMatchManager man, IDialogCoordinator cor)
        {
            Events = ev;
            Manager = man;
            IsPlaying = false;
            Dialogs = cor;
            syncStart = true;
            syncEnd = true;
            toRallyStart = true;
            PlayMode = false;
        }

        public void Pause()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.Viewer));
            IsPlaying = false;
        }

        public void Play()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Play, Media.Source.Viewer));
            IsPlaying = true;
        }

        public void Stop()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.Viewer));
            IsPlaying = false;
            MediaPosition = TimeSpan.Zero;
        }

        public void NextFrame()
        {
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.Viewer));
            MediaPosition = MediaPosition + delta_time;
            IsPlaying = false;


        }
        public void Next5Frames()
        {
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.Viewer));
            MediaPosition = MediaPosition + delta_time;
            IsPlaying = false;

        }
        public void PreviousFrame()
        {
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.Viewer));
            MediaPosition = MediaPosition - delta_time;
            IsPlaying = false;

        }
        public void Previous5Frames()
        {
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.Viewer));
            MediaPosition = MediaPosition - delta_time;
            IsPlaying = false;

        }

        public void Slow(int slow)
        {
            if (slow == 50)
                Events.PublishOnUIThread(new MediaSpeedEvent(Media.Speed.Half));
            else if (slow == 75)
                Events.PublishOnUIThread(new MediaSpeedEvent(Media.Speed.Third));
            else if (slow == 25)
                Events.PublishOnUIThread(new MediaSpeedEvent(Media.Speed.Quarter));
            else if (slow == 150)
                Events.PublishOnUIThread(new MediaSpeedEvent(Media.Speed.Faster));
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

        public IEnumerable<IResult> Open()
        {
            return Manager.LoadVideo();
        }     
        
        public void NextRally()
        {
        }   

        public void PreviousRally()
        {
        }
    }
}
