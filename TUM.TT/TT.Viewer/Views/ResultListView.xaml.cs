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
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);            
        }


        public void Handle(ResultListControlEvent msg)
        {
            var newSelection = Items.Items.Cast<ResultListItem>().Where(i => i.Rally == msg.SelectedRally).FirstOrDefault();

            if (newSelection != null)
                Items.SelectedItem = newSelection;
        }
        public void Handle(FullscreenEvent message)
        {
            switch (message.Fullscreen)
            {
                case true:
                    //Column1.Header = "P";
                    //Column2.Header = "S";    
                    ProportionalColumn.ApplyWidth(Column1, 0);
                    ProportionalColumn.ApplyWidth(Column2, 0);
                    ProportionalColumn.ApplyWidth(Column3, 0);
                    ProportionalColumn.ApplyWidth(Column4, 0);
                    ProportionalColumn.ApplyWidth(Column5, 0);
                    break;
                case false:
                    //Column1.Header = "Punkte";
                    //Column2.Header = "Sätze";
                    ProportionalColumn.ApplyWidth(Column1, 2);
                    ProportionalColumn.ApplyWidth(Column2, 2);
                    ProportionalColumn.ApplyWidth(Column3, 3);
                    ProportionalColumn.ApplyWidth(Column4, 3);
                    ProportionalColumn.ApplyWidth(Column5, 2);
                    break;
                default:
                    break;
            }
        }
    }
}
