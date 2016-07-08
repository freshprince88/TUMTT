using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TT.Models;
using TT.Lib.Events;
using TT.Lib.Managers;

namespace TT.Viewer.ViewModels
{
    public class ServiceStatisticsViewModel : Conductor<IScreen>.Collection.AllActive,
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

        public ServiceStatisticsViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            Manager = man;
            X = "";
            Player1 = "Player 1";
            Player2 = "Player 2";

            BasicFilterStatisticsView = new BasicFilterStatisticsViewModel(this.events, Manager)
            {
                MinRallyLength = 0,
                LastStroke = false,
                StrokeNumber = 0

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

        public void SelectPosition(ToggleButton source)
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

        public void SelectService(ToggleButton source)
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

        public void SelectSpin(ToggleButton source)
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

        protected override void OnActivate()
        {
            this.events.Subscribe(this);
            base.OnActivate();
            // Subscribe ourself to the event bus            
            this.ActivateItem(BasicFilterStatisticsView);
            Player1 = Manager.Match.FirstPlayer.Name.Split(' ')[0];
            Player2 = Manager.Match.SecondPlayer.Name.Split(' ')[0];
            NotifyOfPropertyChange("BasicFilterStatisticsView");
            NotifyOfPropertyChange("BasicFilterStatisticsView.SelectedRallies");
            NotifyOfPropertyChange("Manager.ActivePlaylist");
            this.Refresh();


        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            UpdateSelection(Manager.ActivePlaylist);
            NotifyOfPropertyChange("BasicFilterStatisticsView.SelectedRallies");

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
                var results = BasicFilterStatisticsView.SelectedRallies.Where(r =>
                r.HasPlacementStatistics(0, X) &&
                r.HasServiceStatistics(X) &&
                r.HasServerPositionStatistics(X) &&
                r.HasBasisInformationStatistics(1, X) &&
                r.HasSpinStatistics(X)).ToList();
                
                Manager.SelectedRallies = results;

            }
        }

