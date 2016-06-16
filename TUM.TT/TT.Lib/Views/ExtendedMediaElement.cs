using Caliburn.Micro;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.ComponentModel;
using TT.Lib.Events;


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

        readonly DispatcherTimer mediaTimer;
        bool positionChangedByTimer;

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

            mediaTimer = new DispatcherTimer();
            mediaTimer.Interval = TimeSpan.FromMilliseconds(100);
            mediaTimer.Tick += MediaTimerTickHandler;
            mediaTimer.Start();            
        }

        private void MediaOpenedHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            if(NaturalDuration.HasTimeSpan)
             MediaLength = NaturalDuration.TimeSpan;
        }

        private void MediaEndedHandler(object sender, RoutedEventArgs routedEventArgs)
        {
            MediaPosition = TimeSpan.Zero;
        }

        private void MediaPositionChanged(TimeSpan newValue)
        {
            if (positionChangedByTimer)
            {
                positionChangedByTimer = false;
                return;
            }

            Position = newValue;
        }

        private void MediaTimerTickHandler(object sender, EventArgs e)
        {
            if (!IsPlaying)
                return;
                        
            if (EndPosition != null && EndPosition <= Position && EndPosition != TimeSpan.Zero)
            {
                Pause();
                Execute.OnUIThread(() => Events.PublishOnUIThread(new PlayModeEvent(PlayMode)));
            }             

            positionChangedByTimer = true;
            MediaPosition = Position;            
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
