using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using TT.Models;
using TT.Lib.Events;
using TT.Models.Util.Enums;
using TT.Lib.Managers;
using TT.Lib;
using TT.Lib.Results;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace TT.Viewer.ViewModels
{
    public class ShowAllPlayerViewModel : Conductor<IScreen>.Collection.AllActive, IShell
    {
        public PlayerInformationViewModel Player1InformationView { get; set; }
        public PlayerInformationViewModel Player2InformationView { get; set; }
        public IEventAggregator events { get; private set; }
        public IMatchManager MatchManager { get; set; }
        private readonly IWindowManager _windowManager;
        private IDialogCoordinator DialogCoordinator;

        public ShowAllPlayerViewModel(IWindowManager windowmanager, IEventAggregator eventAggregator, IMatchManager man, IDialogCoordinator coordinator)
        {
            this.DisplayName = "Spielerinformationen";
            this.events = eventAggregator;
            MatchManager = man;
            _windowManager = windowmanager;
            DialogCoordinator = coordinator;
            Player1InformationView = new PlayerInformationViewModel(this.events, MatchManager)
            {
                Player = MatchManager.Match.FirstPlayer,
                Number = 1
            };
            this.ActivateItem(Player1InformationView);
            Player2InformationView = new PlayerInformationViewModel(this.events, MatchManager)
            {
                Player = MatchManager.Match.SecondPlayer,
                Number = 2
            };
            this.ActivateItem(Player2InformationView);
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
            this.ActivateItem(Player1InformationView);
            this.ActivateItem(Player2InformationView);

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
            this.DeactivateItem(Player1InformationView, close);
            this.DeactivateItem(Player2InformationView, close);
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

        #endregion


    }
}
