using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TT.Models;
using TT.Models.Events;
using TT.Models.Managers;

namespace TT.Viewer.ViewModels
{
    public class LastBallStatisticsViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<BasicFilterSelectionChangedEvent>
    {

        #region Properties

        public List<Rally> SelectedRallies { get; private set; }

        #endregion


        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager Manager;

        public LastBallStatisticsViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            Manager = man;
            SelectedRallies = new List<Rally>();
        }

        #region View Methods


        #endregion

        #region Caliburn Hooks

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
            UpdateSelection(Manager.ActivePlaylist);

        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
        }

        #endregion

        #region Event Handlers

        //FilterSelection in BasicFilter Changed
        //Get SelectedRallies and apply own filters
        public void Handle(BasicFilterSelectionChangedEvent message)
        {
            UpdateSelection(Manager.ActivePlaylist);
        }

        #endregion

        #region Helper Methods
        private void UpdateSelection(Playlist list)
        {
            if (list.Rallies != null)
            {
                SelectedRallies = Manager.ActivePlaylist.Rallies.ToList();
                this.events.PublishOnUIThread(new ResultsChangedEvent(SelectedRallies));
            }
        }



        #endregion
    }
}