        public bool HasBasisInformation(Rally r)
        {
            switch (X)
            {
                case "":
                    return true;
                case "TotalServicesCount":
                    return r.Length >= 1;
                case "TotalServicesCountPointPlayer1":
                    return r.Length >= 1 && r.Winner == MatchPlayer.First;
                case "TotalServicesCountPointPlayer2":
                    return r.Length >= 1 && r.Winner == MatchPlayer.Second;
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
                    return r.Strokes[0].IsTopLeft() || r.Strokes[0].IsMidLeft() || r.Strokes[0].IsBotLeft();
                case "PlacementForehandAllPointsWonButton":
                    return (r.Strokes[0].IsTopLeft() || r.Strokes[0].IsMidLeft() || r.Strokes[0].IsBotLeft()) && r.Strokes[0].Player == r.Winner;
                case "PlacementForehandAllDirectPointsWonButton":
                    return (r.Strokes[0].IsTopLeft() || r.Strokes[0].IsMidLeft() || r.Strokes[0].IsBotLeft()) && r.Strokes[0].Player == r.Winner && r.Length < 3;
                case "PlacementForehandAllPointsLostButton":
                    return (r.Strokes[0].IsTopLeft() || r.Strokes[0].IsMidLeft() || r.Strokes[0].IsBotLeft()) && r.Strokes[0].Player != r.Winner;
                #endregion
                #region ForehandLong
                case "PlacementForehandLongTotalButton":
                    return r.Strokes[0].IsTopLeft();
                case "PlacementForehandLongPointsWonButton":
                    return r.Strokes[0].IsTopLeft() && r.Strokes[0].Player == r.Winner;
                case "PlacementForehandLongDirectPointsWonButton":
                    return r.Strokes[0].IsTopLeft() && r.Strokes[0].Player == r.Winner && r.Length < 3;
                case "PlacementForehandLongPointsLostButton":
                    return r.Strokes[0].IsTopLeft() && r.Strokes[0].Player != r.Winner;
                #endregion
                #region ForehandHalfLong
                case "PlacementForehandHalfLongTotalButton":
                    return r.Strokes[0].IsMidLeft();
                case "PlacementForehandHalfLongPointsWonButton":
                    return r.Strokes[0].IsMidLeft() && r.Strokes[0].Player == r.Winner;
                case "PlacementForehandHalfLongDirectPointsWonButton":
                    return r.Strokes[0].IsMidLeft() && r.Strokes[0].Player == r.Winner && r.Length < 3;
                case "PlacementForehandHalfLongPointsLostButton":
                    return r.Strokes[0].IsMidLeft() && r.Strokes[0].Player != r.Winner;
                #endregion
                #region ForehandShort
                case "PlacementForehandShortTotalButton":
                    return r.Strokes[0].IsBotLeft();
                case "PlacementForehandShortPointsWonButton":
                    return r.Strokes[0].IsBotLeft() && r.Strokes[0].Player == r.Winner;
                case "PlacementForehandShortDirectPointsWonButton":
                    return r.Strokes[0].IsBotLeft() && r.Strokes[0].Player == r.Winner && r.Length < 3;
                case "PlacementForehandShortPointsLostButton":
                    return r.Strokes[0].IsBotLeft() && r.Strokes[0].Player != r.Winner;
                #endregion

                #region MiddleAll
                case "PlacementMiddleAllTotalButton":
                    return r.Strokes[0].IsTopMid() || r.Strokes[0].IsMidMid() || r.Strokes[0].IsBotMid();
                case "PlacementMiddleAllPointsWonButton":
                    return (r.Strokes[0].IsTopMid() || r.Strokes[0].IsMidMid() || r.Strokes[0].IsBotMid()) && r.Strokes[0].Player == r.Winner;
                case "PlacementMiddleAllDirectPointsWonButton":
                    return (r.Strokes[0].IsTopMid() || r.Strokes[0].IsMidMid() || r.Strokes[0].IsBotMid()) && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementMiddleAllPointsLostButton":
                    return (r.Strokes[0].IsTopMid() || r.Strokes[0].IsMidMid() || r.Strokes[0].IsBotMid()) && r.Strokes[0].Player != r.Winner;
                #endregion

                #region MiddleLong
                case "PlacementMiddleLongTotalButton":
                    return r.Strokes[0].IsTopMid();
                case "PlacementMiddleLongPointsWonButton":
                    return r.Strokes[0].IsTopMid() && r.Strokes[0].Player == r.Winner;
                case "PlacementMiddleLongDirectPointsWonButton":
                    return r.Strokes[0].IsTopMid() && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementMiddleLongPointsLostButton":
                    return r.Strokes[0].IsTopMid() && r.Strokes[0].Player != r.Winner;
                #endregion

                #region MiddleHalfLong
                case "PlacementMiddleHalfLongTotalButton":
                    return r.Strokes[0].IsMidMid();
                case "PlacementMiddleHalfLongPointsWonButton":
                    return r.Strokes[0].IsMidMid() && r.Strokes[0].Player == r.Winner;
                case "PlacementMiddleHalfLongDirectPointsWonButton":
                    return r.Strokes[0].IsMidMid() && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementMiddleHalfLongPointsLostButton":
                    return r.Strokes[0].IsMidMid() && r.Strokes[0].Player != r.Winner;
                #endregion

                #region MiddleShort
                case "PlacementMiddleShortTotalButton":
                    return r.Strokes[0].IsBotMid();
                case "PlacementMiddleShortPointsWonButton":
                    return r.Strokes[0].IsBotMid() && r.Strokes[0].Player == r.Winner;
                case "PlacementMiddleShortDirectPointsWonButton":
                    return r.Strokes[0].IsBotMid() && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementMiddleShortPointsLostButton":
                    return r.Strokes[0].IsBotMid() && r.Strokes[0].Player != r.Winner;
                #endregion

                #region BackhandAll
                case "PlacementBackhandAllTotalButton":
                    return r.Strokes[0].IsTopRight() || r.Strokes[0].IsMidRight() || r.Strokes[0].IsBotRight();
                case "PlacementBackhandAllPointsWonButton":
                    return (r.Strokes[0].IsTopRight() || r.Strokes[0].IsMidRight() || r.Strokes[0].IsBotRight()) && r.Strokes[0].Player == r.Winner;
                case "PlacementBackhandAllDirectPointsWonButton":
                    return (r.Strokes[0].IsTopRight() || r.Strokes[0].IsMidRight() || r.Strokes[0].IsBotRight()) && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementBackhandAllPointsLostButton":
                    return (r.Strokes[0].IsTopRight() || r.Strokes[0].IsMidRight() || r.Strokes[0].IsBotRight()) && r.Strokes[0].Player != r.Winner;
                #endregion

                #region BackhandLong
                case "PlacementBackhandLongTotalButton":
                    return r.Strokes[0].IsTopRight();
                case "PlacementBackhandLongPointsWonButton":
                    return r.Strokes[0].IsTopRight() && r.Strokes[0].Player == r.Winner;
                case "PlacementBackhandLongDirectPointsWonButton":
                    return r.Strokes[0].IsTopRight() && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementBackhandLongPointsLostButton":
                    return r.Strokes[0].IsTopRight() && r.Strokes[0].Player != r.Winner;
                #endregion

                #region BackhandHalfLong
                case "PlacementBackhandHalfLongTotalButton":
                    return r.Strokes[0].IsMidRight();
                case "PlacementBackhandHalfLongPointsWonButton":
                    return r.Strokes[0].IsMidRight() && r.Strokes[0].Player == r.Winner;
                case "PlacementBackhandHalfLongDirectPointsWonButton":
                    return r.Strokes[0].IsMidRight() && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementBackhandHalfLongPointsLostButton":
                    return r.Strokes[0].IsMidRight() && r.Strokes[0].Player != r.Winner;
                #endregion

                #region BackhandShort
                case "PlacementBackhandShortTotalButton":
                    return r.Strokes[0].IsBotRight();
                case "PlacementBackhandShortPointsWonButton":
                    return r.Strokes[0].IsBotRight() && r.Strokes[0].Player == r.Winner;
                case "PlacementBackhandShortDirectPointsWonButton":
                    return r.Strokes[0].IsBotRight() && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementBackhandShortPointsLostButton":
                    return r.Strokes[0].IsBotRight() && r.Strokes[0].Player != r.Winner;
                #endregion

                #region AllLong
                case "PlacementAllLongTotalButton":
                    return (r.Strokes[0].IsTopLeft() || r.Strokes[0].IsTopMid() || r.Strokes[0].IsTopRight());
                case "PlacementAllLongPointsWonButton":
                    return (r.Strokes[0].IsTopLeft() || r.Strokes[0].IsTopMid() || r.Strokes[0].IsTopRight()) && r.Strokes[0].Player == r.Winner;
                case "PlacementAllLongDirectPointsWonButton":
                    return (r.Strokes[0].IsTopLeft() || r.Strokes[0].IsTopMid() || r.Strokes[0].IsTopRight()) && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementAllLongPointsLostButton":
                    return (r.Strokes[0].IsTopLeft() || r.Strokes[0].IsTopMid() || r.Strokes[0].IsTopRight()) && r.Strokes[0].Player != r.Winner;
                #endregion

                #region AllHalfLong
                case "PlacementAllHalfLongTotalButton":
                    return (r.Strokes[0].IsMidLeft() || r.Strokes[0].IsMidMid() || r.Strokes[0].IsMidRight());
                case "PlacementAllHalfLongPointsWonButton":
                    return (r.Strokes[0].IsMidLeft() || r.Strokes[0].IsMidMid() || r.Strokes[0].IsMidRight()) && r.Strokes[0].Player == r.Winner;
                case "PlacementAllHalfLongDirectPointsWonButton":
                    return (r.Strokes[0].IsMidLeft() || r.Strokes[0].IsMidMid() || r.Strokes[0].IsMidRight()) && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementAllHalfLongPointsLostButton":
                    return (r.Strokes[0].IsMidLeft() || r.Strokes[0].IsMidMid() || r.Strokes[0].IsMidRight()) && r.Strokes[0].Player != r.Winner;
                #endregion

                #region AllShort
                case "PlacementAllShortTotalButton":
                    return (r.Strokes[0].IsBotLeft() || r.Strokes[0].IsBotMid() || r.Strokes[0].IsBotRight());
                case "PlacementAllShortPointsWonButton":
                    return (r.Strokes[0].IsBotLeft() || r.Strokes[0].IsBotMid() || r.Strokes[0].IsBotRight()) && r.Strokes[0].Player == r.Winner;
                case "PlacementAllShortDirectPointsWonButton":
                    return (r.Strokes[0].IsBotLeft() || r.Strokes[0].IsBotMid() || r.Strokes[0].IsBotRight()) && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementAllShortPointsLostButton":
                    return (r.Strokes[0].IsBotLeft() || r.Strokes[0].IsBotMid() || r.Strokes[0].IsBotRight()) && r.Strokes[0].Player != r.Winner;
                #endregion

                #region ServiceErrors
                case "PlacementAllServiceErrorsTotalButton":
                    return r.Server != r.Winner && r.Length == 1;
                #endregion
                default:
                    return true;


            }


        }

