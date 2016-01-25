using Caliburn.Micro;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TT.Lib.Events;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using TT.Lib.Managers;

namespace TT.Viewer.ViewModels
{
    public class PlaylistViewModel : Conductor<PlaylistItem>.Collection.AllActive,
        IDropTarget,
        IHandle<PlaylistNamedEvent>
    {
        private IEventAggregator events;
        private IMatchManager Manager;

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

        public PlaylistViewModel(IEventAggregator e, IMatchManager man)
        {
            events = e;
            Manager = man;
        }

        #region View Methods

        public void ListItemSelected(SelectionChangedEventArgs e)
        {
            PlaylistItem item = e.AddedItems.Count > 0 ? (PlaylistItem)e.AddedItems[0] : null;

            if (item != null)
            {
                //this.events.PublishOnUIThread(new PlaylistChangedEvent(item.Name));
                Manager.ActivePlaylist = item.List;
            }           
        }

        public void Add()
        {
            this.events.PublishOnUIThread(new ShowInputDialogEvent("Please enter a name for the playlist", "New Playlist"));                
        }

        public void Save()
        {
            Manager.SaveMatch();
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

            this.Items.Clear();

            foreach (var playlist in Manager.Match.Playlists)
            {
                string name = playlist.Name;

                this.ActivateItem(new PlaylistItem()
                {
                    Name = name,
                    Count = playlist.Rallies.Count(),
                    List = playlist
                });
            }
        }

        protected override void OnDeactivate(bool close)
        {          
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
            base.OnDeactivate(close);
        }
        #endregion

        #region Events

        public void Handle(PlaylistNamedEvent message)
        {
            string name = message.Name;

            if (name == string.Empty)
                return;


            Playlist p = new Playlist();
            p.Name = name;
            p.Rallies = new List<Rally>();
            Manager.Match.Playlists.Add(p);
            Manager.MatchModified = true;
            this.ActivateItem(new PlaylistItem()
            {
                Name = name,
                Count = p.Rallies.Count()
            });

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

            Playlist list = Manager.Match.Playlists.Where(p => p.Name == targetItem.Name).FirstOrDefault();

            if (!list.Rallies.Contains(sourceItem.Rally))
            {
                list.Rallies.Add(sourceItem.Rally);
                Manager.MatchModified = true;
                targetItem.Count++;
                this.Items.Refresh();
            }            
        }

        #endregion

    }
}
