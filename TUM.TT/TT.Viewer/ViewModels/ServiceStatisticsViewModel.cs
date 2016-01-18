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
using TT.Lib.Managers;

namespace TT.Viewer.ViewModels
{
    public class ServiceStatisticsViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<FilterSelectionChangedEvent>
    {

        #region Properties

        public BasicFilterStatisticsViewModel BasicFilterStatisticsView { get; set; }     
        public List<Rally> SelectedRallies { get; private set; }
        #endregion


        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager Manager;

        public ServiceStatisticsViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            Manager = man;
            SelectedRallies = new List<Rally>();
           
            BasicFilterStatisticsView = new BasicFilterStatisticsViewModel(this.events, Manager)
            {
                MinRallyLength = 0,
                LastStroke = false,
                StrokeNumber = 0

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

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
            this.ActivateItem(BasicFilterStatisticsView);            
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            UpdateSelection(Manager.ActivePlaylist);
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
            UpdateSelection(Manager.ActivePlaylist);
        }

        #endregion

        #region Helper Methods
        private void UpdateSelection(Playlist list)
        {
            if (list.Rallies != null)
            {
                SelectedRallies = BasicFilterStatisticsView.SelectedRallies;
                this.events.PublishOnUIThread(new ResultsChangedEvent(SelectedRallies));
            }
        }



        #endregion
    }
}
