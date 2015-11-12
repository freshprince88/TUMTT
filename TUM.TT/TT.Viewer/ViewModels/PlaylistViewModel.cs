using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Viewer.Events;
using TT.Viewer.Models;

namespace TT.Viewer.ViewModels
{
    public class PlaylistViewModel : Conductor<PlaylistItem>.Collection.AllActive,
        IHandle<MatchOpenedEvent>
    {
        private IEventAggregator events;

        private PlaylistItem _selected;
        public PlaylistItem SelectedItemView
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

        public PlaylistViewModel(IEventAggregator e)
        {
            events = e;
        }

        #region View Methods

        public void ListItemSelected(ListView view)
        {
            PlaylistItem item = (PlaylistItem)view.SelectedItem;
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

        #region Events

        public void Handle(MatchOpenedEvent message)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
