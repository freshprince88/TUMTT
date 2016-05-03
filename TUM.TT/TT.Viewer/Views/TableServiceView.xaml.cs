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
using TT.Lib.Managers;
using TT.Models;
using TT.Models.Util.Enums;
using TT.Viewer.ViewModels;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für TableView.xaml
    /// </summary>
    public partial class TableServiceView : UserControl,
        IHandle<TableViewModeChangedEvent>, IHandle<ShowTableNumbersEvent>
    {
        public IEventAggregator Events { get; set; }
        private IMatchManager Manager;
        private List<Rally> rallies { get; set; }

        public TableServiceView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Manager = IoC.Get<IMatchManager>();
            Events.Subscribe(this);
            //this.Loaded += FillButtons;
        }

        public void Handle(TableViewModeChangedEvent message)
        {
            switch (message.Mode)
            {
                case ViewMode.Position.Top:
                    TopField.Visibility = Visibility.Visible;
                    BottomField.Visibility = Visibility.Hidden;
                    break;
                case ViewMode.Position.Bottom:
                    TopField.Visibility = Visibility.Hidden;
                    BottomField.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        public void Handle(ShowTableNumbersEvent message)
        {
            Dictionary<string, int> positionKeys = message.Numbers;
            this.TopLeft_top.Content = positionKeys["TopLeft"];
            this.TopMid_top.Content = positionKeys["TopMid"];
            this.TopRight_top.Content = positionKeys["TopRight"];
            this.MidLeft_top.Content = positionKeys["MidLeft"];
            this.MidMid_top.Content = positionKeys["MidMid"];
            this.MidRight_top.Content = positionKeys["MidRight"];
            this.BotLeft_top.Content = positionKeys["BotLeft"];
            this.BotMid_top.Content = positionKeys["BotMid"];
            this.BotRight_top.Content = positionKeys["BotRight"];

        }
    }
}
