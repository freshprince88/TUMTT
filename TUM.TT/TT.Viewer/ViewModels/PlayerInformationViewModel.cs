using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Managers;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class PlayerInformationViewModel : Screen
    { private Player player;
        public Player Player 
        {
            get
            {
                return this.player ;
            }

            set
            {
                if (this.player != value)
                {
                    this.player = value;

                    NotifyOfPropertyChange();

                }
    }
}
        public int Number { get; set; }
        public DateTime date { get; set; }
        public IEventAggregator events { get; set; }
        public IMatchManager MatchManager { get; set; }
        public PlayerInformationViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            MatchManager = man;

        }

        public void SetMatchModified()
        {
            

        }


    }
}
