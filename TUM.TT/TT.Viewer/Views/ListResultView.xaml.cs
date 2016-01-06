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
using TT.Lib.Util.Enums;
using TT.Viewer.Events;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ListResultView.xaml
    /// </summary>
    public partial class ListResultView : UserControl,
        IHandle<ResultListControlEvent>
    {

        public IEventAggregator Events { get; private set; }

        public ListResultView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }


        public void Handle(ResultListControlEvent msg)
        {
            int selectedIndex = Items.SelectedIndex;

            switch (msg.Control)
            {
                case Media.Control.Previous:
                    selectedIndex = (selectedIndex - 1) < 0 ? Items.Items.Count - 1 : selectedIndex - 1;
                    Items.SelectedIndex = selectedIndex;
                    break;
                case Media.Control.Next:
                    selectedIndex = (selectedIndex + 1) % Items.Items.Count;
                    Items.SelectedIndex = selectedIndex;
                    break;
                default:
                    break;
            }
        }
    }
}
