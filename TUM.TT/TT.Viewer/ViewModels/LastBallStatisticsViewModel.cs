﻿using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TT.Models;
using TT.Lib.Events;
using TT.Lib.Managers;
using System.Windows.Controls;

namespace TT.Viewer.ViewModels
{
    public class LastBallStatisticsViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<BasicFilterSelectionChangedEvent>
    {

        #region Properties
        public BasicFilterStatisticsViewModel BasicFilterStatisticsView { get; set; }
        public string X { get; private set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
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
            Player1 = "Player 1";
            Player2 = "Player 2";
            BasicFilterStatisticsView = new BasicFilterStatisticsViewModel(this.events, Manager)
            {
                MinRallyLength = 1,
                LastStroke = true,
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

        public void SelectBasisInformation(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                X = source.Name;

            }
            else
            {
                X = "";
            }

            UpdateSelection(Manager.ActivePlaylist);

        }

        public void SelectPlacement(ToggleButton source)

        {

            if (source.IsChecked.Value)
            {
                X = source.Name;

            }
            else
            {
                X = "";
            }

            UpdateSelection(Manager.ActivePlaylist);
        }
        public void SelectContactPosition(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                X = source.Name;

            }
            else
            {
                X = "";
            }

            UpdateSelection(Manager.ActivePlaylist);

        }
        public void SelectTechnique(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                X = source.Name;

            }
            else
            {
                X = "";
            }

            UpdateSelection(Manager.ActivePlaylist);
        }
        public void SelectStepAround(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                X = source.Name;

            }
            else
            {
                X = "";
            }

            UpdateSelection(Manager.ActivePlaylist);
        }


        #endregion

        #region Caliburn Hooks

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
            this.ActivateItem(BasicFilterStatisticsView);
            UpdateSelection(Manager.ActivePlaylist);
            Player1 = Manager.Match.FirstPlayer.Name.Split(' ')[0];
            Player2 = Manager.Match.SecondPlayer.Name.Split(' ')[0];
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
                var results = BasicFilterStatisticsView.SelectedRallies.Where(r => (Convert.ToInt32(r.Length) >2 || Convert.ToInt32(r.Length) == 2 && r.Winner != r.Server) &&
                r.HasPlacementStatistics((r.LastWinnerStroke().Number)-1, X) &&
                r.HasBasisInformationStatistics(r.LastWinnerStroke().Number, X) &&
                r.HasContactPositionStatistics((r.LastWinnerStroke().Number) - 1, X) &&
                r.HasTechniqueStatistics((r.LastWinnerStroke().Number) - 1, X) &&
                r.HasStepAroundStatistics((r.LastWinnerStroke().Number) - 1, X)).ToList();
                Manager.SelectedRallies = results;
            }
        }



        #endregion
    }
}
