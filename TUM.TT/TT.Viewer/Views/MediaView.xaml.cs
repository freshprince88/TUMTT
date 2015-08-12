using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TT.Viewer.Events;
using TT.Viewer.ViewModels;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für MediaView.xaml
    /// </summary>
    public partial class MediaView : UserControl, 
        IHandle<MediaViewModel.PlayPause>, 
        IHandle<MediaViewModel.PlaySpeed>, 
        IHandle<MediaViewModel.MuteUnmute>,
        IHandle<VideoLoadedEvent>
    {
        public IEventAggregator Events { get; set; }
        public MediaView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }
        public void Handle(MediaViewModel.MuteUnmute message)
        {
            switch (message)
            {
                case MediaViewModel.MuteUnmute.Mute:
                    MuteButton.Visibility = Visibility.Hidden;
                    UnmuteButton.Visibility = Visibility.Visible;
                    UnmuteButton.IsChecked = true;
                    break;
                case MediaViewModel.MuteUnmute.Unmute:
                    MuteButton.Visibility = Visibility.Visible;
                    UnmuteButton.Visibility = Visibility.Hidden;
                    UnmuteButton.IsChecked = false;
                    break;
                default:
                    break;
            }
        }

        public void Handle(MediaViewModel.PlayPause message)
        {
            switch (message)
            {
                case MediaViewModel.PlayPause.Play:
                    PlayButton.Visibility = Visibility.Hidden;
                    PauseButton.Visibility = Visibility.Visible;
                    PauseButton.IsChecked = true;
                    break;
                case MediaViewModel.PlayPause.Pause:
                    PlayButton.Visibility = Visibility.Visible;
                    PauseButton.Visibility = Visibility.Hidden;
                    PauseButton.IsChecked = false;
                    break;
                default:
                    break;
            }
        }

        public void Handle(MediaViewModel.PlaySpeed message)
        {
            switch (message)
            {
                case MediaViewModel.PlaySpeed.Quarter:
                    myMediaElement.SpeedRatio = 0.25;
                    Slow50PercentButton.IsChecked = false;
                    Slow75PercentButton.IsChecked = false;
                    break;
                case MediaViewModel.PlaySpeed.Half:
                    myMediaElement.SpeedRatio = 0.50;
                    Slow25PercentButton.IsChecked = false;
                    Slow75PercentButton.IsChecked = false;
                    break;
                case MediaViewModel.PlaySpeed.Third:
                    myMediaElement.SpeedRatio = 0.75;
                    Slow25PercentButton.IsChecked = false;
                    Slow50PercentButton.IsChecked = false;
                    break;
                case MediaViewModel.PlaySpeed.Full:
                    myMediaElement.SpeedRatio = 1;
                    Slow25PercentButton.IsChecked = false;
                    Slow50PercentButton.IsChecked = false;
                    Slow75PercentButton.IsChecked = false;
                    break;
                default:
                    break;
            }
        }


        public void Handle(VideoLoadedEvent message)
        {
            this.myMediaElement.Source = new Uri(message.VideoFile);
        }
    }
}
