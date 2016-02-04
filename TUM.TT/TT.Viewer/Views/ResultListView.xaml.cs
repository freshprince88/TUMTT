using Caliburn.Micro;
using System.Linq;
using System.Windows.Controls;
using TT.Lib.Events;
using TT.Viewer.ViewModels;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ListResultView.xaml
    /// </summary>
    public partial class ResultListView : UserControl,
        IHandle<ResultListControlEvent>
    {

        public IEventAggregator Events { get; private set; }

        public ResultListView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);            
        }


        public void Handle(ResultListControlEvent msg)
        {
            var newSelection = Items.Items.Cast<ResultListItem>().Where(i => i.Rally == msg.SelectedRally).FirstOrDefault();

            if (newSelection != null)
                Items.SelectedItem = newSelection;
        }
    }
}
