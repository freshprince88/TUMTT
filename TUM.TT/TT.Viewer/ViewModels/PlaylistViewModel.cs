using Caliburn.Micro;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TT.Viewer.Events;

namespace TT.Viewer.ViewModels
{
    public class PlaylistViewModel : Conductor<PlaylistItem>.Collection.AllActive,
        IDropTarget,
        IHandle<MatchOpenedEvent>
    {
        private IEventAggregator events;

        public Match Match { get; private set; }

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

        public void Add()
        {

        }

        public void Save()
        {
            this.events.PublishOnUIThread(new SaveMatchEvent(this.Match));
        }

        public void ShowSettings()
        {

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
            this.Match = message.Match;
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

        public void DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as ResultListItem;
            var targetItem = dropInfo.TargetItem as PlaylistItem;

            if (sourceItem != null && targetItem != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as ResultListItem;
            var targetItem = dropInfo.TargetItem as PlaylistItem;

            Playlist list = this.Match.Playlists.Where(p => p.Name == targetItem.Name).FirstOrDefault();

            if(!list.Rallies.Contains(sourceItem.Rally))
                list.Rallies.Add(sourceItem.Rally);

            this.events.PublishOnUIThread(new PlaylistEditedEvent());
        }

        #endregion

    }
}
