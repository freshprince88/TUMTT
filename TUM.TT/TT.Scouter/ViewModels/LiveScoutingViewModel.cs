using System.Linq;
using Caliburn.Micro;
using TT.Lib.Managers;
using TT.Models;
using System.Collections.ObjectModel;
using TT.Lib.Interfaces;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;
using TT.Lib.Util;
using TT.Lib.Results;
using System.Collections.Generic;

namespace TT.Scouter.ViewModels
{
    public class LiveScoutingViewModel : Screen
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;
       
        public LiveViewModel LiveView { get; private set; }

        public LiveScoutingViewModel (IEventAggregator ev, IMatchManager man, IMediaPosition mp, LiveViewModel live)
        {
            Events = ev;
            MatchManager = man;
            LiveView = live;
            

        }


        #region View Methods
        public void SetRallyLength(int length)
        {
            LiveView.SetRallyLength(length);

        }

        public void RallyWon(int player, int length)
        {
            LiveView.RallyWon(player, length);
        }

        public void StartRally()
        {
            LiveView.StartRally();
        }
        #endregion
    }
}
