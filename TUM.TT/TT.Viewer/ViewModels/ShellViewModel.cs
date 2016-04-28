using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TT.Models;
using TT.Models.Events;
using TT.Lib.Managers;

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
        private IMatchManager Manager;
        private IDialogCoordinator DialogCoordinator;

        public ShellViewModel(IEventAggregator eventAggregator, IMatchManager manager, IDialogCoordinator coordinator)
        {
            this.DisplayName = "TUM.TT Viewer";
            Events = eventAggregator;
            Manager = manager;
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
                ActivateItem(new WelcomeViewModel(Manager));
            }
        }

        protected override async void OnDeactivate(bool close)
        {
            Events.Unsubscribe(this);
            if (Manager.MatchModified)
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
                    Coroutine.BeginExecute(Manager.SaveMatch().GetEnumerator(), new CoroutineExecutionContext() { View = this.GetView() });
                    Application.Current.Shutdown();
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
                return  Manager.Match != null && Manager.Match.DefaultPlaylist.FinishedRallies.Any();
            }
        }

        public IEnumerable<IResult> GenerateReport()
        {
            return Manager.GenerateReport();
        }

        #endregion

        #region Events

        public void Handle(MatchOpenedEvent message)
        {
            // We must reconsider, whether we can generate a report now.
            this.NotifyOfPropertyChange(() => this.CanGenerateReport);
            this.ActivateItem(new MatchViewModel(Events, IoC.GetAll<IResultViewTabItem>(), Manager, DialogCoordinator));
        }
        #endregion
        #region Helper Methods
        public IEnumerable<IResult> OpenMatch()
        {
            return Manager.OpenMatch();
        }
        #endregion

    }
}
