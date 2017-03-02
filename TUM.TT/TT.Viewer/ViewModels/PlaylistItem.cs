using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class PlaylistItem : Screen
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                NameStr = value;
            }

        }

        private int _count;       
        public int Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                CountStr = "(" + value + ")";
            }
        }
        private double _playTime;
        public double PlayTime
        {
            get
            {
                return _playTime;
            }
            set
            {
                _playTime = value;
                
                TimeSpan ts = TimeSpan.FromMilliseconds(value);
                PlayTimeStr = "(" + string.Format("{0:D2}:{1:D2}",                        
                        ts.Minutes,
                        ts.Seconds
                        ) + ")";

            }
        }

        public string CountStr { get; private set; }
        public string NameStr { get; private set; }
        public string PlayTimeStr { get; private set; }

        public Playlist List { get; set; }

        public PlaylistItem()
        {
            Name = string.Empty;
            Count = 0;
        }

        public PlaylistItem(string name, int count, double playTime, Playlist l)
        {
            this.Name = name;
            this.Count = count;
            this.PlayTime = playTime;
            this.List = l;
        }
    }
}
