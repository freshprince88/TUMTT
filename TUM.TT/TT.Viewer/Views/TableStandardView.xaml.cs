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
using TT.Viewer.ViewModels;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für TableView.xaml
    /// </summary>
    public partial class TableStandardView : UserControl,
       IHandle<RallyLengthChangedEvent>,IHandle<ResultsChangedEvent>
    {
        #region Properties

        public IEventAggregator Events { get; set; }
        private IMatchManager Manager;
        int strokeNumber;
        #endregion


        public TableStandardView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Manager = IoC.Get<IMatchManager>();
            Events.Subscribe(this);
            strokeNumber = 0;
        }

        public void Handle(RallyLengthChangedEvent message)
        {
            
            strokeNumber = message;


        }
        public void Handle (ResultsChangedEvent message) {


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


            switch (strokeNumber) //Statistik-Filter-Wechsel: strokenumber=0; -> Problem
            {
                case 1:
                    break;
                case 2:

                    
                    if (rallies != null)
                    {
                        
                        topLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schläge[1].IsTopLeft()).Count();
                        topMid = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schläge[1].IsTopMid()).Count();
                        topRight = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schläge[1].IsTopRight()).Count();

                        midLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schläge[1].IsMidLeft()).Count();
                        midMid = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schläge[1].IsMidMid()).Count();
                        midRight = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schläge[1].IsMidRight()).Count();

                        botLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schläge[1].IsBotLeft()).Count();
                        botMid = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schläge[1].IsBotMid()).Count();
                        botRight = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schläge[1].IsBotRight()).Count();
                    }

                    break;

                case 3:
                    if (rallies != null)
                    {
                        topLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schläge[2].IsTopLeft()).Count();
                        topMid = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schläge[2].IsTopMid()).Count();
                        topRight = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schläge[2].IsTopRight()).Count();

                        midLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schläge[2].IsMidLeft()).Count();
                        midMid = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schläge[2].IsMidMid()).Count();
                        midRight = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schläge[2].IsMidRight()).Count();

                        botLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schläge[2].IsBotLeft()).Count();
                        botMid = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schläge[2].IsBotMid()).Count();
                        botRight = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schläge[2].IsBotRight()).Count();
                    }
                    break;
                case 4:
                    if (rallies != null)
                    {
                        topLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schläge[3].IsTopLeft()).Count();
                        topMid = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schläge[3].IsTopMid()).Count();
                        topRight = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schläge[3].IsTopRight()).Count();

                        midLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schläge[3].IsMidLeft()).Count();
                        midMid = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schläge[3].IsMidMid()).Count();
                        midRight = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schläge[3].IsMidRight()).Count();

                        botLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schläge[3].IsBotLeft()).Count();
                        botMid = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schläge[3].IsBotMid()).Count();
                        botRight = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schläge[3].IsBotRight()).Count();
                    }
                    break;
                case 5:
                    if (rallies != null)
                    {
                        topLeft = rallies.Where(r => (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler == r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 1].IsTopLeft())||
                        (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler != r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 2].IsTopLeft())).Count();
                        topMid = rallies.Where(r => (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler == r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 1].IsTopMid())||
                        (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler != r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 2].IsTopMid())).Count();
                        topRight = rallies.Where(r => (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler == r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 1].IsTopRight()) ||
                        (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler != r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 2].IsTopRight())).Count();

                        midLeft = rallies.Where(r => (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler == r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 1].IsMidLeft()) ||
                        (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler != r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 2].IsMidLeft())).Count();
                        midMid = rallies.Where(r => (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler == r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 1].IsMidMid()) ||
                        (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler != r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 2].IsMidMid())).Count();
                        midRight = rallies.Where(r => (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler == r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 1].IsMidRight()) ||
                        (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler != r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 2].IsMidRight())).Count();

                        botLeft = rallies.Where(r => (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler == r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 1].IsBotLeft()) ||
                         (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler != r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 2].IsBotLeft())).Count();
                        botMid = rallies.Where(r => (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler == r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 1].IsBotMid()) ||
                        (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler != r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 2].IsBotMid())).Count();
                        botRight = rallies.Where(r => (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler == r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 1].IsBotRight()) ||
                        (r.Schläge[Convert.ToInt32(r.Length) - 1].Spieler != r.Winner && Convert.ToInt32(r.Length) > 1 && r.Schläge[Convert.ToInt32(r.Length) - 2].IsBotRight())).Count();
                    }
                    break;
                default:
                    break;
            }

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

        #region HelperMethods
        
        #endregion

    }

}
