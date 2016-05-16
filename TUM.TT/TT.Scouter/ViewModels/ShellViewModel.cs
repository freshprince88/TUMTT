using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TT.Models;
using TT.Lib;
using TT.Lib.Managers;
using TT.Lib.Results;
using TT.Scouter.ViewModels;

namespace TT.Scouter
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive,
        IShell
    {
        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        public IEventAggregator Events { get; private set; }
        public IMatchManager MatchManager { get; set; }
        private IDialogCoordinator DialogCoordinator;

        public ShellViewModel(IEventAggregator eventAggregator, IMatchManager manager, IDialogCoordinator coordinator)
        {
            this.DisplayName = "TUM.TT Scouter";
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
                ActivateItem(new WelcomeViewModel(Events, MatchManager));
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
                if (playlist.Rallies.Any()) { 
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

        #region Events

        #endregion

        #region Helper Methods
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
            //if (MatchManager.MatchModified)
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
