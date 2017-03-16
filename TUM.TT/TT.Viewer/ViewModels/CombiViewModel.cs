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

namespace TT.Viewer.ViewModels
{
    public class CombiViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<BasicFilterSelectionChangedEvent>, ISaveCancel

    {
        #region Properties
        public BasicFilterViewModel BasicFilterView { get; set; }

        private Filter pendingFilter;
        private Filter tempFilter;
        private SaveCancelActionType.ActionType pendingType;

        private Combination FilterCombi;
        public IEnumerable<Filter> FilterList
        {
            get
            {
                return FilterCombi.FilterList;
            }
        }
        private Filter _selectedItem;
        public Filter SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                NotifyOfPropertyChange("SelectedItem");
            }
        }

        private int _newFilterStrokeNumber;
        public string NewFilterStrokeNumber
        {
            get
            {
                return _newFilterStrokeNumber.ToString();
            }
            set
            {
                _newFilterStrokeNumber = int.Parse(value);
            }
        }

        private bool _filterTypeOr;
        public bool FilterTypeOr
        {
            get
            {
                return _filterTypeOr;
            }
            set
            {
                _filterTypeOr = value;
                if (FilterTypeOr)
                    FilterType = FilterCombination.CombinationType.Or;
                else
                    FilterType = FilterCombination.CombinationType.And;

                NotifyOfPropertyChange("FilterTypeOr");
                NotifyOfPropertyChange("FilterTypeText");
                FilterList_CollectionChanged(this, null);
            }
        }

        public FilterCombination.CombinationType FilterType
        {
            get
            {
                return FilterCombi.FilterType;
            }
            private set
            {
                FilterCombi.FilterType = value;
            }
        }

        public string FilterTypeText
        {
            get
            {
                if (FilterTypeOr)
                    return "FilterType: OR";
                else
                    return "FilterType: AND";
            }
        }

        #endregion

        private Conductor<IScreen>.Collection.OneActive parent;

        #region Enums



        #endregion

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IViewManager Manager;

        public CombiViewModel(IEventAggregator eventAggregator, IViewManager man, Conductor<IScreen>.Collection.OneActive parent, Combination filterCombi)
        {
            this.parent = parent;
            this.events = eventAggregator;
            this.Manager = man;
            this.FilterCombi = filterCombi;
            BasicFilterView = new BasicFilterViewModel(this.events, Manager, FilterCombi.BasicFilter);

        }

        #region View Methods

        public void AddFilter()
        {
            if (_newFilterStrokeNumber > 0)
            {
                IScreen filterView;
                pendingFilter = new Filter((_newFilterStrokeNumber - 1), "<Enter Name>");
                if (_newFilterStrokeNumber == 1)
                {
                    filterView = new ServiceViewModel(this.events, Manager, pendingFilter, false);
                }
                else
                {
                    filterView = new BallFilterViewModel(this.events, Manager, pendingFilter, false);
                }

                var saveCancelView = new SaveCancelViewModel(this.events, Manager, this, filterView);

                pendingType = SaveCancelActionType.ActionType.Add;

                parent.ActivateItem(saveCancelView);
            }
            else
            {
                System.Windows.MessageBox.Show("Strokeindex starts with 1 = Service, 2 = Return, 3 = 3rd Stroke, ...", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public void EditFilter()
        {
            if (SelectedItem == null)
                return;

            IScreen filterView;
            pendingFilter = new Filter(SelectedItem);
            tempFilter = SelectedItem; // creating a copy of the current filter

            FilterCombi.RemoveFilter(SelectedItem); // So Filter does not affect current SelectedRallies-List
            if (pendingFilter.StrokeNumber > 0)
                filterView = new BallFilterViewModel(this.events, Manager, pendingFilter, false);
            else
                filterView = new ServiceViewModel(this.events, Manager, pendingFilter, false);

            var saveCancelView = new SaveCancelViewModel(this.events, Manager, this, filterView);

            pendingType = SaveCancelActionType.ActionType.Edit;

            parent.ActivateItem(saveCancelView);
        }

        public void DeleteFilter()
        {
            if (SelectedItem == null)
                return;

            var filterToDelete = SelectedItem;
            FilterCombi.RemoveFilter(SelectedItem);
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
            this.ActivateItem(BasicFilterView);

            UpdateSelection(Manager.ActivePlaylist);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            this.DeactivateItem(BasicFilterView, close);

            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
        }

        #endregion

        #region Event Handlers

        //FilterSelection in BasicFilter Changed
        //Get SelectedRallies and apply own filters
        public void Handle(BasicFilterSelectionChangedEvent message)
        {
            UpdateSelection(Manager.MatchManager.ActivePlaylist);
        }

        private void FilterList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateSelection(Manager.MatchManager.ActivePlaylist);
            NotifyOfPropertyChange("FilterList");
        }


        #endregion

        #region Helper Methods

        private void UpdateSelection(Playlist list)
        {
            if (list.Rallies != null)
            {
                Manager.MatchManager.SelectedRallies = FilterCombi.filter(Manager.MatchManager.ActivePlaylist.Rallies);
            }
        }

        #endregion

        #region Save Cancel And Filters

        public void Save()
        {
            switch (pendingType)
            {
                case SaveCancelActionType.ActionType.Add:
                    SaveNewItem();
                    break;
                case SaveCancelActionType.ActionType.Edit:
                    SaveNewItem();
                    break;
            }

            parent.ActivateItem(this);

            FilterList_CollectionChanged(this, null);
        }

        private void SaveNewItem()
        {
            if (pendingFilter.Name.Equals("<Enter Name>"))
                pendingFilter.Name = "New Filter";

            Manager.Filters.Add(pendingFilter);
            FilterCombi.AddFilter(pendingFilter);
        }

        public void Cancel()
        {
            switch (pendingType)
            {
                case SaveCancelActionType.ActionType.Add:
                    // No need to do anything
                    break;
                case SaveCancelActionType.ActionType.Edit:
                    FilterCombi.AddFilter(tempFilter);
                    break;
            }

            parent.ActivateItem(this);
        }

        #endregion


    }
}
