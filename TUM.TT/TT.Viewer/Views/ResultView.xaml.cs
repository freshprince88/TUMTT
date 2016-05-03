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
using Caliburn.Micro;

using TT.Lib.Events;
using TT.Viewer.ViewModels;
using Itenso.Windows.Controls.ListViewLayout;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ResultView.xaml
    /// </summary>
    public partial class ResultView : UserControl, IHandle<FullscreenEvent>
    {
        public IEventAggregator Events { get; private set; }
        public ResultView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }

        public void Handle(FullscreenEvent message)
        {
            switch (message.Fullscreen)
            {
                case true:                  
                    MiniStatistic.Visibility = Visibility.Collapsed;                    
                    break;
                case false:
                    MiniStatistic.Visibility = Visibility.Visible;                    
                    break;
                default:
                    break;
            }
        }
    }
}
