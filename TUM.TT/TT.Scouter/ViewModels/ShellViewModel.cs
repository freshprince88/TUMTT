using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Scouter.ViewModels;

namespace TT.Scouter
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive, 
        IShell,
        IHandle<SetShellContentEvent>
    {
        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        public IEventAggregator Events { get; private set; }
        private IMatchManager Manager;
        private IDialogCoordinator DialogCoordinator;

        public ShellViewModel(IEventAggregator eventAggregator, IMatchManager manager, IDialogCoordinator coordinator)
        {
            this.DisplayName = "TUM.TT";
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
                ActivateItem(new WelcomeViewModel(Events));
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
                    Manager.SaveMatch();
                    Application.Current.Shutdown();
                }
            }
        }

        #endregion

        #region Events

        public void Handle(SetShellContentEvent message)
        {
            this.ActivateItem(message.ViewModel);
        }
        #endregion
    }
}