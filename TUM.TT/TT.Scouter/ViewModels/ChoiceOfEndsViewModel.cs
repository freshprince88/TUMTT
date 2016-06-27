using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Managers;
using System.Windows.Controls.Primitives;
using TT.Models;
using TT.Lib.Events;
using TT.Models.Util.Enums;

namespace TT.Scouter.ViewModels
{
    public class ChoiceOfEndsViewModel : Screen
    {

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager Manager;
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public LiveViewModel LiveView { get; private set; }


        public ChoiceOfEndsViewModel(IEventAggregator eventAggregator, IMatchManager man, LiveViewModel live)
        {
            this.events = eventAggregator;
            Manager = man;
            LiveView = live;
            Player1 = "Spieler 1";
            Player2 = "Spieler 2";


        }


        #region Caliburn Hooks

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        /// 

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }
        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
            if (Manager.Match.FirstPlayer.Name != null)
                Player1 = Manager.Match.FirstPlayer.Name.Split(' ')[0];
            if (Manager.Match.SecondPlayer.Name != null)
                Player2 = Manager.Match.SecondPlayer.Name.Split(' ')[0];

        }


        /// <summary>
        /// Handles deactivation of this view model.
        /// </summary>
        /// <param name="close">Whether the view model is closed</param>
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
        }



        #endregion
    }
}
