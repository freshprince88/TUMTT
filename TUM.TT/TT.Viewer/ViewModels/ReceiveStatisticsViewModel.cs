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
                var results = BasicFilterStatisticsView.SelectedRallies.Where(r => Convert.ToInt32(r.Length) > 1 && HasPlacement(r) && r.HasBasisInformationStatistics(2,X) && HasContactPosition(r) && r.HasTechniqueStatistics(1, X)).ToList();
                this.events.PublishOnUIThread(new ResultsChangedEvent(results));
            }
        }

        //public bool HasBasisInformation(Rally r)
        //{
        //    switch (X)
        //    {
        //        case "":
        //            return true;
        //        case "TotalReceivesCount":
        //            return Convert.ToInt32(r.Length) >= 2;
        //        case "TotalReceivesCountPointPlayer1":
        //            return Convert.ToInt32(r.Length) >= 2 && r.Winner == "First";
        //        case "TotalReceivesCountPointPlayer2":
        //            return Convert.ToInt32(r.Length) >= 2 && r.Winner == "Second";
        //        default:
        //            return true;

        //    }
        //}

        public bool HasPlacement(Rally r)
        {
            switch (X)
            {
                case "":
                    return true;

                #region ForehandAll
                case "PlacementForehandAllTotalButton":
                    return r.Schläge[1].IsTopLeft() || r.Schläge[1].IsMidLeft() || r.Schläge[1].IsBotLeft();
                case "PlacementForehandAllPointsWonButton":
                    return (r.Schläge[1].IsTopLeft() || r.Schläge[1].IsMidLeft() || r.Schläge[1].IsBotLeft()) && r.Schläge[1].Spieler == r.Winner;
                case "PlacementForehandAllDirectPointsWonButton":
                    return (r.Schläge[1].IsTopLeft() || r.Schläge[1].IsMidLeft() || r.Schläge[1].IsBotLeft()) && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementForehandAllPointsLostButton":
                    return (r.Schläge[1].IsTopLeft() || r.Schläge[1].IsMidLeft() || r.Schläge[1].IsBotLeft()) && r.Schläge[1].Spieler != r.Winner;
                #endregion
                #region ForehandLong
                case "PlacementForehandLongTotalButton":
                    return r.Schläge[1].IsTopLeft();
                case "PlacementForehandLongPointsWonButton":
                    return r.Schläge[1].IsTopLeft() && r.Schläge[1].Spieler == r.Winner;
                case "PlacementForehandLongDirectPointsWonButton":
                    return r.Schläge[1].IsTopLeft() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementForehandLongPointsLostButton":
                    return r.Schläge[1].IsTopLeft() && r.Schläge[1].Spieler != r.Winner;
                #endregion
                #region ForehandHalfLong
                case "PlacementForehandHalfLongTotalButton":
                    return r.Schläge[1].IsMidLeft();
                case "PlacementForehandHalfLongPointsWonButton":
                    return r.Schläge[1].IsMidLeft() && r.Schläge[1].Spieler == r.Winner;
                case "PlacementForehandHalfLongDirectPointsWonButton":
                    return r.Schläge[1].IsMidLeft() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementForehandHalfLongPointsLostButton":
                    return r.Schläge[1].IsMidLeft() && r.Schläge[1].Spieler != r.Winner;
                #endregion
                #region ForehandShort
                case "PlacementForehandShortTotalButton":
                    return r.Schläge[1].IsBotLeft();
                case "PlacementForehandShortPointsWonButton":
                    return r.Schläge[1].IsBotLeft() && r.Schläge[1].Spieler == r.Winner;
                case "PlacementForehandShortDirectPointsWonButton":
                    return r.Schläge[1].IsBotLeft() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementForehandShortPointsLostButton":
                    return r.Schläge[1].IsBotLeft() && r.Schläge[1].Spieler != r.Winner;
                #endregion
                #region MiddleAll
                case "PlacementMiddleAllTotalButton":
                    return r.Schläge[1].IsTopMid() || r.Schläge[1].IsMidMid() || r.Schläge[1].IsBotMid();
                case "PlacementMiddleAllPointsWonButton":
                    return (r.Schläge[1].IsTopMid() || r.Schläge[1].IsMidMid() || r.Schläge[1].IsBotMid()) && r.Schläge[1].Spieler == r.Winner;
                case "PlacementMiddleAllDirectPointsWonButton":
                    return (r.Schläge[1].IsTopMid() || r.Schläge[1].IsMidMid() || r.Schläge[1].IsBotMid()) && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementMiddleAllPointsLostButton":
                    return (r.Schläge[1].IsTopMid() || r.Schläge[1].IsMidMid() || r.Schläge[1].IsBotMid()) && r.Schläge[1].Spieler != r.Winner;
                #endregion
                #region MiddleLong
                case "PlacementMiddleLongTotalButton":
                    return r.Schläge[1].IsTopMid();
                case "PlacementMiddleLongPointsWonButton":
                    return r.Schläge[1].IsTopMid() && r.Schläge[1].Spieler == r.Winner;
                case "PlacementMiddleLongDirectPointsWonButton":
                    return r.Schläge[1].IsTopMid() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementMiddleLongPointsLostButton":
                    return r.Schläge[1].IsTopMid() && r.Schläge[1].Spieler != r.Winner;
                #endregion
                #region MiddleHalfLong
                case "PlacementMiddleHalfLongTotalButton":
                    return r.Schläge[1].IsMidMid();
                case "PlacementMiddleHalfLongPointsWonButton":
                    return r.Schläge[1].IsMidMid() && r.Schläge[1].Spieler == r.Winner;
                case "PlacementMiddleHalfLongDirectPointsWonButton":
                    return r.Schläge[1].IsMidMid() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementMiddleHalfLongPointsLostButton":
                    return r.Schläge[1].IsMidMid() && r.Schläge[1].Spieler != r.Winner;
                #endregion
                #region MiddleShort
                case "PlacementMiddleShortTotalButton":
                    return r.Schläge[1].IsBotMid();
                case "PlacementMiddleShortPointsWonButton":
                    return r.Schläge[1].IsBotMid() && r.Schläge[1].Spieler == r.Winner;
                case "PlacementMiddleShortDirectPointsWonButton":
                    return r.Schläge[1].IsBotMid() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementMiddleShortPointsLostButton":
                    return r.Schläge[1].IsBotMid() && r.Schläge[1].Spieler != r.Winner;
                #endregion
                #region BackhandAll
                case "PlacementBackhandAllTotalButton":
                    return r.Schläge[1].IsTopRight() || r.Schläge[1].IsMidRight() || r.Schläge[1].IsBotRight();
                case "PlacementBackhandAllPointsWonButton":
                    return (r.Schläge[1].IsTopRight() || r.Schläge[1].IsMidRight() || r.Schläge[1].IsBotRight()) && r.Schläge[1].Spieler == r.Winner;
                case "PlacementBackhandAllDirectPointsWonButton":
                    return (r.Schläge[1].IsTopRight() || r.Schläge[1].IsMidRight() || r.Schläge[1].IsBotRight()) && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementBackhandAllPointsLostButton":
                    return (r.Schläge[1].IsTopRight() || r.Schläge[1].IsMidRight() || r.Schläge[1].IsBotRight()) && r.Schläge[1].Spieler != r.Winner;
                #endregion
                #region BackhandLong
                case "PlacementBackhandLongTotalButton":
                    return r.Schläge[1].IsTopRight();
                case "PlacementBackhandLongPointsWonButton":
                    return r.Schläge[1].IsTopRight() && r.Schläge[1].Spieler == r.Winner;
                case "PlacementBackhandLongDirectPointsWonButton":
                    return r.Schläge[1].IsTopRight() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementBackhandLongPointsLostButton":
                    return r.Schläge[1].IsTopRight() && r.Schläge[1].Spieler != r.Winner;
                #endregion
                #region BackhandHalfLong
                case "PlacementBackhandHalfLongTotalButton":
                    return r.Schläge[1].IsMidRight();
                case "PlacementBackhandHalfLongPointsWonButton":
                    return r.Schläge[1].IsMidRight() && r.Schläge[1].Spieler == r.Winner;
                case "PlacementBackhandHalfLongDirectPointsWonButton":
                    return r.Schläge[1].IsMidRight() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementBackhandHalfLongPointsLostButton":
                    return r.Schläge[1].IsMidRight() && r.Schläge[1].Spieler != r.Winner;
                #endregion
                #region BackhandShort
                case "PlacementBackhandShortTotalButton":
                    return r.Schläge[1].IsBotRight();
                case "PlacementBackhandShortPointsWonButton":
                    return r.Schläge[1].IsBotRight() && r.Schläge[1].Spieler == r.Winner;
                case "PlacementBackhandShortDirectPointsWonButton":
                    return r.Schläge[1].IsBotRight() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementBackhandShortPointsLostButton":
                    return r.Schläge[1].IsBotRight() && r.Schläge[1].Spieler != r.Winner;
                #endregion
                #region AllLong
                case "PlacementAllLongTotalButton":
                    return r.Schläge[1].IsLong();
                case "PlacementAllLongPointsWonButton":
                    return r.Schläge[1].IsLong() && r.Schläge[1].Spieler == r.Winner;
                case "PlacementAllLongDirectPointsWonButton":
                    return r.Schläge[1].IsLong() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementAllLongPointsLostButton":
                    return r.Schläge[1].IsLong() && r.Schläge[1].Spieler != r.Winner;
                #endregion
                #region AllHalfLong
                case "PlacementAllHalfLongTotalButton":
                    return r.Schläge[1].IsHalfLong();
                case "PlacementAllHalfLongPointsWonButton":
                    return r.Schläge[1].IsHalfLong() && r.Schläge[1].Spieler == r.Winner;
                case "PlacementAllHalfLongDirectPointsWonButton":
                    return r.Schläge[1].IsHalfLong() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementAllHalfLongPointsLostButton":
                    return r.Schläge[1].IsHalfLong() && r.Schläge[1].Spieler != r.Winner;
                #endregion
                #region AllShort
                case "PlacementAllShortTotalButton":
                    return r.Schläge[1].IsShort();
                case "PlacementAllShortPointsWonButton":
                    return r.Schläge[1].IsShort() && r.Schläge[1].Spieler == r.Winner;
                case "PlacementAllShortDirectPointsWonButton":
                    return r.Schläge[1].IsShort() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "PlacementAllShortPointsLostButton":
                    return r.Schläge[1].IsShort() && r.Schläge[1].Spieler != r.Winner;
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
                    return r.Schläge[1].Balltreffpunkt == "über";
                case "OverTheTablePointsWonButton":
                    return r.Schläge[1].Balltreffpunkt == "über" && r.Schläge[1].Spieler == r.Winner;
                case "OverTheTableDirectPointsWonButton":
                    return r.Schläge[1].Balltreffpunkt == "über" && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "OverTheTablePointsLostButton":
                    return r.Schläge[1].Balltreffpunkt == "über" && r.Schläge[1].Spieler != r.Winner;

                #endregion

                #region at the table
                case "AtTheTableTotalButton":
                    return r.Schläge[1].Balltreffpunkt == "hinter";
                case "AtTheTablePointsWonButton":
                    return r.Schläge[1].Balltreffpunkt == "hinter" && r.Schläge[1].Spieler == r.Winner;
                case "AtTheTableDirectPointsWonButton":
                    return r.Schläge[1].Balltreffpunkt == "hinter" && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "AtTheTablePointsLostButton":
                    return r.Schläge[1].Balltreffpunkt == "hinter" && r.Schläge[1].Spieler != r.Winner;

                #endregion

                #region half distance
                case "HalfDistanceTotalButton":
                    return r.Schläge[1].Balltreffpunkt == "Halbdistanz";
                case "HalfDistancePointsWonButton":
                    return r.Schläge[1].Balltreffpunkt == "Halbdistanz" && r.Schläge[1].Spieler == r.Winner;
                case "HalfDistanceDirectPointsWonButton":
                    return r.Schläge[1].Balltreffpunkt == "Halbdistanz" && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
                case "HalfDistancePointsLostButton":
                    return r.Schläge[1].Balltreffpunkt == "Halbdistanz" && r.Schläge[1].Spieler != r.Winner;

                #endregion

                default:
                    return true;

            }





            #endregion
        }

        //public bool HasTechnique(Rally r)
        //{
        //    switch (X)
        //    {
        //        case "":
        //            return true;

        //        #region Flip 
        //        case "ForehandFlipTotalButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Flip";
        //        case "ForehandFlipPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Flip" && r.Schläge[1].Spieler == r.Winner;
        //        case "ForehandFlipDirectPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Flip" && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "ForehandFlipPointsLostButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Flip" && r.Schläge[1].Spieler != r.Winner;

        //        case "BackhandFlipTotalButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Flip";
        //        case "BackhandFlipPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Flip" && r.Schläge[1].Spieler == r.Winner;
        //        case "BackhandFlipDirectPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Flip" && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "BackhandFlipPointsLostButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Flip" && r.Schläge[1].Spieler != r.Winner;

        //        case "AllFlipTotalButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand")  && r.Schläge[1].Schlagtechnik.Art == "Flip";
        //        case "AllFlipPointsWonButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.Schläge[1].Schlagtechnik.Art == "Flip" && r.Schläge[1].Spieler == r.Winner;
        //        case "AllFlipDirectPointsWonButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.Schläge[1].Schlagtechnik.Art == "Flip" && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "AllFlipPointsLostButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.Schläge[1].Schlagtechnik.Art == "Flip" && r.Schläge[1].Spieler != r.Winner;

        //        #endregion

        //        #region Push short

        //        case "ForehandPushShortTotalButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsShort();
        //        case "ForehandPushShortPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsShort() && r.Schläge[1].Spieler == r.Winner;
        //        case "ForehandPushShortDirectPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsShort() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "ForehandPushShortPointsLostButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsShort() && r.Schläge[1].Spieler != r.Winner;

        //        case "BackhandPushShortTotalButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsShort();
        //        case "BackhandPushShortPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsShort() && r.Schläge[1].Spieler == r.Winner;
        //        case "BackhandPushShortDirectPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsShort() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "BackhandPushShortPointsLostButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsShort() && r.Schläge[1].Spieler != r.Winner;

        //        case "AllPushShortTotalButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.Schläge[1].IsShort() && r.Schläge[1].Schlagtechnik.Art == "Schupf";
        //        case "AllPushShortPointsWonButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.Schläge[1].IsShort() && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].Spieler == r.Winner;
        //        case "AllPushShortDirectPointsWonButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.Schläge[1].IsShort() && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "AllPushShortPointsLostButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.Schläge[1].IsShort() && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].Spieler != r.Winner;

        //        #endregion

        //        #region Push halflong

        //        case "ForehandPushHalfLongTotalButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsHalfLong();
        //        case "ForehandPushHalfLongPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsHalfLong() && r.Schläge[1].Spieler == r.Winner;
        //        case "ForehandPushHalfLongDirectPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsHalfLong() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "ForehandPushHalfLongPointsLostButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsHalfLong() && r.Schläge[1].Spieler != r.Winner;

        //        case "BackhandPushHalfLongTotalButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsHalfLong();
        //        case "BackhandPushHalfLongPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsHalfLong() && r.Schläge[1].Spieler == r.Winner;
        //        case "BackhandPushHalfLongDirectPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsHalfLong() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "BackhandPushHalfLongPointsLostButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsHalfLong() && r.Schläge[1].Spieler != r.Winner;

        //        case "AllPushHalfLongTotalButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.Schläge[1].IsHalfLong() && r.Schläge[1].Schlagtechnik.Art == "Schupf";
        //        case "AllPushHalfLongPointsWonButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.Schläge[1].IsHalfLong() && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].Spieler == r.Winner;
        //        case "AllPushHalfLongDirectPointsWonButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.Schläge[1].IsHalfLong() && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "AllPushHalfLongPointsLostButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.Schläge[1].IsHalfLong() && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].Spieler != r.Winner;


        //        #endregion

        //        #region Push long

        //        case "ForehandPushLongTotalButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsLong();
        //        case "ForehandPushLongPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsLong() && r.Schläge[1].Spieler == r.Winner;
        //        case "ForehandPushLongDirectPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsLong() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "ForehandPushLongPointsLostButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsLong() && r.Schläge[1].Spieler != r.Winner;

        //        case "BackhandPushLongTotalButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsLong();
        //        case "BackhandPushLongPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsLong() && r.Schläge[1].Spieler == r.Winner;
        //        case "BackhandPushLongDirectPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsLong() && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "BackhandPushLongPointsLostButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].IsLong() && r.Schläge[1].Spieler != r.Winner;

        //        case "AllPushLongTotalButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.Schläge[1].IsLong() && r.Schläge[1].Schlagtechnik.Art == "Schupf";
        //        case "AllPushLongPointsWonButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.Schläge[1].IsLong() && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].Spieler == r.Winner;
        //        case "AllPushLongDirectPointsWonButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.Schläge[1].IsLong() && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "AllPushLongPointsLostButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.Schläge[1].IsLong() && r.Schläge[1].Schlagtechnik.Art == "Schupf" && r.Schläge[1].Spieler != r.Winner;


        //        #endregion

        //        #region Topspin diagonal

        //        case "ForehandTopspinDiagonalTotalButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsDiagonal(1);
        //        case "ForehandTopspinDiagonalPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsDiagonal(1) && r.Schläge[1].Spieler == r.Winner;
        //        case "ForehandTopspinDiagonalDirectPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsDiagonal(1) && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "ForehandTopspinDiagonalPointsLostButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsDiagonal(1) && r.Schläge[1].Spieler != r.Winner;

        //        case "BackhandTopspinDiagonalTotalButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsDiagonal(1);
        //        case "BackhandTopspinDiagonalPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsDiagonal(1) && r.Schläge[1].Spieler == r.Winner;
        //        case "BackhandTopspinDiagonalDirectPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsDiagonal(1) && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "BackhandTopspinDiagonalPointsLostButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsDiagonal(1) && r.Schläge[1].Spieler != r.Winner;

        //        case "AllTopspinDiagonalTotalButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.IsDiagonal(1) && r.Schläge[1].Schlagtechnik.Art == "Topspin";
        //        case "AllTopspinDiagonalPointsWonButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.IsDiagonal(1) && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.Schläge[1].Spieler == r.Winner;
        //        case "AllTopspinDiagonalDirectPointsWonButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.IsDiagonal(1) && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "AllTopspinDiagonalPointsLostButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.IsDiagonal(1) && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.Schläge[1].Spieler != r.Winner;

        //        #endregion

        //        #region Topspin Middle

        //        case "ForehandTopspinMiddleTotalButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsMiddle(1);
        //        case "ForehandTopspinMiddlePointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsMiddle(1) && r.Schläge[1].Spieler == r.Winner;
        //        case "ForehandTopspinMiddleDirectPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsMiddle(1) && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "ForehandTopspinMiddlePointsLostButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsMiddle(1) && r.Schläge[1].Spieler != r.Winner;

        //        case "BackhandTopspinMiddleTotalButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsMiddle(1);
        //        case "BackhandTopspinMiddlePointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsMiddle(1) && r.Schläge[1].Spieler == r.Winner;
        //        case "BackhandTopspinMiddleDirectPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsMiddle(1) && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "BackhandTopspinMiddlePointsLostButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsMiddle(1) && r.Schläge[1].Spieler != r.Winner;

        //        case "AllTopspinMiddleTotalButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.IsMiddle(1) && r.Schläge[1].Schlagtechnik.Art == "Topspin";
        //        case "AllTopspinMiddlePointsWonButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.IsMiddle(1) && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.Schläge[1].Spieler == r.Winner;
        //        case "AllTopspinMiddleDirectPointsWonButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.IsMiddle(1) && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "AllTopspinMiddlePointsLostButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.IsMiddle(1) && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.Schläge[1].Spieler != r.Winner;

        //        #endregion

        //        #region Topspin parallel

        //        case "ForehandTopspinParallelTotalButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsParallel(1);
        //        case "ForehandTopspinParallelPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsParallel(1) && r.Schläge[1].Spieler == r.Winner;
        //        case "ForehandTopspinParallelDirectPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsParallel(1) && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "ForehandTopspinParallelPointsLostButton":
        //            return r.Schläge[1].Schlägerseite == "Vorhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsParallel(1) && r.Schläge[1].Spieler != r.Winner;

        //        case "BackhandTopspinParallelTotalButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsParallel(1);
        //        case "BackhandTopspinParallelPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsParallel(1) && r.Schläge[1].Spieler == r.Winner;
        //        case "BackhandTopspinParallelDirectPointsWonButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsParallel(1) && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "BackhandTopspinParallelPointsLostButton":
        //            return r.Schläge[1].Schlägerseite == "Rückhand" && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.IsParallel(1) && r.Schläge[1].Spieler != r.Winner;

        //        case "AllTopspinParallelTotalButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.IsParallel(1) && r.Schläge[1].Schlagtechnik.Art == "Topspin";
        //        case "AllTopspinParallelPointsWonButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.IsParallel(1) && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.Schläge[1].Spieler == r.Winner;
        //        case "AllTopspinParallelDirectPointsWonButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.IsParallel(1) && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.Schläge[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4;
        //        case "AllTopspinParallelPointsLostButton":
        //            return (r.Schläge[1].Schlägerseite == "Vorhand" || r.Schläge[1].Schlägerseite == "Rückhand") && r.IsParallel(1) && r.Schläge[1].Schlagtechnik.Art == "Topspin" && r.Schläge[1].Spieler != r.Winner;

        //        #endregion





        //        default:
        //            return true;
        //    }

        //    }
    }
}
