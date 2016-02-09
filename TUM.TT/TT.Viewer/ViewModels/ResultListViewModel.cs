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
using TT.Lib.Managers;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;

namespace TT.Viewer.ViewModels
{
    public class ResultListViewModel : Conductor<ResultListItem>.Collection.AllActive, IResultViewTabItem,
        IHandle<ResultsChangedEvent>
    {
        private IEventAggregator events;
        private IDialogCoordinator dialogs;
        private IMatchManager manager;


        public ResultListViewModel(IEventAggregator e, IDialogCoordinator c, IMatchManager man)
        {
            this.DisplayName = "Hitlist";
            events = e;
            dialogs = c;
            manager = man;
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

        public void RightMouseDown(MouseButtonEventArgs e)
        {
            //e.Handled = true;
        }

        #endregion

        #region Event Handlers

        public void Handle(ResultsChangedEvent message)
        {

            for (int i = Items.Count - 1; i >= 0; i--)
            {
                this.DeactivateItem(Items[i], true);
            }

            foreach (var rally in message.Rallies)
            {
                this.ActivateItem(new ResultListItem(rally));
            }
            this.Items.Refresh();
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
