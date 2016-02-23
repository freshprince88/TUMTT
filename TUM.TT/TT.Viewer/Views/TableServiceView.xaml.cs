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
using TT.Lib.Models;
using TT.Lib.Util.Enums;
using TT.Viewer.ViewModels;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für TableView.xaml
    /// </summary>
    public partial class TableServiceView : UserControl,
        IHandle<TableViewModeChangedEvent>, IHandle<ResultsChangedEvent>
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

        public void Handle(ResultsChangedEvent message)
        {


            var rallies = new LinkedList<Rally>(message.Rallies);

            int topLeft = 0;
            int topMid = 0;
            int topRight = 0;

            int midLeft = 0;
            int midMid = 0;
            int midRight = 0;

            int botLeft = 0;
            int botMid = 0;
            int botRight = 0;

            topLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsTopLeft()).Count();
            topMid = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsTopMid()).Count();
            topRight = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsTopRight()).Count();

            midLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsMidLeft()).Count();
            midMid = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsMidMid()).Count();
            midRight = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsMidRight()).Count();

            botLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsBotLeft()).Count();
            botMid = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsBotMid()).Count();
            botRight = rallies.Where(r => Convert.ToInt32(r.Length) > 0 && r.Schläge[0].IsBotRight()).Count();


            this.TopLeft_bot.Content = topLeft;
            this.TopLeft_top.Content = topLeft;

            this.TopMid_bot.Content = topMid;
            this.TopMid_top.Content = topMid;

            this.TopRight_bot.Content = topRight;
            this.TopRight_top.Content = topRight;

            this.MidLeft_bot.Content = midLeft;
            this.MidLeft_top.Content = midLeft;

            this.MidMid_bot.Content = midMid;
            this.MidMid_top.Content = midMid;

            this.MidRight_bot.Content = midRight;
            this.MidRight_top.Content = midRight;

            this.BotLeft_bot.Content = botLeft;
            this.BotLeft_top.Content = botLeft;

            this.BotMid_bot.Content = botMid;
            this.BotMid_top.Content = botMid;

            this.BotRight_bot.Content = botRight;
            this.BotRight_top.Content = botRight;
        }


        //public void FillButtons(Object sender, RoutedEventArgs e)
        //{
        //    if (Manager.ActivePlaylist != null)
        //    {
        //        rallies = Manager.ActivePlaylist.Rallies.Where(r => Convert.ToInt32(r.Length) > 0).ToList();
        //        int topLeft = rallies.Where(r => r.Schläge[0].IsTopLeft()).Count();
        //        int topMid = rallies.Where(r => r.Schläge[0].IsTopMid()).Count();
        //        int topRight = rallies.Where(r => r.Schläge[0].IsTopRight()).Count();

        //        int midLeft = rallies.Where(r => r.Schläge[0].IsMidLeft()).Count();
        //        int midMid = rallies.Where(r => r.Schläge[0].IsMidMid()).Count();
        //        int midRight = rallies.Where(r => r.Schläge[0].IsMidRight()).Count();

        //        int botLeft = rallies.Where(r => r.Schläge[0].IsBotLeft()).Count();
        //        int botMid = rallies.Where(r => r.Schläge[0].IsBotMid()).Count();
        //        int botRight = rallies.Where(r => r.Schläge[0].IsBotRight()).Count();

        //        this.TopLeft_bot.Content = topLeft;
        //        this.TopLeft_top.Content = topLeft;

        //        this.TopMid_bot.Content = topMid;
        //        this.TopMid_top.Content = topMid;

        //        this.TopRight_bot.Content = topRight;
        //        this.TopRight_top.Content = topRight;

        //        this.MidLeft_bot.Content = midLeft;
        //        this.MidLeft_top.Content = midLeft;

        //        this.MidMid_bot.Content = midMid;
        //        this.MidMid_top.Content = midMid;

        //        this.MidRight_bot.Content = midRight;
        //        this.MidRight_top.Content = midRight;

        //        this.BotLeft_bot.Content = botLeft;
        //        this.BotLeft_top.Content = botLeft;

        //        this.BotMid_bot.Content = botMid;
        //        this.BotMid_top.Content = botMid;

        //        this.BotRight_bot.Content = botRight;
        //        this.BotRight_top.Content = botRight;
        //    }
        //}
    }
}
