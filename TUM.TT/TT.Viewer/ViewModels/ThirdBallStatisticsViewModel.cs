using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TT.Lib.Models;
using TT.Lib.Events;
using TT.Lib.Managers;

namespace TT.Viewer.ViewModels
{
    public class ThirdBallStatisticsViewModel : Conductor<IScreen>.Collection.AllActive,
        IHandle<BasicFilterSelectionChangedEvent>
    {

        #region Properties

        public BasicFilterStatisticsViewModel BasicFilterStatisticsView { get; set; }
        public string X { get; private set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        #endregion


        /// <summary>
        /// Gets the event bus of this shell.
        /// </summary>
        private IEventAggregator events;
        private IMatchManager Manager;

        public ThirdBallStatisticsViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            Manager = man;
            X = "";
            Player1 = "Spieler 1";
            Player2 = "Spieler 2";

            BasicFilterStatisticsView = new BasicFilterStatisticsViewModel(this.events, Manager)
            {
                MinRallyLength = 2,
                LastStroke = false,
                StrokeNumber = 2
            };
        }

        #region View Methods
        public void StatButtonClick(Grid parent, string btnName)
        {
            foreach (ToggleButton btn in parent.FindChildren<ToggleButton>())
            {
                if (btn.Name != btnName)
                    btn.IsChecked = false;
            }
        }
        public void SelectBasisInformation(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                X = source.Name;

            }
            else
            {
                X = "";
            }

            UpdateSelection(Manager.ActivePlaylist);

        }

        public void SelectPlacement(ToggleButton source)

        {

            if (source.IsChecked.Value)
            {
                X = source.Name;

            }
            else
            {
                X = "";
            }

            UpdateSelection(Manager.ActivePlaylist);
        }

        public void SelectContactPosition(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                X = source.Name;

            }
            else
            {
                X = "";
            }

