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
    public class ServiceStatisticsViewModel : Conductor<IScreen>.Collection.AllActive
        
    {

        #region Properties
       

        #endregion

        #region Enums

        

        #endregion

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;

        public ServiceStatisticsViewModel(IEventAggregator eventAggregator)
        {
            this.events = eventAggregator;
            
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
        

        #endregion

        #region Helper Methods

       
       

        #endregion
    }
}
