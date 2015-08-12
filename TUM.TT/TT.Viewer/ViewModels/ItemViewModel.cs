using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TT.Viewer.ViewModels
{
    public class ItemViewModel : Screen
    {
        public string Score { get; private set; }
        public string Sets { get; private set; }
        public string Server { get; private set; }
        public string Point { get; private set; }
        public string Length { get; private set; }

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
