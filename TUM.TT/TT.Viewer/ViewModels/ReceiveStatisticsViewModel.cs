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
    public class ReceiveStatisticsViewModel : Conductor<IScreen>.Collection.AllActive,
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

        public ReceiveStatisticsViewModel(IEventAggregator eventAggregator, IMatchManager man)
        {
            this.events = eventAggregator;
            Manager = man;
            X = "";
            Player1 = "Spieler 1";
            Player2 = "Spieler 2";

            BasicFilterStatisticsView = new BasicFilterStatisticsViewModel(this.events, Manager)
            {
                MinRallyLength = 1,
                LastStroke = false,
                StrokeNumber = 1
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
                var results = BasicFilterStatisticsView.SelectedRallies.Where(r => Convert.ToInt32(r.Length) > 1 && HasPlacement(r) && HasBasisInformation(r) && HasContactPosition(r) && HasTechnique(r)).ToList();
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
                    return Convert.ToInt32(r.Length) >= 2;
                case "TotalServicesCountPointPlayer1":
                    return Convert.ToInt32(r.Length) >= 2 && r.Winner == "First";
                case "TotalServicesCountPointPlayer2":
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

                #region ForhandAll
                case "PlacementForhandAllTotalButton":
                    return r.Schlag[1].IsTopLeft() || r.Schlag[1].IsMidLeft() || r.Schlag[1].IsBotLeft();
                case "PlacementForhandAllPointsWonButton":
                    return (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsMidLeft() || r.Schlag[1].IsBotLeft()) && r.Schlag[1].Spieler == r.Winner;
                case "PlacementForhandAllDirectPointsWonButton":
                    return (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsMidLeft() || r.Schlag[1].IsBotLeft()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementForhandAllPointsLostButton":
                    return (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsMidLeft() || r.Schlag[1].IsBotLeft()) && r.Schlag[1].Spieler != r.Winner;
                #endregion
                #region ForhandLong
                case "PlacementForhandLongTotalButton":
                    return r.Schlag[1].IsTopLeft();
                case "PlacementForhandLongPointsWonButton":
                    return r.Schlag[1].IsTopLeft() && r.Schlag[1].Spieler == r.Winner;
                case "PlacementForhandLongDirectPointsWonButton":
                    return r.Schlag[1].IsTopLeft() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementForhandLongPointsLostButton":
                    return r.Schlag[1].IsTopLeft() && r.Schlag[1].Spieler != r.Winner;
                #endregion
                #region ForhandHalfLong
                case "PlacementForhandHalfLongTotalButton":
                    return r.Schlag[1].IsMidLeft();
                case "PlacementForhandHalfLongPointsWonButton":
                    return r.Schlag[1].IsMidLeft() && r.Schlag[1].Spieler == r.Winner;
                case "PlacementForhandHalfLongDirectPointsWonButton":
                    return r.Schlag[1].IsMidLeft() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementForhandHalfLongPointsLostButton":
                    return r.Schlag[1].IsMidLeft() && r.Schlag[1].Spieler != r.Winner;
                #endregion
                #region ForhandShort
                case "PlacementForhandShortTotalButton":
                    return r.Schlag[1].IsBotLeft();
                case "PlacementForhandShortPointsWonButton":
                    return r.Schlag[1].IsBotLeft() && r.Schlag[1].Spieler == r.Winner;
                case "PlacementForhandShortDirectPointsWonButton":
                    return r.Schlag[1].IsBotLeft() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementForhandShortPointsLostButton":
                    return r.Schlag[1].IsBotLeft() && r.Schlag[1].Spieler != r.Winner;
                #endregion
                #region MiddleAll
                case "PlacementMiddleAllTotalButton":
                    return r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid();
                case "PlacementMiddleAllPointsWonButton":
                    return (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner;
                case "PlacementMiddleAllDirectPointsWonButton":
                    return (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementMiddleAllPointsLostButton":
                    return (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler != r.Winner;
                #endregion
                #region MiddleLong
                case "PlacementMiddleLongTotalButton":
                    return r.Schlag[1].IsTopMid();
                case "PlacementMiddleLongPointsWonButton":
                    return r.Schlag[1].IsTopMid() && r.Schlag[1].Spieler == r.Winner;
                case "PlacementMiddleLongDirectPointsWonButton":
                    return r.Schlag[1].IsTopMid() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementMiddleLongPointsLostButton":
                    return r.Schlag[1].IsTopMid() && r.Schlag[1].Spieler != r.Winner;
                #endregion
                #region MiddleHalfLong
                case "PlacementMiddleHalfLongTotalButton":
                    return r.Schlag[1].IsMidMid();
                case "PlacementMiddleHalfLongPointsWonButton":
                    return r.Schlag[1].IsMidMid() && r.Schlag[1].Spieler == r.Winner;
                case "PlacementMiddleHalfLongDirectPointsWonButton":
                    return r.Schlag[1].IsMidMid() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementMiddleHalfLongPointsLostButton":
                    return r.Schlag[1].IsMidMid() && r.Schlag[1].Spieler != r.Winner;
                #endregion
                #region MiddleShort
                case "PlacementMiddleShortTotalButton":
                    return r.Schlag[1].IsBotMid();
                case "PlacementMiddleShortPointsWonButton":
                    return r.Schlag[1].IsBotMid() && r.Schlag[1].Spieler == r.Winner;
                case "PlacementMiddleShortDirectPointsWonButton":
                    return r.Schlag[1].IsBotMid() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementMiddleShortPointsLostButton":
                    return r.Schlag[1].IsBotMid() && r.Schlag[1].Spieler != r.Winner;
                #endregion
                #region BackhandAll
                case "PlacementBackhandAllTotalButton":
                    return r.Schlag[1].IsTopRight() || r.Schlag[1].IsMidRight() || r.Schlag[1].IsBotRight();
                case "PlacementBackhandAllPointsWonButton":
                    return (r.Schlag[1].IsTopRight() || r.Schlag[1].IsMidRight() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Spieler == r.Winner;
                case "PlacementBackhandAllDirectPointsWonButton":
                    return (r.Schlag[1].IsTopRight() || r.Schlag[1].IsMidRight() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementBackhandAllPointsLostButton":
                    return (r.Schlag[1].IsTopRight() || r.Schlag[1].IsMidRight() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Spieler != r.Winner;
                #endregion
                #region BackhandLong
                case "PlacementBackhandLongTotalButton":
                    return r.Schlag[1].IsTopRight();
                case "PlacementBackhandLongPointsWonButton":
                    return r.Schlag[1].IsTopRight() && r.Schlag[1].Spieler == r.Winner;
                case "PlacementBackhandLongDirectPointsWonButton":
                    return r.Schlag[1].IsTopRight() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementBackhandLongPointsLostButton":
                    return r.Schlag[1].IsTopRight() && r.Schlag[1].Spieler != r.Winner;
                #endregion
                #region BackhandHalfLong
                case "PlacementBackhandHalfLongTotalButton":
                    return r.Schlag[1].IsMidRight();
                case "PlacementBackhandHalfLongPointsWonButton":
                    return r.Schlag[1].IsMidRight() && r.Schlag[1].Spieler == r.Winner;
                case "PlacementBackhandHalfLongDirectPointsWonButton":
                    return r.Schlag[1].IsMidRight() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementBackhandHalfLongPointsLostButton":
                    return r.Schlag[1].IsMidRight() && r.Schlag[1].Spieler != r.Winner;
                #endregion
                #region BackhandShort
                case "PlacementBackhandShortTotalButton":
                    return r.Schlag[1].IsBotRight();
                case "PlacementBackhandShortPointsWonButton":
                    return r.Schlag[1].IsBotRight() && r.Schlag[1].Spieler == r.Winner;
                case "PlacementBackhandShortDirectPointsWonButton":
                    return r.Schlag[1].IsBotRight() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementBackhandShortPointsLostButton":
                    return r.Schlag[1].IsBotRight() && r.Schlag[1].Spieler != r.Winner;
                #endregion
                #region AllLong
                case "PlacementAllLongTotalButton":
                    return (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight());
                case "PlacementAllLongPointsWonButton":
                    return (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight()) && r.Schlag[1].Spieler == r.Winner;
                case "PlacementAllLongDirectPointsWonButton":
                    return (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementAllLongPointsLostButton":
                    return (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight()) && r.Schlag[1].Spieler != r.Winner;
                #endregion
                #region AllHalfLong
                case "PlacementAllHalfLongTotalButton":
                    return (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight());
                case "PlacementAllHalfLongPointsWonButton":
                    return (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight()) && r.Schlag[1].Spieler == r.Winner;
                case "PlacementAllHalfLongDirectPointsWonButton":
                    return (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementAllHalfLongPointsLostButton":
                    return (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight()) && r.Schlag[1].Spieler != r.Winner;
                #endregion
                #region AllShort
                case "PlacementAllShortTotalButton":
                    return (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight());
                case "PlacementAllShortPointsWonButton":
                    return (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Spieler == r.Winner;
                case "PlacementAllShortDirectPointsWonButton":
                    return (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementAllShortPointsLostButton":
                    return (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Spieler != r.Winner;
                #endregion

                #region ReceiveErrors
                case "PlacementAllServiceErrorsTotalButton":
                    return r.Server == r.Winner && r.Length == 2;
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
                    return r.Schlag[1].Balltreffpunkt == "über";
                case "OverTheTablePointsWonButton":
                    return r.Schlag[1].Balltreffpunkt == "über" && r.Schlag[1].Spieler == r.Winner;
                case "OverTheTableDirectPointsWonButton":
                    return r.Schlag[1].Balltreffpunkt == "über" && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "OverTheTablePointsLostButton":
                    return r.Schlag[1].Balltreffpunkt == "über" && r.Schlag[1].Spieler != r.Winner;

                #endregion

                #region at the table
                case "AtTheTableTotalButton":
                    return r.Schlag[1].Balltreffpunkt == "hinter";
                case "AtTheTablePointsWonButton":
                    return r.Schlag[1].Balltreffpunkt == "hinter" && r.Schlag[1].Spieler == r.Winner;
                case "AtTheTableDirectPointsWonButton":
                    return r.Schlag[1].Balltreffpunkt == "hinter" && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "AtTheTablePointsLostButton":
                    return r.Schlag[1].Balltreffpunkt == "hinter" && r.Schlag[1].Spieler != r.Winner;

                #endregion

                #region half distance
                case "HaldDistanceTotalButton":
                    return r.Schlag[1].Balltreffpunkt == "Halbdistanz";
                case "HaldDistancePointsWonButton":
                    return r.Schlag[1].Balltreffpunkt == "Halbdistanz" && r.Schlag[1].Spieler == r.Winner;
                case "HaldDistanceDirectPointsWonButton":
                    return r.Schlag[1].Balltreffpunkt == "Halbdistanz" && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "HaldDistancePointsLostButton":
                    return r.Schlag[1].Balltreffpunkt == "Halbdistanz" && r.Schlag[1].Spieler != r.Winner;

                #endregion

                default:
                    return true;

            }





            #endregion
        }

        public bool HasTechnique(Rally r)
        {
            switch (X)
            {
                case "":
                    return true;

                #region Flip 
                case "ForhandFlipTotalButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Flip";
                case "ForhandFlipPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler == r.Winner;
                case "ForhandFlipDirectPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "ForhandFlipPointsLostButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler != r.Winner;

                case "BackhandFlipTotalButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Flip";
                case "BackhandFlipPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler == r.Winner;
                case "BackhandFlipDirectPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "BackhandFlipPointsLostButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler != r.Winner;

                case "AllFlipTotalButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand")  && r.Schlag[1].Schlagtechnik.Art == "Flip";
                case "AllFlipPointsWonButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler == r.Winner;
                case "AllFlipDirectPointsWonButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "AllFlipPointsLostButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler != r.Winner;

                #endregion

                #region Push short

                case "ForhandPushShortTotalButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight());
                case "ForhandPushShortPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Spieler == r.Winner;
                case "ForhandPushShortDirectPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "ForhandPushShortPointsLostButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Spieler != r.Winner;

                case "BackhandPushShortTotalButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight());
                case "BackhandPushShortPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Spieler == r.Winner;
                case "BackhandPushShortDirectPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "BackhandPushShortPointsLostButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Spieler != r.Winner;

                case "AllPushShortTotalButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Schlagtechnik.Art == "Schupf";
                case "AllPushShortPointsWonButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].Spieler == r.Winner;
                case "AllPushShortDirectPointsWonButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "AllPushShortPointsLostButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && (r.Schlag[1].IsBotLeft() || r.Schlag[1].IsBotMid() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].Spieler != r.Winner;

                #endregion

                #region Push halflong

                case "ForhandPushHalfLongTotalButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight());
                case "ForhandPushHalfLongPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight()) && r.Schlag[1].Spieler == r.Winner;
                case "ForhandPushHalfLongDirectPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "ForhandPushHalfLongPointsLostButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight()) && r.Schlag[1].Spieler != r.Winner;

                case "BackhandPushHalfLongTotalButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight());
                case "BackhandPushHalfLongPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight()) && r.Schlag[1].Spieler == r.Winner;
                case "BackhandPushHalfLongDirectPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "BackhandPushHalfLongPointsLostButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight()) && r.Schlag[1].Spieler != r.Winner;

                case "AllPushHalfLongTotalButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight()) && r.Schlag[1].Schlagtechnik.Art == "Schupf";
                case "AllPushHalfLongPointsWonButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight()) && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].Spieler == r.Winner;
                case "AllPushHalfLongDirectPointsWonButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight()) && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "AllPushHalfLongPointsLostButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && (r.Schlag[1].IsMidLeft() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsMidRight()) && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].Spieler != r.Winner;


                #endregion

                #region Push long

                case "ForhandPushLongTotalButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight());
                case "ForhandPushLongPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight()) && r.Schlag[1].Spieler == r.Winner;
                case "ForhandPushLongDirectPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "ForhandPushLongPointsLostButton":
                    return r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight()) && r.Schlag[1].Spieler != r.Winner;

                case "BackhandPushLongTotalButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight());
                case "BackhandPushLongPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight()) && r.Schlag[1].Spieler == r.Winner;
                case "BackhandPushLongDirectPointsWonButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "BackhandPushLongPointsLostButton":
                    return r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight()) && r.Schlag[1].Spieler != r.Winner;

                case "AllPushLongTotalButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight()) && r.Schlag[1].Schlagtechnik.Art == "Schupf";
                case "AllPushLongPointsWonButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight()) && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].Spieler == r.Winner;
                case "AllPushLongDirectPointsWonButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight()) && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "AllPushLongPointsLostButton":
                    return (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsTopMid() || r.Schlag[1].IsTopRight()) && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].Spieler != r.Winner;


                #endregion


               
                default:
                    return true;
            }

            }
    }
}
