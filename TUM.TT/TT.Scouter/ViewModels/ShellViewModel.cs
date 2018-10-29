using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Threading.Tasks;
using TT.Lib;
using TT.Lib.Managers;
using TT.Lib.Results;
using TT.Lib.Events;
using System;
using TT.Lib.Util;

namespace TT.Scouter.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive,
        IShell,
        IHandle<MatchOpenedEvent>, IHandle<RalliesStrokesAddedEvent>, IHandle<CloudSyncActivityStatusChangedEvent>
    {
        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        public IEventAggregator Events { get; private set; }
        public IMatchManager MatchManager { get; set; }
        public ICloudSyncManager CloudSyncManager { get; set; }
        private IDialogCoordinator DialogCoordinator;
        private readonly IWindowManager _windowManager;


        public ShellViewModel(IWindowManager windowmanager, IEventAggregator eventAggregator, IMatchManager manager, ICloudSyncManager cloudSyncManager, IDialogCoordinator coordinator)
        {
            this.DisplayName = "";
            _windowManager = windowmanager;
            Events = eventAggregator;
            MatchManager = manager;
            DialogCoordinator = coordinator;
            CloudSyncManager = cloudSyncManager;
            CloudSyncManager.AutoUpload = true;
        }

        #region Caliburn hooks

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();

            // Subscribe ourself to the event bus
            //this.Events.Subscribe(this);
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            CloudSyncManager.Login();
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed  
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Events.Subscribe(this);            

            if (this.ActiveItem == null)
            {
                ActivateItem(new WelcomeViewModel(Events, MatchManager));
            }

            string registry = @"Software\Technische Universität München\Table Tennis Analysis\Secure";
            Secure scr = new Secure(Secure.Mode.Date);
            bool validVersion = scr.Algorithm("xyz", registry);
            if (validVersion != true)
                this.TryClose();
        }

        /// <summary>
        /// Determines whether the view model can be closed.
        /// </summary>
        /// <param name="callback">Called to perform the closing</param>
        public async override void CanClose(Action<bool> callback)
        {
            if (MatchManager.Match != null && MatchManager.Match.SyncToCloud)
            {
                var canCloseSync = await CanCloseSync();
                callback(canCloseSync);
                return;
            }

            var context = new CoroutineExecutionContext()
            {
                Target = this,
                View = this.GetView() as DependencyObject,
            };

            Coroutine.BeginExecute(
                this.PrepareClose().GetEnumerator(),
                context,
                (sender, args) =>
                {
                    callback(args.WasCancelled != true);
                });
        }

        /// <summary>
        /// Prepare the closing of this view model.
        /// </summary>
        /// <returns>The actions to execute before closing</returns>
        private IEnumerable<IResult> PrepareClose()
        {
            if (MatchManager.MatchModified)
            {
                var question = new YesNoQuestionResult()
                {
                    Title = "Save the match?",
                    Question = "The match is modified. Save changes?",
                    AllowCancel = true
                };
                yield return question;

                if (question.Result)
                {
                    foreach (var action in MatchManager.SaveMatch())
                    {
                        yield return action;
                    }
                }
            }
        }

        private async Task<bool> CanCloseSync()
        {
            if (CloudSyncManager.ActivityStauts != ActivityStauts.None)
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Wait",
                    NegativeButtonText = "Cancel",
                    FirstAuxiliaryButtonText = "Quit and Cancel Sync",
                };
                var result = await DialogCoordinator.ShowMessageAsync(this, "Sync Activity in Progress", "Wait for the sync activity to finish or quit?",
                    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

                if(result == MessageDialogResult.Affirmative)
                {
                    ShowUploadSettings();
                    return false;
                }
                else if(result == MessageDialogResult.FirstAuxiliary)
                {
                    CloudSyncManager.CancelSync();
                    return true;
                }
                else if(result == MessageDialogResult.Negative)
                {
                    return false;
                }
            }

            if (CloudSyncManager.IsUploadRequired || MatchManager.MatchModified)
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Save, Upload & Quit",
                    NegativeButtonText = "Cancel",
                    FirstAuxiliaryButtonText = "Quit without saving",
                };
                var result = await DialogCoordinator.ShowMessageAsync(this, "Save & Upload the match?", "The match is modified. Save and upload changes?",
                    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

                if (result == MessageDialogResult.Affirmative)
                {
                    try
                    {
                        await CloudSyncManager.UpdateAnalysis();
                    }
                    catch (CloudException e)
                    {
                        await DialogCoordinator.ShowMessageAsync(this, "Upload Error", e.Message + e.InnerException?.Message);
                        return false;
                    }
                    return true;
                }
                else if (result == MessageDialogResult.FirstAuxiliary)
                {
                    return true;
                }
                else if (result == MessageDialogResult.Negative)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Events
        private void SetMatchModified(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            MatchManager.MatchModified = true;

        }

        public void Handle(MatchOpenedEvent message)
        {
            // We must reconsider, whether we can generate a report now.

            this.NotifyOfPropertyChange(() => this.CanSaveMatch);
            this.NotifyOfPropertyChange(() => this.CanSaveMatchAs);
            this.NotifyOfPropertyChange(() => this.CanShowPlayer);
            this.NotifyOfPropertyChange(() => this.CanShowCompetition);
            this.NotifyOfPropertyChange(() => this.CanShowUploadSettings);
            MatchManager.Match.PropertyChanged += SetMatchModified;
            MatchManager.Match.FirstPlayer.PropertyChanged += SetMatchModified;
            MatchManager.Match.SecondPlayer.PropertyChanged += SetMatchModified;
            int countRallies = MatchManager.Match.Rallies.Count;
            for (int i = 0; i < countRallies; i++)
            {
                MatchManager.Match.Rallies[i].PropertyChanged += SetMatchModified;
                int countStrokes = MatchManager.Match.Rallies[i].Strokes.Count();
                for (int j = 0; j < countStrokes; j++)
                {
                    MatchManager.Match.Rallies[i].Strokes[j].PropertyChanged += SetMatchModified;
                    if (MatchManager.Match.Rallies[i].Strokes[j].Spin != null)
                        MatchManager.Match.Rallies[i].Strokes[j].Spin.PropertyChanged += SetMatchModified;
                    if (MatchManager.Match.Rallies[i].Strokes[j].Stroketechnique != null)
                        MatchManager.Match.Rallies[i].Strokes[j].Stroketechnique.PropertyChanged += SetMatchModified;
                    if (MatchManager.Match.Rallies[i].Strokes[j].Placement != null)
                        MatchManager.Match.Rallies[i].Strokes[j].Placement.PropertyChanged += SetMatchModified;
                }
            }

        }

        public void Handle(RalliesStrokesAddedEvent message)
        {
            int countRallies = MatchManager.Match.Rallies.Count;
            for (int i = 0; i < countRallies; i++)
            {
                MatchManager.Match.Rallies[i].PropertyChanged += SetMatchModified;
                int countStrokes = MatchManager.Match.Rallies[i].Strokes.Count();
                for (int j = 0; j < countStrokes; j++)
                {
                    MatchManager.Match.Rallies[i].Strokes[j].PropertyChanged += SetMatchModified;
                    if (MatchManager.Match.Rallies[i].Strokes[j].Spin != null)
                        MatchManager.Match.Rallies[i].Strokes[j].Spin.PropertyChanged += SetMatchModified;
                    if (MatchManager.Match.Rallies[i].Strokes[j].Stroketechnique != null)
                        MatchManager.Match.Rallies[i].Strokes[j].Stroketechnique.PropertyChanged += SetMatchModified;
                    if (MatchManager.Match.Rallies[i].Strokes[j].Placement != null)
                        MatchManager.Match.Rallies[i].Strokes[j].Placement.PropertyChanged += SetMatchModified;



                }
            }
        }
        public void Handle(CloudSyncActivityStatusChangedEvent messege)
        {
            NotifyOfPropertyChange(() => IsUploadingMatch);
            NotifyOfPropertyChange(() => IsCloudSyncEnabled);
        }

        #endregion

        #region View Methods

        public IEnumerable<IResult> OpenNewMatch()
        {
            MatchManager.CreateNewMatch();
            this.NotifyOfPropertyChange(() => this.CanSaveMatch);
            this.NotifyOfPropertyChange(() => this.CanSaveMatchAs);
            this.NotifyOfPropertyChange(() => this.CanShowPlayer);
            this.NotifyOfPropertyChange(() => this.CanShowCompetition);
            this.NotifyOfPropertyChange(() => this.CanShowUploadSettings);
            this.NotifyOfPropertyChange(() => this.IsCloudSyncEnabled);
            Events.PublishOnUIThread(new HideMenuEvent());
            var next = ShowScreenResult.Of<NewMatchViewModel>();
            yield return next;
            MatchManager.MatchSaveAs = false;
        }
        public IEnumerable<IResult> OpenMatch()
        {
            //      Load Match
            //      Open RemoteView in Shell
            foreach (IResult result in MatchManager.OpenMatch())
            {
                yield return result;
            }
            var next = ShowScreenResult.Of<MainViewModel>();
            next.Properties.Add("SelectedTab", MainViewModel.Tabs.Remote);
            yield return next;
        }

        public IEnumerable<IResult> SaveMatch()
        {
            if (MatchManager.MatchModified)
            {
                foreach (var action in MatchManager.SaveMatch())
                {
                    yield return action;
                }
            }
        }

        public IEnumerable<IResult> SaveMatchAs()
        {
            if (MatchManager.MatchSaveAs)
            {


                foreach (var action in MatchManager.SaveMatchAs())
                {
                    yield return action;
                }
            }
        }

        public IEnumerable<IResult> OpenMatchWithoutVideo()
        {
            foreach (IResult result in MatchManager.OpenLiveMatch())
            {
                yield return result;
            }
            var next = ShowScreenResult.Of<NewMatchViewModel>();
            yield return next;
        }

        public bool CanSaveMatch
        {
            get
            {
                return MatchManager.Match != null;
            }
        }
        public bool CanSaveMatchAs
        {
            get
            {
                return MatchManager.Match != null;
            }
        }

        public bool CanShowUploadSettings
        {
            get
            {
                return MatchManager.Match != null;
            }
        }
        public bool CanShowPlayer
        {
            get
            {
                if (MatchManager.Match != null)
                {
                    if (MatchManager.Match.FirstPlayer != null && MatchManager.Match.SecondPlayer != null)
                    {
                        return true;
                    }
                    return false;
                }
                return false;

            }
        }
        public bool CanShowCompetition
        {
            get
            {
                return MatchManager.Match != null;
            }
        }
        public bool IsUploadingMatch
        {
            get
            {
                return CloudSyncManager.ActivityStauts != ActivityStauts.None;
            }
        }
        public bool IsCloudSyncEnabled
        {
            get
            {
                return CloudSyncManager.ActivityStauts == ActivityStauts.None
                    && MatchManager.Match != null
                    && MatchManager.Match.SyncToCloud;
            }
        }


        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name) ? Application.Current.Windows.OfType<T>().Any() : Application.Current.Windows.OfType<T>().Any(wde => wde.Name.Equals(name));
        }
        public void ShowPlayer()

        {
            if (IsWindowOpen<Window>("ShowPlayer"))
            {
                Application.Current.Windows.OfType<Window>().Where(win => win.Name == "ShowPlayer").FirstOrDefault().Focus();

            }
            else
            {
                _windowManager.ShowWindow(new ShowAllPlayerViewModel(_windowManager, Events, MatchManager, DialogCoordinator));
            }

        }
        public void ShowCompetition()
        {
            if (IsWindowOpen<Window>("ShowCompetition"))
            {
                Application.Current.Windows.OfType<Window>().Where(win => win.Name == "ShowCompetition").FirstOrDefault().Focus();

            }
            else
            {
                _windowManager.ShowWindow(new ShowCompetitionViewModel(_windowManager, Events, MatchManager, DialogCoordinator));
            }
        }
        public void OpenITTV()
        {
            if (IsWindowOpen<Window>("ITTV"))
            {
                Application.Current.Windows.OfType<Window>().Where(win => win.Name == "ITTV").FirstOrDefault().Focus();

            }
            else
            {
                _windowManager.ShowWindow(new IttvDownloadViewModel(_windowManager, Events, MatchManager, DialogCoordinator));
            }
        }
        public void ShowKeyBindingEditor()
        {
            if (IsWindowOpen<Window>("KeyBindingEditor"))
            {
                Application.Current.Windows.OfType<Window>().Where(win => win.Name == "KeyBindingEditor").FirstOrDefault().Focus();

            }
            else
            {
                _windowManager.ShowWindow(new KeyBindingEditorViewModel(_windowManager, Events, DialogCoordinator));
            }
        }
        public void ShowUploadSettings()
        {
            _windowManager.ShowDialog(new ShowUploadSettingsViewModel(_windowManager, Events, MatchManager, CloudSyncManager, DialogCoordinator));
        }
        public void ShowBatchUpload()
        {
            try
            {
                _windowManager.ShowDialog(new BatchUploadViewModel(_windowManager, Events, MatchManager, CloudSyncManager, DialogCoordinator));
            }
            catch (InvalidOperationException) { /* File selection canceled */} 
        }
        #endregion
    }
}
