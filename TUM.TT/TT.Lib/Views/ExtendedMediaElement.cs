using Caliburn.Micro;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.ComponentModel;
using TT.Lib.Events;
using System.Diagnostics;

namespace TT.Lib.Views
{
    public class ExtendedMediaElement : MediaElement, 
        INotifyPropertyChangedEx,
        IHandle<DeactivationEvent>
    {
        public static readonly DependencyProperty MediaLengthProperty = DependencyProperty.Register("MediaLength",
            typeof(TimeSpan),
            typeof(ExtendedMediaElement));

        public static readonly DependencyProperty MediaPositionProperty = DependencyProperty.Register("MediaPosition",
            typeof(TimeSpan),
            typeof(ExtendedMediaElement),
            new PropertyMetadata(MediaPositionChanged));

        // Using a DependencyProperty as the backing store for EndPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndPositionProperty =
            DependencyProperty.Register("EndPosition", typeof(TimeSpan), typeof(ExtendedMediaElement));

        // Using a DependencyProperty as the backing store for IsPlaying.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPlayingProperty =
            DependencyProperty.Register("IsPlaying", typeof(bool), typeof(ExtendedMediaElement), new PropertyMetadata(true));

        public bool? PlayMode
        {
            get { return (bool?)GetValue(PlayModeProperty); }
            set { SetValue(PlayModeProperty, value); NotifyOfPropertyChange(); }
        }

        // Using a DependencyProperty as the backing store for PlayMode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayModeProperty =
            DependencyProperty.Register("PlayMode", typeof(bool?), typeof(ExtendedMediaElement), new PropertyMetadata(false));


        private static void PlayModeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((ExtendedMediaElement)obj).PlayModeChanged((bool?)e.NewValue);
        }

        private void PlayModeChanged(bool? newValue)
        {
            switch (newValue)
            {
                case null:
                    break;
                case false:
                    break;
                case true:
                    break;
                default:
                    break;
            }
        }

        private static void MediaPositionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ((ExtendedMediaElement)obj).MediaPositionChanged((TimeSpan)e.NewValue);
        }

        private readonly DispatcherTimer mediaTimer;
        private bool positionChangedByTimer;
        private bool firstStart = true;
        private bool mediaOpened = false;
        private TimeSpan? userMediaPosition;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsPlaying
        {
            get { return (bool)GetValue(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); NotifyOfPropertyChange(); }
        }

        public TimeSpan MediaLength
        {
            get { return (TimeSpan)base.GetValue(MediaLengthProperty); }
            set { SetValue(MediaLengthProperty, value); NotifyOfPropertyChange(); }
        }

        public TimeSpan MediaPosition
        {
            get { return (TimeSpan)base.GetValue(MediaPositionProperty); }
            set { SetValue(MediaPositionProperty, value); NotifyOfPropertyChange(); }
        }

        public TimeSpan EndPosition
        {
            get { return (TimeSpan)GetValue(EndPositionProperty); }
            set { SetValue(EndPositionProperty, value); }
        }

        public virtual bool IsNotifying { get; set; }
        private IEventAggregator Events;

        public ExtendedMediaElement()
        {
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
            MediaOpened += MediaOpenedHandler;
            MediaEnded += MediaEndedHandler;
            this.Volume = 1.0;

            mediaTimer = new DispatcherTimer();
            mediaTimer.Interval = TimeSpan.FromMilliseconds(100);
            mediaTimer.Tick += MediaTimerTickHandler;
            mediaTimer.Start();
        }

        private void MediaOpenedHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            Debug.WriteLine("{2} MediaOpenedHandler sender={0} routedEventArgs={1} userMediaPosition={3} IsLoaded={4}", sender, routedEventArgs, DateTime.Now.TimeOfDay, userMediaPosition, IsLoaded);
            // don't set anything if the media isn't loaded (can happen here)
            if (!IsLoaded)
                return;

            if (NaturalDuration.HasTimeSpan)
                MediaLength = NaturalDuration.TimeSpan;

            // if the user tried to set position but the media wasn't opened, set it now
            if (userMediaPosition != null)
            {
                Position = (TimeSpan)userMediaPosition;
                userMediaPosition = null;
            }
            mediaOpened = true;
        }

        private void MediaEndedHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            MediaPosition = TimeSpan.Zero;
        }

        private void MediaPositionChanged(TimeSpan newValue)
        {
            Debug.WriteLine("{2} MediaPositionChanged newValue={0} positionChangedByTimer={1}", newValue, positionChangedByTimer, DateTime.Now.TimeOfDay);
            if (positionChangedByTimer)
            {
                positionChangedByTimer = false;
                return;
            }

            // if the media isn't opened yet, save the position
            if (!mediaOpened)
            {
                userMediaPosition = newValue;
                return;
            }

            Position = newValue;
        }

        private void MediaTimerTickHandler(object sender, EventArgs e)
        {
            if (!IsPlaying)
                return;

            Debug.WriteLine("{2} MediaTimerTickHandler sender={0} Position={1} mediaOpened={3}", sender, Position, DateTime.Now.TimeOfDay, mediaOpened);

            // don't set any positions if the media isn't loaded yet after Play()
            if (!mediaOpened)
                return;

            // this is a workaround for the bug that video playback always starts at 0 for the first rally that gets selected
            if (firstStart)
            {
                Debug.WriteLine("{2} MediaTimerTickHandler firstStart=true Position={0} MediaPosition={1}", Position, MediaPosition, DateTime.Now.TimeOfDay);
                if (Position.TotalMilliseconds != MediaPosition.TotalMilliseconds)
                    Position = MediaPosition;
                firstStart = false;
            }

            positionChangedByTimer = true;
            MediaPosition = Position;

            if (EndPosition != null && EndPosition <= Position && EndPosition != TimeSpan.Zero)
            {
                Execute.OnUIThread(() => Events.PublishOnUIThread(new PlayModeEvent(PlayMode)));
            }                             
        }

        public void PlayWithState()
        {
            IsPlaying = true;
            Play();
        }

        public void PauseWithState()
        {
            IsPlaying = false;
            Pause();
        }

        public void StopWithState()
        {
            IsPlaying = false;
            Stop();
        }

        public void NotifyOfPropertyChange([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (IsNotifying && PropertyChanged != null)
            {
                Execute.OnUIThread(() => OnPropertyChanged(new PropertyChangedEventArgs(propertyName)));
            }
        }

        public void Refresh()
        {
            NotifyOfPropertyChange(string.Empty);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event directly.
        /// </summary>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Handle(DeactivationEvent message)
        {
            mediaTimer.Stop();
            mediaTimer.Tick -= MediaTimerTickHandler;
            Events.Unsubscribe(this);
        }
    }
}
