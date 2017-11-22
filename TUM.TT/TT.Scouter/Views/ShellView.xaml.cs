using Caliburn.Micro;
using System.Windows;
using TT.Lib.Events;

namespace TT.Scouter.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : MahApps.Metro.Controls.MetroWindow, IHandle<HideMenuEvent>
    {
        public IEventAggregator Events { get; private set; }
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
    }
}
