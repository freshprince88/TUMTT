using Caliburn.Micro;
using System;
using System.Collections.Generic;
using TT.Lib.Managers;
using TT.Models;
using TT.Lib.Results;
using TT.Lib;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace TT.Scouter.ViewModels
{
    public class NewMatchViewModel : Screen
    {
        private IEventAggregator Events;
        private IMatchManager MatchManager;
        public Match Match { get { return MatchManager.Match; } }

        public NewMatchViewModel(IEventAggregator ev, IMatchManager man)
        {
            Events = ev;
            MatchManager = man;           

        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        #region View Methods

        public IEnumerable<IResult> AddNewPlayer(string s)
        {
            // Show new Player Screen
            int num = Convert.ToInt32(s);
            var nextScreen = ShowScreenResult.Of<NewPlayerViewModel>();
            switch (num)
            {
                case 1:
                    nextScreen.Properties.Add("Player", Match.FirstPlayer);
                    break;
                case 2:
                    nextScreen.Properties.Add("Player", Match.SecondPlayer);
                    break;
                default:
                    break;
            }
            yield return nextScreen;
        }

        public IEnumerable<IResult> SaveMatchDetails()
        {
            //Save Match Details in MatchManager and go to Video Choice
            bool canNext = !String.IsNullOrWhiteSpace(Match.Tournament) && !String.IsNullOrWhiteSpace(Match.FirstPlayer.Name) && !String.IsNullOrWhiteSpace(Match.SecondPlayer.Name);

            if (!canNext)
            {
                var mySettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "OK",
                    AnimateShow = true,
                    AnimateHide = false
                };
                IDialogCoordinator coordinator = IoC.Get<IDialogCoordinator>();
                var context = new CoroutineExecutionContext()
                {
                    Target = this,
                    View = this.GetView() as DependencyObject,
                };
                coordinator.ShowMessageAsync(context.Target, "Missing Information",
                    "You have to provide at least a tournament name and names for both players to continue!",
                    MessageDialogStyle.Affirmative, mySettings);

                yield return new DoNothingResult();
            }
            else
            {

                MatchManager.MatchModified = true;
                NotifyOfPropertyChange("MatchModified");

                Rally first = new Rally();
                first.Number = 1;

                MatchManager.ActivePlaylist.Rallies.Add(first);
                first.UpdateServerAndScore();

                var nextScreen = ShowScreenResult.Of<VideoSourceViewModel>();
                yield return nextScreen;
            }
        }

        #endregion
    }
}
