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
using TT.Models.Events;
using TT.Models.Managers;

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
            Player1 = "Spieler 1";
            Player2 = "Spieler 2";

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
            base.OnActivate();
            // Subscribe ourself to the event bus
            this.events.Subscribe(this);
            this.ActivateItem(BasicFilterStatisticsView);
            Player1 = Manager.Match.FirstPlayer.Name.Split(' ')[0];
            Player2 = Manager.Match.SecondPlayer.Name.Split(' ')[0];
        }

        protected override void OnViewReady(object view)
        {
            base.OnViewReady(view);
            UpdateSelection(Manager.ActivePlaylist);
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
                var results = BasicFilterStatisticsView.SelectedRallies.Where(r => HasServices(r) && HasPlacement(r) && HasPosition(r) && HasSpin(r) && HasBasisInformation(r)).ToList();
                this.events.PublishOnUIThread(new ResultsChangedEvent(results));
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
                    return r.Schläge[0].IsTopLeft() || r.Schläge[0].IsMidLeft() || r.Schläge[0].IsBotLeft();
                case "PlacementForehandAllPointsWonButton":
                    return (r.Schläge[0].IsTopLeft() || r.Schläge[0].IsMidLeft() || r.Schläge[0].IsBotLeft()) && r.Schläge[0].Spieler == r.Winner;
                case "PlacementForehandAllDirectPointsWonButton":
                    return (r.Schläge[0].IsTopLeft() || r.Schläge[0].IsMidLeft() || r.Schläge[0].IsBotLeft()) && r.Schläge[0].Spieler == r.Winner && r.Length < 3;
                case "PlacementForehandAllPointsLostButton":
                    return (r.Schläge[0].IsTopLeft() || r.Schläge[0].IsMidLeft() || r.Schläge[0].IsBotLeft()) && r.Schläge[0].Spieler != r.Winner;
                #endregion
                #region ForehandLong
                case "PlacementForehandLongTotalButton":
                    return r.Schläge[0].IsTopLeft();
                case "PlacementForehandLongPointsWonButton":
                    return r.Schläge[0].IsTopLeft() && r.Schläge[0].Spieler == r.Winner;
                case "PlacementForehandLongDirectPointsWonButton":
                    return r.Schläge[0].IsTopLeft() && r.Schläge[0].Spieler == r.Winner && r.Length < 3;
                case "PlacementForehandLongPointsLostButton":
                    return r.Schläge[0].IsTopLeft() && r.Schläge[0].Spieler != r.Winner;
                #endregion
                #region ForehandHalfLong
                case "PlacementForehandHalfLongTotalButton":
                    return r.Schläge[0].IsMidLeft();
                case "PlacementForehandHalfLongPointsWonButton":
                    return r.Schläge[0].IsMidLeft() && r.Schläge[0].Spieler == r.Winner;
                case "PlacementForehandHalfLongDirectPointsWonButton":
                    return r.Schläge[0].IsMidLeft() && r.Schläge[0].Spieler == r.Winner && r.Length < 3;
                case "PlacementForehandHalfLongPointsLostButton":
                    return r.Schläge[0].IsMidLeft() && r.Schläge[0].Spieler != r.Winner;
                #endregion
                #region ForehandShort
                case "PlacementForehandShortTotalButton":
                    return r.Schläge[0].IsBotLeft();
                case "PlacementForehandShortPointsWonButton":
                    return r.Schläge[0].IsBotLeft() && r.Schläge[0].Spieler == r.Winner;
                case "PlacementForehandShortDirectPointsWonButton":
                    return r.Schläge[0].IsBotLeft() && r.Schläge[0].Spieler == r.Winner && r.Length < 3;
                case "PlacementForehandShortPointsLostButton":
                    return r.Schläge[0].IsBotLeft() && r.Schläge[0].Spieler != r.Winner;
                #endregion

                #region MiddleAll
                case "PlacementMiddleAllTotalButton":
                    return r.Schläge[0].IsTopMid() || r.Schläge[0].IsMidMid() || r.Schläge[0].IsBotMid();
                case "PlacementMiddleAllPointsWonButton":
                    return (r.Schläge[0].IsTopMid() || r.Schläge[0].IsMidMid() || r.Schläge[0].IsBotMid()) && r.Schläge[0].Spieler == r.Winner;
                case "PlacementMiddleAllDirectPointsWonButton":
                    return (r.Schläge[0].IsTopMid() || r.Schläge[0].IsMidMid() || r.Schläge[0].IsBotMid()) && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementMiddleAllPointsLostButton":
                    return (r.Schläge[0].IsTopMid() || r.Schläge[0].IsMidMid() || r.Schläge[0].IsBotMid()) && r.Schläge[0].Spieler != r.Winner;
                #endregion

                #region MiddleLong
                case "PlacementMiddleLongTotalButton":
                    return r.Schläge[0].IsTopMid();
                case "PlacementMiddleLongPointsWonButton":
                    return r.Schläge[0].IsTopMid() && r.Schläge[0].Spieler == r.Winner;
                case "PlacementMiddleLongDirectPointsWonButton":
                    return r.Schläge[0].IsTopMid() && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementMiddleLongPointsLostButton":
                    return r.Schläge[0].IsTopMid() && r.Schläge[0].Spieler != r.Winner;
                #endregion

                #region MiddleHalfLong
                case "PlacementMiddleHalfLongTotalButton":
                    return r.Schläge[0].IsMidMid();
                case "PlacementMiddleHalfLongPointsWonButton":
                    return r.Schläge[0].IsMidMid() && r.Schläge[0].Spieler == r.Winner;
                case "PlacementMiddleHalfLongDirectPointsWonButton":
                    return r.Schläge[0].IsMidMid() && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementMiddleHalfLongPointsLostButton":
                    return r.Schläge[0].IsMidMid() && r.Schläge[0].Spieler != r.Winner;
                #endregion

                #region MiddleShort
                case "PlacementMiddleShortTotalButton":
                    return r.Schläge[0].IsBotMid();
                case "PlacementMiddleShortPointsWonButton":
                    return r.Schläge[0].IsBotMid() && r.Schläge[0].Spieler == r.Winner;
                case "PlacementMiddleShortDirectPointsWonButton":
                    return r.Schläge[0].IsBotMid() && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementMiddleShortPointsLostButton":
                    return r.Schläge[0].IsBotMid() && r.Schläge[0].Spieler != r.Winner;
                #endregion

                #region BackhandAll
                case "PlacementBackhandAllTotalButton":
                    return r.Schläge[0].IsTopRight() || r.Schläge[0].IsMidRight() || r.Schläge[0].IsBotRight();
                case "PlacementBackhandAllPointsWonButton":
                    return (r.Schläge[0].IsTopRight() || r.Schläge[0].IsMidRight() || r.Schläge[0].IsBotRight()) && r.Schläge[0].Spieler == r.Winner;
                case "PlacementBackhandAllDirectPointsWonButton":
                    return (r.Schläge[0].IsTopRight() || r.Schläge[0].IsMidRight() || r.Schläge[0].IsBotRight()) && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementBackhandAllPointsLostButton":
                    return (r.Schläge[0].IsTopRight() || r.Schläge[0].IsMidRight() || r.Schläge[0].IsBotRight()) && r.Schläge[0].Spieler != r.Winner;
                #endregion

                #region BackhandLong
                case "PlacementBackhandLongTotalButton":
                    return r.Schläge[0].IsTopRight();
                case "PlacementBackhandLongPointsWonButton":
                    return r.Schläge[0].IsTopRight() && r.Schläge[0].Spieler == r.Winner;
                case "PlacementBackhandLongDirectPointsWonButton":
                    return r.Schläge[0].IsTopRight() && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementBackhandLongPointsLostButton":
                    return r.Schläge[0].IsTopRight() && r.Schläge[0].Spieler != r.Winner;
                #endregion

                #region BackhandHalfLong
                case "PlacementBackhandHalfLongTotalButton":
                    return r.Schläge[0].IsMidRight();
                case "PlacementBackhandHalfLongPointsWonButton":
                    return r.Schläge[0].IsMidRight() && r.Schläge[0].Spieler == r.Winner;
                case "PlacementBackhandHalfLongDirectPointsWonButton":
                    return r.Schläge[0].IsMidRight() && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementBackhandHalfLongPointsLostButton":
                    return r.Schläge[0].IsMidRight() && r.Schläge[0].Spieler != r.Winner;
                #endregion

                #region BackhandShort
                case "PlacementBackhandShortTotalButton":
                    return r.Schläge[0].IsBotRight();
                case "PlacementBackhandShortPointsWonButton":
                    return r.Schläge[0].IsBotRight() && r.Schläge[0].Spieler == r.Winner;
                case "PlacementBackhandShortDirectPointsWonButton":
                    return r.Schläge[0].IsBotRight() && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementBackhandShortPointsLostButton":
                    return r.Schläge[0].IsBotRight() && r.Schläge[0].Spieler != r.Winner;
                #endregion

                #region AllLong
                case "PlacementAllLongTotalButton":
                    return (r.Schläge[0].IsTopLeft() || r.Schläge[0].IsTopMid() || r.Schläge[0].IsTopRight());
                case "PlacementAllLongPointsWonButton":
                    return (r.Schläge[0].IsTopLeft() || r.Schläge[0].IsTopMid() || r.Schläge[0].IsTopRight()) && r.Schläge[0].Spieler == r.Winner;
                case "PlacementAllLongDirectPointsWonButton":
                    return (r.Schläge[0].IsTopLeft() || r.Schläge[0].IsTopMid() || r.Schläge[0].IsTopRight()) && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementAllLongPointsLostButton":
                    return (r.Schläge[0].IsTopLeft() || r.Schläge[0].IsTopMid() || r.Schläge[0].IsTopRight()) && r.Schläge[0].Spieler != r.Winner;
                #endregion

                #region AllHalfLong
                case "PlacementAllHalfLongTotalButton":
                    return (r.Schläge[0].IsMidLeft() || r.Schläge[0].IsMidMid() || r.Schläge[0].IsMidRight());
                case "PlacementAllHalfLongPointsWonButton":
                    return (r.Schläge[0].IsMidLeft() || r.Schläge[0].IsMidMid() || r.Schläge[0].IsMidRight()) && r.Schläge[0].Spieler == r.Winner;
                case "PlacementAllHalfLongDirectPointsWonButton":
                    return (r.Schläge[0].IsMidLeft() || r.Schläge[0].IsMidMid() || r.Schläge[0].IsMidRight()) && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementAllHalfLongPointsLostButton":
                    return (r.Schläge[0].IsMidLeft() || r.Schläge[0].IsMidMid() || r.Schläge[0].IsMidRight()) && r.Schläge[0].Spieler != r.Winner;
                #endregion

                #region AllShort
                case "PlacementAllShortTotalButton":
                    return (r.Schläge[0].IsBotLeft() || r.Schläge[0].IsBotMid() || r.Schläge[0].IsBotRight());
                case "PlacementAllShortPointsWonButton":
                    return (r.Schläge[0].IsBotLeft() || r.Schläge[0].IsBotMid() || r.Schläge[0].IsBotRight()) && r.Schläge[0].Spieler == r.Winner;
                case "PlacementAllShortDirectPointsWonButton":
                    return (r.Schläge[0].IsBotLeft() || r.Schläge[0].IsBotMid() || r.Schläge[0].IsBotRight()) && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementAllShortPointsLostButton":
                    return (r.Schläge[0].IsBotLeft() || r.Schläge[0].IsBotMid() || r.Schläge[0].IsBotRight()) && r.Schläge[0].Spieler != r.Winner;
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
                    return r.Schläge[0].IsLeftServicePosition();
                case "PositionLeftPointsWonButton":
                    return r.Schläge[0].IsLeftServicePosition() && r.Schläge[0].Spieler == r.Winner;
                case "PositionLeftDirectPointsWonButton":
                    return r.Schläge[0].IsLeftServicePosition() && r.Schläge[0].Spieler == r.Winner && r.Length < 3;
                case "PositionLeftPointsLostButton":
                    return r.Schläge[0].IsLeftServicePosition() && r.Schläge[0].Spieler != r.Winner;
                #endregion
                #region Position Middle
                case "PositionMiddleTotalButton":
                    return r.Schläge[0].IsMiddleServicePosition();
                case "PositionMiddlePointsWonButton":
                    return r.Schläge[0].IsMiddleServicePosition() && r.Schläge[0].Spieler == r.Winner;
                case "PositionMiddleDirectPointsWonButton":
                    return r.Schläge[0].IsMiddleServicePosition() && r.Schläge[0].Spieler == r.Winner && r.Length < 3;
                case "PositionMiddlePointsLostButton":
                    return r.Schläge[0].IsMiddleServicePosition() && r.Schläge[0].Spieler != r.Winner;
                #endregion
                #region Position Right
                case "PositionRightTotalButton":
                    return r.Schläge[0].IsRightServicePosition();
                case "PositionRightPointsWonButton":
                    return r.Schläge[0].IsRightServicePosition() && r.Schläge[0].Spieler == r.Winner;
                case "PositionRightDirectPointsWonButton":
                    return r.Schläge[0].IsRightServicePosition() && r.Schläge[0].Spieler == r.Winner && r.Length < 3;
                case "PositionRightPointsLostButton":
                    return r.Schläge[0].IsRightServicePosition() && r.Schläge[0].Spieler != r.Winner;
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
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Pendulum";
                case "ForehandPendulumPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Pendulum" && r.Schläge[0].Spieler == r.Winner;
                case "ForehandPendulumDirectPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Pendulum" && r.Schläge[0].Spieler == r.Winner && r.Length < 3;
                case "ForehandPendulumPointsLostButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Pendulum" && r.Schläge[0].Spieler != r.Winner;
                case "BackhandPendulumTotalButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Pendulum";
                case "BackhandPendulumPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Pendulum" && r.Schläge[0].Spieler == r.Winner;
                case "BackhandPendulumDirectPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Pendulum" && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "BackhandPendulumPointsLostButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Pendulum" && r.Schläge[0].Spieler != r.Winner;
                case "AllPendulumTotalButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Pendulum";
                case "AllPendulumPointsWonButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Pendulum" && r.Schläge[0].Spieler == r.Winner;
                case "AllPendulumDirectPointsWonButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Pendulum" && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllPendulumPointsLostButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Pendulum" && r.Schläge[0].Spieler != r.Winner;

                #endregion

                #region ReversePendulum
                case "ForehandReversePendulumTotalButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Gegenläufer";
                case "ForehandReversePendulumPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Gegenläufer" && r.Schläge[0].Spieler == r.Winner;
                case "ForehandReversePendulumDirectPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Gegenläufer" && r.Schläge[0].Spieler == r.Winner && r.Length < 3;
                case "ForehandReversePendulumPointsLostButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Gegenläufer" && r.Schläge[0].Spieler != r.Winner;
                case "BackhandReversePendulumTotalButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Gegenläufer";
                case "BackhandReversePendulumPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Gegenläufer" && r.Schläge[0].Spieler == r.Winner;
                case "BackhandReversePendulumDirectPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Gegenläufer" && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "BackhandReversePendulumPointsLostButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Gegenläufer" && r.Schläge[0].Spieler != r.Winner;
                case "AllReversePendulumTotalButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Gegenläufer";
                case "AllReversePendulumPointsWonButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Gegenläufer" && r.Schläge[0].Spieler == r.Winner;
                case "AllReversePendulumDirectPointsWonButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Gegenläufer" && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllReversePendulumPointsLostButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Gegenläufer" && r.Schläge[0].Spieler != r.Winner;

                #endregion

                #region Tomahawk
                case "ForehandTomahawkTotalButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Tomahawk";
                case "ForehandTomahawkPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Tomahawk" && r.Schläge[0].Spieler == r.Winner;
                case "ForehandTomahawkDirectPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Tomahawk" && r.Schläge[0].Spieler == r.Winner && r.Length < 3;
                case "ForehandTomahawkPointsLostButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Tomahawk" && r.Schläge[0].Spieler != r.Winner;
                case "BackhandTomahawkTotalButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Tomahawk";
                case "BackhandTomahawkPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Tomahawk" && r.Schläge[0].Spieler == r.Winner;
                case "BackhandTomahawkDirectPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Tomahawk" && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "BackhandTomahawkPointsLostButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Tomahawk" && r.Schläge[0].Spieler != r.Winner;
                case "AllTomahawkTotalButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Tomahawk";
                case "AllTomahawkPointsWonButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Tomahawk" && r.Schläge[0].Spieler == r.Winner;
                case "AllTomahawkDirectPointsWonButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Tomahawk" && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllTomahawkPointsLostButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Tomahawk" && r.Schläge[0].Spieler != r.Winner;

                #endregion

                #region Special
                case "ForehandSpecialTotalButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Spezial";
                case "ForehandSpecialPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Spezial" && r.Schläge[0].Spieler == r.Winner;
                case "ForehandSpecialDirectPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Spezial" && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "ForehandSpecialPointsLostButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && r.Schläge[0].Aufschlagart == "Spezial" && r.Schläge[0].Spieler != r.Winner;
                case "BackhandSpecialTotalButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Spezial";
                case "BackhandSpecialPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Spezial" && r.Schläge[0].Spieler == r.Winner;
                case "BackhandSpecialDirectPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Spezial" && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "BackhandSpecialPointsLostButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && r.Schläge[0].Aufschlagart == "Spezial" && r.Schläge[0].Spieler != r.Winner;
                case "AllSpecialTotalButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Spezial";
                case "AllSpecialPointsWonButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Spezial" && r.Schläge[0].Spieler == r.Winner;
                case "AllSpecialDirectPointsWonButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Spezial" && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllSpecialPointsLostButton":
                    return (r.Schläge[0].Schlägerseite == "Vorhand" || r.Schläge[0].Schlägerseite == "Rückhand") && r.Schläge[0].Aufschlagart == "Spezial" && r.Schläge[0].Spieler != r.Winner;

                #endregion

                #region All Forehand Services
                case "ForehandAllTotalButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && (r.Schläge[0].Aufschlagart == "Pendulum" || r.Schläge[0].Aufschlagart == "Gegenläufer" || r.Schläge[0].Aufschlagart == "Tomahawk" || r.Schläge[0].Aufschlagart == "Spezial");
                case "ForehandAllPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && (r.Schläge[0].Aufschlagart == "Pendulum" || r.Schläge[0].Aufschlagart == "Gegenläufer" || r.Schläge[0].Aufschlagart == "Tomahawk" || r.Schläge[0].Aufschlagart == "Spezial") && r.Schläge[0].Spieler == r.Winner;
                case "ForehandAllDirectPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && (r.Schläge[0].Aufschlagart == "Pendulum" || r.Schläge[0].Aufschlagart == "Gegenläufer" || r.Schläge[0].Aufschlagart == "Tomahawk" || r.Schläge[0].Aufschlagart == "Spezial") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "ForehandAllPointsLostButton":
                    return r.Schläge[0].Schlägerseite == "Vorhand" && (r.Schläge[0].Aufschlagart == "Pendulum" || r.Schläge[0].Aufschlagart == "Gegenläufer" || r.Schläge[0].Aufschlagart == "Tomahawk" || r.Schläge[0].Aufschlagart == "Spezial") && r.Schläge[0].Spieler != r.Winner;
                #endregion

                #region All Backhand Services
                case "BackhandAllTotalButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && (r.Schläge[0].Aufschlagart == "Pendulum" || r.Schläge[0].Aufschlagart == "Gegenläufer" || r.Schläge[0].Aufschlagart == "Tomahawk" || r.Schläge[0].Aufschlagart == "Spezial");
                case "BackhandAllPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && (r.Schläge[0].Aufschlagart == "Pendulum" || r.Schläge[0].Aufschlagart == "Gegenläufer" || r.Schläge[0].Aufschlagart == "Tomahawk" || r.Schläge[0].Aufschlagart == "Spezial") && r.Schläge[0].Spieler == r.Winner;
                case "BackhandAllDirectPointsWonButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && (r.Schläge[0].Aufschlagart == "Pendulum" || r.Schläge[0].Aufschlagart == "Gegenläufer" || r.Schläge[0].Aufschlagart == "Tomahawk" || r.Schläge[0].Aufschlagart == "Spezial") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "BackhandAllPointsLostButton":
                    return r.Schläge[0].Schlägerseite == "Rückhand" && (r.Schläge[0].Aufschlagart == "Pendulum" || r.Schläge[0].Aufschlagart == "Gegenläufer" || r.Schläge[0].Aufschlagart == "Tomahawk" || r.Schläge[0].Aufschlagart == "Spezial") && r.Schläge[0].Spieler != r.Winner;
                #endregion


                #region All Services
                case "AllServicesTotalButton":
                    return (r.Schläge[0].Schlägerseite == "Rückhand" || r.Schläge[0].Schlägerseite == "Vorhand") && (r.Schläge[0].Aufschlagart == "Pendulum" || r.Schläge[0].Aufschlagart == "Gegenläufer" || r.Schläge[0].Aufschlagart == "Tomahawk" || r.Schläge[0].Aufschlagart == "Spezial");
                case "AllServicesPointsWonButton":
                    return (r.Schläge[0].Schlägerseite == "Rückhand" || r.Schläge[0].Schlägerseite == "Vorhand") && (r.Schläge[0].Aufschlagart == "Pendulum" || r.Schläge[0].Aufschlagart == "Gegenläufer" || r.Schläge[0].Aufschlagart == "Tomahawk" || r.Schläge[0].Aufschlagart == "Spezial") && r.Schläge[0].Spieler == r.Winner;
                case "AllServicesDirectPointsWonButton":
                    return (r.Schläge[0].Schlägerseite == "Rückhand" || r.Schläge[0].Schlägerseite == "Vorhand") && (r.Schläge[0].Aufschlagart == "Pendulum" || r.Schläge[0].Aufschlagart == "Gegenläufer" || r.Schläge[0].Aufschlagart == "Tomahawk" || r.Schläge[0].Aufschlagart == "Spezial") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllServicesPointsLostButton":
                    return (r.Schläge[0].Schlägerseite == "Rückhand" || r.Schläge[0].Schlägerseite == "Vorhand") && (r.Schläge[0].Aufschlagart == "Pendulum" || r.Schläge[0].Aufschlagart == "Gegenläufer" || r.Schläge[0].Aufschlagart == "Tomahawk" || r.Schläge[0].Aufschlagart == "Spezial") && r.Schläge[0].Spieler != r.Winner;
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
                    return (r.Schläge[0].Spin.SL == "1" && r.Schläge[0].Spin.ÜS == "1");
                case "UpSideLeftPointsWonButton":
                    return (r.Schläge[0].Spin.SL == "1" && r.Schläge[0].Spin.ÜS == "1") && r.Schläge[0].Spieler == r.Winner;
                case "UpSideLeftDirectPointsWonButton":
                    return (r.Schläge[0].Spin.SL == "1" && r.Schläge[0].Spin.ÜS == "1") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "UpSideLeftPointsLostButton":
                    return (r.Schläge[0].Spin.SL == "1" && r.Schläge[0].Spin.ÜS == "1") && r.Schläge[0].Spieler != r.Winner;

                case "UpTotalButton":
                    return (r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0" && r.Schläge[0].Spin.ÜS == "1");
                case "UpPointsWonButton":
                    return (r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0" && r.Schläge[0].Spin.ÜS == "1") && r.Schläge[0].Spieler == r.Winner;
                case "UpDirectPointsWonButton":
                    return (r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0" && r.Schläge[0].Spin.ÜS == "1") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "UpPointsLostButton":
                    return (r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0" && r.Schläge[0].Spin.ÜS == "1") && r.Schläge[0].Spieler != r.Winner;

                case "UpSideRightTotalButton":
                    return (r.Schläge[0].Spin.SR == "1" && r.Schläge[0].Spin.ÜS == "1");
                case "UpSideRightPointsWonButton":
                    return (r.Schläge[0].Spin.SR == "1" && r.Schläge[0].Spin.ÜS == "1") && r.Schläge[0].Spieler == r.Winner;
                case "UpSideRightDirectPointsWonButton":
                    return (r.Schläge[0].Spin.SR == "1" && r.Schläge[0].Spin.ÜS == "1") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "UpSideRightPointsLostButton":
                    return (r.Schläge[0].Spin.SR == "1" && r.Schläge[0].Spin.ÜS == "1") && r.Schläge[0].Spieler != r.Winner;

                case "UpAllTotalButton":
                    return r.Schläge[0].Spin.ÜS == "1";
                case "UpAllPointsWonButton":
                    return r.Schläge[0].Spin.ÜS == "1" && r.Schläge[0].Spieler == r.Winner;
                case "UpAllDirectPointsWonButton":
                    return r.Schläge[0].Spin.ÜS == "1" && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "UpAllPointsLostButton":
                    return r.Schläge[0].Spin.ÜS == "1" && r.Schläge[0].Spieler != r.Winner;

                #endregion

                #region No UpDown Spin

                case "SideLeftTotalButton":
                    return (r.Schläge[0].Spin.SL == "1" && r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0");
                case "SideLeftPointsWonButton":
                    return (r.Schläge[0].Spin.SL == "1" && r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0") && r.Schläge[0].Spieler == r.Winner;
                case "SideLeftDirectPointsWonButton":
                    return (r.Schläge[0].Spin.SL == "1" && r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "SideLeftPointsLostButton":
                    return (r.Schläge[0].Spin.SL == "1" && r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0") && r.Schläge[0].Spieler != r.Winner;

                case "NoSpinTotalButton":
                    return (r.Schläge[0].Spin.No == "1" && r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0" && r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0");
                case "NoSpinPointsWonButton":
                    return (r.Schläge[0].Spin.No == "1" && r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0" && r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0") && r.Schläge[0].Spieler == r.Winner;
                case "NoSpinDirectPointsWonButton":
                    return (r.Schläge[0].Spin.No == "1" && r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0" && r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "NoSpinPointsLostButton":
                    return (r.Schläge[0].Spin.No == "1" && r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0" && r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0") && r.Schläge[0].Spieler != r.Winner;

                case "SideRightTotalButton":
                    return (r.Schläge[0].Spin.SR == "1" && r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0");
                case "SideRightPointsWonButton":
                    return (r.Schläge[0].Spin.SR == "1" && r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0") && r.Schläge[0].Spieler == r.Winner;
                case "SideRightDirectPointsWonButton":
                    return (r.Schläge[0].Spin.SR == "1" && r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "SideRightPointsLostButton":
                    return (r.Schläge[0].Spin.SR == "1" && r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0") && r.Schläge[0].Spieler != r.Winner;

                case "NoUpDownAllTotalButton":
                    return (r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0");
                case "NoUpDownAllPointsWonButton":
                    return (r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0") && r.Schläge[0].Spieler == r.Winner;
                case "NoUpDownAllDirectPointsWonButton":
                    return (r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "NoUpDownAllPointsLostButton":
                    return (r.Schläge[0].Spin.ÜS == "0" && r.Schläge[0].Spin.US == "0") && r.Schläge[0].Spieler != r.Winner;

                #endregion

                #region DownSpin

                case "DownSideLeftTotalButton":
                    return (r.Schläge[0].Spin.SL == "1" && r.Schläge[0].Spin.US == "1");
                case "DownSideLeftPointsWonButton":
                    return (r.Schläge[0].Spin.SL == "1" && r.Schläge[0].Spin.US == "1") && r.Schläge[0].Spieler == r.Winner;
                case "DownSideLeftDirectPointsWonButton":
                    return (r.Schläge[0].Spin.SL == "1" && r.Schläge[0].Spin.US == "1") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "DownSideLeftPointsLostButton":
                    return (r.Schläge[0].Spin.SL == "1" && r.Schläge[0].Spin.US == "1") && r.Schläge[0].Spieler != r.Winner;

                case "DownTotalButton":
                    return (r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0" && r.Schläge[0].Spin.US == "1");
                case "DownPointsWonButton":
                    return (r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0" && r.Schläge[0].Spin.US == "1") && r.Schläge[0].Spieler == r.Winner;
                case "DownDirectPointsWonButton":
                    return (r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0" && r.Schläge[0].Spin.US == "1") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "DownPointsLostButton":
                    return (r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0" && r.Schläge[0].Spin.US == "1") && r.Schläge[0].Spieler != r.Winner;

                case "DownSideRightTotalButton":
                    return (r.Schläge[0].Spin.SR == "1" && r.Schläge[0].Spin.US == "1");
                case "DownSideRightPointsWonButton":
                    return (r.Schläge[0].Spin.SR == "1" && r.Schläge[0].Spin.US == "1") && r.Schläge[0].Spieler == r.Winner;
                case "DownSideRightDirectPointsWonButton":
                    return (r.Schläge[0].Spin.SR == "1" && r.Schläge[0].Spin.US == "1") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "DownSideRightPointsLostButton":
                    return (r.Schläge[0].Spin.SR == "1" && r.Schläge[0].Spin.US == "1") && r.Schläge[0].Spieler != r.Winner;

                case "DownAllTotalButton":
                    return r.Schläge[0].Spin.US == "1";
                case "DownAllPointsWonButton":
                    return r.Schläge[0].Spin.US == "1" && r.Schläge[0].Spieler == r.Winner;
                case "DownAllDirectPointsWonButton":
                    return r.Schläge[0].Spin.US == "1" && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "DownAllPointsLostButton":
                    return r.Schläge[0].Spin.US == "1" && r.Schläge[0].Spieler != r.Winner;

                #endregion

                #region SideLeft All

                case "SideLeftAllTotalButton":
                    return (r.Schläge[0].Spin.SL == "1");
                case "SideLeftAllPointsWonButton":
                    return (r.Schläge[0].Spin.SL == "1") && r.Schläge[0].Spieler == r.Winner;
                case "SideLeftAllDirectPointsWonButton":
                    return (r.Schläge[0].Spin.SL == "1") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "SideLeftAllPointsLostButton":
                    return (r.Schläge[0].Spin.SL == "1") && r.Schläge[0].Spieler != r.Winner;

                #endregion

                #region No SideSpin All

                case "NoSideAllTotalButton":
                    return (r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0");
                case "NoSideAllPointsWonButton":
                    return (r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0") && r.Schläge[0].Spieler == r.Winner;
                case "NoSideAllDirectPointsWonButton":
                    return (r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "NoSideAllPointsLostButton":
                    return (r.Schläge[0].Spin.SL == "0" && r.Schläge[0].Spin.SR == "0") && r.Schläge[0].Spieler != r.Winner;


                #endregion

                #region SideRight All

                case "SideRightAllTotalButton":
                    return (r.Schläge[0].Spin.SR == "1");
                case "SideRightAllPointsWonButton":
                    return (r.Schläge[0].Spin.SR == "1") && r.Schläge[0].Spieler == r.Winner;
                case "SideRightAllDirectPointsWonButton":
                    return (r.Schläge[0].Spin.SR == "1") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "SideRightAllPointsLostButton":
                    return (r.Schläge[0].Spin.SR == "1") && r.Schläge[0].Spieler != r.Winner;


                #endregion

                #region All Spins Total

                case "AllSpinTotalButton":
                    return (r.Schläge[0].Spin.SR == "1" || r.Schläge[0].Spin.SL == "1" || r.Schläge[0].Spin.US == "1" || r.Schläge[0].Spin.ÜS == "1" || r.Schläge[0].Spin.No == "1");
                case "AllSpinPointsWonButton":
                    return (r.Schläge[0].Spin.SR == "1" || r.Schläge[0].Spin.SL == "1" || r.Schläge[0].Spin.US == "1" || r.Schläge[0].Spin.ÜS == "1" || r.Schläge[0].Spin.No == "1") && r.Schläge[0].Spieler == r.Winner;
                case "AllSpinDirectPointsWonButton":
                    return (r.Schläge[0].Spin.SR == "1" || r.Schläge[0].Spin.SL == "1" || r.Schläge[0].Spin.US == "1" || r.Schläge[0].Spin.ÜS == "1" || r.Schläge[0].Spin.No == "1") && r.Schläge[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllSpinPointsLostButton":
                    return (r.Schläge[0].Spin.SR == "1" || r.Schläge[0].Spin.SL == "1" || r.Schläge[0].Spin.US == "1" || r.Schläge[0].Spin.ÜS == "1" || r.Schläge[0].Spin.No == "1") && r.Schläge[0].Spieler != r.Winner;


                #endregion

                default:
                    return true;
            }
        }

        private double AufSchlägePosition(Rally r)
        {
            Schlag service = r.Schläge.Where(s => s.Nummer == 1).FirstOrDefault();
            double aufSchlägePosition;
            double seite = service.Platzierung.WY == double.NaN ? 999 : Convert.ToDouble(service.Platzierung.WY);
            if (seite >= 137)
            {
                aufSchlägePosition = 152.5 - (service.Spielerposition == double.NaN ? 999 : Convert.ToDouble(service.Spielerposition));
            }
            else
            {
                aufSchlägePosition = service.Spielerposition == double.NaN ? 999 : Convert.ToDouble(service.Spielerposition);
            }

            return aufSchlägePosition;

        }



        #endregion
    }
}
