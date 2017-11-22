using Caliburn.Micro;
using System.Linq;
using System.Windows.Controls;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Models;
using TT.Lib.Views;


namespace TT.Scouter.Views
{
    /// <summary>
    /// Interaction logic for RemoteView.xaml
    /// </summary>
    public partial class RemoteView : ControlWithBindableKeyGestures
    {
        public IEventAggregator Events { get; private set; }
        public IMatchManager Manager { get; private set; }

        public RemoteView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
            Manager = IoC.Get<IMatchManager>();
        }
    }
}
