using Caliburn.Micro;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TT.Lib.Events;
using TT.Viewer.ViewModels;
using Itenso.Windows.Controls.ListViewLayout;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ListLargeTableView.xaml
    /// </summary>
    public partial class ResultLargeTableView : UserControl,
        IHandle<ResultListControlEvent>,
        IHandle<FullscreenEvent>
    {

        public IEventAggregator Events { get; private set; }

        public ResultLargeTableView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);            
        }


        public void Handle(ResultListControlEvent msg)
        {
            
        }
        public void Handle(FullscreenEvent message)
        {
            switch (message.Fullscreen)
            {
                case true:
                case false:
                    break;
                default:
                    break;
            }
        }

        private void Items_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void CheckSchlagrichtung_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckSpin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckHand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckPunkt_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
