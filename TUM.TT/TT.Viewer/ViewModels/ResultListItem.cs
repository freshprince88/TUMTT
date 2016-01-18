using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TT.Lib.Models;
using TT.Lib.Events;
using TT.Lib.Managers;

namespace TT.Viewer.ViewModels
{
    public class ResultListItem : Screen
    {
        public string Score { get; set; }
        public string Sets { get; set; }
        public string Server { get; set; }
        public string Point { get; set; }
        public string Length { get; set; }

        public int RallyStart { get; set; }
        public int RallyEnd { get; set; }

        public Rally Rally { get; set; }
        public IEventAggregator Events { get; set; }
        public IMatchManager Manager { get; set; }

        public string Player1Name { get; set; }
        public string Player2Name { get; set; }


        public ResultListItem()
        {
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
            Manager = IoC.Get<IMatchManager>();
            Rally = null;
            Score = String.Empty;
            Sets = String.Empty;
            Server = String.Empty;
            Point = String.Empty;
            Length = String.Empty;
            RallyStart = 0;
            RallyEnd = 0;

        }

        public ResultListItem(Rally rally) //TODO: Wenn Server = First -> Name von Player 1 usw.

        {
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
            string test = this.Player1Name;

            Rally = rally;

            string score = String.Format("{0} : {1}", rally.CurrentRallyScore.First, rally.CurrentRallyScore.Second);
            string sets = String.Format("({0} : {1})", rally.CurrentSetScore.First, rally.CurrentSetScore.Second);

            Score = score;
            Sets = sets;
            if (test != null)
            {
                if (rally.Server == "First")
                {
                    Server = Manager.Match.FirstPlayer.Name.Split(' ')[0]; // ist immer null...aber ka wieso
                }
                if (rally.Server == "Second")
                {
                    Server = Manager.Match.SecondPlayer.Name.Split(' ')[0]; // ist immer null...aber ka wieso
                }
            }

            Server = rally.Server;
            Point = rally.Winner;
            Length = rally.Length;
            RallyStart = Convert.ToInt32(rally.Anfang);
            RallyEnd = Convert.ToInt32(rally.Ende);
        }
    }
}
