using Caliburn.Micro;
using System.Linq;
using System.Windows.Controls;
using TT.Models.Events;
using TT.Models;

namespace TT.Scouter.Views
{
    /// <summary>
    /// Interaction logic for RemoteView.xaml
    /// </summary>
    public partial class RemoteView : UserControl,
        IHandle<ResultListControlEvent>
    {
        public IEventAggregator Events { get; private set; }

        public RemoteView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }


        public void Handle(ResultListControlEvent msg)
        {
            var newSelection = Items.Items.Cast<Rally>().Where(i => i.Equals(msg.SelectedRally)).FirstOrDefault();

            if (newSelection != null && Items.SelectedItem != newSelection)
                Items.SelectedItem = newSelection;
            else
            {
                if (newSelection != null)
                {
                    Events.PublishOnUIThread(new VideoPlayEvent()
                    {
                        Current = newSelection
                    });
                }
            }

        }
    }
}
