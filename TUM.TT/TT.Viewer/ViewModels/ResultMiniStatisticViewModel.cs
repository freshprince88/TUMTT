using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using Xceed.Wpf.Toolkit;
using TT.Lib.Events;
using TT.Models.Util.Enums;
using TT.Models;
using TT.Lib.Managers;

namespace TT.Viewer.ViewModels
{
    public class ResultMiniStatisticViewModel : Conductor<IScreen>.Collection.AllActive, IHandle<ResultsChangedEvent>
    {
        #region Properties
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public double PointsPlayer1 { get; set; }
        public double PointsPlayer2 { get; set; }
        public string PointsPlayer1Percent { get; set; }
        public string PointsPlayer2Percent { get; set; }
        public double totalRalliesCount { get; set; }

        #endregion


        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager Manager;

        public ResultMiniStatisticViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            Manager = man;
            Player1 = "Spieler 1";
            Player2 = "Spieler 2";

            
        }
        #region Caliburn Hooks

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
            Player1 = Manager.Match.FirstPlayer.Name.Split(' ')[0];
            Player2 = Manager.Match.SecondPlayer.Name.Split(' ')[0];
        }

        

        

        #endregion

        #region Event Handlers
        public void Handle(ResultsChangedEvent message)
        {
            List<Rally> ralliesSelected = message.Rallies.ToList();
            totalRalliesCount = ralliesSelected.Count();
            PointsPlayer1 = ralliesSelected.Where(r => r.Winner == MatchPlayer.First).Count();
            PointsPlayer2 = ralliesSelected.Where(r => r.Winner == MatchPlayer.Second).Count();
            PointsPlayer1Percent = Math.Round((PointsPlayer1 / totalRalliesCount) * 100,2) + " %";
            PointsPlayer2Percent = Math.Round((PointsPlayer2 / totalRalliesCount) * 100,2) + " %";
            NotifyOfPropertyChange("PointsPlayer1");
            NotifyOfPropertyChange("PointsPlayer2");
            NotifyOfPropertyChange("PointsPlayer1Percent");
            NotifyOfPropertyChange("PointsPlayer2Percent");
            NotifyOfPropertyChange("Player1");
            NotifyOfPropertyChange("Player2");

        }

        #endregion
    }
}
