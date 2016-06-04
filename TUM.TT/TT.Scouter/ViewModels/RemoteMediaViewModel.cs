using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;
using TT.Lib.Results;
using TT.Models.Util.Enums;
using TT.Scouter.Interfaces;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using TT.Scouter.Util.Model;

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
        private Calibration calibration;

        public RemoteMediaViewModel(IEventAggregator ev, IMatchManager man, IDialogCoordinator cor)
        {
            Events = ev;
            Manager = man;
            IsPlaying = false;
            Dialogs = cor;
            calibration = new Calibration();
            calibration.Lines.CollectionChanged += Lines_CollectionChanged;
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

        #region Calibration Methods

        public void CalibrateTable()
        {
            if (calibration.Lines.Count > 0)
            {
                Events.BeginPublishOnUIThread(new DeleteLinesEvent(calibration.Lines.ToList<Line>()));
                calibration.Lines.Clear();
            }
            calibration.startCalibrating();
        }

        public void ToogleCalibration()
        {
            if (calibration.isCalibrated)
            {
                foreach(Line l in calibration.Lines)
                {
                    if (l.Visibility == Visibility.Visible)
                        l.Visibility = Visibility.Hidden;
                    else
                        l.Visibility = Visibility.Visible;
                }
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

                if (calibration.IsPointInPolygon(p))
                {
                    Point pointOnTable = calibration.getPointPositionToTable(p);
                    Line l = new Line();
                    l.X1 = p.X;
                    l.Y1 = p.Y;
                    l.X2 = pointOnTable.X;
                    l.Y2 = pointOnTable.Y;
                    l.Stroke = System.Windows.Media.Brushes.AliceBlue;
                    l.StrokeThickness = 4;

                    Events.PublishOnUIThread(new DrawLineEvent(l));
                }
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
