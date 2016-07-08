using Caliburn.Micro;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TT.Lib.Managers;
using TT.Models;
using System.Collections.Generic;
using System.Windows;
using TT.Models.Util.Enums;

namespace TT.Scouter.ViewModels
{
    public class SpinRadioViewModel : Screen
    {

        private IMatchManager MatchManager;
        public ServiceDetailViewModel ServiceDetailView { get; private set;}
       
        public SpinRadioViewModel(IMatchManager man, ServiceDetailViewModel sdv)
        {
            MatchManager = man;
            ServiceDetailView = sdv;
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
            //this.events.Subscribe(this);
        }


        /// <summary>
        /// Handles deactivation of this view model.
        /// </summary>
        /// <param name="close">Whether the view model is closed</param>
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            // Unsubscribe ourself to the event bus
            //this.events.Unsubscribe(this);
        }
        #endregion

        #region View Methods

        public void SelectSpin(ToggleButton source)
        {

            ServiceDetailView.SelectSpin(source);
        }

        #endregion


            #region Helper Methods
        public void MutualExclusiveToggleButtonClick(Grid parent, ToggleButton tb)
        {
            foreach (ToggleButton btn in parent.FindChildren<ToggleButton>())
            {
                if (btn.Name != tb.Name)
                    btn.IsChecked = false;
            }

        }
        #endregion

    }
}
