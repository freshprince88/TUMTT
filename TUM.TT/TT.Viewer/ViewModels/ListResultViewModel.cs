﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Viewer.Events;

namespace TT.Viewer.ViewModels
{
    public class ListResultViewModel : Conductor<ResultListItem>.Collection.AllActive, IResultViewTabItem,
        IHandle<ResultsChangedEvent>
    {
        private IEventAggregator events;

        private ResultListItem _selected;
        public ResultListItem SelectedItemView
        {
            get
            {
                return _selected;
            }
            set
            {
                if (_selected == value) return;
                _selected = value;
                NotifyOfPropertyChange("SelectedItemView");
            }
        }

        public ListResultViewModel(IEventAggregator e)
        {
            this.DisplayName = "Hitlist";
            events = e;
        }

        #region View Methods

        public void ListItemSelected(ListView view)
        {
            ResultListItem item = (ResultListItem)view.SelectedItem;

            if (item != null)
            {
                var prevIdx = view.SelectedIndex > 0 ? view.SelectedIndex - 1 : 0;
                var nextIDx = view.SelectedIndex + 1 < Items.Count ? view.SelectedIndex + 1 : view.SelectedIndex;

                ResultListItem prev = prevIdx != view.SelectedIndex ? (ResultListItem)view.Items[prevIdx] : item;
                ResultListItem next = nextIDx != view.SelectedIndex ? (ResultListItem)view.Items[nextIDx] : item;


                this.events.PublishOnUIThread(new VideoPlayEvent()
                {
                    Start = item.RallyStart,
                    End = item.RallyEnd,
                    Next = next.RallyStart,
                    Previous = prev.RallyStart
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
                string score = String.Format("{0} : {1}", rally.CurrentRallyScore.First, rally.CurrentRallyScore.Second);
                string sets = String.Format("({0} : {1})", rally.CurrentSetScore.First, rally.CurrentSetScore.Second);

                //this.ActivateItem(new ItemViewModel(score, sets, rally.Server, rally.Winner, rally.Length));
                this.ActivateItem(new ResultListItem()
                {
                    Score = score,
                    Sets = sets,
                    Server = rally.Server,
                    Point = rally.Winner,
                    Length = rally.Length,
                    RallyStart = Convert.ToInt32(rally.Anfang),
                    RallyEnd = Convert.ToInt32(rally.Ende)
                });
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
