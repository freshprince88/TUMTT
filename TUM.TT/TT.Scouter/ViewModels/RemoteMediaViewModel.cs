using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;
using TT.Lib.Results;
using TT.Models.Util.Enums;
using TT.Lib.Interfaces;

namespace TT.Scouter.ViewModels
{
    public class RemoteMediaViewModel : Screen, IMediaPosition
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

        public Match Match { get { return Manager.Match; } }
        private IEventAggregator Events;
        private IMatchManager Manager;
        private IDialogCoordinator Dialogs;
        public bool syncStart { get; set; }
        public bool syncEnd { get; set; }

        private bool _toRallyStart;
        public bool toRallyStart {
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

        public RemoteMediaViewModel(IEventAggregator ev, IMatchManager man, IDialogCoordinator cor)
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
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.RemoteScouter));
            IsPlaying = false;
        }

        public void Play()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Play, Media.Source.RemoteScouter));
            IsPlaying = true;
        }

        public void Stop()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.RemoteScouter));
            IsPlaying = false;
            MediaPosition = TimeSpan.Zero;
        }

        public void NextFrame()
        {
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.RemoteScouter));
            MediaPosition = MediaPosition + delta_time;
            IsPlaying = false;
            

        }
        public void Next5Frames()
        {
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.RemoteScouter));
            MediaPosition = MediaPosition + delta_time;
            IsPlaying = false;

        }
        public void PreviousFrame()
        {
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.RemoteScouter));
            MediaPosition = MediaPosition - delta_time;
            IsPlaying = false;

        }
        public void Previous5Frames()
        {
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.RemoteScouter));
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

        public IEnumerable<IResult> Sync()
        {
            if (syncStart == true && syncEnd == true)
            {
                //Show Dialog to get Match Synchro
                InputDialogResult dialog = new InputDialogResult()
                {
                    Title = "Synchronize Video with Data",
                    Question = "Please set the Offset in seconds! Current offset: " + (Match.Synchro / 1000)
                };
                yield return dialog;
                double seconds = Convert.ToDouble(dialog.Result);
                Match.Synchro = seconds * 1000;
            }

            else if (syncStart==true && syncEnd == false)
            {   //Show Dialog to get Start Offset
                InputDialogResult dialog = new InputDialogResult()
                {
                    Title = "Set Start Offset",
                    Question = "Please set the Offset in seconds!"
                };
                yield return dialog;
                double startOffset = Convert.ToDouble(dialog.Result)*1000;
                Match.StartOffset(startOffset);
            }
            else if (syncStart==false && syncEnd == true)
            {//Show Dialog to get End Offset
                InputDialogResult dialog = new InputDialogResult()
                {
                    Title = "Set End Offset",
                    Question = "Please set the Offset in seconds!"
                };
                yield return dialog;
                double endOffset = Convert.ToDouble(dialog.Result)* 1000;
                Match.EndOffset(endOffset);
            }
            else
            {
                var errorDialog = new ErrorMessageResult()
                {
                    Title = "Keine Offset Option ausgewählt!",
                    Message = "Bitte wählen sie per Rechtsklick entsprechende Optionen aus!",
                    Dialogs = Dialogs
                };
                yield return errorDialog;
            }


        }
    }
}
