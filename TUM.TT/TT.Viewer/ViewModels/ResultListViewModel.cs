﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Lib.Util.Enums;
using TT.Lib.Events;
using TT.Lib.Managers;

namespace TT.Viewer.ViewModels
{
    public class ResultListViewModel : Conductor<ResultListItem>.Collection.AllActive, IResultViewTabItem,
        IHandle<ResultsChangedEvent>
    {
        public Match Match { get; set; }
        private IEventAggregator events;


        public ResultListViewModel(IEventAggregator e)
        {
            this.DisplayName = "Hitlist";
            events = e;
        }

        #region View Methods

        public void ListItemSelected(SelectionChangedEventArgs e)
        {
            ResultListItem item = e.AddedItems.Count > 0 ? (ResultListItem)e.AddedItems[0] : null;

            if (item != null)
            {
                this.events.PublishOnUIThread(new VideoPlayEvent()
                {
                    Current = item.Rally
                });
            }           
        }
        #endregion

        #region Event Handlers

        public void Handle(ResultsChangedEvent message)
        {

            this.Items.Clear();
            foreach (var rally in message.Rallies)
            {
                this.ActivateItem(new ResultListItem(rally));
            }
        }

        #endregion

        #region Caliburn Hooks

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
        }
        #endregion
    }
}