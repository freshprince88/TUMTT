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
    /// Interaktionslogik für ListResultView.xaml
    /// </summary>
    public partial class ResultListView : UserControl,
        IHandle<ResultListControlEvent>,
        IHandle<FullscreenEvent>
    {

        public IEventAggregator Events { get; private set; }

        public ResultListView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var vm = args.NewValue as ResultListViewModel;
            if (vm != null)
            {
                ReduceHitlist(vm.IsFullScreen);
            }
        }

        public void Handle(ResultListControlEvent msg)
        {
            var newSelection = List.Items.Cast<ResultListItem>().Where(i => i.Rally == msg.SelectedRally).FirstOrDefault();

            if (newSelection != null && List.SelectedItem != newSelection)
                List.SelectedItem = newSelection;


        }

        private void ReduceHitlist(bool toggle)
        {
            if (toggle)
            {
                Column1.Header = "S";
                ProportionalColumn.ApplyWidth(Column0, 1);
                ProportionalColumn.ApplyWidth(Column1, 2);
                ProportionalColumn.ApplyWidth(Column2, 0);
                ProportionalColumn.ApplyWidth(Column3, 0);
                ProportionalColumn.ApplyWidth(Column4, 0);
                ProportionalColumn.ApplyWidth(Column5, 0);
            }
            else
            {
                Column1.Header = "Score";
                ProportionalColumn.ApplyWidth(Column0, 1);
                ProportionalColumn.ApplyWidth(Column1, 2);
                ProportionalColumn.ApplyWidth(Column2, 2);
                ProportionalColumn.ApplyWidth(Column3, 3);
                ProportionalColumn.ApplyWidth(Column4, 3);
                ProportionalColumn.ApplyWidth(Column5, 2);
            }
        }

        public void Handle(FullscreenEvent message)
        {
            ReduceHitlist(message.Fullscreen);
        }

        private void Items_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
        private void Items_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
