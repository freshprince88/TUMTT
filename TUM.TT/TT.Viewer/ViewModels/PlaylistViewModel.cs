using Caliburn.Micro;
using GongSolutions.Wpf.DragDrop;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TT.Lib.Events;
using TT.Lib.Results;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using TT.Lib.Managers;
using TT.Models;
using NReco.VideoConverter;
using TT.Lib.Util;
using TT.Lib;

namespace TT.Viewer.ViewModels
{
    public class PlaylistViewModel : Conductor<PlaylistItem>.Collection.AllActive,
        IDropTarget,
        IHandle<PlaylistChangedEvent>
    {
        private IEventAggregator events;
        private IMatchManager MatchManager { get; set; }
        private IDialogCoordinator Dialogs;
        public bool singleRalliesBool { get; set; }
        public bool rallyCollectionBool { get; set; }
        

        public PlaylistViewModel(IEventAggregator e, IMatchManager man, IDialogCoordinator dc)
        {
            events = e;
            MatchManager = man;
            Dialogs = dc;
            singleRalliesBool = false;
            rallyCollectionBool = true;    
        }

        #region View Methods

        public void ListItemSelected(SelectionChangedEventArgs e)
        {
            PlaylistItem item = e.AddedItems.Count > 0 ? (PlaylistItem)e.AddedItems[0] : null;

            if (item != null)
            {
                //this.events.PublishOnUIThread(new PlaylistChangedEvent(item.Name));
                MatchManager.ActivePlaylist = item.List;
            }           
        }

        public async void Add()
        {
            var shell = (IoC.Get<IShell>() as Screen);
            var name = await Dialogs.ShowInputAsync(shell, "New Playlist", "Please enter a name for the playlist");
            //this.events.PublishOnUIThread(new ShowInputDialogEvent("Please enter a name for the playlist", "New Playlist")); 
            if(name != null && name != string.Empty)
            {
                Playlist p = new Playlist();
                p.Name = name;
                MatchManager.Match.Playlists.Add(p);
                MatchManager.MatchModified = true;
                NotifyOfPropertyChange("MatchManager.MatchModified");
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
            MatchManager.SaveMatch();
        }

        public void ShowSettings()
        {

        }
        public async void DeletePlaylist()
        {
            if (MatchManager.ActivePlaylist.Name != "Alle" && MatchManager.ActivePlaylist.Name != "Markiert")

            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Delete",
                    NegativeButtonText = "Cancel",
                    AnimateShow = true,
                    AnimateHide = false
                };
                var shell = (IoC.Get<IShell>() as Screen);
                var result = await Dialogs.ShowMessageAsync(shell, "Delete Playlist  '" + MatchManager.ActivePlaylist.Name + "' ?",
                    "Sure?", MessageDialogStyle.AffirmativeAndNegative, mySettings);

                if (result == MessageDialogResult.Negative)
                {
                }
                else
                {
                    Playlist p = MatchManager.ActivePlaylist;
                    PlaylistItem p1 = this.Items[0];
                    int i = MatchManager.Match.Playlists.IndexOf(p);
                    MatchManager.Match.Playlists.RemoveAt(i);
                    this.Items.RemoveAt(i);
                    this.Items.ElementAt(0);
                    this.Items.Refresh();
                    //Manager.ActivePlaylist = Manager.Match.Playlists[0];  //TODO: Playlist in der ListView auswählen
                    events.PublishOnUIThread(new PlaylistDeletedEvent());
                    MatchManager.MatchModified = true;
                    NotifyOfPropertyChange("MatchManager.MatchModified");

                }
            }
        }

        public IEnumerable<IResult> ExportPlaylist()
        {
            //TODO Ort Auswählen
            //TODO Auswahlmöglichkeit: einzelne Ballwechsel-Videos, alle Ballwechsel in einem Video
            string videoName = MatchManager.Match.VideoFile.Split('\\').Last();
            videoName = videoName.Split('.').First();
            videoName = videoName + "_("+ MatchManager.ActivePlaylist.Name+")";
            if (singleRalliesBool != false || rallyCollectionBool != false)
            {
                var exportDialog = new ExportPlaylistDialogResult()
                {
                    Title = "Export playlist...",
                    // Filter=
                    DefaultFileName = videoName,

                };
                yield return exportDialog;

                string folderName = exportDialog.Result;




                var progressDialog = new ExportPlaylistSaveResult(MatchManager, Dialogs, folderName, singleRalliesBool, rallyCollectionBool);
                yield return progressDialog;
            }
            else
            {
                var errorDialog = new ErrorMessageResult()
                {
                    Title = "Keine Exportoptionen ausgewählt!",
                    Message = "Bitte wählen sie per Rechtsklick entsprechende Optionen aus!",
                    Dialogs = Dialogs
                };
                yield return errorDialog;

            }
        }

        public void singleRallies()
        {
            singleRalliesBool = !singleRalliesBool;
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

            Playlist list = MatchManager.Match.Playlists.Where(p => p.Name == targetItem.Name).FirstOrDefault();

            if (list != null && !list.Rallies.Contains(sourceItem.Rally))
            {
                list.Rallies.Add(sourceItem.Rally);
                MatchManager.MatchModified = true;
                NotifyOfPropertyChange("MatchManager.MatchModified");
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

            foreach (var playlist in MatchManager.Match.Playlists)
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
