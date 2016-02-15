using Caliburn.Micro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TT.Lib.Events;
using TT.Lib.Managers;
using TT.Lib.Models;

namespace TT.Viewer.Views
{
    /// <summary>
    /// Interaktionslogik für ReceiveStatisticsView.xaml
    /// </summary>
    public partial class ReceiveStatisticsView : UserControl,
        IHandle<StatisticDetailChangedEvent>,
        IHandle<BasicFilterSelectionChangedEvent>
    {
        #region Properties
        public IEventAggregator Events { get; set; }
        public IMatchManager Manager { get; set; }
        public List<Rally> SelectedRallies { get; private set; }

        #endregion
        public ReceiveStatisticsView()
        {
            InitializeComponent();
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
            Manager = IoC.Get<IMatchManager>();
        }

        public void Handle(StatisticDetailChangedEvent message)
        {
            bool isChecked = message.DetailChecked;
            bool percent = message.Percent;

            if (isChecked == true)
            {
                foreach (ToggleButton btn in this.StatisticsContainer.FindChildren<ToggleButton>())
                {
                    if (btn.Name.Contains("PointsWon") || btn.Name.Contains("DirectPointsWon") || btn.Name.Contains("PointsLost"))
                    {
                        btn.Visibility = Visibility.Visible;
                    }

                }
            }
            if (isChecked == false)
            {
                foreach (ToggleButton btn in this.StatisticsContainer.FindChildren<ToggleButton>())
                {
                    if (btn.Name.Contains("PointsWon") || btn.Name.Contains("DirectPointsWon") || btn.Name.Contains("PointsLost"))
                    {
                        btn.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        public void Handle(BasicFilterSelectionChangedEvent message)
        {
            this.SelectedRallies = message.Rallies.ToList();
            UpdateButtonContent();
        }

        private void UpdateButtonContent()
        {
            UpdateButtonContentBasisInformation();
            UpdateButtonContentPlacement();
            UpdateButtonContentPosition();
            UpdateButtonContentTechnique();
            UpdateButtonContentStepAround();
        }

        private void UpdateButtonContentBasisInformation()
        {
            TotalReceivesCount.Content = SelectedRallies.Where(r => Convert.ToInt32(r.Length) >= 2).Count();
            TotalReceivesCountPointPlayer1.Content = SelectedRallies.Where(r => Convert.ToInt32(r.Length) >= 2 && r.Winner == "First").Count();
            TotalReceivesCountPointPlayer2.Content = SelectedRallies.Where(r => Convert.ToInt32(r.Length) >= 2 && r.Winner == "Second").Count();
        }
        private void UpdateButtonContentPlacement()
        {
            //#region ForehandAll
            //PlacementForehandAllTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsMidLeft() || r.Schlag[1].IsBotLeft())).Count();
            //PlacementForehandAllPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsMidLeft() || r.Schlag[1].IsBotLeft()) && r.Schlag[1].Spieler == r.Winner).Count();
            //PlacementForehandAllDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsMidLeft() || r.Schlag[1].IsBotLeft()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //PlacementForehandAllPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[1].IsTopLeft() || r.Schlag[1].IsMidLeft() || r.Schlag[1].IsBotLeft()) && r.Schlag[1].Spieler != r.Winner).Count();
            //#endregion
            //#region ForehandLong
            //PlacementForehandLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsTopLeft()).Count();
            //PlacementForehandLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsTopLeft() && r.Schlag[1].Spieler == r.Winner).Count();
            //PlacementForehandLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsTopLeft() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //PlacementForehandLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsTopLeft() && r.Schlag[1].Spieler != r.Winner).Count();
            //#endregion
            //#region ForehandHalfLong
            //PlacementForehandHalfLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsMidLeft()).Count();
            //PlacementForehandHalfLongPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].IsMidLeft()) && r.Schlag[1].Spieler == r.Winner).Count();
            //PlacementForehandHalfLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsMidLeft() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //PlacementForehandHalfLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsMidLeft() && r.Schlag[1].Spieler != r.Winner).Count();
            //#endregion
            //#region ForehandShort
            //PlacementForehandShortTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsBotLeft()).Count();
            //PlacementForehandShortPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsBotLeft() && r.Schlag[1].Spieler == r.Winner).Count();
            //PlacementForehandShortDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsBotLeft() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //PlacementForehandShortPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsBotLeft() && r.Schlag[1].Spieler != r.Winner).Count();
            //#endregion

            //#region MiddleAll
            //PlacementMiddleAllTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()).Count();
            //PlacementMiddleAllPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner).Count();
            //PlacementMiddleAllDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //PlacementMiddleAllPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler != r.Winner).Count();
            //#endregion
            //#region MiddleLong
            //PlacementMiddleLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsTopMid()).Count();
            //PlacementMiddleLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsTopMid() && r.Schlag[1].Spieler == r.Winner).Count();
            //PlacementMiddleLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsTopMid() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //PlacementMiddleLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsTopMid() && r.Schlag[1].Spieler != r.Winner).Count();
            //#endregion
            //#region MiddleHalfLong
            //PlacementMiddleHalfLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsMidMid()).Count();
            //PlacementMiddleHalfLongPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].IsMidMid()) && r.Schlag[1].Spieler == r.Winner).Count();
            //PlacementMiddleHalfLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsMidMid() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //PlacementMiddleHalfLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsMidMid() && r.Schlag[1].Spieler != r.Winner).Count();
            //#endregion
            //#region MiddleShort
            //PlacementMiddleShortTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsBotMid()).Count();
            //PlacementMiddleShortPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsBotMid() && r.Schlag[1].Spieler == r.Winner).Count();
            //PlacementMiddleShortDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsBotMid() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //PlacementMiddleShortPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsBotMid() && r.Schlag[1].Spieler != r.Winner).Count();
            //#endregion

            //#region BackhandAll
            //PlacementBackhandAllTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsTopRight() || r.Schlag[1].IsMidRight() || r.Schlag[1].IsBotRight()).Count();
            //PlacementBackhandAllPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].IsTopRight() || r.Schlag[1].IsMidRight() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Spieler == r.Winner).Count();
            //PlacementBackhandAllDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].IsTopRight() || r.Schlag[1].IsMidRight() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //PlacementBackhandAllPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[1].IsTopRight() || r.Schlag[1].IsMidRight() || r.Schlag[1].IsBotRight()) && r.Schlag[1].Spieler != r.Winner).Count();
            //#endregion
            //#region BackhandLong
            //PlacementBackhandLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsTopRight()).Count();
            //PlacementBackhandLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsTopRight() && r.Schlag[1].Spieler == r.Winner).Count();
            //PlacementBackhandLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsTopRight() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //PlacementBackhandLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsTopRight() && r.Schlag[1].Spieler != r.Winner).Count();
            //#endregion
            //#region BackhandHalfLong
            //PlacementBackhandHalfLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsMidRight()).Count();
            //PlacementBackhandHalfLongPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].IsMidRight()) && r.Schlag[1].Spieler == r.Winner).Count();
            //PlacementBackhandHalfLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsMidRight() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //PlacementBackhandHalfLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsMidRight() && r.Schlag[1].Spieler != r.Winner).Count();
            //#endregion
            //#region BackhandShort
            //PlacementBackhandShortTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsBotRight()).Count();
            //PlacementBackhandShortPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsBotRight() && r.Schlag[1].Spieler == r.Winner).Count();
            //PlacementBackhandShortDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsBotRight() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //PlacementBackhandShortPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsBotRight() && r.Schlag[1].Spieler != r.Winner).Count();
            //#endregion

            //#region AllLong
            //PlacementAllLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsLong()).Count();
            //PlacementAllLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsLong() && r.Schlag[1].Spieler == r.Winner).Count();
            //PlacementAllLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsLong() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //PlacementAllLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsLong() && r.Schlag[1].Spieler != r.Winner).Count();
            //#endregion
            //#region AllHalfLong
            //PlacementAllHalfLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsHalfLong()).Count();
            //PlacementAllHalfLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler == r.Winner).Count();
            //PlacementAllHalfLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //PlacementAllHalfLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler != r.Winner).Count();
            //#endregion
            //#region AllShort
            //PlacementAllShortTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsShort()).Count();
            //PlacementAllShortPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsShort() && r.Schlag[1].Spieler == r.Winner).Count();
            //PlacementAllShortDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsShort() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //PlacementAllShortPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].IsShort() && r.Schlag[1].Spieler != r.Winner).Count();
            //#endregion

            //#region ServiceErrors
            //PlacementAllServiceErrorsTotalButton.Content = SelectedRallies.Where(r => (r.Server != r.Winner && r.Length == 2)).Count();
            //#endregion

        }
        private void UpdateButtonContentPosition()
        {
            #region over the table
            //OverTheTableTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Balltreffpunkt=="über").Count();
            //    OverTheTablePointsWonButton.Content= SelectedRallies.Where(r => r.Schlag[1].Balltreffpunkt == "über" && r.Schlag[1].Spieler == r.Winner).Count();
            //OverTheTableDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Balltreffpunkt == "über" && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //OverTheTablePointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Balltreffpunkt == "über" && r.Schlag[1].Spieler != r.Winner).Count();
            #endregion

            #region at the table
            //AtTheTableTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Balltreffpunkt == "hinter").Count();
            //AtTheTablePointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Balltreffpunkt == "hinter" && r.Schlag[1].Spieler == r.Winner).Count();
            //AtTheTableDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Balltreffpunkt == "hinter" && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //AtTheTablePointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Balltreffpunkt == "hinter" && r.Schlag[1].Spieler != r.Winner).Count();
            #endregion

            #region half distance
            //HalfDistanceTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Balltreffpunkt == "Halbdistanz").Count();
            //HalfDistancePointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Balltreffpunkt == "Halbdistanz" && r.Schlag[1].Spieler == r.Winner).Count();
            //HalfDistanceDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Balltreffpunkt == "Halbdistanz" && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //HalfDistancePointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Balltreffpunkt == "Halbdistanz" && r.Schlag[1].Spieler != r.Winner).Count();

            #endregion


        }
        private void UpdateButtonContentTechnique()
        {
            // #region Flip
            // ForehandFlipTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Flip").Count();
            // ForehandFlipPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler == r.Winner).Count();
            // ForehandFlipDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // ForehandFlipPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler != r.Winner).Count();

            // BackhandFlipTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Flip").Count();
            // BackhandFlipPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler == r.Winner).Count();
            // BackhandFlipDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // BackhandFlipPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler != r.Winner).Count();

            // AllFlipTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Flip").Count();
            // AllFlipPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler == r.Winner).Count();
            // AllFlipDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // AllFlipPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Flip" && r.Schlag[1].Spieler != r.Winner).Count();

            // #endregion

            // #region Push short

            // ForehandPushShortTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsShort()).Count();
            //ForehandPushShortPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsShort() && r.Schlag[1].Spieler == r.Winner).Count();
            // ForehandPushShortDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsShort() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // ForehandPushShortPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsShort() && r.Schlag[1].Spieler != r.Winner).Count();

            // BackhandPushShortTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsShort()).Count();
            // BackhandPushShortPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsShort() && r.Schlag[1].Spieler == r.Winner).Count();
            // BackhandPushShortDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsShort() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // BackhandPushShortPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsShort() && r.Schlag[1].Spieler != r.Winner).Count();

            // AllPushShortTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsShort()).Count();
            // AllPushShortPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsShort() && r.Schlag[1].Spieler == r.Winner).Count();
            // AllPushShortDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsShort() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // AllPushShortPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsShort() && r.Schlag[1].Spieler != r.Winner).Count();

            // #endregion

            // #region Push halflong

            // ForehandPushHalfLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsHalfLong()).Count();
            // ForehandPushHalfLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler == r.Winner).Count();
            // ForehandPushHalfLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // ForehandPushHalfLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler != r.Winner).Count();

            // BackhandPushHalfLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsHalfLong()).Count();
            // BackhandPushHalfLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler == r.Winner).Count();
            // BackhandPushHalfLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // BackhandPushHalfLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler != r.Winner).Count();

            // AllPushHalfLongTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsHalfLong()).Count();
            // AllPushHalfLongPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler == r.Winner).Count();
            // AllPushHalfLongDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // AllPushHalfLongPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler != r.Winner).Count();

            // #endregion

            // #region Push long

            // ForehandPushLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsLong()).Count();
            // ForehandPushLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsLong() && r.Schlag[1].Spieler == r.Winner).Count();
            // ForehandPushLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsLong() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // ForehandPushLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsLong() && r.Schlag[1].Spieler != r.Winner).Count();

            // BackhandPushLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsLong()).Count();
            // BackhandPushLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsLong() && r.Schlag[1].Spieler == r.Winner).Count();
            // BackhandPushLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsLong() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // BackhandPushLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsLong() && r.Schlag[1].Spieler != r.Winner).Count();

            // AllPushLongTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsLong()).Count();
            // AllPushLongPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsLong() && r.Schlag[1].Spieler == r.Winner).Count();
            // AllPushLongDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsLong() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // AllPushLongPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Schupf" && r.Schlag[1].IsLong() && r.Schlag[1].Spieler != r.Winner).Count();

            // #endregion

            // #region Forehand Topspin diagonal

            // //was ist Diagonal ??

            // ForehandTopspinDiagonalTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && Math.Abs(Convert.ToInt32(r.Schlag[0].Platzierung.WX) - Convert.ToInt32(r.Schlag[1].Platzierung.WX))>100).Count();
            // ForehandTopspinDiagonalPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && Math.Abs(Convert.ToInt32(r.Schlag[0].Platzierung.WX) - Convert.ToInt32(r.Schlag[1].Platzierung.WX)) > 100 && r.Schlag[1].Spieler == r.Winner).Count();
            // ForehandTopspinDiagonalDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && Math.Abs(Convert.ToInt32(r.Schlag[0].Platzierung.WX) - Convert.ToInt32(r.Schlag[1].Platzierung.WX)) > 100 && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // ForehandTopspinDiagonalPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && Math.Abs(Convert.ToInt32(r.Schlag[0].Platzierung.WX) - Convert.ToInt32(r.Schlag[1].Platzierung.WX)) > 100 && r.Schlag[1].Spieler != r.Winner).Count();

            // BackhandTopspinDiagonalTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && Math.Abs(Convert.ToInt32(r.Schlag[0].Platzierung.WX) - Convert.ToInt32(r.Schlag[1].Platzierung.WX)) > 100).Count();
            // BackhandTopspinDiagonalPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && Math.Abs(Convert.ToInt32(r.Schlag[0].Platzierung.WX) - Convert.ToInt32(r.Schlag[1].Platzierung.WX)) > 100 && r.Schlag[1].Spieler == r.Winner).Count();
            // BackhandTopspinDiagonalDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && Math.Abs(Convert.ToInt32(r.Schlag[0].Platzierung.WX) - Convert.ToInt32(r.Schlag[1].Platzierung.WX)) > 100 && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // BackhandTopspinDiagonalPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && Math.Abs(Convert.ToInt32(r.Schlag[0].Platzierung.WX) - Convert.ToInt32(r.Schlag[1].Platzierung.WX)) > 100 && r.Schlag[1].Spieler != r.Winner).Count();

            // AllTopspinDiagonalTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Topspin" && Math.Abs(Convert.ToInt32(r.Schlag[0].Platzierung.WX) - Convert.ToInt32(r.Schlag[1].Platzierung.WX)) > 100).Count();
            // AllTopspinDiagonalPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Topspin" && Math.Abs(Convert.ToInt32(r.Schlag[0].Platzierung.WX) - Convert.ToInt32(r.Schlag[1].Platzierung.WX)) > 100 && r.Schlag[1].Spieler == r.Winner).Count();
            // AllTopspinDiagonalDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Topspin" && Math.Abs(Convert.ToInt32(r.Schlag[0].Platzierung.WX) - Convert.ToInt32(r.Schlag[1].Platzierung.WX)) > 100 && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // AllTopspinDiagonalPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Topspin" && Math.Abs(Convert.ToInt32(r.Schlag[0].Platzierung.WX) - Convert.ToInt32(r.Schlag[1].Platzierung.WX)) > 100 && r.Schlag[1].Spieler != r.Winner).Count();

            // #endregion

            // #region Forehand Topspin Middle

            // //was ist Mitte ??

            // ForehandTopspinMiddleTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid())).Count();
            // ForehandTopspinMiddlePointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner).Count();
            // ForehandTopspinMiddleDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // ForehandTopspinMiddlePointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Vorhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler != r.Winner).Count();

            // BackhandTopspinMiddleTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid())).Count();
            // BackhandTopspinMiddlePointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner).Count();
            // BackhandTopspinMiddleDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // BackhandTopspinMiddlePointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == "Rückhand" && r.Schlag[1].Schlagtechnik.Art == "Topspin" && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler != r.Winner).Count();

            // AllTopspinMiddleTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Topspin" && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid())).Count();
            // AllTopspinMiddlePointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Topspin" && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner).Count();
            // AllTopspinMiddleDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Topspin" && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            // AllTopspinMiddlePointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == "Vorhand" || r.Schlag[1].Schlägerseite == "Rückhand") && r.Schlag[1].Schlagtechnik.Art == "Topspin" && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler != r.Winner).Count();

            // #endregion

            #region Flip
            //ForehandFlipTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Flip').Count();
            //ForehandFlipPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Flip' && r.Schlag[1].Spieler == r.Winner).Count();
            //ForehandFlipDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Flip' && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //ForehandFlipPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Flip' && r.Schlag[1].Spieler != r.Winner).Count();

            //BackhandFlipTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Flip').Count();
            //BackhandFlipPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Flip' && r.Schlag[1].Spieler == r.Winner).Count();
            //BackhandFlipDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Flip' && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //BackhandFlipPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Flip' && r.Schlag[1].Spieler != r.Winner).Count();

            //AllFlipTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Flip').Count();
            //AllFlipPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Flip' && r.Schlag[1].Spieler == r.Winner).Count();
            //AllFlipDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Flip' && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //AllFlipPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Flip' && r.Schlag[1].Spieler != r.Winner).Count();

            #endregion

            #region Push short

            //ForehandPushShortTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsShort()).Count();
            //ForehandPushShortPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsShort() && r.Schlag[1].Spieler == r.Winner).Count();
            //ForehandPushShortDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsShort() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //ForehandPushShortPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsShort() && r.Schlag[1].Spieler != r.Winner).Count();

            //BackhandPushShortTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsShort()).Count();
            //BackhandPushShortPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsShort() && r.Schlag[1].Spieler == r.Winner).Count();
            //BackhandPushShortDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsShort() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //BackhandPushShortPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsShort() && r.Schlag[1].Spieler != r.Winner).Count();

            //AllPushShortTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsShort()).Count();
            //AllPushShortPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsShort() && r.Schlag[1].Spieler == r.Winner).Count();
            //AllPushShortDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsShort() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //AllPushShortPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsShort() && r.Schlag[1].Spieler != r.Winner).Count();

            #endregion

            #region Push halflong

            //ForehandPushHalfLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsHalfLong()).Count();
            //ForehandPushHalfLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler == r.Winner).Count();
            //ForehandPushHalfLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //ForehandPushHalfLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler != r.Winner).Count();

            //BackhandPushHalfLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsHalfLong()).Count();
            //BackhandPushHalfLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler == r.Winner).Count();
            //BackhandPushHalfLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //BackhandPushHalfLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler != r.Winner).Count();

            //AllPushHalfLongTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsHalfLong()).Count();
            //AllPushHalfLongPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler == r.Winner).Count();
            //AllPushHalfLongDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //AllPushHalfLongPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsHalfLong() && r.Schlag[1].Spieler != r.Winner).Count();

            #endregion

            #region Push long

            //ForehandPushLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsLong()).Count();
            //ForehandPushLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsLong() && r.Schlag[1].Spieler == r.Winner).Count();
            //ForehandPushLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsLong() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //ForehandPushLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsLong() && r.Schlag[1].Spieler != r.Winner).Count();

            //BackhandPushLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsLong()).Count();
            //BackhandPushLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsLong() && r.Schlag[1].Spieler == r.Winner).Count();
            //BackhandPushLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsLong() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //BackhandPushLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsLong() && r.Schlag[1].Spieler != r.Winner).Count();

            //AllPushLongTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsLong()).Count();
            //AllPushLongPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsLong() && r.Schlag[1].Spieler == r.Winner).Count();
            //AllPushLongDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsLong() && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //AllPushLongPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Schupf' && r.Schlag[1].IsLong() && r.Schlag[1].Spieler != r.Winner).Count();

            #endregion

            #region Forehand Topspin diagonal

            //was ist Diagonal ??

            //ForehandTopspinDiagonalTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && r.IsDiagonal(1)).Count();
            //ForehandTopspinDiagonalPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && r.IsDiagonal(1) && r.Schlag[1].Spieler == r.Winner).Count();
            //ForehandTopspinDiagonalDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && r.IsDiagonal(1) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //ForehandTopspinDiagonalPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && r.IsDiagonal(1) && r.Schlag[1].Spieler != r.Winner).Count();

            //BackhandTopspinDiagonalTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && r.IsDiagonal(1)).Count();
            //BackhandTopspinDiagonalPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && r.IsDiagonal(1) && r.Schlag[1].Spieler == r.Winner).Count();
            //BackhandTopspinDiagonalDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && r.IsDiagonal(1) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //BackhandTopspinDiagonalPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && r.IsDiagonal(1) && r.Schlag[1].Spieler != r.Winner).Count();

            //AllTopspinDiagonalTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && r.IsDiagonal(1)).Count();
            //AllTopspinDiagonalPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && r.IsDiagonal(1) && r.Schlag[1].Spieler == r.Winner).Count();
            //AllTopspinDiagonalDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && r.IsDiagonal(1) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //AllTopspinDiagonalPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && r.IsDiagonal(1) && r.Schlag[1].Spieler != r.Winner).Count();

            #endregion

            #region Forehand Topspin Middle

            //was ist Mitte ??

            //ForehandTopspinMiddleTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid())).Count();
            //ForehandTopspinMiddlePointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner).Count();
            //ForehandTopspinMiddleDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //ForehandTopspinMiddlePointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Vorhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler != r.Winner).Count();

            //BackhandTopspinMiddleTotalButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid())).Count();
            //BackhandTopspinMiddlePointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner).Count();
            //BackhandTopspinMiddleDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //BackhandTopspinMiddlePointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[1].Schlägerseite == 'Rückhand' && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler != r.Winner).Count();

            //AllTopspinMiddleTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid())).Count();
            //AllTopspinMiddlePointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner).Count();
            //AllTopspinMiddleDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler == r.Winner && Convert.ToInt32(r.Length) < 4).Count();
            //AllTopspinMiddlePointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[1].Schlägerseite == 'Vorhand' || r.Schlag[1].Schlägerseite == 'Rückhand') && r.Schlag[1].Schlagtechnik.Art == 'Topspin' && (r.Schlag[1].IsTopMid() || r.Schlag[1].IsMidMid() || r.Schlag[1].IsBotMid()) && r.Schlag[1].Spieler != r.Winner).Count();

            #endregion






        }
        private void UpdateButtonContentStepAround()
        {
            

        }
    }
}
