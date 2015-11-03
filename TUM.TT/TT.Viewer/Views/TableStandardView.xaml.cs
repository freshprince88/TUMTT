﻿using Caliburn.Micro;
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
using TT.Viewer.Events;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für TableView.xaml
    /// </summary>
    public partial class TableStandardView : UserControl,
       IHandle<FilterSwitchedEvent>,
       IHandle<RallyLengthChangedEvent>
    {
        #region Properties

        public IEventAggregator Events { get; set; }
        private List<MatchRally> rallies { get; set; }
        #endregion


        public TableStandardView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }
        public void Handle(FilterSwitchedEvent message)
        {
            if (message.Match.Rallies != null)
            {
                rallies = message.Match.Rallies.Where(r => Convert.ToInt32(r.Length) > 0).ToList();
            }
        }
        public void Handle(RallyLengthChangedEvent message)
        {
            switch(message)
            {
                case 1:
                    break;
                case 2:
                    if (rallies != null) { 
                    int topLeft = rallies.Where(r => Convert.ToInt32(r.Length)>1 && r.Schlag[1].IsTopLeft()).Count();
                    int topMid = rallies.Where(r => Convert.ToInt32(r.Length) >1 && r.Schlag[1].IsTopMid()).Count();
                    int topRight = rallies.Where(r => Convert.ToInt32(r.Length) >1 && r.Schlag[1].IsTopRight()).Count();

                    int midLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schlag[1].IsMidLeft()).Count();
                    int midMid = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schlag[1].IsMidMid()).Count();
                    int midRight = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schlag[1].IsMidRight()).Count();

                    int botLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schlag[1].IsBotLeft()).Count();
                    int botMid = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schlag[1].IsBotMid()).Count();
                    int botRight = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schlag[1].IsBotRight()).Count();

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
                    break;
           
                case 3:
                    if (rallies != null)
                    {
                        int topLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schlag[2].IsTopLeft()).Count();
                        int topMid = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schlag[2].IsTopMid()).Count();
                        int topRight = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schlag[2].IsTopRight()).Count();

                        int midLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schlag[2].IsMidLeft()).Count();
                        int midMid = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schlag[2].IsMidMid()).Count();
                        int midRight = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schlag[2].IsMidRight()).Count();

                        int botLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schlag[2].IsBotLeft()).Count();
                        int botMid = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schlag[2].IsBotMid()).Count();
                        int botRight = rallies.Where(r => Convert.ToInt32(r.Length) > 2 && r.Schlag[2].IsBotRight()).Count();

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
                    break;
                case 4:
                    if (rallies != null)
                    {
                        int topLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schlag[3].IsTopLeft()).Count();
                        int topMid = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schlag[3].IsTopMid()).Count();
                        int topRight = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schlag[3].IsTopRight()).Count();

                        int midLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schlag[3].IsMidLeft()).Count();
                        int midMid = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schlag[3].IsMidMid()).Count();
                        int midRight = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schlag[3].IsMidRight()).Count();

                        int botLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schlag[3].IsBotLeft()).Count();
                        int botMid = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schlag[3].IsBotMid()).Count();
                        int botRight = rallies.Where(r => Convert.ToInt32(r.Length) > 3 && r.Schlag[3].IsBotRight()).Count();

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
                        break;
                case 5: //TODO funktioniert noch nicht richtig!!!
                    if (rallies != null)
                    {
                        int topLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schlag[Convert.ToInt32(r.Length) - 1].IsTopLeft()).Count();
                        int topMid = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schlag[Convert.ToInt32(r.Length) - 1].IsTopMid()).Count();
                        int topRight = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schlag[Convert.ToInt32(r.Length) - 1].IsTopRight()).Count();

                        int midLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schlag[Convert.ToInt32(r.Length) - 1].IsMidLeft()).Count();
                        int midMid = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schlag[Convert.ToInt32(r.Length) - 1].IsMidMid()).Count();
                        int midRight = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schlag[Convert.ToInt32(r.Length) - 1].IsMidRight()).Count();

                        int botLeft = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schlag[Convert.ToInt32(r.Length) - 1].IsBotLeft()).Count();
                        int botMid = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schlag[Convert.ToInt32(r.Length) - 1].IsBotMid()).Count();
                        int botRight = rallies.Where(r => Convert.ToInt32(r.Length) > 1 && r.Schlag[Convert.ToInt32(r.Length) - 1].IsBotRight()).Count();

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
                    break;
                default:
                    break;
            }
        }

        //public void Handle(FilterSwitchedEvent message)
        //{

        //    if (message.Match.Rallies != null)
        //    {
        //        rallies = message.Match.Rallies.Where(r => Convert.ToInt32(r.Length) > 0).ToList();
        //        int topLeft = rallies.Where(r => r.Schlag[].IsTopLeft()).Count();
        //        int topMid = rallies.Where(r => r.Schlag[1].IsTopMid()).Count();
        //        int topRight = rallies.Where(r => r.Schlag[1].IsTopRight()).Count();

        //        int midLeft = rallies.Where(r => r.Schlag[1].IsMidLeft()).Count();
        //        int midMid = rallies.Where(r => r.Schlag[1].IsMidMid()).Count();
        //        int midRight = rallies.Where(r => r.Schlag[1].IsMidRight()).Count();

        //        int botLeft = rallies.Where(r => r.Schlag[1].IsBotLeft()).Count();
        //        int botMid = rallies.Where(r => r.Schlag[1].IsBotMid()).Count();
        //        int botRight = rallies.Where(r => r.Schlag[1].IsBotRight()).Count();

        //        this.Btn1_bot.Content = topLeft;
        //        this.Btn1_top.Content = topLeft;

        //        this.Btn2_bot.Content = topMid;
        //        this.Btn2_top.Content = topMid;

        //        this.Btn3_bot.Content = topRight;
        //        this.Btn3_top.Content = topRight;

        //        this.Btn4_bot.Content = midLeft;
        //        this.Btn4_top.Content = midLeft;

        //        this.Btn5_bot.Content = midMid;
        //        this.Btn5_top.Content = midMid;

        //        this.Btn6_bot.Content = midRight;
        //        this.Btn6_top.Content = midRight;

        //        this.Btn7_bot.Content = botLeft;
        //        this.Btn7_top.Content = botLeft;

        //        this.Btn8_bot.Content = botMid;
        //        this.Btn8_top.Content = botMid;

        //        this.Btn9_bot.Content = botRight;
        //        this.Btn9_top.Content = botRight;
        //    }


        //}
    }

}
