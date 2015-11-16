﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Viewer.Events;

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
            this.events.PublishOnUIThread(new PlaylistChangedEvent(item.Name));
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
            this.Items.Clear();

            foreach (var playlist in message.Match.Playlists)
            {
                string name = playlist.Name;

                this.ActivateItem(new PlaylistItem()
                {
                    Name = name,
                    Count = playlist.Rallies.Count()
                });
            }
        }

        #endregion
    }
}
