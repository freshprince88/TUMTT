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
using TT.Viewer.ViewModels;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für MediaView.xaml
    /// </summary>
    public partial class MediaView : UserControl , IHandle<MediaViewModel.PlayPause>
    {
        public IEventAggregator Events { get; set; }
        public MediaView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }
        public void Handle(MediaViewModel.PlayPause message)
        {
            switch (message)
            {
                case MediaViewModel.PlayPause.Played:
                    PlayButton.Visibility = Visibility.Visible;
                    PauseButton.Visibility = Visibility.Hidden;
                    break;
                case MediaViewModel.PlayPause.Paused:
                    PlayButton.Visibility = Visibility.Hidden;
                    PauseButton.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

    }
}
