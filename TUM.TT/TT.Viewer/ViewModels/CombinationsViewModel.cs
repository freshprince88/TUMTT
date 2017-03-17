using Caliburn.Micro;
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
using TT.Viewer.Views;
using TT.Lib.Views;
using System.Collections.ObjectModel;
using System.IO;
using TT.Models.Serialization;
using TT.Models.Util.Enums;
using TT.Lib.ViewModels;
using System.Windows.Controls;

namespace TT.Viewer.ViewModels
{
    public class CombinationsViewModel : Conductor<IScreen>.Collection.AllActive, ISaveCancel

    {
        #region Properties

        private SaveCancelActionType.ActionType pendingType;
        private Combination pendingCombination;

        public IEnumerable<Combination> SortedCombinationList
        {
            get
            {
                return Manager.Combinations.OrderBy(c => c.Name);
            }
        }

        private CombinationList SelectedCombinations;

        #endregion

        private INavigationViewModel navigationController;

        #region Enums



        #endregion

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IViewManager Manager;

        public CombinationsViewModel(IEventAggregator eventAggregator, IViewManager man, INavigationViewModel navigationController)
        {
            this.navigationController = navigationController;
            this.events = eventAggregator;
            this.Manager = man;
            Manager.Combinations.CollectionChanged += Combinations_CollectionChanged;
            SelectedCombinations = new CombinationList();
        }

        #region View Methods

        public void AddCombination()
        {
            IScreen filterView;
            pendingCombination = new Combination();

            filterView = new CombiViewModel(this.events, Manager, navigationController, pendingCombination);

            var saveCancelView = new SaveCancelViewModel(this.events, Manager, this, filterView);

            pendingType = SaveCancelActionType.ActionType.Add;

            navigationController.ActivateItem(saveCancelView);
        }
        
        public void EditCombination()
        {

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
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
        }


        #endregion

        #region Event Handlers

        private void Combinations_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyOfPropertyChange("SortedCombinationList");
        }


        #endregion

        #region Helper Methods

        public void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Combination c in e.AddedItems)
                SelectedCombinations.Add(c);

            foreach (Combination c in e.RemovedItems)
                SelectedCombinations.Remove(c);

            UpdateSelection();
        }

        private void UpdateSelection()
        {
            if (Manager.ActivePlaylist.Rallies != null)
            {
                Manager.SelectedRallies = SelectedCombinations.filter(Manager.ActivePlaylist.Rallies);
            }
        }

        #endregion

        #region Save Cancel And Filters


        public void Save()
        {
            Manager.Combinations.Add(pendingCombination);
            navigationController.NavigateBack();
        }

        public void Cancel()
        {
            navigationController.NavigateBack();
        }

        #endregion


    }
}
