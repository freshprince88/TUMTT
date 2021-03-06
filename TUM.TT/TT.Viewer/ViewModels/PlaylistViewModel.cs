﻿using Caliburn.Micro;
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
using System.Collections.ObjectModel;

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
            if (name != null && name != string.Empty)
            {
                Playlist p = new Playlist(MatchManager.Match);
                p.Name = name;
                MatchManager.Match.Playlists.Add(p);
                MatchManager.MatchModified = true;
                NotifyOfPropertyChange("MatchManager.MatchModified");
                double pt = 0;
                for (int c = 0; c < p.Rallies.Count(); c++)
                {
                    pt = pt + (p.Rallies[c].End - p.Rallies[c].Start);
                }
                double test = pt;
                this.ActivateItem(new PlaylistItem()
                {
                    Name = name,
                    Count = p.Rallies.Count(),

                    PlayTime = pt,
                    List = p
                });
            }
        }

        public void Save()
        {
            MatchManager.SaveMatch();
        }

        public async void RenamePlaylist()
        {
            if (MatchManager.ActivePlaylist.Name != "Alle" && MatchManager.ActivePlaylist.Name != "Markiert")
                
            {

                var shell = (IoC.Get<IShell>() as Screen);
                var newName = await Dialogs.ShowInputAsync(shell, "Rename Playlist  '" + MatchManager.ActivePlaylist.Name + "' ?",
                    "Sure?");
                string oldName = MatchManager.ActivePlaylist.Name;
                if (newName != null && newName != string.Empty)
                {
                    Playlist list = MatchManager.Match.Playlists.Where(p => p.Name == oldName).FirstOrDefault();
                    PlaylistItem playListItem = this.Items.Where(p => p.Name == oldName).FirstOrDefault();

                    if (list != null)
                    {
                        MatchManager.Match.Playlists.Where(p => p.Name == oldName).FirstOrDefault().Name = newName;
                        this.Items.Where(p => p.Name == oldName).FirstOrDefault().Name = newName;
                        MatchManager.ActivePlaylist = MatchManager.Match.Playlists.Where(p => p.Name == newName).FirstOrDefault();
                        events.PublishOnUIThread(new PlaylistChangedEvent(MatchManager.ActivePlaylist));
                        MatchManager.MatchModified = true;
                        NotifyOfPropertyChange("MatchManager.MatchModified");
                        //MatchManager.RenamePlaylist(MatchManager.ActivePlaylist.Name, name);

                    }
                }
            }
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
            videoName = videoName + "_(" + MatchManager.ActivePlaylist.Name + ")";
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
                    Title = "No Export Option chosen!",
                    Message = "Right-Click to choose an Option!",
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
            if (dropInfo.Data is ResultListItem && dropInfo.TargetItem is PlaylistItem)
            {
                var sourceItem = dropInfo.Data as ResultListItem;
                var targetItem = dropInfo.TargetItem as PlaylistItem;

                if (sourceItem != null && targetItem != null)
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }

            else if (dropInfo.Data is IEnumerable<ResultListItem> && dropInfo.TargetItem is PlaylistItem)
            {
                var sourceItem = dropInfo.Data as IEnumerable<ResultListItem>;
                var targetItem = dropInfo.TargetItem as PlaylistItem;

                if (sourceItem != null && targetItem != null)
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                    dropInfo.Effects = DragDropEffects.Copy;
                }

            }
            else if (dropInfo.Data is Rally && dropInfo.TargetItem is PlaylistItem)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                dropInfo.Effects = DragDropEffects.Copy;
            }
            else if (dropInfo.TargetItem is PlaylistItem)
            {
                Rally data = ((DataObject)dropInfo.Data).GetData(typeof(Rally)) as Rally;
                if (data != null)
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                    dropInfo.Effects = DragDropEffects.Copy;
                }
            }

        }

        public void Drop(IDropInfo dropInfo)
        {

            if (dropInfo.Data is ResultListItem && dropInfo.TargetItem is PlaylistItem)
            {
                var sourceItem = dropInfo.Data as ResultListItem;
                var targetItem = dropInfo.TargetItem as PlaylistItem;
                Playlist list = MatchManager.Match.Playlists.Where(p => p.Name == targetItem.Name).FirstOrDefault();
                if (list != null && !list.Rallies.Contains(sourceItem.Rally))
                {
                    list.Add(sourceItem.Rally);
                    //Sort List after Rally-Number
                    list.Sort();
                    MatchManager.MatchModified = true;
                    NotifyOfPropertyChange("MatchManager.MatchModified");
                    targetItem.Count++;
                    targetItem.PlayTime = targetItem.PlayTime + (sourceItem.Rally.End - sourceItem.Rally.Start);
                    this.Items.Refresh();
                }
            }
            else if (dropInfo.Data is IEnumerable<ResultListItem> && dropInfo.TargetItem is PlaylistItem)
            {
                var sourceItem = dropInfo.Data as IEnumerable<ResultListItem>;
                var targetItem = dropInfo.TargetItem as PlaylistItem;
                Playlist list = MatchManager.Match.Playlists.Where(p => p.Name == targetItem.Name).FirstOrDefault();
                var temp = sourceItem.Select(i => i.Rally).ToList();
                var dropNumbers = temp.Select(r => r.Number).ToList();
                var plNumbers = list.Rallies.Select(r => r.Number).ToList();
                var except = dropNumbers.Except(plNumbers);
                var newItems = temp.Where(r => except.Contains(r.Number));
                list.AddRange(newItems);
                targetItem.Count = list.Rallies.Count;
                targetItem.PlayTime = calcPlaytime(list);
                //Sort List after Rally-Number                        
                list.Sort();
                targetItem.PlayTime= calcPlaytime(list);
                this.Items.Refresh();
                MatchManager.MatchModified = true;
                NotifyOfPropertyChange("MatchManager.MatchModified");
            }
            else if (dropInfo.TargetItem is PlaylistItem)
            {
                Rally sourceItem;
                if (dropInfo.Data is Rally)
                    sourceItem = (Rally)dropInfo.Data;
                else
                {
                    sourceItem = ((DataObject) dropInfo.Data).GetData(typeof(Rally)) as Rally;
                    if (sourceItem == null)
                        return;
                }

                var targetItem = dropInfo.TargetItem as PlaylistItem;
                var playlist = MatchManager.Match.Playlists.FirstOrDefault(p => p.Name == targetItem.Name);
                if (playlist != null && !playlist.Rallies.Contains(sourceItem))
                {
                    playlist.Add(sourceItem);
                    //Sort List after Rally-Number
                    playlist.Sort();
                    MatchManager.MatchModified = true;
                    NotifyOfPropertyChange("MatchManager.MatchModified");
                    targetItem.Count++;
                    targetItem.PlayTime = targetItem.PlayTime + (sourceItem.End - sourceItem.Start);
                    this.Items.Refresh();
                }
            }

        }

        public void Handle(PlaylistChangedEvent message)
        {
            var selected = this.Items.Where(p => p.Name == message.List.Name).FirstOrDefault();

            if (selected != null)
            {
                selected.Count = message.List.Rallies.Count;
                selected.PlayTime = calcPlaytime(message.List);
                this.Items.Refresh();
            }
        }

        #endregion

        #region Helper Methods
        public IEnumerable<ResultListItem> WithoutLast<ResultListItem>(IEnumerable<ResultListItem> source)
        {
            using (var e = source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    for (var value = e.Current; e.MoveNext(); value = e.Current)
                    {
                        yield return value;
                    }
                }
            }
        }

        public ObservableCollection<Rally> Sort(ObservableCollection<Rally> r)
        {
            List<Rally> sorted = r.OrderBy(x => x.Number).ToList();
            int ptr = 0;
            while (ptr < sorted.Count)
            {
                if (!r[ptr].Equals(sorted[ptr]))
                {
                    Rally t = r[ptr];
                    r.RemoveAt(ptr);
                    r.Insert(sorted.IndexOf(t), t);
                }
                else
                {
                    ptr++;
                }
            }
            return r;
        }
        public double calcPlaytime(Playlist l)
        {
            double pt = 0;
            for (int c = 0; c < l.Rallies.Count(); c++)
            {
                pt = pt + (l.Rallies[c].End - l.Rallies[c].Start);
            }
            return pt;

        }
        private void LoadPlaylists()
        {
            this.Items.Clear();

            foreach (var playlist in MatchManager.Match.Playlists)
            {
                string name = playlist.Name;
                double pt = 0;
                for (int c = 0; c < playlist.Rallies.Count(); c++)
                {
                    pt = pt + (playlist.Rallies[c].End - playlist.Rallies[c].Start);
                }
                double test = pt;

                this.ActivateItem(new PlaylistItem()
                {
                    Name = name,
                    Count = playlist.Rallies.Count(),
                    PlayTime = pt,
                    List = playlist
                });
            }
        }

        #endregion

    }
}
