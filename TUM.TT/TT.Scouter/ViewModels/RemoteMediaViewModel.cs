using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Lib.Models;
using TT.Lib.Results;
using TT.Lib.Util.Enums;
using TT.Scouter.Interfaces;

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

        public RemoteMediaViewModel(IEventAggregator ev, IMatchManager man, IDialogCoordinator cor)
        {
            Events = ev;
            Manager = man;
            IsPlaying = false;
            Dialogs = cor;          
        }

        public void Pause()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause));
            IsPlaying = false;
        }

        public void Play()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Play));
            IsPlaying = true;
        }

        public void Stop()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause));
            IsPlaying = false;
            MediaPosition = TimeSpan.Zero;
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
    }
}
