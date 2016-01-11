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
using TT.Lib.Events;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ServiceStatisticsView.xaml
    /// </summary>
    public partial class ServiceStatisticsView : UserControl,
        IHandle<StatisticDetailChangedEvent>
    {
        public IEventAggregator Events { get; set; }

        public ServiceStatisticsView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }

        public void Handle(StatisticDetailChangedEvent message)
        {
            bool isChecked = message.DetailChecked;
            bool percent = message.Percent;

            //TODO: Switch Button text
        }
    }
}
