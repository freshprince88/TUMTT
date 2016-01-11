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
using TT.Lib.Util.Enums;
using TT.Viewer.ViewModels;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für TableView.xaml
    /// </summary>
    public partial class TableServiceView : UserControl,
        IHandle<TableViewModeChangedEvent>,
        IHandle<FilterSwitchedEvent>
    {
        public IEventAggregator Events { get; set; }
        private List<Rally> rallies { get; set; }

        public TableServiceView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
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

        public void Handle(FilterSwitchedEvent message)
        {
            if (message.Playlist.Rallies != null)
            {
                rallies = message.Playlist.Rallies.Where(r => Convert.ToInt32(r.Length) > 0).ToList();
                int topLeft = rallies.Where(r => r.Schlag[0].IsTopLeft()).Count();
                int topMid = rallies.Where(r => r.Schlag[0].IsTopMid()).Count();
                int topRight = rallies.Where(r => r.Schlag[0].IsTopRight()).Count();

                int midLeft = rallies.Where(r => r.Schlag[0].IsMidLeft()).Count();
                int midMid = rallies.Where(r => r.Schlag[0].IsMidMid()).Count();
                int midRight = rallies.Where(r => r.Schlag[0].IsMidRight()).Count();

                int botLeft = rallies.Where(r => r.Schlag[0].IsBotLeft()).Count();
                int botMid = rallies.Where(r => r.Schlag[0].IsBotMid()).Count();
                int botRight = rallies.Where(r => r.Schlag[0].IsBotRight()).Count();

                this.Btn1_bot.Content = topLeft;
                this.Btn1_top.Content = topLeft;

                this.Btn2_bot.Content = topMid;
                this.Btn2_top.Content = topMid;

                this.Btn3_bot.Content = topRight;
                this.Btn3_top.Content = topRight;

                this.Btn4_bot.Content = midLeft;
                this.Btn4_top.Content = midLeft;

                this.Btn5_bot.Content = midMid;
                this.Btn5_top.Content = midMid;

                this.Btn6_bot.Content = midRight;
                this.Btn6_top.Content = midRight;

                this.Btn7_bot.Content = botLeft;
                this.Btn7_top.Content = botLeft;

                this.Btn8_bot.Content = botMid;
                this.Btn8_top.Content = botMid;

                this.Btn9_bot.Content = botRight;
                this.Btn9_top.Content = botRight;
            }
        }
    }
}
