using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using TT.Lib;
using TT.Lib.Util;
using TT.Lib.Managers;
using TT.Models;
using TT.Models.Api;
using System.Windows.Input;

namespace TT.Scouter.ViewModels
{
    public enum ItemStatus { Waiting, Uploading, Done, Error, Canceled }
    public enum FileType { Video, TTO }

    public class UploadItem
    {
        public string Filename { get; set; }
        public string Filepath { get; set; }
        public FileType FileType { get; set; }
        public ItemStatus Status { get; set; } = ItemStatus.Waiting;
    }

    public class BatchUploadViewModel : Conductor<IScreen>.Collection.AllActive, IShell, INotifyPropertyChangedEx
    {
        public IEventAggregator events { get; private set; }
        public IMatchManager MatchManager { get; set; }
        public ICloudSyncManager CloudSyncManager { get; set; }
        private readonly IWindowManager _windowManager;
        private IDialogCoordinator DialogCoordinator;
        public Match Match { get { return MatchManager.Match; } }

        private readonly OpenFileDialog openFileDialog = new OpenFileDialog();
        public ObservableCollection<UploadItem> UploadItems { get; private set; } = new ObservableCollection<UploadItem>();
        private CancellationTokenSource TokenSource = new CancellationTokenSource();
        private int progressCount = 0;

        #region Calculated Properties
        public double Progress
        {
            get
            {
                if (UploadItems.Count == 0) return 0;
                return (progressCount / (double) UploadItems.Count) * 100;
            }
            set  {}
        }
        #endregion

        public BatchUploadViewModel(IWindowManager windowmanager, IEventAggregator eventAggregator, IMatchManager man, ICloudSyncManager cloudSyncManager, IDialogCoordinator coordinator)
        {
            this.DisplayName = "Batch Upload";
            this.events = eventAggregator;
            MatchManager = man;
            CloudSyncManager = cloudSyncManager;
            _windowManager = windowmanager;
            DialogCoordinator = coordinator;
        }

        #region Caliburn Hooks

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);

        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
        }

        /// <summary>
        /// Determines whether the view model can be closed.
        /// </summary>
        /// <param name="callback">Called to perform the closing</param>
        public async override void CanClose(Action<bool> callback)
        {
            if (CloudSyncManager.ActivityStauts != ActivityStauts.None)
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Wait",
                    NegativeButtonText = "Cancel Upload"
                };
                var result = await DialogCoordinator.ShowMessageAsync(this, "Upload in Progress", "Wait for upload to finish or quit?",
                    MessageDialogStyle.AffirmativeAndNegative, mySettings);

                if(result == MessageDialogResult.Negative)
                {
                    Cancel();
                    callback(true);
                    return;
                }
                callback(false);
                return;
            }
            callback(true);
        }
        #endregion


        #region Logic
        private void InitializeOpenFileDialog()
        {
            // Set the file dialog to filter for graphics files.
            openFileDialog.Filter =
                "Table Tennis Obeservations (*.TTO)|*.TTO|" +
                "Videos (*.MP4)|*.MP4|" +
                "All files (*.*)|*.*";

            // Allow the user to select multiple images.
            openFileDialog.Multiselect = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.Title = "Select Files...";
        }

        public async void OpenFiles()
        {
            InitializeOpenFileDialog();
            DialogResult dr = openFileDialog.ShowDialog();
            if (dr != DialogResult.OK)
            {
                TryClose();
                return;
            }
            foreach (string file in openFileDialog.FileNames)
            {
                var extention = Path.GetExtension(file).ToLower();
                var filename = Path.GetFileName(file);
                FileType type = FileType.TTO;
                if (extention == ".tto")
                {
                    type = FileType.TTO;
                }
                else if (extention == ".mp4")
                {
                    type = FileType.Video;
                    try
                    {
                        MatchMetaExtensions.MatchMetaFromMagicFilename(filename);
                    }
                    catch (ArgumentException)
                    {
                        await DialogCoordinator.ShowMessageAsync(this, "Error reading files",
                            "The following video filename is not parseable. Please rename the file to the magic filename pattern.\n\n" + filename);
                        TryClose();
                        return;
                    }
                }
                else
                {
                    await DialogCoordinator.ShowMessageAsync(this, "Error reading files",
                        "Selected files contain unrecognized files. Batch upload only supports .tto and .mp4 files.");
                    TryClose();
                    return;
                }
                UploadItems.Add(new UploadItem()
                {
                    Filepath = file,
                    Filename = filename,
                    FileType = type
                });
            }
            await Run();
        }

        private async Task<int> Run()
        {
            CloudSyncManager.AutoUpload = false;

            for (int i = 0; i < UploadItems.Count; i++)
            {
                if (TokenSource.IsCancellationRequested)
                {
                    progressCount = 0;
                    NotifyOfPropertyChange(() => Progress);
                    break;
                }

                UploadItem item = UploadItems[i];
                item.Status = ItemStatus.Uploading;
                UpdateItem(item);

                if(item.FileType == FileType.TTO)
                {
                    item.Status = await UploadTTO(item);
                }
                else if(item.FileType == FileType.Video)
                {
                    item.Status = await UploadVideo(item);
                }

                UpdateItem(item);
                progressCount++;
                NotifyOfPropertyChange(() => Progress);
            }
            
            CloudSyncManager.AutoUpload = true;
            return progressCount;
        }

        public async Task<ItemStatus> UploadTTO(UploadItem item)
        {
            await Coroutine.ExecuteAsync(MatchManager.OpenMatch(item.Filepath).GetEnumerator());
            MatchManager.Match.SyncToCloud = true;
            try
            {
                await CloudSyncManager.UploadMatch();
            }
            catch (Exception)
            {
                return ItemStatus.Error;
            }

            if (TokenSource.IsCancellationRequested)
            {
                return ItemStatus.Canceled;
            }
            return ItemStatus.Done;
        }

        public async Task<ItemStatus> UploadVideo(UploadItem item)
        {
            MatchMeta meta = MatchMetaExtensions.MatchMetaFromMagicFilename(item.Filename);
            try
            {
                await CloudSyncManager.UploadMetaVideo(meta, item.Filepath);
            }
            catch (Exception e)
            {
                await DialogCoordinator.ShowMessageAsync(this, "Errer during uplaod", e.Message + e.InnerException?.Message);
                return ItemStatus.Error;
            }

            if (TokenSource.IsCancellationRequested)
            {
                return ItemStatus.Canceled;
            }

            return ItemStatus.Done;
        }

        public void Cancel()
        {
            TokenSource.Cancel();
            CloudSyncManager.CancelSync();
        }
        #endregion
        
        #region Helper Methods
        public void UpdateItem(UploadItem item)
        {
            int index = UploadItems.IndexOf(item);
            UploadItems.RemoveAt(index);
            UploadItems.Insert(index, item);
        }
        #endregion

    }
}
