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
using TT.Lib.Events;

namespace TT.Scouter.ViewModels
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

        public ShellViewModel(IEventAggregator eventAggregator, IMatchManager manager, IDialogCoordinator coordinator)
        {
            this.DisplayName = "";
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
        private void SetMatchModified(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            MatchManager.MatchModified = true;

        }

        public void Handle(MatchOpenedEvent message)
        {
            // We must reconsider, whether we can generate a report now.

            this.NotifyOfPropertyChange(() => this.CanSaveMatch);
            this.NotifyOfPropertyChange(() => this.CanShowPlayer);
            this.NotifyOfPropertyChange(() => this.CanShowCompetition);
            MatchManager.Match.PropertyChanged += SetMatchModified;
            MatchManager.Match.FirstPlayer.PropertyChanged += SetMatchModified;
            MatchManager.Match.SecondPlayer.PropertyChanged += SetMatchModified;
            int countRallies = MatchManager.ActivePlaylist.Rallies.Count;
            for (int i = 0; i < countRallies; i++)
            {
                MatchManager.ActivePlaylist.Rallies[i].PropertyChanged += SetMatchModified;
            }

        }
        #endregion

        #region View Methods

        public IEnumerable<IResult> OpenNewMatch()
        {
            MatchManager.CreateNewMatch();
            this.NotifyOfPropertyChange(() => this.CanSaveMatch);
            this.NotifyOfPropertyChange(() => this.CanShowPlayer);
            this.NotifyOfPropertyChange(() => this.CanShowCompetition);
            Events.PublishOnUIThread(new HideMenuEvent());
            var next = ShowScreenResult.Of<NewMatchViewModel>();
            yield return next;
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
                return MatchManager.Match != null ;
            }
        }
        public bool CanShowPlayer
        {
            get
            {   if (MatchManager.Match != null)
                {
                    if(MatchManager.Match.FirstPlayer != null && MatchManager.Match.SecondPlayer != null)
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
                return MatchManager.Match != null ;
            }
        }


        public void ShowPlayer()
        {

        }
        public void ShowCompetition()
        {

        }
        #endregion
    }
}
