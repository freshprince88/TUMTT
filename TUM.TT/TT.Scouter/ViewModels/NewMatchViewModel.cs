using Caliburn.Micro;
using System;
using System.Collections.Generic;
using TT.Models.Managers;
using TT.Models;
using TT.Models.Results;

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
            //TODO: Save Match Details in MatchManager and go to Video Choice
            MatchManager.MatchModified = true;

            Rally first = new Rally();
            first.Nummer = 1;

            MatchManager.ActivePlaylist.Rallies.Add(first);
            first.UpdateServerAndScore();

            var nextScreen = ShowScreenResult.Of<VideoSourceViewModel>();
            yield return nextScreen;
        }

        #endregion
    }
}
