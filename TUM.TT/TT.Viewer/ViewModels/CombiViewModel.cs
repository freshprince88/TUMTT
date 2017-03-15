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
        IHandle<BasicFilterSelectionChangedEvent>, ISaveCancle

    {
        #region Properties
        public BasicFilterViewModel BasicFilterView { get; set; }
        public TableKombiViewModel TableKombi { get; private set; }

        private Filter pendingFilter;
        private SaveCancleActionType.ActionType pendingType;

        private FilterList FilterList;
        public List<String> FilterNames
        {
            get
            {
                return FilterList.Select(f => f.Name).ToList<String>();
            }
        }
        private int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                NotifyOfPropertyChange("SelectedIndex");
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

        #endregion

        private Conductor<IScreen>.Collection.OneActive parent;

        #region Enums



        #endregion

        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager Manager;

        public CombiViewModel(IEventAggregator eventAggregator, IMatchManager man, Conductor<IScreen>.Collection.OneActive parent)
        {
            this.parent = parent;
            this.events = eventAggregator;
            this.Manager = man;
            BasicFilterView = new BasicFilterViewModel(this.events, Manager)
            {
                MinRallyLength= 0,
                PlayerLabel = "Aufschlag:"
            };

            TableKombi = new TableKombiViewModel(this.events)
            {
                ButtonsVisible = true,
                TopButtonPositions = new List<int>() { 1,2,3 },
                BottomButtonPositions = new List<int>() { 7,8,9 },
            };

            FilterList = new FilterList(LoadFilter());
            FilterList.CollectionChanged += FilterList_CollectionChanged;
            _selectedIndex = -1;
        }

        #region View Methods

        public void AddFilter()
        {
            pendingFilter = new Filter(_newFilterStrokeNumber, "<Enter Name>");
            var ballFilterView = new BallFilterViewModel(this.events, Manager, pendingFilter, false);
            var saveCancleView = new SaveCancleViewModel(this.events, Manager, this, ballFilterView);

            pendingType = SaveCancleActionType.ActionType.Add;

            parent.ActivateItem(saveCancleView);
        }

        public void EditFilter()
        {
            if (SelectedIndex < 0)
                return;

            var filterToEdit = FilterList[SelectedIndex];
            pendingFilter = new Filter(filterToEdit); // creating a copy of the current filter

            FilterList.DisableFilter(filterToEdit); // So Filter does not affect current SelectedRallies-List
            var ballFilterView = new BallFilterViewModel(this.events, Manager, filterToEdit, false);
            FilterList.ToogleFilter(filterToEdit, pendingFilter.Enabled); // back to normal
            var saveCancleView = new SaveCancleViewModel(this.events, Manager, this, ballFilterView);

            pendingType = SaveCancleActionType.ActionType.Edit;

            parent.ActivateItem(saveCancleView);
        }

        public void DeleteFilter()
        {
            if (SelectedIndex < 0)
                return;

            var filterToDelete = FilterList[SelectedIndex];
            DeleteFilter(filterToDelete);
            FilterList.RemoveAt(SelectedIndex);
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
            UpdateSelection(Manager.ActivePlaylist);
        }

        private void FilterList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateSelection(Manager.ActivePlaylist);
            NotifyOfPropertyChange("FilterNames");
        }


        #endregion

        #region Helper Methods

        private void UpdateSelection(Playlist list)
        {
            if (list.Rallies != null)
            {
                Manager.SelectedRallies = FilterList.filter(FilterCombination.CombinationType.Or, BasicFilterView.SelectedRallies);
            }
        }

        #endregion

        #region Save Cancle And Filters

        public void Save()
        {
            switch (pendingType)
            {
                case SaveCancleActionType.ActionType.Add:
                    SaveNewItem();
                    break;
                case SaveCancleActionType.ActionType.Edit:
                    SaveFilter(FilterList[SelectedIndex]);
                    break;
            }

            parent.ActivateItem(this);

            FilterList_CollectionChanged(this, null);
        }

        private void SaveNewItem()
        {
            if (pendingFilter.Name.Equals("<Enter Name>"))
                pendingFilter.Name = "New Filter";

            FilterList.Add(pendingFilter);
            SaveFilter(pendingFilter);
        }

        public void Cancle()
        {
            switch (pendingType)
            {
                case SaveCancleActionType.ActionType.Add:
                    // No need to do anything
                    break;
                case SaveCancleActionType.ActionType.Edit:
                    FilterList[SelectedIndex] = pendingFilter;
                    break;
            }

            parent.ActivateItem(this);
        }

        private IEnumerable<Filter> LoadFilter()
        {
            var tempFilterList = new List<Filter>();
            var serializer = new XmlFilterSerializer();

            DirectoryInfo d = new DirectoryInfo(Filter.FILTER_PATH);
            if (d.Exists)
            {
                FileInfo[] Files = d.GetFiles("*.flt");
                foreach (FileInfo file in Files)
                {
                    var stream = file.OpenRead();
                    tempFilterList.Add(serializer.Deserialize(stream));
                    stream.Close();
                }
            }
            return tempFilterList;
        }

        private void SaveFilter(Filter filter)
        {
            var serializer = new XmlFilterSerializer();

            DirectoryInfo d = new DirectoryInfo(Filter.FILTER_PATH);
            if (!d.Exists)
                d.Create();

            try
            {
                var filterStream = File.Create(d.FullName + "\\" + filter.ID + ".flt");
                serializer.Serialize(filterStream, filter);
                filterStream.Close();
            } catch (Exception e)
            {
                var message = String.Concat("Couldn't save Filter: ", e.Message);
                System.Windows.MessageBox.Show(message, "Error while trying to save Filter", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void DeleteFilter(Filter filter)
        {
            DirectoryInfo d = new DirectoryInfo(Filter.FILTER_PATH);
            if (d.Exists)
            {
                try
                {
                File.Delete(d.FullName + "\\" + filter.ID + ".flt");
                } catch { }
            }
        }


        #endregion


    }
}
