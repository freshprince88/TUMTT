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
                    return Convert.ToInt32(r.Length) >= 1;
                case "TotalServicesCountPointPlayer1":
                    return Convert.ToInt32(r.Length) >= 1 && r.Winner == "First";
                case "TotalServicesCountPointPlayer2":
                    return Convert.ToInt32(r.Length) >= 1 && r.Winner == "Second";
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

                #region ForhandAll
                case "PlacementForhandAllTotalButton":
                    return r.Schlag[0].IsTopLeft() || r.Schlag[0].IsMidLeft() || r.Schlag[0].IsBotLeft();
                case "PlacementForhandAllPointsWonButton":
                    return (r.Schlag[0].IsTopLeft() || r.Schlag[0].IsMidLeft() || r.Schlag[0].IsBotLeft()) && r.Schlag[0].Spieler == r.Winner;
                case "PlacementForhandAllDirectPointsWonButton":
                    return (r.Schlag[0].IsTopLeft() || r.Schlag[0].IsMidLeft() || r.Schlag[0].IsBotLeft()) && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementForhandAllPointsLostButton":
                    return (r.Schlag[0].IsTopLeft() || r.Schlag[0].IsMidLeft() || r.Schlag[0].IsBotLeft()) && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region ForhandLong
                case "PlacementForhandLongTotalButton":
                    return r.Schlag[0].IsTopLeft();
                case "PlacementForhandLongPointsWonButton":
                    return r.Schlag[0].IsTopLeft() && r.Schlag[0].Spieler == r.Winner;
                case "PlacementForhandLongDirectPointsWonButton":
                    return r.Schlag[0].IsTopLeft() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementForhandLongPointsLostButton":
                    return r.Schlag[0].IsTopLeft() && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region ForhandHalfLong
                case "PlacementForhandHalfLongTotalButton":
                    return r.Schlag[0].IsMidLeft();
                case "PlacementForhandHalfLongPointsWonButton":
                    return r.Schlag[0].IsMidLeft() && r.Schlag[0].Spieler == r.Winner;
                case "PlacementForhandHalfLongDirectPointsWonButton":
                    return r.Schlag[0].IsMidLeft() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementForhandHalfLongPointsLostButton":
                    return r.Schlag[0].IsMidLeft() && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region ForhandShort
                case "PlacementForhandShortTotalButton":
                    return r.Schlag[0].IsBotLeft();
                case "PlacementForhandShortPointsWonButton":
                    return r.Schlag[0].IsBotLeft() && r.Schlag[0].Spieler == r.Winner;
                case "PlacementForhandShortDirectPointsWonButton":
                    return r.Schlag[0].IsBotLeft() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementForhandShortPointsLostButton":
                    return r.Schlag[0].IsBotLeft() && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region MiddleAll
                case "PlacementMiddleAllTotalButton":
                    return r.Schlag[0].IsTopMid() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsBotMid();
                case "PlacementMiddleAllPointsWonButton":
                    return (r.Schlag[0].IsTopMid() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsBotMid()) && r.Schlag[0].Spieler == r.Winner;
                case "PlacementMiddleAllDirectPointsWonButton":
                    return (r.Schlag[0].IsTopMid() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsBotMid()) && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementMiddleAllPointsLostButton":
                    return (r.Schlag[0].IsTopMid() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsBotMid()) && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region MiddleLong
                case "PlacementMiddleLongTotalButton":
                    return r.Schlag[0].IsTopMid();
                case "PlacementMiddleLongPointsWonButton":
                    return r.Schlag[0].IsTopMid() && r.Schlag[0].Spieler == r.Winner;
                case "PlacementMiddleLongDirectPointsWonButton":
                    return r.Schlag[0].IsTopMid() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementMiddleLongPointsLostButton":
                    return r.Schlag[0].IsTopMid() && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region MiddleHalfLong
                case "PlacementMiddleHalfLongTotalButton":
                    return r.Schlag[0].IsMidMid();
                case "PlacementMiddleHalfLongPointsWonButton":
                    return r.Schlag[0].IsMidMid() && r.Schlag[0].Spieler == r.Winner;
                case "PlacementMiddleHalfLongDirectPointsWonButton":
                    return r.Schlag[0].IsMidMid() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementMiddleHalfLongPointsLostButton":
                    return r.Schlag[0].IsMidMid() && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region MiddleShort
                case "PlacementMiddleShortTotalButton":
                    return r.Schlag[0].IsBotMid();
                case "PlacementMiddleShortPointsWonButton":
                    return r.Schlag[0].IsBotMid() && r.Schlag[0].Spieler == r.Winner;
                case "PlacementMiddleShortDirectPointsWonButton":
                    return r.Schlag[0].IsBotMid() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementMiddleShortPointsLostButton":
                    return r.Schlag[0].IsBotMid() && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region BackhandAll
                case "PlacementBackhandAllTotalButton":
                    return r.Schlag[0].IsTopRight() || r.Schlag[0].IsMidRight() || r.Schlag[0].IsBotRight();
                case "PlacementBackhandAllPointsWonButton":
                    return (r.Schlag[0].IsTopRight() || r.Schlag[0].IsMidRight() || r.Schlag[0].IsBotRight()) && r.Schlag[0].Spieler == r.Winner;
                case "PlacementBackhandAllDirectPointsWonButton":
                    return (r.Schlag[0].IsTopRight() || r.Schlag[0].IsMidRight() || r.Schlag[0].IsBotRight()) && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementBackhandAllPointsLostButton":
                    return (r.Schlag[0].IsTopRight() || r.Schlag[0].IsMidRight() || r.Schlag[0].IsBotRight()) && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region BackhandLong
                case "PlacementBackhandLongTotalButton":
                    return r.Schlag[0].IsTopRight();
                case "PlacementBackhandLongPointsWonButton":
                    return r.Schlag[0].IsTopRight() && r.Schlag[0].Spieler == r.Winner;
                case "PlacementBackhandLongDirectPointsWonButton":
                    return r.Schlag[0].IsTopRight() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementBackhandLongPointsLostButton":
                    return r.Schlag[0].IsTopRight() && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region BackhandHalfLong
                case "PlacementBackhandHalfLongTotalButton":
                    return r.Schlag[0].IsMidRight();
                case "PlacementBackhandHalfLongPointsWonButton":
                    return r.Schlag[0].IsMidRight() && r.Schlag[0].Spieler == r.Winner;
                case "PlacementBackhandHalfLongDirectPointsWonButton":
                    return r.Schlag[0].IsMidRight() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementBackhandHalfLongPointsLostButton":
                    return r.Schlag[0].IsMidRight() && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region BackhandShort
                case "PlacementBackhandShortTotalButton":
                    return r.Schlag[0].IsBotRight();
                case "PlacementBackhandShortPointsWonButton":
                    return r.Schlag[0].IsBotRight() && r.Schlag[0].Spieler == r.Winner;
                case "PlacementBackhandShortDirectPointsWonButton":
                    return r.Schlag[0].IsBotRight() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementBackhandShortPointsLostButton":
                    return r.Schlag[0].IsBotRight() && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region AllLong
                case "PlacementAllLongTotalButton":
                    return (r.Schlag[0].IsTopLeft() || r.Schlag[0].IsTopMid() || r.Schlag[0].IsTopRight());
                case "PlacementAllLongPointsWonButton":
                    return (r.Schlag[0].IsTopLeft() || r.Schlag[0].IsTopMid() || r.Schlag[0].IsTopRight()) && r.Schlag[0].Spieler == r.Winner;
                case "PlacementAllLongDirectPointsWonButton":
                    return (r.Schlag[0].IsTopLeft() || r.Schlag[0].IsTopMid() || r.Schlag[0].IsTopRight()) && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementAllLongPointsLostButton":
                    return (r.Schlag[0].IsTopLeft() || r.Schlag[0].IsTopMid() || r.Schlag[0].IsTopRight()) && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region AllHalfLong
                case "PlacementAllHalfLongTotalButton":
                    return (r.Schlag[0].IsMidLeft() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsMidRight());
                case "PlacementAllHalfLongPointsWonButton":
                    return (r.Schlag[0].IsMidLeft() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsMidRight()) && r.Schlag[0].Spieler == r.Winner;
                case "PlacementAllHalfLongDirectPointsWonButton":
                    return (r.Schlag[0].IsMidLeft() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsMidRight()) && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementAllHalfLongPointsLostButton":
                    return (r.Schlag[0].IsMidLeft() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsMidRight()) && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region AllShort
                case "PlacementAllShortTotalButton":
                    return (r.Schlag[0].IsBotLeft() || r.Schlag[0].IsBotMid() || r.Schlag[0].IsBotRight());
                case "PlacementAllShortPointsWonButton":
                    return (r.Schlag[0].IsBotLeft() || r.Schlag[0].IsBotMid() || r.Schlag[0].IsBotRight()) && r.Schlag[0].Spieler == r.Winner;
                case "PlacementAllShortDirectPointsWonButton":
                    return (r.Schlag[0].IsBotLeft() || r.Schlag[0].IsBotMid() || r.Schlag[0].IsBotRight()) && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PlacementAllShortPointsLostButton":
                    return (r.Schlag[0].IsBotLeft() || r.Schlag[0].IsBotMid() || r.Schlag[0].IsBotRight()) && r.Schlag[0].Spieler != r.Winner;
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
                    return r.Schlag[0].IsLeftServicePosition();
                case "PositionLeftPointsWonButton":
                    return r.Schlag[0].IsLeftServicePosition() && r.Schlag[0].Spieler == r.Winner;
                case "PositionLeftDirectPointsWonButton":
                    return r.Schlag[0].IsLeftServicePosition() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PositionLeftPointsLostButton":
                    return r.Schlag[0].IsLeftServicePosition() && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region Position Middle
                case "PositionMiddleTotalButton":
                    return r.Schlag[0].IsMiddleServicePosition();
                case "PositionMiddlePointsWonButton":
                    return r.Schlag[0].IsMiddleServicePosition() && r.Schlag[0].Spieler == r.Winner;
                case "PositionMiddleDirectPointsWonButton":
                    return r.Schlag[0].IsMiddleServicePosition() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PositionMiddlePointsLostButton":
                    return r.Schlag[0].IsMiddleServicePosition() && r.Schlag[0].Spieler != r.Winner;
                #endregion
                #region Position Right
                case "PositionRightTotalButton":
                    return r.Schlag[0].IsRightServicePosition();
                case "PositionRightPointsWonButton":
                    return r.Schlag[0].IsRightServicePosition() && r.Schlag[0].Spieler == r.Winner;
                case "PositionRightDirectPointsWonButton":
                    return r.Schlag[0].IsRightServicePosition() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "PositionRightPointsLostButton":
                    return r.Schlag[0].IsRightServicePosition() && r.Schlag[0].Spieler != r.Winner;
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

                case "ForhandPendulumTotalButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Pendulum";
                case "ForhandPendulumPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler == r.Winner;
                case "ForhandPendulumDirectPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "ForhandPendulumPointsLostButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler != r.Winner;
                case "BackhandPendulumTotalButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Pendulum";
                case "BackhandPendulumPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler == r.Winner;
                case "BackhandPendulumDirectPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "BackhandPendulumPointsLostButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler != r.Winner;
                case "AllPendulumTotalButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Pendulum";
                case "AllPendulumPointsWonButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler == r.Winner;
                case "AllPendulumDirectPointsWonButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllPendulumPointsLostButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler != r.Winner;

                #endregion

                #region ReversePendulum 

                case "ForhandReversePendulumTotalButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Gegenläufer";
                case "ForhandReversePendulumPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler == r.Winner;
                case "ForhandReversePendulumDirectPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "ForhandReversePendulumPointsLostButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler != r.Winner;
                case "BackhandReversePendulumTotalButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Gegenläufer";
                case "BackhandReversePendulumPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler == r.Winner;
                case "BackhandReversePendulumDirectPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "BackhandReversePendulumPointsLostButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler != r.Winner;
                case "AllReversePendulumTotalButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Gegenläufer";
                case "AllReversePendulumPointsWonButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler == r.Winner;
                case "AllReversePendulumDirectPointsWonButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllReversePendulumPointsLostButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler != r.Winner;

                #endregion
                #region Tomahawk 

                case "ForhandTomahawkTotalButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Tomahawk";
                case "ForhandTomahawkPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler == r.Winner;
                case "ForhandTomahawkDirectPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "ForhandTomahawkPointsLostButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler != r.Winner;
                case "BackhandTomahawkTotalButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Tomahawk";
                case "BackhandTomahawkPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler == r.Winner;
                case "BackhandTomahawkDirectPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "BackhandTomahawkPointsLostButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler != r.Winner;
                case "AllTomahawkTotalButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Tomahawk";
                case "AllTomahawkPointsWonButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler == r.Winner;
                case "AllTomahawkDirectPointsWonButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllTomahawkPointsLostButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler != r.Winner;

                #endregion

                #region Special 

                case "ForhandSpecialTotalButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Spezial";
                case "ForhandSpecialPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler == r.Winner;
                case "ForhandSpecialDirectPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "ForhandSpecialPointsLostButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler != r.Winner;
                case "BackhandSpecialTotalButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Spezial";
                case "BackhandSpecialPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler == r.Winner;
                case "BackhandSpecialDirectPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "BackhandSpecialPointsLostButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler != r.Winner;
                case "AllSpecialTotalButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Spezial";
                case "AllSpecialPointsWonButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler == r.Winner;
                case "AllSpecialDirectPointsWonButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllSpecialPointsLostButton":
                    return (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler != r.Winner;

                #endregion

                #region All Forhand Services
                case "ForhandAllTotalButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial");
                case "ForhandAllPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler == r.Winner;
                case "ForhandAllDirectPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "ForhandAllPointsLostButton":
                    return r.Schlag[0].Schlägerseite == "Vorhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler != r.Winner;
                #endregion

                #region All Backhand Services
                case "BackhandAllTotalButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial");
                case "BackhandAllPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler == r.Winner;
                case "BackhandAllDirectPointsWonButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "BackhandAllPointsLostButton":
                    return r.Schlag[0].Schlägerseite == "Rückhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler != r.Winner;
                #endregion


                #region All Services
                case "AllServicesTotalButton":
                    return (r.Schlag[0].Schlägerseite == "Rückhand" || r.Schlag[0].Schlägerseite == "Vorhand") && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial");
                case "AllServicesPointsWonButton":
                    return (r.Schlag[0].Schlägerseite == "Rückhand" || r.Schlag[0].Schlägerseite == "Vorhand") && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler == r.Winner;
                case "AllServicesDirectPointsWonButton":
                    return (r.Schlag[0].Schlägerseite == "Rückhand" || r.Schlag[0].Schlägerseite == "Vorhand") && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllServicesPointsLostButton":
                    return (r.Schlag[0].Schlägerseite == "Rückhand" || r.Schlag[0].Schlägerseite == "Vorhand") && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler != r.Winner;
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
                    return (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.ÜS == "1");
                case "UpSideLeftPointsWonButton":
                    return (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler == r.Winner;
                case "UpSideLeftDirectPointsWonButton":
                    return (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "UpSideLeftPointsLostButton":
                    return (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler != r.Winner;

                case "UpTotalButton":
                    return (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "1");
                case "UpPointsWonButton":
                    return (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler == r.Winner;
                case "UpDirectPointsWonButton":
                    return (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "UpPointsLostButton":
                    return (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler != r.Winner;

                case "UpSideRightTotalButton":
                    return (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "1");
                case "UpSideRightPointsWonButton":
                    return (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler == r.Winner;
                case "UpSideRightDirectPointsWonButton":
                    return (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "UpSideRightPointsLostButton":
                    return (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler != r.Winner;

                case "UpAllTotalButton":
                    return r.Schlag[0].Spin.ÜS == "1";
                case "UpAllPointsWonButton":
                    return r.Schlag[0].Spin.ÜS == "1" && r.Schlag[0].Spieler == r.Winner;
                case "UpAllDirectPointsWonButton":
                    return r.Schlag[0].Spin.ÜS == "1" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "UpAllPointsLostButton":
                    return r.Schlag[0].Spin.ÜS == "1" && r.Schlag[0].Spieler != r.Winner;

                #endregion

                #region No UpDown Spin

                case "SideLeftTotalButton":
                    return (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0");
                case "SideLeftPointsWonButton":
                    return (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner;
                case "SideLeftDirectPointsWonButton":
                    return (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "SideLeftPointsLostButton":
                    return (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler != r.Winner;

                case "NoSpinTotalButton":
                    return (r.Schlag[0].Spin.No == "1" && r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0");
                case "NoSpinPointsWonButton":
                    return (r.Schlag[0].Spin.No == "1" && r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner;
                case "NoSpinDirectPointsWonButton":
                    return (r.Schlag[0].Spin.No == "1" && r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "NoSpinPointsLostButton":
                    return (r.Schlag[0].Spin.No == "1" && r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler != r.Winner;

                case "SideRightTotalButton":
                    return (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0");
                case "SideRightPointsWonButton":
                    return (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner;
                case "SideRightDirectPointsWonButton":
                    return (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "SideRightPointsLostButton":
                    return (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler != r.Winner;

                case "NoUpDownAllTotalButton":
                    return (r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0");
                case "NoUpDownAllPointsWonButton":
                    return (r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner;
                case "NoUpDownAllDirectPointsWonButton":
                    return (r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "NoUpDownAllPointsLostButton":
                    return (r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler != r.Winner;

                #endregion

                #region DownSpin

                case "DownSideLeftTotalButton":
                    return (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.US == "1");
                case "DownSideLeftPointsWonButton":
                    return (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler == r.Winner;
                case "DownSideLeftDirectPointsWonButton":
                    return (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "DownSideLeftPointsLostButton":
                    return (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler != r.Winner;

                case "DownTotalButton":
                    return (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.US == "1");
                case "DownPointsWonButton":
                    return (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler == r.Winner;
                case "DownDirectPointsWonButton":
                    return (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "DownPointsLostButton":
                    return (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler != r.Winner;

                case "DownSideRightTotalButton":
                    return (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.US == "1");
                case "DownSideRightPointsWonButton":
                    return (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler == r.Winner;
                case "DownSideRightDirectPointsWonButton":
                    return (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "DownSideRightPointsLostButton":
                    return (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler != r.Winner;

                case "DownAllTotalButton":
                    return r.Schlag[0].Spin.US == "1";
                case "DownAllPointsWonButton":
                    return r.Schlag[0].Spin.US == "1" && r.Schlag[0].Spieler == r.Winner;
                case "DownAllDirectPointsWonButton":
                    return r.Schlag[0].Spin.US == "1" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "DownAllPointsLostButton":
                    return r.Schlag[0].Spin.US == "1" && r.Schlag[0].Spieler != r.Winner;

                #endregion

                #region SideLeft All

                case "SideLeftAllTotalButton":
                    return (r.Schlag[0].Spin.SL == "1");
                case "SideLeftAllPointsWonButton":
                    return (r.Schlag[0].Spin.SL == "1") && r.Schlag[0].Spieler == r.Winner;
                case "SideLeftAllDirectPointsWonButton":
                    return (r.Schlag[0].Spin.SL == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "SideLeftAllPointsLostButton":
                    return (r.Schlag[0].Spin.SL == "1") && r.Schlag[0].Spieler != r.Winner;

                #endregion

                #region No SideSpin All

                case "NoSideAllTotalButton":
                    return (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0");
                case "NoSideAllPointsWonButton":
                    return (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0") && r.Schlag[0].Spieler == r.Winner;
                case "NoSideAllDirectPointsWonButton":
                    return (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "NoSideAllPointsLostButton":
                    return (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0") && r.Schlag[0].Spieler != r.Winner;


                #endregion

                #region SideRight All

                case "SideRightAllTotalButton":
                    return (r.Schlag[0].Spin.SR == "1");
                case "SideRightAllPointsWonButton":
                    return (r.Schlag[0].Spin.SR == "1") && r.Schlag[0].Spieler == r.Winner;
                case "SideRightAllDirectPointsWonButton":
                    return (r.Schlag[0].Spin.SR == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "SideRightAllPointsLostButton":
                    return (r.Schlag[0].Spin.SR == "1") && r.Schlag[0].Spieler != r.Winner;


                #endregion

                #region All Spins Total

                case "AllSpinTotalButton":
                    return (r.Schlag[0].Spin.SR == "1" || r.Schlag[0].Spin.SL == "1" || r.Schlag[0].Spin.US == "1" || r.Schlag[0].Spin.ÜS == "1" || r.Schlag[0].Spin.No == "1");
                case "AllSpinPointsWonButton":
                    return (r.Schlag[0].Spin.SR == "1" || r.Schlag[0].Spin.SL == "1" || r.Schlag[0].Spin.US == "1" || r.Schlag[0].Spin.ÜS == "1" || r.Schlag[0].Spin.No == "1") && r.Schlag[0].Spieler == r.Winner;
                case "AllSpinDirectPointsWonButton":
                    return (r.Schlag[0].Spin.SR == "1" || r.Schlag[0].Spin.SL == "1" || r.Schlag[0].Spin.US == "1" || r.Schlag[0].Spin.ÜS == "1" || r.Schlag[0].Spin.No == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3;
                case "AllSpinPointsLostButton":
                    return (r.Schlag[0].Spin.SR == "1" || r.Schlag[0].Spin.SL == "1" || r.Schlag[0].Spin.US == "1" || r.Schlag[0].Spin.ÜS == "1" || r.Schlag[0].Spin.No == "1") && r.Schlag[0].Spieler != r.Winner;


                #endregion

                default:
                    return true;
            }
        }
        private double AufschlagPosition(Rally r)
        {
            Schlag service = r.Schlag.Where(s => s.Nummer == "1").FirstOrDefault();
            double aufschlagPosition;
            double seite = service.Platzierung.WY == "" ? 999 : Convert.ToDouble(service.Platzierung.WY);
            if (seite >= 137)
            {
                aufschlagPosition = 152.5 - (service.Spielerposition == "" ? 999 : Convert.ToDouble(service.Spielerposition));
            }
            else
            {
                aufschlagPosition = service.Spielerposition == "" ? 999 : Convert.ToDouble(service.Spielerposition);
            }

            return aufschlagPosition;

        }



        #endregion
    }
}