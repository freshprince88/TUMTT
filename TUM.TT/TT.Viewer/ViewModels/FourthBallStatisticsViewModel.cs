﻿using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TT.Lib.Models;
using TT.Lib.Events;

namespace TT.Viewer.ViewModels
{
    public class FourthBallStatisticsViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<FilterSwitchedEvent>,
        IHandle<FilterSelectionChangedEvent>
    {

        #region Properties
        public BasicFilterStatisticsViewModel BasicFilterStatisticsView { get; set; }
        public List<Rally> SelectedRallies { get; private set; }
        public Playlist ActivePlaylist { get; private set; }


        #endregion

        #region Enums



        #endregion

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public FourthBallStatisticsViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
            SelectedRallies = new List<Rally>();
            ActivePlaylist = new Playlist();
            BasicFilterStatisticsView = new BasicFilterStatisticsViewModel(this.events)
            {
                MinRallyLength = 3,
                LastStroke = false,
                StrokeNumber = 3
            };
        }

        #region View Methods
        public void StatButtonClick(Grid parent, string btnName)
        {
            foreach (ToggleButton btn in parent.FindChildren<ToggleButton>())
            {
                if (btn.Name != btnName)
                    btn.IsChecked = false;
            }
        }

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
            this.ActivateItem(BasicFilterStatisticsView);

        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            this.DeactivateItem(BasicFilterStatisticsView, close);
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
                SelectedRallies = BasicFilterStatisticsView.SelectedRallies.Where(r => Convert.ToInt32(r.Length) > 3).ToList();
                this.events.PublishOnUIThread(new ResultsChangedEvent(SelectedRallies));
            }
        }



        #endregion
    }
}
