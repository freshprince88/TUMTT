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
    public class CombiViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<FilterSwitchedEvent>,
        IHandle<FilterSelectionChangedEvent>

    {
        #region Properties
        public BasicFilterViewModel BasicFilterView { get; set; }
        public List<MatchRally> SelectedRallies { get; private set; }
        public Match Match { get; private set; } 

        public TableKombiViewModel TableKombi { get; private set; } 

        #endregion
        #region Enums

       

        #endregion

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public CombiViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
            SelectedRallies = new List<MatchRally>();
            Match = new Match();
            BasicFilterView = new BasicFilterViewModel(this.events)
            {
                MinRallyLength = 0,
                PlayerLabel = "Aufschlag:"
            };
                    TableKombi = new TableKombiViewModel(this.events)
            {
                ButtonsVisible = true,
                TopButtonPositions = new List<int>() { 1,2,3 },
                BottomButtonPositions = new List<int>() { 7,8,9 },
            };
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
            this.Match = message.Match;
            if (this.Match.FirstPlayer != null && this.Match.SecondPlayer != null)
            {
                
            }
            UpdateSelection();
        }

        #endregion
        #region Helper Methods

        private void UpdateSelection()
        {
            if (this.Match.Rallies != null)
            {
                SelectedRallies = BasicFilterView.SelectedRallies; 
                // .Where().ToList();
                this.events.PublishOnUIThread(new ResultsChangedEvent(SelectedRallies));
            }
        }
       

        #endregion


    }
}
