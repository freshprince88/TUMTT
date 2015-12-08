using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TT.Lib.Models;
using TT.Viewer.Events;

namespace TT.Viewer.ViewModels
{
    public class ThirdBallStatisticsViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<FilterSwitchedEvent>,
        IHandle<FilterSelectionChangedEvent>
    {

        #region Properties

        public List<Rally> SelectedRallies { get; private set; }
        public Playlist ActivePlaylist { get; private set; }


        #endregion

        #region Enums



        #endregion

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public ThirdBallStatisticsViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
            SelectedRallies = new List<Rally>();
            ActivePlaylist = new Playlist();

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
        public void Handle(FilterSelectionChangedEvent message)
        {
            UpdateSelection();
        }
        public void Handle(FilterSwitchedEvent message)
        {
            this.ActivePlaylist = message.Playlist;

            UpdateSelection();
        }

        #endregion

        #region Helper Methods
        private void UpdateSelection()
        {
            if (this.ActivePlaylist.Rallies != null)
            {
                SelectedRallies = this.ActivePlaylist.Rallies.Where(r => Convert.ToInt32(r.Length) > 2).ToList();
                this.events.PublishOnUIThread(new ResultsChangedEvent(SelectedRallies));
            }
        }



        #endregion
    }
}
