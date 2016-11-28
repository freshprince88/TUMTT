using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using System.Collections.Generic;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;
using TT.Lib.Results;
using TT.Models.Util.Enums;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using TT.Scouter.Util.Model;
using TT.Lib.Interfaces;
using System.Linq;

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

        public Match Match { get { return Manager.Match; } }
        private IEventAggregator Events;
        private IMatchManager Manager;
        private IDialogCoordinator Dialogs;
        private Calibration calibration;
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
        
        public RemoteMediaViewModel(IEventAggregator ev, IMatchManager man, IDialogCoordinator cor, Calibration cal)
        {
            Events = ev;
            Manager = man;
            Dialogs = cor;
            syncStart = true;
            syncEnd = true;
            toRallyStart = true;
            PlayMode = false;             
            IsPlaying = false;
            calibration = cal;
            calibration.Lines.CollectionChanged += Lines_CollectionChanged;
            calibration.MidLines.CollectionChanged += Lines_CollectionChanged;
            calibration.GridLines.CollectionChanged += Lines_CollectionChanged;
        }
        

        #region Media Methods
        public void Pause()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.RemoteScouter));
        }

        public void Play()
        {
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Play, Media.Source.RemoteScouter));
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
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.RemoteScouter));
            MediaPosition = TimeSpan.Zero;
        }

        public void NextFrame()
        {            
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.RemoteScouter));
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            MediaPosition = MediaPosition + delta_time;
        }

        public void Next5Frames()
        {            
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.RemoteScouter));
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 200);
            MediaPosition = MediaPosition + delta_time;
        }

        public void PreviousFrame()
        {            
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.RemoteScouter));
            TimeSpan delta_time = new TimeSpan(0, 0, 0, 0, 40);
            MediaPosition = MediaPosition - delta_time;
        }

        public void Previous5Frames()
        {            
            Events.PublishOnUIThread(new MediaControlEvent(Media.Control.Pause, Media.Source.RemoteScouter));
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
        #endregion

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

        #region Calibration Methods

        public void CalibrateTable()
        {
            if (calibration.Lines.Count > 0)
            {
                Events.BeginPublishOnUIThread(new DeleteLinesEvent(calibration.Lines.ToList<Line>()));
                calibration.Lines.Clear();
                Events.BeginPublishOnUIThread(new DeleteLinesEvent(calibration.MidLines.ToList<Line>()));
                calibration.MidLines.Clear();
                Events.BeginPublishOnUIThread(new DeleteLinesEvent(calibration.GridLines.ToList<Line>()));
                calibration.GridLines.Clear();
            }
            calibration.startCalibrating();
        }

        public void ToogleCalibration()
        {
            if (calibration.isCalibrated)
            {
                calibration.toggleCalibration();
            }
        }

        public void MouseDown(MouseButtonEventArgs e, System.Windows.Controls.Grid mediaContainer)
        {
            if (calibration.isCalibrating)
            {
                System.Windows.Point p = e.GetPosition(mediaContainer);

                calibration.AddPoint(p);
            }
            else if(calibration.isCalibrated)
            {
                System.Windows.Point p = e.GetPosition(mediaContainer);

                calibration.calcPointPositionOnTable(p);
            }
        }
        

        private void Lines_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case (System.Collections.Specialized.NotifyCollectionChangedAction.Add):
                {
                    foreach (Line l in e.NewItems)
                    {
                        Events.PublishOnUIThread(new DrawLineEvent(l));
                    }
                }
                break;
            }
        }

        #endregion
    }
}