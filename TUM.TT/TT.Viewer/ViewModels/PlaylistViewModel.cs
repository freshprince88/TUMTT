using Caliburn.Micro;
using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TT.Models.Events;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using TT.Lib.Managers;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class PlaylistViewModel : Conductor<PlaylistItem>.Collection.AllActive,
        IDropTarget,
        IHandle<PlaylistChangedEvent>
    {
        private IEventAggregator events;
        private IMatchManager Manager;
        private IDialogCoordinator Dialogs;

        public PlaylistViewModel(IEventAggregator e, IMatchManager man, IDialogCoordinator dc)
        {
            events = e;
            Manager = man;
            Dialogs = dc;            
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

        public async void Add()
        {
            var name = await Dialogs.ShowInputAsync(this, "New Playlist", "Please enter a name for the playlist");
            //this.events.PublishOnUIThread(new ShowInputDialogEvent("Please enter a name for the playlist", "New Playlist")); 
            if(name != null && name != string.Empty)
            {
                Playlist p = new Playlist();
                p.Name = name;
                Manager.Match.Playlists.Add(p);
                Manager.MatchModified = true;
                this.ActivateItem(new PlaylistItem()
                {
                    Name = name,
                    Count = p.Rallies.Count(),
                    List = p
                });
            }               
        }

        public void Save()
        {
            Manager.SaveMatch();
        }

        public void ShowSettings()
        {

        }
        public async void DeletePlaylist()
        {
            if (Manager.ActivePlaylist.Name != "Alle" && Manager.ActivePlaylist.Name != "Markiert")

            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Delete",
                    NegativeButtonText = "Cancel",
                    AnimateShow = true,
                    AnimateHide = false
                };

                var result = await Dialogs.ShowMessageAsync(this, "Delete Playlist  '" + Manager.ActivePlaylist.Name + "' ?",
                    "Sure?", MessageDialogStyle.AffirmativeAndNegative, mySettings);

                if (result == MessageDialogResult.Negative)
                {
                }
                else
                {
                    Playlist p = Manager.ActivePlaylist;
                    PlaylistItem p1 = this.Items[0];
                    int i = Manager.Match.Playlists.IndexOf(p);
                    Manager.Match.Playlists.RemoveAt(i);
                    this.Items.RemoveAt(i);
                    this.Items.ElementAt(0);
                    this.Items.Refresh();
                    //Manager.ActivePlaylist = Manager.Match.Playlists[0];  //TODO: Playlist in der ListView auswählen
                    events.PublishOnUIThread(new PlaylistDeletedEvent());
                }
            }
        }

        #endregion

        #region Caliburn Hooks

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);

            LoadPlaylists();
        }

        protected override void OnDeactivate(bool close)
        {          
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
            base.OnDeactivate(close);
        }

        #endregion

        #region Events

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

            if (list != null && !list.Rallies.Contains(sourceItem.Rally))
            {
                list.Rallies.Add(sourceItem.Rally);
                Manager.MatchModified = true;
                targetItem.Count++;
                this.Items.Refresh();
            }            
        }

        public void Handle(PlaylistChangedEvent message)
        {
            var selected = this.Items.Where(p => p.Name == message.List.Name).FirstOrDefault();

            if(selected != null)
            {
                selected.Count = message.List.Rallies.Count;
                this.Items.Refresh();
            }
        }

        #endregion

        #region Helper Methods

        private void LoadPlaylists()
        {
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

        #endregion

    }
}
