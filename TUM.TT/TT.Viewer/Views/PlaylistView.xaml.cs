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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaction logic for PlaylistView.xaml
    /// </summary>
    public partial class PlaylistView : UserControl,
        IHandle<ShowInputDialogEvent>,
        IHandle<ShowPlaylistSettingsEvent>
    {
        public IEventAggregator Events { get; private set; }

        public PlaylistView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }


        #region Events

        public async void Handle(ShowInputDialogEvent message)
        {
            var metroWindow = System.Windows.Application.Current.Windows.OfType<ShellView>().FirstOrDefault();
            var result = await metroWindow.ShowInputAsync(message.Header, message.Content);

            //user pressed cancel
            if (result == null)
            {
                this.Events.PublishOnUIThread(new PlaylistNamedEvent(string.Empty));
            }
            else
            {
                this.Events.PublishOnUIThread(new PlaylistNamedEvent(result));
            }                
        }

        public void Handle(ShowPlaylistSettingsEvent message)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