            UpdateSelection(Manager.ActivePlaylist);

        }

        public void SelectTechnique(ToggleButton source)
        {
            if (source.IsChecked.Value)
            {
                X = source.Name;

            }
            else
            {
                X = "";
            }

            UpdateSelection(Manager.ActivePlaylist);
        }
        #endregion

        #region Caliburn Hooks

        /// <summary>
        /// Initializes this view model.
        /// </summary>

        protected override void OnActivate()
        {
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
            this.ActivateItem(BasicFilterStatisticsView);
            UpdateSelection(Manager.ActivePlaylist);
            Player1 = Manager.Match.FirstPlayer.Name.Split(' ')[0];
            Player2 = Manager.Match.SecondPlayer.Name.Split(' ')[0];
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            this.DeactivateItem(BasicFilterStatisticsView, close);
            // Unsubscribe ourself to the event bus
            this.events.Unsubscribe(this);
        }

        #endregion

        #region Event Handlers

        //FilterSelection in BasicFilter Changed
        //Get SelectedRallies and apply own filters
        public void Handle(BasicFilterSelectionChangedEvent message)
        {
            UpdateSelection(Manager.ActivePlaylist);
        }

        #endregion

        #region Helper Methods
        private void UpdateSelection(Playlist list)
        {
            if (list.Rallies != null)
            {
                var results = BasicFilterStatisticsView.SelectedRallies.Where(r => Convert.ToInt32(r.Length) > 2 && HasPlacement(r) && HasBasisInformation(r) && HasContactPosition(r)).ToList();
                this.events.PublishOnUIThread(new ResultsChangedEvent(results));
            }
        }
        public bool HasBasisInformation(Rally r)
        {
            switch (X)
            {
                case "":
                    return true;
                case "TotalThirdBallsCount":
                    return Convert.ToInt32(r.Length) >= 2;
                case "TotalThirdBallsCountPointPlayer1":
                    return Convert.ToInt32(r.Length) >= 2 && r.Winner == "First";
                case "TotalThirdBallsCountPointPlayer2":
                    return Convert.ToInt32(r.Length) >= 2 && r.Winner == "Second";
                default:
                    return true;

            }
        }

        public bool HasPlacement(Rally r)
        {
            switch (X)
            {
                case "":
                    return true;

                #region ForehandAll
                case "PlacementForehandAllTotalButton":
                    return r.Schlag[2].IsTopLeft() || r.Schlag[2].IsMidLeft() || r.Schlag[2].IsBotLeft();
                case "PlacementForehandAllPointsWonButton":
                    return (r.Schlag[2].IsTopLeft() || r.Schlag[2].IsMidLeft() || r.Schlag[2].IsBotLeft()) && r.Schlag[2].Spieler == r.Winner;
                case "PlacementForehandAllDirectPointsWonButton":
                    return (r.Schlag[2].IsTopLeft() || r.Schlag[2].IsMidLeft() || r.Schlag[2].IsBotLeft()) && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "PlacementForehandAllPointsLostButton":
                    return (r.Schlag[2].IsTopLeft() || r.Schlag[2].IsMidLeft() || r.Schlag[2].IsBotLeft()) && r.Schlag[2].Spieler != r.Winner;
                #endregion
                #region ForehandLong
                case "PlacementForehandLongTotalButton":
                    return r.Schlag[2].IsTopLeft();
                case "PlacementForehandLongPointsWonButton":
                    return r.Schlag[2].IsTopLeft() && r.Schlag[2].Spieler == r.Winner;
                case "PlacementForehandLongDirectPointsWonButton":
                    return r.Schlag[2].IsTopLeft() && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "PlacementForehandLongPointsLostButton":
                    return r.Schlag[2].IsTopLeft() && r.Schlag[2].Spieler != r.Winner;
                #endregion
                #region ForehandHalfLong
                case "PlacementForehandHalfLongTotalButton":
                    return r.Schlag[2].IsMidLeft();
                case "PlacementForehandHalfLongPointsWonButton":
                    return r.Schlag[2].IsMidLeft() && r.Schlag[2].Spieler == r.Winner;
                case "PlacementForehandHalfLongDirectPointsWonButton":
                    return r.Schlag[2].IsMidLeft() && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "PlacementForehandHalfLongPointsLostButton":
                    return r.Schlag[2].IsMidLeft() && r.Schlag[2].Spieler != r.Winner;
                #endregion
                #region ForehandShort
                case "PlacementForehandShortTotalButton":
                    return r.Schlag[2].IsBotLeft();
                case "PlacementForehandShortPointsWonButton":
                    return r.Schlag[2].IsBotLeft() && r.Schlag[2].Spieler == r.Winner;
                case "PlacementForehandShortDirectPointsWonButton":
                    return r.Schlag[2].IsBotLeft() && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "PlacementForehandShortPointsLostButton":
                    return r.Schlag[2].IsBotLeft() && r.Schlag[2].Spieler != r.Winner;
                #endregion
                #region MiddleAll
                case "PlacementMiddleAllTotalButton":
                    return r.Schlag[2].IsTopMid() || r.Schlag[2].IsMidMid() || r.Schlag[2].IsBotMid();
                case "PlacementMiddleAllPointsWonButton":
                    return (r.Schlag[2].IsTopMid() || r.Schlag[2].IsMidMid() || r.Schlag[2].IsBotMid()) && r.Schlag[2].Spieler == r.Winner;
                case "PlacementMiddleAllDirectPointsWonButton":
                    return (r.Schlag[2].IsTopMid() || r.Schlag[2].IsMidMid() || r.Schlag[2].IsBotMid()) && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "PlacementMiddleAllPointsLostButton":
                    return (r.Schlag[2].IsTopMid() || r.Schlag[2].IsMidMid() || r.Schlag[2].IsBotMid()) && r.Schlag[2].Spieler != r.Winner;
                #endregion
                #region MiddleLong
                case "PlacementMiddleLongTotalButton":
                    return r.Schlag[2].IsTopMid();
                case "PlacementMiddleLongPointsWonButton":
                    return r.Schlag[2].IsTopMid() && r.Schlag[2].Spieler == r.Winner;
                case "PlacementMiddleLongDirectPointsWonButton":
                    return r.Schlag[2].IsTopMid() && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "PlacementMiddleLongPointsLostButton":
                    return r.Schlag[2].IsTopMid() && r.Schlag[2].Spieler != r.Winner;
                #endregion
                #region MiddleHalfLong
                case "PlacementMiddleHalfLongTotalButton":
                    return r.Schlag[2].IsMidMid();
                case "PlacementMiddleHalfLongPointsWonButton":
                    return r.Schlag[2].IsMidMid() && r.Schlag[2].Spieler == r.Winner;
                case "PlacementMiddleHalfLongDirectPointsWonButton":
                    return r.Schlag[2].IsMidMid() && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "PlacementMiddleHalfLongPointsLostButton":
                    return r.Schlag[2].IsMidMid() && r.Schlag[2].Spieler != r.Winner;
                #endregion
                #region MiddleShort
                case "PlacementMiddleShortTotalButton":
                    return r.Schlag[2].IsBotMid();
                case "PlacementMiddleShortPointsWonButton":
                    return r.Schlag[2].IsBotMid() && r.Schlag[2].Spieler == r.Winner;
                case "PlacementMiddleShortDirectPointsWonButton":
                    return r.Schlag[2].IsBotMid() && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "PlacementMiddleShortPointsLostButton":
                    return r.Schlag[2].IsBotMid() && r.Schlag[2].Spieler != r.Winner;
                #endregion
                #region BackhandAll
                case "PlacementBackhandAllTotalButton":
                    return r.Schlag[2].IsTopRight() || r.Schlag[2].IsMidRight() || r.Schlag[2].IsBotRight();
                case "PlacementBackhandAllPointsWonButton":
                    return (r.Schlag[2].IsTopRight() || r.Schlag[2].IsMidRight() || r.Schlag[2].IsBotRight()) && r.Schlag[2].Spieler == r.Winner;
                case "PlacementBackhandAllDirectPointsWonButton":
                    return (r.Schlag[2].IsTopRight() || r.Schlag[2].IsMidRight() || r.Schlag[2].IsBotRight()) && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "PlacementBackhandAllPointsLostButton":
                    return (r.Schlag[2].IsTopRight() || r.Schlag[2].IsMidRight() || r.Schlag[2].IsBotRight()) && r.Schlag[2].Spieler != r.Winner;
                #endregion
                #region BackhandLong
                case "PlacementBackhandLongTotalButton":
                    return r.Schlag[2].IsTopRight();
                case "PlacementBackhandLongPointsWonButton":
                    return r.Schlag[2].IsTopRight() && r.Schlag[2].Spieler == r.Winner;
                case "PlacementBackhandLongDirectPointsWonButton":
                    return r.Schlag[2].IsTopRight() && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "PlacementBackhandLongPointsLostButton":
                    return r.Schlag[2].IsTopRight() && r.Schlag[2].Spieler != r.Winner;
                #endregion
                #region BackhandHalfLong
                case "PlacementBackhandHalfLongTotalButton":
                    return r.Schlag[2].IsMidRight();
                case "PlacementBackhandHalfLongPointsWonButton":
                    return r.Schlag[2].IsMidRight() && r.Schlag[2].Spieler == r.Winner;
                case "PlacementBackhandHalfLongDirectPointsWonButton":
                    return r.Schlag[2].IsMidRight() && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "PlacementBackhandHalfLongPointsLostButton":
                    return r.Schlag[2].IsMidRight() && r.Schlag[2].Spieler != r.Winner;
                #endregion
                #region BackhandShort
                case "PlacementBackhandShortTotalButton":
                    return r.Schlag[2].IsBotRight();
                case "PlacementBackhandShortPointsWonButton":
                    return r.Schlag[2].IsBotRight() && r.Schlag[2].Spieler == r.Winner;
                case "PlacementBackhandShortDirectPointsWonButton":
                    return r.Schlag[2].IsBotRight() && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "PlacementBackhandShortPointsLostButton":
                    return r.Schlag[2].IsBotRight() && r.Schlag[2].Spieler != r.Winner;
                #endregion
                #region AllLong
                case "PlacementAllLongTotalButton":
                    return (r.Schlag[2].IsTopLeft() || r.Schlag[2].IsTopMid() || r.Schlag[2].IsTopRight());
                case "PlacementAllLongPointsWonButton":
                    return (r.Schlag[2].IsTopLeft() || r.Schlag[2].IsTopMid() || r.Schlag[2].IsTopRight()) && r.Schlag[2].Spieler == r.Winner;
                case "PlacementAllLongDirectPointsWonButton":
                    return (r.Schlag[2].IsTopLeft() || r.Schlag[2].IsTopMid() || r.Schlag[2].IsTopRight()) && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "PlacementAllLongPointsLostButton":
                    return (r.Schlag[2].IsTopLeft() || r.Schlag[2].IsTopMid() || r.Schlag[2].IsTopRight()) && r.Schlag[2].Spieler != r.Winner;
                #endregion
                #region AllHalfLong
                case "PlacementAllHalfLongTotalButton":
                    return (r.Schlag[2].IsMidLeft() || r.Schlag[2].IsMidMid() || r.Schlag[2].IsMidRight());
                case "PlacementAllHalfLongPointsWonButton":
                    return (r.Schlag[2].IsMidLeft() || r.Schlag[2].IsMidMid() || r.Schlag[2].IsMidRight()) && r.Schlag[2].Spieler == r.Winner;
                case "PlacementAllHalfLongDirectPointsWonButton":
                    return (r.Schlag[2].IsMidLeft() || r.Schlag[2].IsMidMid() || r.Schlag[2].IsMidRight()) && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "PlacementAllHalfLongPointsLostButton":
                    return (r.Schlag[2].IsMidLeft() || r.Schlag[2].IsMidMid() || r.Schlag[2].IsMidRight()) && r.Schlag[2].Spieler != r.Winner;
                #endregion
                #region AllShort
                case "PlacementAllShortTotalButton":
                    return (r.Schlag[2].IsBotLeft() || r.Schlag[2].IsBotMid() || r.Schlag[2].IsBotRight());
                case "PlacementAllShortPointsWonButton":
                    return (r.Schlag[2].IsBotLeft() || r.Schlag[2].IsBotMid() || r.Schlag[2].IsBotRight()) && r.Schlag[2].Spieler == r.Winner;
                case "PlacementAllShortDirectPointsWonButton":
                    return (r.Schlag[2].IsBotLeft() || r.Schlag[2].IsBotMid() || r.Schlag[2].IsBotRight()) && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "PlacementAllShortPointsLostButton":
                    return (r.Schlag[2].IsBotLeft() || r.Schlag[2].IsBotMid() || r.Schlag[2].IsBotRight()) && r.Schlag[2].Spieler != r.Winner;
                #endregion

                #region ReceiveErrors
                case "PlacementAllServiceErrorsTotalButton":
                    return r.Server != r.Winner && r.Length == 3;
                #endregion
                default:
                    return true;


            }


        }

        public bool HasContactPosition(Rally r)
        {
            switch (X)
            {
                case "":
                    return true;

                #region Over the table
                case "OverTheTableTotalButton":
                    return r.Schlag[2].Balltreffpunkt == "über";
                case "OverTheTablePointsWonButton":
                    return r.Schlag[2].Balltreffpunkt == "über" && r.Schlag[2].Spieler == r.Winner;
                case "OverTheTableDirectPointsWonButton":
                    return r.Schlag[2].Balltreffpunkt == "über" && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "OverTheTablePointsLostButton":
                    return r.Schlag[2].Balltreffpunkt == "über" && r.Schlag[2].Spieler != r.Winner;

                #endregion

                #region at the table
                case "AtTheTableTotalButton":
                    return r.Schlag[2].Balltreffpunkt == "hinter";
                case "AtTheTablePointsWonButton":
                    return r.Schlag[2].Balltreffpunkt == "hinter" && r.Schlag[2].Spieler == r.Winner;
                case "AtTheTableDirectPointsWonButton":
                    return r.Schlag[2].Balltreffpunkt == "hinter" && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "AtTheTablePointsLostButton":
                    return r.Schlag[2].Balltreffpunkt == "hinter" && r.Schlag[2].Spieler != r.Winner;

                #endregion

                #region half distance
                case "HaldDistanceTotalButton":
                    return r.Schlag[2].Balltreffpunkt == "Halbdistanz";
                case "HaldDistancePointsWonButton":
                    return r.Schlag[2].Balltreffpunkt == "Halbdistanz" && r.Schlag[2].Spieler == r.Winner;
                case "HaldDistanceDirectPointsWonButton":
                    return r.Schlag[2].Balltreffpunkt == "Halbdistanz" && r.Schlag[2].Spieler == r.Winner && Convert.ToInt32(r.Length) < 5;
                case "HaldDistancePointsLostButton":
                    return r.Schlag[2].Balltreffpunkt == "Halbdistanz" && r.Schlag[2].Spieler != r.Winner;

                #endregion

                default:
                    return true;

            }

        }


#endregion
    }
}