        public bool HasPosition(Rally r)
        {
            switch (X)
            {
                case "":
                    return true;

                #region Position Left
                case "PositionLeftTotalButton":
                    return r.Strokes[0].IsLeftServicePosition();
                case "PositionLeftPointsWonButton":
                    return r.Strokes[0].IsLeftServicePosition() && r.Strokes[0].Player == r.Winner;
                case "PositionLeftDirectPointsWonButton":
                    return r.Strokes[0].IsLeftServicePosition() && r.Strokes[0].Player == r.Winner && r.Length < 3;
                case "PositionLeftPointsLostButton":
                    return r.Strokes[0].IsLeftServicePosition() && r.Strokes[0].Player != r.Winner;
                #endregion
                #region Position Middle
                case "PositionMiddleTotalButton":
                    return r.Strokes[0].IsMiddleServicePosition();
                case "PositionMiddlePointsWonButton":
                    return r.Strokes[0].IsMiddleServicePosition() && r.Strokes[0].Player == r.Winner;
                case "PositionMiddleDirectPointsWonButton":
                    return r.Strokes[0].IsMiddleServicePosition() && r.Strokes[0].Player == r.Winner && r.Length < 3;
                case "PositionMiddlePointsLostButton":
                    return r.Strokes[0].IsMiddleServicePosition() && r.Strokes[0].Player != r.Winner;
                #endregion
                #region Position Right
                case "PositionRightTotalButton":
                    return r.Strokes[0].IsRightServicePosition();
                case "PositionRightPointsWonButton":
                    return r.Strokes[0].IsRightServicePosition() && r.Strokes[0].Player == r.Winner;
                case "PositionRightDirectPointsWonButton":
                    return r.Strokes[0].IsRightServicePosition() && r.Strokes[0].Player == r.Winner && r.Length < 3;
                case "PositionRightPointsLostButton":
                    return r.Strokes[0].IsRightServicePosition() && r.Strokes[0].Player != r.Winner;
                #endregion

                default:
                    return true;
            }


        }

