using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TT.Lib;
using TT.Lib.Managers;
using TT.Lib.Results;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class ShowCompetitionViewModel : Conductor<IScreen>.Collection.AllActive, IShell, INotifyPropertyChangedEx
    {
        public IEventAggregator events { get; private set; }
        public IMatchManager MatchManager { get; set; }
        private readonly IWindowManager _windowManager;
        private IDialogCoordinator DialogCoordinator;
        public Match Match { get { return MatchManager.Match; } }
        public ShowCompetitionViewModel(IWindowManager windowmanager, IEventAggregator eventAggregator, IMatchManager man, IDialogCoordinator coordinator)
        {
            this.DisplayName = "Competition Details";
            this.events = eventAggregator;
            MatchManager = man;
            _windowManager = windowmanager;
            DialogCoordinator = coordinator;


            MatchManager.Match.PropertyChanged += SetMatchModified;
        }
        /// <summary>
        /// Set MatchModified=true, if match informations are modified
        /// </summary>

        private void SetMatchModified(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            MatchManager.MatchModified = true;

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

        protected override async void OnDeactivate(bool close)
        {

            if (MatchManager.MatchModified)
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Save and Close",
                    NegativeButtonText = "Cancel",
                    FirstAuxiliaryButtonText = "Close Without Saving",
                    AnimateShow = true,
                    AnimateHide = false
                };

                var result = await DialogCoordinator.ShowMessageAsync(this, "Close Window?",
                    "You didn't save your changes?",
                    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary, mySettings);

                bool _shutdown = result == MessageDialogResult.Affirmative;

                if (_shutdown)
                {
                    Coroutine.BeginExecute(MatchManager.SaveMatch().GetEnumerator(), new CoroutineExecutionContext() { View = this.GetView() });
                    Application.Current.Shutdown();
                }
            }
            events.Unsubscribe(this);
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



                var question = new YesNoCloseQuestionResult()
                {
                    Title = "Save the Changes?",
                    Question = "The Player Informations are modified. Save changes?",
                    AllowCancel = true,

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

        public bool CanSaveMatch
        {
            get
            {
                return MatchManager.Match != null && MatchManager.Match.DefaultPlaylist.FinishedRallies.Any();
            }
        }
        #endregion

        #region Events

        #endregion
        #region Helper Methods


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
                _windowManager.ShowWindow(new ShowAllPlayerViewModel(_windowManager, events, MatchManager, DialogCoordinator));
            }

        }


        #endregion


    }
}
