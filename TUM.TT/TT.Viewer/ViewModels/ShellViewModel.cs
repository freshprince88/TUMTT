using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TT.Lib;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Lib.Results;

using TT.Lib.Views;

using TT.Models;


namespace TT.Viewer.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive,
        IShell,
        IHandle<MatchOpenedEvent>
    {
        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        public IEventAggregator Events { get; private set; }
        public IMatchManager MatchManager { get; set; }
        private IDialogCoordinator DialogCoordinator;
        private readonly IWindowManager _windowManager;





        public ShellViewModel(IWindowManager windowmanager, IEventAggregator eventAggregator, IMatchManager manager, IDialogCoordinator coordinator)
        {
            this.DisplayName = "";

            _windowManager = windowmanager;
            Events = eventAggregator;
            MatchManager = manager;
            DialogCoordinator = coordinator;


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

        protected override async void OnDeactivate(bool close)
        {
            Events.Unsubscribe(this);
            if (MatchManager.MatchModified)
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Save and Quit",
                    NegativeButtonText = "Cancel",
                    FirstAuxiliaryButtonText = "Quit Without Saving",
                    AnimateShow = true,
                    AnimateHide = false
                };

                var result = await DialogCoordinator.ShowMessageAsync(this, "Quit application?",
                    "Sure you want to quit application?",
                    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

                bool _shutdown = result == MessageDialogResult.Affirmative;

                if (_shutdown)
                {
                    Coroutine.BeginExecute(MatchManager.SaveMatch().GetEnumerator(), new CoroutineExecutionContext() { View = this.GetView() });
                    Application.Current.Shutdown();
                }
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

                var playlist = MatchManager.Match.Playlists.Where(p => p.Name == "Alle").FirstOrDefault();
                var lastRally = playlist.Rallies.LastOrDefault();
                //TODO
                if (playlist.Rallies.Any())
                {
                    if (lastRally.Winner == MatchPlayer.None)
                        playlist.Rallies.Remove(lastRally);
                }

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
            return MatchManager.GenerateReport();
        }

        #endregion

        #region Events

        public void Handle(MatchOpenedEvent message)
        {
            // We must reconsider, whether we can generate a report now.
            this.NotifyOfPropertyChange(() => this.CanGenerateReport);
            this.NotifyOfPropertyChange(() => this.CanSaveMatch);
            this.NotifyOfPropertyChange(() => this.CanShowPlayer);
            this.NotifyOfPropertyChange(() => this.CanShowCompetition);
            this.ActivateItem(new MatchViewModel(Events, IoC.GetAll<IResultViewTabItem>().OrderBy(i => i.GetOrderInResultView()), MatchManager, DialogCoordinator));
        }
        #endregion

        #region Helper Methods
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

        #endregion

    }
}
