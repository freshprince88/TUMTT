using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Lib.Util.Enums;
using TT.Viewer.Events;

namespace TT.Viewer.ViewModels
{
    public class ListResultViewModel : Conductor<ResultListItem>.Collection.AllActive, IResultViewTabItem,
        IHandle<ResultsChangedEvent>
    {
        private IEventAggregator events;


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
                //this.ActivateItem(new ItemViewModel(score, sets, rally.Server, rally.Winner, rally.Length));
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