        public bool HasServices(Rally r)
        {
            switch (X)
            {
                case "":
                    return true;
                #region Pendulum
                case "ForehandPendulumTotalButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Pendulum";
                case "ForehandPendulumPointsWonButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Pendulum" && r.Strokes[0].Player == r.Winner;
                case "ForehandPendulumDirectPointsWonButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Pendulum" && r.Strokes[0].Player == r.Winner && r.Length < 3;
                case "ForehandPendulumPointsLostButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Pendulum" && r.Strokes[0].Player != r.Winner;
                case "BackhandPendulumTotalButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Pendulum";
                case "BackhandPendulumPointsWonButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Pendulum" && r.Strokes[0].Player == r.Winner;
                case "BackhandPendulumDirectPointsWonButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Pendulum" && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "BackhandPendulumPointsLostButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Pendulum" && r.Strokes[0].Player != r.Winner;
                case "AllPendulumTotalButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Pendulum";
                case "AllPendulumPointsWonButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Pendulum" && r.Strokes[0].Player == r.Winner;
                case "AllPendulumDirectPointsWonButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Pendulum" && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllPendulumPointsLostButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Pendulum" && r.Strokes[0].Player != r.Winner;

                #endregion

                #region ReversePendulum
                case "ForehandReversePendulumTotalButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Reverse";
                case "ForehandReversePendulumPointsWonButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Reverse" && r.Strokes[0].Player == r.Winner;
                case "ForehandReversePendulumDirectPointsWonButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Reverse" && r.Strokes[0].Player == r.Winner && r.Length < 3;
                case "ForehandReversePendulumPointsLostButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Reverse" && r.Strokes[0].Player != r.Winner;
                case "BackhandReversePendulumTotalButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Reverse";
                case "BackhandReversePendulumPointsWonButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Reverse" && r.Strokes[0].Player == r.Winner;
                case "BackhandReversePendulumDirectPointsWonButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Reverse" && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "BackhandReversePendulumPointsLostButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Reverse" && r.Strokes[0].Player != r.Winner;
                case "AllReversePendulumTotalButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Reverse";
                case "AllReversePendulumPointsWonButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Reverse" && r.Strokes[0].Player == r.Winner;
                case "AllReversePendulumDirectPointsWonButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Reverse" && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllReversePendulumPointsLostButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Reverse" && r.Strokes[0].Player != r.Winner;

                #endregion

                #region Tomahawk
                case "ForehandTomahawkTotalButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Tomahawk";
                case "ForehandTomahawkPointsWonButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Tomahawk" && r.Strokes[0].Player == r.Winner;
                case "ForehandTomahawkDirectPointsWonButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Tomahawk" && r.Strokes[0].Player == r.Winner && r.Length < 3;
                case "ForehandTomahawkPointsLostButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Tomahawk" && r.Strokes[0].Player != r.Winner;
                case "BackhandTomahawkTotalButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Tomahawk";
                case "BackhandTomahawkPointsWonButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Tomahawk" && r.Strokes[0].Player == r.Winner;
                case "BackhandTomahawkDirectPointsWonButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Tomahawk" && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "BackhandTomahawkPointsLostButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Tomahawk" && r.Strokes[0].Player != r.Winner;
                case "AllTomahawkTotalButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Tomahawk";
                case "AllTomahawkPointsWonButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Tomahawk" && r.Strokes[0].Player == r.Winner;
                case "AllTomahawkDirectPointsWonButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Tomahawk" && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllTomahawkPointsLostButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Tomahawk" && r.Strokes[0].Player != r.Winner;

                #endregion

                #region Special
                case "ForehandSpecialTotalButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Special";
                case "ForehandSpecialPointsWonButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Special" && r.Strokes[0].Player == r.Winner;
                case "ForehandSpecialDirectPointsWonButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Special" && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "ForehandSpecialPointsLostButton":
                    return r.Strokes[0].Side == "Forehand" && r.Strokes[0].Servicetechnique == "Special" && r.Strokes[0].Player != r.Winner;
                case "BackhandSpecialTotalButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Special";
                case "BackhandSpecialPointsWonButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Special" && r.Strokes[0].Player == r.Winner;
                case "BackhandSpecialDirectPointsWonButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Special" && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "BackhandSpecialPointsLostButton":
                    return r.Strokes[0].Side == "Backhand" && r.Strokes[0].Servicetechnique == "Special" && r.Strokes[0].Player != r.Winner;
                case "AllSpecialTotalButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Special";
                case "AllSpecialPointsWonButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Special" && r.Strokes[0].Player == r.Winner;
                case "AllSpecialDirectPointsWonButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Special" && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllSpecialPointsLostButton":
                    return (r.Strokes[0].Side == "Forehand" || r.Strokes[0].Side == "Backhand") && r.Strokes[0].Servicetechnique == "Special" && r.Strokes[0].Player != r.Winner;

                #endregion

                #region All Forehand Services
                case "ForehandAllTotalButton":
                    return r.Strokes[0].Side == "Forehand" && (r.Strokes[0].Servicetechnique == "Pendulum" || r.Strokes[0].Servicetechnique == "Reverse" || r.Strokes[0].Servicetechnique == "Tomahawk" || r.Strokes[0].Servicetechnique == "Special");
                case "ForehandAllPointsWonButton":
                    return r.Strokes[0].Side == "Forehand" && (r.Strokes[0].Servicetechnique == "Pendulum" || r.Strokes[0].Servicetechnique == "Reverse" || r.Strokes[0].Servicetechnique == "Tomahawk" || r.Strokes[0].Servicetechnique == "Special") && r.Strokes[0].Player == r.Winner;
                case "ForehandAllDirectPointsWonButton":
                    return r.Strokes[0].Side == "Forehand" && (r.Strokes[0].Servicetechnique == "Pendulum" || r.Strokes[0].Servicetechnique == "Reverse" || r.Strokes[0].Servicetechnique == "Tomahawk" || r.Strokes[0].Servicetechnique == "Special") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "ForehandAllPointsLostButton":
                    return r.Strokes[0].Side == "Forehand" && (r.Strokes[0].Servicetechnique == "Pendulum" || r.Strokes[0].Servicetechnique == "Reverse" || r.Strokes[0].Servicetechnique == "Tomahawk" || r.Strokes[0].Servicetechnique == "Special") && r.Strokes[0].Player != r.Winner;
                #endregion

                #region All Backhand Services
                case "BackhandAllTotalButton":
                    return r.Strokes[0].Side == "Backhand" && (r.Strokes[0].Servicetechnique == "Pendulum" || r.Strokes[0].Servicetechnique == "Reverse" || r.Strokes[0].Servicetechnique == "Tomahawk" || r.Strokes[0].Servicetechnique == "Special");
                case "BackhandAllPointsWonButton":
                    return r.Strokes[0].Side == "Backhand" && (r.Strokes[0].Servicetechnique == "Pendulum" || r.Strokes[0].Servicetechnique == "Reverse" || r.Strokes[0].Servicetechnique == "Tomahawk" || r.Strokes[0].Servicetechnique == "Special") && r.Strokes[0].Player == r.Winner;
                case "BackhandAllDirectPointsWonButton":
                    return r.Strokes[0].Side == "Backhand" && (r.Strokes[0].Servicetechnique == "Pendulum" || r.Strokes[0].Servicetechnique == "Reverse" || r.Strokes[0].Servicetechnique == "Tomahawk" || r.Strokes[0].Servicetechnique == "Special") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "BackhandAllPointsLostButton":
                    return r.Strokes[0].Side == "Backhand" && (r.Strokes[0].Servicetechnique == "Pendulum" || r.Strokes[0].Servicetechnique == "Reverse" || r.Strokes[0].Servicetechnique == "Tomahawk" || r.Strokes[0].Servicetechnique == "Special") && r.Strokes[0].Player != r.Winner;
                #endregion


                #region All Services
                case "AllServicesTotalButton":
                    return (r.Strokes[0].Side == "Backhand" || r.Strokes[0].Side == "Forehand") && (r.Strokes[0].Servicetechnique == "Pendulum" || r.Strokes[0].Servicetechnique == "Reverse" || r.Strokes[0].Servicetechnique == "Tomahawk" || r.Strokes[0].Servicetechnique == "Special");
                case "AllServicesPointsWonButton":
                    return (r.Strokes[0].Side == "Backhand" || r.Strokes[0].Side == "Forehand") && (r.Strokes[0].Servicetechnique == "Pendulum" || r.Strokes[0].Servicetechnique == "Reverse" || r.Strokes[0].Servicetechnique == "Tomahawk" || r.Strokes[0].Servicetechnique == "Special") && r.Strokes[0].Player == r.Winner;
                case "AllServicesDirectPointsWonButton":
                    return (r.Strokes[0].Side == "Backhand" || r.Strokes[0].Side == "Forehand") && (r.Strokes[0].Servicetechnique == "Pendulum" || r.Strokes[0].Servicetechnique == "Reverse" || r.Strokes[0].Servicetechnique == "Tomahawk" || r.Strokes[0].Servicetechnique == "Special") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllServicesPointsLostButton":
                    return (r.Strokes[0].Side == "Backhand" || r.Strokes[0].Side == "Forehand") && (r.Strokes[0].Servicetechnique == "Pendulum" || r.Strokes[0].Servicetechnique == "Reverse" || r.Strokes[0].Servicetechnique == "Tomahawk" || r.Strokes[0].Servicetechnique == "Special") && r.Strokes[0].Player != r.Winner;
                #endregion
                default:
                    return true;
            }
        }

