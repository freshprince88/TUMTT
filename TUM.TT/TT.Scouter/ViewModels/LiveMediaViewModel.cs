using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;
using TT.Lib.Results;
using TT.Models.Util.Enums;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using TT.Scouter.Util.Model;
using TT.Lib.Interfaces;
using TT.Lib.Util;
using System.Windows.Controls;

namespace TT.Scouter.ViewModels
{
    public class LiveMediaViewModel : Screen, IMediaPosition
    {
        /// <summary>
        /// Sets key bindings for ControlWithBindableKeyGestures
        /// </summary>
        public Dictionary<string, KeyBinding> KeyBindings
        {
            get
            {
                //get all method names of this class
                var methodNames = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public).Select(info => info.Name);

                //get all existing key gestures that match the method names
                var keyGesture = ShortcutFactory.Instance.KeyGestures.Where(pair => methodNames.Contains(pair.Key));

                //return relevant key gestures
                return keyGesture.ToDictionary(x => x.Key, x => (KeyBinding)x.Value); // TODO
            }
            set { }
        }

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
                {
                    _mediaPos = value;
                    NotifyOfPropertyChange();
                }
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
        private bool _oneBackwards;
        public bool OneBackwardsChecked
        {
            get
            {
                return _oneBackwards;
            }
            set
            {
                if (_oneBackwards != value)
                    _oneBackwards = value;
                NotifyOfPropertyChange("OneBackwardsChecked");
            }
        }
        private bool _twoBackwards;
        public bool TwoBackwardsChecked
        {
            get
            {
                return _twoBackwards;
            }
            set
            {
                if (_twoBackwards != value)
                    _twoBackwards = value;
                NotifyOfPropertyChange("TwoBackwardsChecked");
            }
        }
        private bool _threeBackwards;
        public bool ThreeBackwardsChecked
        {
            get
            {
                return _threeBackwards;
            }
            set
            {
                if (_threeBackwards != value)
                    _threeBackwards = value;
                NotifyOfPropertyChange("ThreeBackwardsChecked");
            }
        }
        private bool _fourBackwards;
        public bool FourBackwardsChecked
        {
            get
            {
                return _fourBackwards;
            }
            set
            {
                if (_fourBackwards != value)
                    _fourBackwards = value;
                NotifyOfPropertyChange("FourBackwardsChecked");
            }
        }
        private bool _fiveBackwards;
        public bool FiveBackwardsChecked
        {
            get
            {
                return _fiveBackwards;
            }
            set
            {
                if (_fiveBackwards != value)
                    _fiveBackwards = value;
                NotifyOfPropertyChange("FiveBackwardsChecked");
            }
        }
        private bool _sixBackwards;
        public bool SixBackwardsChecked
        {
            get
            {
                return _sixBackwards;
            }
            set
            {
                if (_sixBackwards != value)
                    _sixBackwards = value;
                NotifyOfPropertyChange("SixBackwardsChecked");
            }
        }
        private bool _sevenBackwards;
        public bool SevenBackwardsChecked
        {
            get
            {
                return _sevenBackwards;
            }
            set
            {
                if (_sevenBackwards != value)
                    _sevenBackwards = value;
                NotifyOfPropertyChange("SevenBackwardsChecked");
            }
        }
        private bool _threeForward;


        private bool _oneForward;
        public bool OneForwardChecked
        {
            get
            {
                return _oneForward;
            }
            set
            {
                if (_oneForward != value)
                    _oneForward = value;
                NotifyOfPropertyChange("OneForwardChecked");
            }
        }
        private bool _twoForward;
        public bool TwoForwardChecked
        {
            get
            {
                return _twoForward;
            }
            set
            {
                if (_twoForward != value)
                    _twoForward = value;
                NotifyOfPropertyChange("TwoForwardChecked");
            }
        }
        public bool ThreeForwardChecked
        {
            get
            {
                return _threeForward;
            }
            set
            {
                if (_threeForward != value)
                    _threeForward = value;
                NotifyOfPropertyChange("ThreeForwardChecked");
            }
        }
        private bool _fourForward;
        public bool FourForwardChecked
        {
            get
            {
                return _fourForward;
            }
            set
            {
                if (_fourForward != value)
                    _fourForward = value;
                NotifyOfPropertyChange("FourForwardChecked");
            }
        }
        private bool _fiveForward;
        public bool FiveForwardChecked
        {
            get
            {
                return _fiveForward;
            }
            set
            {
                if (_fiveForward != value)
                    _fiveForward = value;
                NotifyOfPropertyChange("FiveForwardChecked");
            }
        }
        private bool _sixForward;
        public bool SixForwardChecked
        {
            get
            {
                return _sixForward;
            }
            set
            {
                if (_sixForward != value)
                    _sixForward = value;
                NotifyOfPropertyChange("SixForwardChecked");
            }
        }
        private bool _sevenForward;
        public bool SevenForwardChecked
        {
            get
            {
                return _sevenForward;
            }
            set
            {
                if (_sevenForward != value)
                    _sevenForward = value;
                NotifyOfPropertyChange("SevenForwardChecked");
            }
        }

