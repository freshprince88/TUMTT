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
using TT.Viewer.Views;
using TT.Lib.Views;
using System.Collections.ObjectModel;
using System.IO;
using TT.Models.Serialization;
using TT.Models.Util.Enums;

namespace TT.Viewer.ViewModels
{
    public class CombinationsViewModel : Conductor<IScreen>.Collection.AllActive, ISaveCancel

    {
        #region Properties

        private SaveCancelActionType.ActionType pendingType;
        private Combination pendingCombination;

        private ObservableCollection<Combination> _combinations;
        public ObservableCollection<Combination> Combinations
        {
            get
            {
                return _combinations;
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

        public CombinationsViewModel(IEventAggregator eventAggregator, IViewManager man, Conductor<IScreen>.Collection.OneActive parent)
        {
            this.parent = parent;
            this.events = eventAggregator;
            this.Manager = man;
            _combinations = man.Combinations;
            
        }

        #region View Methods

        public void AddCombination()
        {
            //    IScreen filterView;
            //    pendingCombination = new Combination(this.Manager);


            //    var saveCancleView = new SaveCancleViewModel(this.events, Manager, this, filterView);

            //    pendingType = SaveCancelActionType.ActionType.Add;

            //    parent.ActivateItem(saveCancleView);
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




        #endregion

        #region Helper Methods

        #endregion

        #region Save Cancel And Filters


        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}
