﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Lib.Events;
using TT.Lib.Managers;

namespace TT.Viewer.ViewModels
{
    public class FilterStatisticsViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public FilterViewModel FilterView { get; set; }
        public StatisticsViewModel StatisticsView { get; set; }

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager Manager;

        public FilterStatisticsViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            this.Manager = man;

        }
        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
            FilterView = new FilterViewModel(this.events, Manager);
            StatisticsView = new StatisticsViewModel(this.events);
            // Activate the Filter model
            if (this.ActiveItem == null)
            {
                this.ActivateItem(FilterView);
            }
        }

        public void FilterStatisticsSelected(SelectionChangedEventArgs args)
        {
            TabItem selected = args.AddedItems[0] as TabItem;
            switch (selected.Name)
            { //wie kann ich zwischen den 2 TabControls unterscheiden?

                case "FilterTab":
                    this.ActivateItem(FilterView);                  
                    break;
                case "StatisticsTab":
                    this.ActivateItem(StatisticsView);                  
                    break;
            }
        }
    }
}