        public bool HasSpin(Rally r)
        {
            switch (X)
            {
                case "":
                    return true;
                #region UpSpin

                case "UpSideLeftTotalButton":
                    return (r.Strokes[0].Spin.SL == "1" && r.Strokes[0].Spin.TS == "1");
                case "UpSideLeftPointsWonButton":
                    return (r.Strokes[0].Spin.SL == "1" && r.Strokes[0].Spin.TS == "1") && r.Strokes[0].Player == r.Winner;
                case "UpSideLeftDirectPointsWonButton":
                    return (r.Strokes[0].Spin.SL == "1" && r.Strokes[0].Spin.TS == "1") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "UpSideLeftPointsLostButton":
                    return (r.Strokes[0].Spin.SL == "1" && r.Strokes[0].Spin.TS == "1") && r.Strokes[0].Player != r.Winner;

                case "UpTotalButton":
                    return (r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0" && r.Strokes[0].Spin.TS == "1");
                case "UpPointsWonButton":
                    return (r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0" && r.Strokes[0].Spin.TS == "1") && r.Strokes[0].Player == r.Winner;
                case "UpDirectPointsWonButton":
                    return (r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0" && r.Strokes[0].Spin.TS == "1") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "UpPointsLostButton":
                    return (r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0" && r.Strokes[0].Spin.TS == "1") && r.Strokes[0].Player != r.Winner;

                case "UpSideRightTotalButton":
                    return (r.Strokes[0].Spin.SR == "1" && r.Strokes[0].Spin.TS == "1");
                case "UpSideRightPointsWonButton":
                    return (r.Strokes[0].Spin.SR == "1" && r.Strokes[0].Spin.TS == "1") && r.Strokes[0].Player == r.Winner;
                case "UpSideRightDirectPointsWonButton":
                    return (r.Strokes[0].Spin.SR == "1" && r.Strokes[0].Spin.TS == "1") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "UpSideRightPointsLostButton":
                    return (r.Strokes[0].Spin.SR == "1" && r.Strokes[0].Spin.TS == "1") && r.Strokes[0].Player != r.Winner;

                case "UpAllTotalButton":
                    return r.Strokes[0].Spin.TS == "1";
                case "UpAllPointsWonButton":
                    return r.Strokes[0].Spin.TS == "1" && r.Strokes[0].Player == r.Winner;
                case "UpAllDirectPointsWonButton":
                    return r.Strokes[0].Spin.TS == "1" && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "UpAllPointsLostButton":
                    return r.Strokes[0].Spin.TS == "1" && r.Strokes[0].Player != r.Winner;

                #endregion

                #region No UpDown Spin

                case "SideLeftTotalButton":
                    return (r.Strokes[0].Spin.SL == "1" && r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0");
                case "SideLeftPointsWonButton":
                    return (r.Strokes[0].Spin.SL == "1" && r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0") && r.Strokes[0].Player == r.Winner;
                case "SideLeftDirectPointsWonButton":
                    return (r.Strokes[0].Spin.SL == "1" && r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "SideLeftPointsLostButton":
                    return (r.Strokes[0].Spin.SL == "1" && r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0") && r.Strokes[0].Player != r.Winner;

                case "NoSpinTotalButton":
                    return (r.Strokes[0].Spin.No == "1" && r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0" && r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0");
                case "NoSpinPointsWonButton":
                    return (r.Strokes[0].Spin.No == "1" && r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0" && r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0") && r.Strokes[0].Player == r.Winner;
                case "NoSpinDirectPointsWonButton":
                    return (r.Strokes[0].Spin.No == "1" && r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0" && r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "NoSpinPointsLostButton":
                    return (r.Strokes[0].Spin.No == "1" && r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0" && r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0") && r.Strokes[0].Player != r.Winner;

                case "SideRightTotalButton":
                    return (r.Strokes[0].Spin.SR == "1" && r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0");
                case "SideRightPointsWonButton":
                    return (r.Strokes[0].Spin.SR == "1" && r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0") && r.Strokes[0].Player == r.Winner;
                case "SideRightDirectPointsWonButton":
                    return (r.Strokes[0].Spin.SR == "1" && r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "SideRightPointsLostButton":
                    return (r.Strokes[0].Spin.SR == "1" && r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0") && r.Strokes[0].Player != r.Winner;

                case "NoUpDownAllTotalButton":
                    return (r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0");
                case "NoUpDownAllPointsWonButton":
                    return (r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0") && r.Strokes[0].Player == r.Winner;
                case "NoUpDownAllDirectPointsWonButton":
                    return (r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "NoUpDownAllPointsLostButton":
                    return (r.Strokes[0].Spin.TS == "0" && r.Strokes[0].Spin.US == "0") && r.Strokes[0].Player != r.Winner;

                #endregion

                #region DownSpin

                case "DownSideLeftTotalButton":
                    return (r.Strokes[0].Spin.SL == "1" && r.Strokes[0].Spin.US == "1");
                case "DownSideLeftPointsWonButton":
                    return (r.Strokes[0].Spin.SL == "1" && r.Strokes[0].Spin.US == "1") && r.Strokes[0].Player == r.Winner;
                case "DownSideLeftDirectPointsWonButton":
                    return (r.Strokes[0].Spin.SL == "1" && r.Strokes[0].Spin.US == "1") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "DownSideLeftPointsLostButton":
                    return (r.Strokes[0].Spin.SL == "1" && r.Strokes[0].Spin.US == "1") && r.Strokes[0].Player != r.Winner;

                case "DownTotalButton":
                    return (r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0" && r.Strokes[0].Spin.US == "1");
                case "DownPointsWonButton":
                    return (r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0" && r.Strokes[0].Spin.US == "1") && r.Strokes[0].Player == r.Winner;
                case "DownDirectPointsWonButton":
                    return (r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0" && r.Strokes[0].Spin.US == "1") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "DownPointsLostButton":
                    return (r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0" && r.Strokes[0].Spin.US == "1") && r.Strokes[0].Player != r.Winner;

                case "DownSideRightTotalButton":
                    return (r.Strokes[0].Spin.SR == "1" && r.Strokes[0].Spin.US == "1");
                case "DownSideRightPointsWonButton":
                    return (r.Strokes[0].Spin.SR == "1" && r.Strokes[0].Spin.US == "1") && r.Strokes[0].Player == r.Winner;
                case "DownSideRightDirectPointsWonButton":
                    return (r.Strokes[0].Spin.SR == "1" && r.Strokes[0].Spin.US == "1") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "DownSideRightPointsLostButton":
                    return (r.Strokes[0].Spin.SR == "1" && r.Strokes[0].Spin.US == "1") && r.Strokes[0].Player != r.Winner;

                case "DownAllTotalButton":
                    return r.Strokes[0].Spin.US == "1";
                case "DownAllPointsWonButton":
                    return r.Strokes[0].Spin.US == "1" && r.Strokes[0].Player == r.Winner;
                case "DownAllDirectPointsWonButton":
                    return r.Strokes[0].Spin.US == "1" && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "DownAllPointsLostButton":
                    return r.Strokes[0].Spin.US == "1" && r.Strokes[0].Player != r.Winner;

                #endregion

                #region SideLeft All

                case "SideLeftAllTotalButton":
                    return (r.Strokes[0].Spin.SL == "1");
                case "SideLeftAllPointsWonButton":
                    return (r.Strokes[0].Spin.SL == "1") && r.Strokes[0].Player == r.Winner;
                case "SideLeftAllDirectPointsWonButton":
                    return (r.Strokes[0].Spin.SL == "1") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "SideLeftAllPointsLostButton":
                    return (r.Strokes[0].Spin.SL == "1") && r.Strokes[0].Player != r.Winner;

                #endregion

                #region No SideSpin All

                case "NoSideAllTotalButton":
                    return (r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0");
                case "NoSideAllPointsWonButton":
                    return (r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0") && r.Strokes[0].Player == r.Winner;
                case "NoSideAllDirectPointsWonButton":
                    return (r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "NoSideAllPointsLostButton":
                    return (r.Strokes[0].Spin.SL == "0" && r.Strokes[0].Spin.SR == "0") && r.Strokes[0].Player != r.Winner;


                #endregion

                #region SideRight All

                case "SideRightAllTotalButton":
                    return (r.Strokes[0].Spin.SR == "1");
                case "SideRightAllPointsWonButton":
                    return (r.Strokes[0].Spin.SR == "1") && r.Strokes[0].Player == r.Winner;
                case "SideRightAllDirectPointsWonButton":
                    return (r.Strokes[0].Spin.SR == "1") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "SideRightAllPointsLostButton":
                    return (r.Strokes[0].Spin.SR == "1") && r.Strokes[0].Player != r.Winner;


                #endregion

                #region All Spins Total

                case "AllSpinTotalButton":
                    return (r.Strokes[0].Spin.SR == "1" || r.Strokes[0].Spin.SL == "1" || r.Strokes[0].Spin.US == "1" || r.Strokes[0].Spin.TS == "1" || r.Strokes[0].Spin.No == "1");
                case "AllSpinPointsWonButton":
                    return (r.Strokes[0].Spin.SR == "1" || r.Strokes[0].Spin.SL == "1" || r.Strokes[0].Spin.US == "1" || r.Strokes[0].Spin.TS == "1" || r.Strokes[0].Spin.No == "1") && r.Strokes[0].Player == r.Winner;
                case "AllSpinDirectPointsWonButton":
                    return (r.Strokes[0].Spin.SR == "1" || r.Strokes[0].Spin.SL == "1" || r.Strokes[0].Spin.US == "1" || r.Strokes[0].Spin.TS == "1" || r.Strokes[0].Spin.No == "1") && r.Strokes[0].Player == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllSpinPointsLostButton":
                    return (r.Strokes[0].Spin.SR == "1" || r.Strokes[0].Spin.SL == "1" || r.Strokes[0].Spin.US == "1" || r.Strokes[0].Spin.TS == "1" || r.Strokes[0].Spin.No == "1") && r.Strokes[0].Player != r.Winner;


                #endregion

                default:
                    return true;
            }
        }

        private double AufSchlägePosition(Rally r)
        {
            Stroke service = r.Strokes.Where(s => s.Number == 1).FirstOrDefault();
            double aufSchlägePosition;
            double seite = service.Placement.WY == double.NaN ? 999 : Convert.ToDouble(service.Placement.WY);
            if (seite >= 137)
            {
                aufSchlägePosition = 152.5 - (service.Playerposition == double.NaN ? 999 : Convert.ToDouble(service.Playerposition));
            }
            else
            {
                aufSchlägePosition = service.Playerposition == double.NaN ? 999 : Convert.ToDouble(service.Playerposition);
            }

            return aufSchlägePosition;

        }



        #endregion
    }
}