        public bool toRallyStart { get; set; }

        public Match Match { get { return Manager.Match; } }

        private IEventAggregator Events;
        private IMatchManager Manager;
        private IDialogCoordinator Dialogs;

        public LiveMediaViewModel(IEventAggregator ev, IMatchManager man, IDialogCoordinator cor)
        {
            Events = ev;
            Manager = man;
            Dialogs = cor;
            IsPlaying = false;
            OneBackwardsChecked = false;
            TwoBackwardsChecked = true;
            ThreeBackwardsChecked = false;
            FourBackwardsChecked = false;
            FiveBackwardsChecked = false;
            SixBackwardsChecked = false;
            SevenBackwardsChecked = false;
            OneForwardChecked = false;
            TwoForwardChecked = false;
            ThreeForwardChecked = true;
            FourForwardChecked = false;
            FiveForwardChecked = false;
            SixForwardChecked = false;
            SevenForwardChecked = false;
        }

        #region Caliburn hooks

        protected override void OnActivate()
        {
            base.OnActivate();
            Events.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            Events.Unsubscribe(this);
            base.OnDeactivate(close);
        }

        #endregion  

        #region Event Handlers
     
        #endregion

        #region View Methods

        public void Play()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Play, Media.Source.LiveScouter));
        }

        public void Pause()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.LiveScouter));
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
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.LiveScouter));
            MediaPosition = TimeSpan.Zero;
        }

        public void Slow(int slow)
        {
            if (slow == 50)
                Events.PublishOnUIThread(new MediaLiveScouterSpeedEvent(Media.LiveScouterSpeed.Half));
            else if (slow == 75)
                Events.PublishOnUIThread(new MediaLiveScouterSpeedEvent(Media.LiveScouterSpeed.Third));
            else if (slow == 25)
                Events.PublishOnUIThread(new MediaLiveScouterSpeedEvent(Media.LiveScouterSpeed.Quarter));
            else if (slow == 150)
                Events.PublishOnUIThread(new MediaLiveScouterSpeedEvent(Media.LiveScouterSpeed.Faster));
            else
                Events.PublishOnUIThread(new MediaLiveScouterSpeedEvent(Media.LiveScouterSpeed.Full));
        }

        public void SkipBackwards()
        {
            if (OneBackwardsChecked)
            {
                TimeSpan delta_time = new TimeSpan(0, 0, 0, 1, 0);
                MediaPosition = MediaPosition - delta_time;
            }
            if (TwoBackwardsChecked)
            {
                TimeSpan delta_time = new TimeSpan(0, 0, 0, 2, 0);
                MediaPosition = MediaPosition - delta_time;
            }
            if (ThreeBackwardsChecked)
            {
                TimeSpan delta_time = new TimeSpan(0, 0, 0, 3, 0);
                MediaPosition = MediaPosition - delta_time;
            }
            if (FourBackwardsChecked)
            {
                TimeSpan delta_time = new TimeSpan(0, 0, 0, 4, 0);
                MediaPosition = MediaPosition - delta_time;
            }
            if (FiveBackwardsChecked)
            {
                TimeSpan delta_time = new TimeSpan(0, 0, 0, 5, 0);
                MediaPosition = MediaPosition - delta_time;
            }
            if (SixBackwardsChecked)
            {
                TimeSpan delta_time = new TimeSpan(0, 0, 0, 6, 0);
                MediaPosition = MediaPosition - delta_time;
            }
            if (SevenBackwardsChecked)
            {
                TimeSpan delta_time = new TimeSpan(0, 0, 0, 7, 0);
                MediaPosition = MediaPosition - delta_time;
            }
        }
        public void SkipForward()
        {
            if (OneForwardChecked)
            {
                TimeSpan delta_time = new TimeSpan(0, 0, 0, 1, 0);
                MediaPosition = MediaPosition + delta_time;
            }
            if (TwoForwardChecked)
            {
                TimeSpan delta_time = new TimeSpan(0, 0, 0, 2, 0);
                MediaPosition = MediaPosition + delta_time;
            }
            if (ThreeForwardChecked)
            {
                TimeSpan delta_time = new TimeSpan(0, 0, 0, 3, 0);
                MediaPosition = MediaPosition + delta_time;
            }
            if (FourForwardChecked)
            {
                TimeSpan delta_time = new TimeSpan(0, 0, 0, 4, 0);
                MediaPosition = MediaPosition + delta_time;
            }
            if (FiveForwardChecked)
            {
                TimeSpan delta_time = new TimeSpan(0, 0, 0, 5, 0);
                MediaPosition = MediaPosition + delta_time;
            }
            if (SixForwardChecked)
            {
                TimeSpan delta_time = new TimeSpan(0, 0, 0, 6, 0);
                MediaPosition = MediaPosition + delta_time;
            }
            if (SevenForwardChecked)
            {
                TimeSpan delta_time = new TimeSpan(0, 0, 0, 7, 0);
                MediaPosition = MediaPosition + delta_time;
            }
        }

        public void Mute()
        {
            IsMuted = true;
            Events.PublishOnUIThread(new MediaLiveScouterMuteEvent(Media.Mute.Mute));
        }

        public void UnMute()
        {
            IsMuted = false;
            Events.PublishOnUIThread(new MediaLiveScouterMuteEvent(Media.Mute.Unmute));
        }

        #endregion

        #region Helper Methods for Shortcuts
        public void PlayPauseLiveMode()
        {
                PlayPause();
        }
        #endregion

        #region Helper Methods
        public void SetDefaultSkipBackwardsDuration(MenuItem m)
        {
            switch (m.Header.ToString())
            {
                case "1":
                    OneBackwardsChecked = true;
                    TwoBackwardsChecked = false;
                    ThreeBackwardsChecked = false;
                    FourBackwardsChecked = false;
                    FiveBackwardsChecked = false;
                    SixBackwardsChecked = false;
                    SevenBackwardsChecked = false;
                    break;
                case "2":
                    OneBackwardsChecked = false;
                    TwoBackwardsChecked = true;
                    ThreeBackwardsChecked = false;
                    FourBackwardsChecked = false;
                    FiveBackwardsChecked = false;
                    SixBackwardsChecked = false;
                    SevenBackwardsChecked = false;
                    break;
                case "3":
                    OneBackwardsChecked = false;
                    TwoBackwardsChecked = false;
                    ThreeBackwardsChecked = true;
                    FourBackwardsChecked = false;
                    FiveBackwardsChecked = false;
                    SixBackwardsChecked = false;
                    SevenBackwardsChecked = false;
                    break;
                case "4":
                    OneBackwardsChecked = false;
                    TwoBackwardsChecked = false;
                    ThreeBackwardsChecked = false;
                    FourBackwardsChecked = true;
                    FiveBackwardsChecked = false;
                    SixBackwardsChecked = false;
                    SevenBackwardsChecked = false;
                    break;
                case "5":
                    OneBackwardsChecked = false;
                    TwoBackwardsChecked = false;
                    ThreeBackwardsChecked = false;
                    FourBackwardsChecked = false;
                    FiveBackwardsChecked = true;
                    SixBackwardsChecked = false;
                    SevenBackwardsChecked = false;
                    break;
                case "6":
                    OneBackwardsChecked = false;
                    TwoBackwardsChecked = false;
                    ThreeBackwardsChecked = false;
                    FourBackwardsChecked = false;
                    FiveBackwardsChecked = false;
                    SixBackwardsChecked = true;
                    SevenBackwardsChecked = false;
                    break;
                case "7":
                    OneBackwardsChecked = false;
                    TwoBackwardsChecked = false;
                    ThreeBackwardsChecked = false;
                    FourBackwardsChecked = false;
                    FiveBackwardsChecked = false;
                    SixBackwardsChecked = false;
                    SevenBackwardsChecked = true;
                    break;
            }
        }
        public void SetDefaultSkipForwardDuration(MenuItem m)
        {
            switch (m.Header.ToString())
            {
                case "1":
                    OneForwardChecked = true;
                    TwoForwardChecked = false;
                    ThreeForwardChecked = false;
                    FourForwardChecked = false;
                    FiveForwardChecked = false;
                    SixForwardChecked = false;
                    SevenForwardChecked = false;
                    break;
                case "2":
                    OneForwardChecked = false;
                    TwoForwardChecked = true;
                    ThreeForwardChecked = false;
                    FourForwardChecked = false;
                    FiveForwardChecked = false;
                    SixForwardChecked = false;
                    SevenForwardChecked = false;
                    break;
                case "3":
                    OneForwardChecked = false;
                    TwoForwardChecked = false;
                    ThreeForwardChecked = true;
                    FourForwardChecked = false;
                    FiveForwardChecked = false;
                    SixForwardChecked = false;
                    SevenForwardChecked = false;
                    break;
                case "4":
                    OneForwardChecked = false;
                    TwoForwardChecked = false;
                    ThreeForwardChecked = false;
                    FourForwardChecked = true;
                    FiveForwardChecked = false;
                    SixForwardChecked = false;
                    SevenForwardChecked = false;
                    break;
                case "5":
                    OneForwardChecked = false;
                    TwoForwardChecked = false;
                    ThreeForwardChecked = false;
                    FourForwardChecked = false;
                    FiveForwardChecked = true;
                    SixForwardChecked = false;
                    SevenForwardChecked = false;
                    break;
                case "6":
                    OneForwardChecked = false;
                    TwoForwardChecked = false;
                    ThreeForwardChecked = false;
                    FourForwardChecked = false;
                    FiveForwardChecked = false;
                    SixForwardChecked = true;
                    SevenForwardChecked = false;
                    break;
                case "7":
                    OneForwardChecked = false;
                    TwoForwardChecked = false;
                    ThreeForwardChecked = false;
                    FourForwardChecked = false;
                    FiveForwardChecked = false;
                    SixForwardChecked = false;
                    SevenForwardChecked = true;
                    break;
            }


        }    
        #endregion
    }

}
