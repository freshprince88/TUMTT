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
using System.Windows.Shapes;
using TT.Lib.Events;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ShellView.xaml
    /// </summary>
    public partial class ShellView : MahApps.Metro.Controls.MetroWindow, IHandle<HideMenuEvent>, IHandle<FullscreenEvent>, IHandle<FullscreenHideAllEvent>
    {
        public IEventAggregator Events { get; private set; }
        WindowState currentStateNonFullscreen { get; set; }
        public ShellView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
            MenuFlyout.IsOpen = true;
            MenuFlyout.IsPinned = true;
        }


        public void Handle(HideMenuEvent message)
        {
            MenuFlyout.IsOpen = false;
            MenuFlyout.IsPinned = false;
        }

        private void ShowMenu(object sender, RoutedEventArgs e)
        {
            MenuFlyout.IsOpen = !MenuFlyout.IsOpen;

        }

        public void Handle(FullscreenEvent message)
        {
            switch (message.Fullscreen)
            {
                case true:
                    currentStateNonFullscreen = this.WindowState;
                    SolidColorBrush bg = new SolidColorBrush();
                    bg.Opacity = 0.5;
                    Background = bg;
                    WindowState = WindowState.Maximized;
                    ShowTitleBar = false;
                    IgnoreTaskbarOnMaximize = true;
                    MenuButton.Visibility = Visibility.Collapsed;


                    // Second Screen!!
                    //var secondaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();

                    //if (secondaryScreen != null)
                    //{
                    //    if (!this.IsLoaded)
                    //        this.WindowStartupLocation = WindowStartupLocation.Manual;

                    //    var workingArea = secondaryScreen.WorkingArea;
                    //    this.Left = workingArea.Left;
                    //    this.Top = workingArea.Top;
                    //    this.Width = workingArea.Width;
                    //    this.Height = workingArea.Height;
                    //    // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                    //    if (this.IsLoaded)
                    //        this.WindowState = WindowState.Maximized;
                    //}



                    //if (System.Windows.Forms.Screen.AllScreens.Count() == 2)
                    //{
                    //    var primaryDisplay = System.Windows.Forms.Screen.AllScreens.ElementAtOrDefault(0);
                    //    var extendedDisplay = System.Windows.Forms.Screen.AllScreens.FirstOrDefault(s => s != primaryDisplay) ?? primaryDisplay;
                    //}


                        break;
                case false:
                    ClearValue(BackgroundProperty);
                    WindowState = currentStateNonFullscreen;
                    ShowTitleBar = true;
                    IgnoreTaskbarOnMaximize = false;
                    MenuButton.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        public void Handle(FullscreenHideAllEvent message)
        {
            switch (message.Hide)
            {
                case true:

                    break;
                case false:

                    break;
                default:
                    break;
            }
        }
    }
}
