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
    /// Interaktionslogik für TableView.xaml
    /// </summary>
    public partial class TableView : UserControl,
        IHandle<TableViewModeChangedEvent>,
        IHandle<ServiceViewLoadedEvent>
    {
        public IEventAggregator Events { get; set; }

        public TableView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }

        public void Handle(TableViewModeChangedEvent message)
        {
            switch (message.Mode)
            {
                case TableServiceViewModel.ViewMode.Top:
                    TopField.Visibility = Visibility.Visible;
                    BottomField.Visibility = Visibility.Hidden;
                    break;
                case TableServiceViewModel.ViewMode.Bottom:
                    TopField.Visibility = Visibility.Hidden;
                    BottomField.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        public void Handle(ServiceViewLoadedEvent message)
        {
            throw new NotImplementedException();
        }
    }
}
