using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TT.Viewer.ViewModels
{
    public class ItemViewModel : Screen
    {
        public string Score { get; set; }
        public string Sets { get; set; }
        public string Server { get; set; }
        public string Point { get; set; }
        public string Length { get; set; }

        public double RallyStart { get; set; }
        public double RallyEnd { get; set; }

        public ItemViewModel()
        {
            Score = String.Empty;
            Sets = String.Empty;
            Server = String.Empty;
            Point = String.Empty;
            Length = String.Empty;
            RallyStart = 0;
            RallyEnd = 0;
        }

        public ItemViewModel(string score, string sets, string server, string point, string length)
        {
            Score = score;
            Sets = sets;
            Server = server;
            Point = point;
            Length = length;
        }
    }
}
