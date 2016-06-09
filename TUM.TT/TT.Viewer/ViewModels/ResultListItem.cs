using Caliburn.Micro;
using System;
using TT.Models;
using TT.Lib.Managers;

namespace TT.Viewer.ViewModels
{
    public class ResultListItem : Screen
    {
        public int Number { get; set; }
        public string Score { get; set; }
        public string Sets { get; set; }
        public string Server { get; set; }
        public string Point { get; set; }
        public string Length { get; set; }

        public int RallyStart { get; set; }
        public int RallyEnd { get; set; }

        public Rally Rally { get; set; }
        public IMatchManager Manager { get; set; }

        public string Player1Name { get; set; }
        public string Player2Name { get; set; }

        public ResultListItem(Rally rally)
        {
            Manager = IoC.Get<IMatchManager>();

            Rally = rally;
            Number = rally.Nummer;
            Score= String.Format("{0} : {1}", rally.CurrentRallyScore.First, rally.CurrentRallyScore.Second);
            Sets = String.Format("({0} : {1})", rally.CurrentSetScore.First, rally.CurrentSetScore.Second);

            if (rally.Server == MatchPlayer.First)
            {
                Server = Manager.Match.FirstPlayer.Name.Split(' ')[0]; 
            }
            if (rally.Server == MatchPlayer.Second)
            {
                Server = Manager.Match.SecondPlayer.Name.Split(' ')[0];
            }
            if (rally.Winner == MatchPlayer.First)
            {
                Point = Manager.Match.FirstPlayer.Name.Split(' ')[0];
            }
            if (rally.Winner == MatchPlayer.Second)
            {
                Point = Manager.Match.SecondPlayer.Name.Split(' ')[0];
            }

            Length = rally.Length.ToString();
            RallyStart = Convert.ToInt32(rally.Anfang);
            RallyEnd = Convert.ToInt32(rally.Ende);
        }

        public void DeleteRally(ResultListItem r)
        {            
            Manager.DeleteRally(r.Rally);
        }
    }
}
