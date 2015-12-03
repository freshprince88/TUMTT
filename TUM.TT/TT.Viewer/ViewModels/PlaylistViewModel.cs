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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace TT.Viewer.ViewModels
{
    public class PlaylistViewModel : Conductor<PlaylistItem>.Collection.AllActive,
        IDropTarget,
        IHandle<MatchOpenedEvent>,
        IHandle<PlaylistNamedEvent>
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
            this.events.PublishOnUIThread(new ShowInputDialogEvent("Please enter a name for the playlist", "New Playlist"));                
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

        protected override void OnDeactivate(bool close)
        {          
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
            base.OnDeactivate(close);
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

        public void Handle(PlaylistNamedEvent message)
        {
            string name = message.Name;

            if (name == string.Empty)
                return;


            Playlist p = new Playlist();
            p.Name = name;
            p.Rallies = new List<Rally>();
            this.Match.Playlists.Add(p);

            this.ActivateItem(new PlaylistItem()
            {
                Name = name,
                Count = p.Rallies.Count()
            });

            this.events.PublishOnUIThread(new MatchEditedEvent(this.Match));
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

            if (!list.Rallies.Contains(sourceItem.Rally))
            {
                list.Rallies.Add(sourceItem.Rally);
                targetItem.Count++;
                this.Items.Refresh();
            }

            this.events.PublishOnUIThread(new MatchEditedEvent(this.Match));
        }

        #endregion

    }
}
