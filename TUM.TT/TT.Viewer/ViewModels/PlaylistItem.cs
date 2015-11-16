using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.ViewModels
{
    public class PlaylistItem : Screen
    {
        public string Name { get; set; }

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

        public string CountStr { get; private set; }

        public PlaylistItem()
        {
            Name = string.Empty;
            Count = 0;
        }

        public PlaylistItem(string name, int count)
        {
            this.Name = name;
            this.Count = count;
        }
    }
}
