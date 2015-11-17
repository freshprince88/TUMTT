using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public ResultListItem()
        {
            Rally = null;
            Score = String.Empty;
            Sets = String.Empty;
            Server = String.Empty;
            Point = String.Empty;
            Length = String.Empty;
            RallyStart = 0;
            RallyEnd = 0;
        }

        public ResultListItem(Rally rally)
        {
            Rally = rally;

            string score = String.Format("{0} : {1}", rally.CurrentRallyScore.First, rally.CurrentRallyScore.Second);
            string sets = String.Format("({0} : {1})", rally.CurrentSetScore.First, rally.CurrentSetScore.Second);

            Score = score;
            Sets = sets;
            Server = rally.Server;
            Point = rally.Winner;
            Length = rally.Length;
            RallyStart = Convert.ToInt32(rally.Anfang);
            RallyEnd = Convert.ToInt32(rally.Ende);
        }
    }
}
