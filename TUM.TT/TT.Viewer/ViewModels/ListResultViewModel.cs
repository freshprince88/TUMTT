using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Lib.Util.Enums;
using TT.Lib.Events;

namespace TT.Viewer.ViewModels
{
    public class ListResultViewModel : Conductor<ResultListItem>.Collection.AllActive, IResultViewTabItem,
        IHandle<ResultsChangedEvent>,IHandle<MatchInformationEvent>
    {
        public Match Match { get; set; }
        private IEventAggregator events;
        private bool blockEvents;


        public ListResultViewModel(IEventAggregator e)
        {
            this.DisplayName = "Hitlist";
            events = e;
            blockEvents = true;
        }

        #region View Methods

        public void ListItemSelected(SelectionChangedEventArgs e)
        {
            ResultListItem item = e.AddedItems.Count > 0 ? (ResultListItem)e.AddedItems[0] : null;

            if (item != null && !blockEvents)
            {
                this.events.PublishOnUIThread(new VideoPlayEvent()
                {
                    Current = item.Rally
                });
            }

            blockEvents = false;
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

        public void Handle(MatchInformationEvent message)
        {
            this.Match = message.Match;
        }
        #endregion
    }
}
