using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using TT.Lib;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Lib.Results;
using TT.Models;
using Application = System.Windows.Application;


namespace TT.Viewer.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive,
        IShell,
        IHandle<MatchOpenedEvent>,
        IHandle<ReportPreviewChangedEvent>
    {
        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        public IEventAggregator Events { get; private set; }
        public IMatchManager MatchManager { get; set; }
        private IDialogCoordinator DialogCoordinator;
        private readonly IWindowManager _windowManager;
        private List<string> TmpFiles { get; set; }

        public ShellViewModel(IWindowManager windowmanager, IEventAggregator eventAggregator, IMatchManager manager, IDialogCoordinator coordinator)
        {
            this.DisplayName = "";

            _windowManager = windowmanager;
            Events = eventAggregator;
            MatchManager = manager;
            DialogCoordinator = coordinator;

            // for translation testing - don't set for production!
            //CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("de-DE");
            //CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("de-DE");
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

        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Events.Subscribe(this);

            if (this.ActiveItem == null)
            {
                ActivateItem(new WelcomeViewModel(MatchManager));
            }
        }

        protected override void OnDeactivate(bool close)
        {
            Events.Unsubscribe(this);
            if (close)
            {
                TryDeleteTmpFiles();
                IoC.Get<IReportGenerationQueueManager>().Dispose();
            }
        }

        /// <summary>
        /// Determines whether the view model can be closed.
        /// </summary>
        /// <param name="callback">Called to perform the closing</param>
        public override void CanClose(System.Action<bool> callback)
        {
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

        #endregion

        #region View Methods

        /// <summary>
        /// Gets a value indicating whether a report can be generated.
        /// </summary>
        public bool CanGenerateReport
        {
            get
            {
                return MatchManager.Match != null && MatchManager.Match.DefaultPlaylist.FinishedRallies.Any();
            }
        }
        public bool CanSaveMatch
        {
            get
            {
                return MatchManager.Match != null && MatchManager.Match.DefaultPlaylist.FinishedRallies.Any();
            }
        }
        public bool CanShowPlayer
        {
            get
            {
                return MatchManager.Match != null && MatchManager.Match.DefaultPlaylist.FinishedRallies.Any();
            }
        }
        public bool CanShowCompetition
        {
            get
            {
                return MatchManager.Match != null && MatchManager.Match.DefaultPlaylist.FinishedRallies.Any();
            }
        }

        public IEnumerable<IResult> GenerateReport()
        {
            var reportVm = new ReportSettingsViewModel(MatchManager, IoC.Get<IReportGenerationQueueManager>(), _windowManager, Events, DialogCoordinator);
            reportVm.GenerateReport();
            foreach (var res in reportVm.SaveGeneratedReport())
                yield return res;
            reportVm.DiscardViewModel(true);
        }

        #endregion

        #region Events

        public void Handle(MatchOpenedEvent message)
        {
            //DeactivateItem(ActiveItem, true);
            // We must reconsider, whether we can generate a report now.
            NotifyOfPropertyChange(() => this.CanGenerateReport);
            NotifyOfPropertyChange(() => this.CanSaveMatch);
            NotifyOfPropertyChange(() => this.CanShowPlayer);
            NotifyOfPropertyChange(() => this.CanShowCompetition);
            this.ActivateItem(new MatchViewModel(Events, IoC.GetAll<IResultViewTabItem>().OrderBy(i => i.GetOrderInResultView()), MatchManager, DialogCoordinator));

            var reportVm = new ReportSettingsViewModel(MatchManager, IoC.Get<IReportGenerationQueueManager>(), _windowManager, Events, DialogCoordinator);
            reportVm.GenerateReport();
            reportVm.DiscardViewModel(true);
        }
        
        public void Handle(ReportPreviewChangedEvent message)
        {
            if (TmpFiles == null)
                TmpFiles = new List<string>();
            if (!TmpFiles.Contains(message.ReportPreviewPath))
                TmpFiles.Add(message.ReportPreviewPath);
        }
        #endregion

        #region Helper Methods
        private void TryDeleteTmpFiles()
        {
            if (TmpFiles != null)
                foreach (string path in TmpFiles)
                    try
                    {
                        File.Delete(path);
                    } catch (Exception) { /* best effort */}
        }

        public IEnumerable<IResult> OpenMatch()
        {
            return MatchManager.OpenMatch();
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
        public static bool IsWindowOpen<T>(string name ="") where T : Window
        {
            return string.IsNullOrEmpty(name) ? Application.Current.Windows.OfType<T>().Any() : Application.Current.Windows.OfType<T>().Any(wde => wde.Name.Equals(name));
        }
        public void ShowPlayer()
            
        {  if (IsWindowOpen<Window>("ShowPlayer"))
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

        public void ShowReportSettings()
        {
            if (IsWindowOpen<Window>("ReportSettings"))
            {
                Application.Current.Windows.OfType<Window>().Where(win => win.Name == "ReportSettings").FirstOrDefault().Focus();

            }
            else
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.CanResizeWithGrip;
                settings.Width = Math.Min(SystemParameters.PrimaryScreenWidth - 20, 1200);
                settings.Height = Math.Min(SystemParameters.PrimaryScreenHeight - 35, 860);
                _windowManager.ShowDialog(new ReportSettingsViewModel(MatchManager, IoC.Get<IReportGenerationQueueManager>(), _windowManager, Events, DialogCoordinator), null, settings);
            }
        }
        #endregion

    }
}
