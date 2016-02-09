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
    /// Interaktionslogik für ServiceStatisticsView.xaml
    /// </summary>
    public partial class ServiceStatisticsView : UserControl,
        IHandle<StatisticDetailChangedEvent>,
        IHandle<BasicFilterSelectionChangedEvent>
    {
        #region Properties
        public IEventAggregator Events { get; set; }
        public IMatchManager Manager { get; set; }
        public List<Rally> SelectedRallies { get; private set; }

        #endregion

        public ServiceStatisticsView()
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
            UpdateButtonContentSpin();
        }

        
        private void UpdateButtonContentBasisInformation()
        {
            //TotalServicesCount.Content = SelectedRallies.Where(r => Convert.ToInt32(r.Length) >=1 ).Count();
            TotalServicesCountPointPlayer1.Content = SelectedRallies.Where(r => Convert.ToInt32(r.Length) >= 1 && r.Winner=="First").Count();
            TotalServicesCountPointPlayer2.Content = SelectedRallies.Where(r => Convert.ToInt32(r.Length) >= 1 && r.Winner == "Second").Count();
        }
        private void UpdateButtonContentPlacement()
        {
            #region ForhandAll
            //PlacementForhandAllTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsTopLeft() || r.Schlag[0].IsMidLeft() || r.Schlag[0].IsBotLeft()).Count();
            PlacementForhandAllPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsTopLeft() || r.Schlag[0].IsMidLeft() || r.Schlag[0].IsBotLeft()) && r.Schlag[0].Spieler == r.Winner).Count();
            PlacementForhandAllDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsTopLeft() || r.Schlag[0].IsMidLeft() || r.Schlag[0].IsBotLeft()) && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PlacementForhandAllPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsTopLeft() || r.Schlag[0].IsMidLeft() || r.Schlag[0].IsBotLeft()) && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion
            #region ForhandLong
            PlacementForhandLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsTopLeft()).Count();
            PlacementForhandLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsTopLeft() && r.Schlag[0].Spieler == r.Winner).Count();
            PlacementForhandLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsTopLeft() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PlacementForhandLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsTopLeft() && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion
            #region ForhandHalfLong
            PlacementForhandHalfLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsMidLeft()).Count();
            PlacementForhandHalfLongPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsMidLeft()) && r.Schlag[0].Spieler == r.Winner).Count();
            PlacementForhandHalfLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsMidLeft() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PlacementForhandHalfLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsMidLeft() && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion
            #region ForhandShort
            PlacementForhandShortTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsBotLeft()).Count();
            PlacementForhandShortPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsBotLeft() && r.Schlag[0].Spieler == r.Winner).Count();
            PlacementForhandShortDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsBotLeft() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PlacementForhandShortPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsBotLeft() && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion

            #region MiddleAll
            PlacementMiddleAllTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsTopMid() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsBotMid()).Count();
            PlacementMiddleAllPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsTopMid() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsBotMid()) && r.Schlag[0].Spieler == r.Winner).Count();
            PlacementMiddleAllDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsTopMid() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsBotMid()) && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PlacementMiddleAllPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsTopMid() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsBotMid()) && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion
            #region MiddleLong
            PlacementMiddleLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsTopMid()).Count();
            PlacementMiddleLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsTopMid() && r.Schlag[0].Spieler == r.Winner).Count();
            PlacementMiddleLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsTopMid() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PlacementMiddleLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsTopMid() && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion
            #region MiddleHalfLong
            PlacementMiddleHalfLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsMidMid()).Count();
            PlacementMiddleHalfLongPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsMidMid()) && r.Schlag[0].Spieler == r.Winner).Count();
            PlacementMiddleHalfLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsMidMid() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PlacementMiddleHalfLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsMidMid() && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion
            #region MiddleShort
            PlacementMiddleShortTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsBotMid()).Count();
            PlacementMiddleShortPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsBotMid() && r.Schlag[0].Spieler == r.Winner).Count();
            PlacementMiddleShortDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsBotMid() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PlacementMiddleShortPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsBotMid() && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion

            #region BackhandAll
            PlacementBackhandAllTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsTopRight() || r.Schlag[0].IsMidRight() || r.Schlag[0].IsBotRight()).Count();
            PlacementBackhandAllPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsTopRight() || r.Schlag[0].IsMidRight() || r.Schlag[0].IsBotRight()) && r.Schlag[0].Spieler == r.Winner).Count();
            PlacementBackhandAllDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsTopRight() || r.Schlag[0].IsMidRight() || r.Schlag[0].IsBotRight()) && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PlacementBackhandAllPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsTopRight() || r.Schlag[0].IsMidRight() || r.Schlag[0].IsBotRight()) && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion
            #region BackhandLong
            PlacementBackhandLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsTopRight()).Count();
            PlacementBackhandLongPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsTopRight() && r.Schlag[0].Spieler == r.Winner).Count();
            PlacementBackhandLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsTopRight() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PlacementBackhandLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsTopRight() && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion
            #region BackhandHalfLong
            PlacementBackhandHalfLongTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsMidRight()).Count();
            PlacementBackhandHalfLongPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsMidRight()) && r.Schlag[0].Spieler == r.Winner).Count();
            PlacementBackhandHalfLongDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsMidRight() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PlacementBackhandHalfLongPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsMidRight() && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion
            #region BackhandShort
            PlacementBackhandShortTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsBotRight()).Count();
            PlacementBackhandShortPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsBotRight() && r.Schlag[0].Spieler == r.Winner).Count();
            PlacementBackhandShortDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsBotRight() && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PlacementBackhandShortPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].IsBotRight() && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion

            #region AllLong
            PlacementAllLongTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsTopLeft() || r.Schlag[0].IsTopMid() || r.Schlag[0].IsTopRight())).Count();
            PlacementAllLongPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsTopLeft() || r.Schlag[0].IsTopMid() || r.Schlag[0].IsTopRight()) && r.Schlag[0].Spieler == r.Winner).Count();
            PlacementAllLongDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsTopLeft() || r.Schlag[0].IsTopMid() || r.Schlag[0].IsTopRight()) && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PlacementAllLongPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsTopLeft() || r.Schlag[0].IsTopMid() || r.Schlag[0].IsTopRight()) && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion
            #region AllHalfLong
            PlacementAllHalfLongTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsMidLeft() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsMidRight())).Count();
            PlacementAllHalfLongPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsMidLeft() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsMidRight()) && r.Schlag[0].Spieler == r.Winner).Count();
            PlacementAllHalfLongDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsMidLeft() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsMidRight()) && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PlacementAllHalfLongPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsMidLeft() || r.Schlag[0].IsMidMid() || r.Schlag[0].IsMidRight()) && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion
            #region AllShort
            PlacementAllShortTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsBotLeft() || r.Schlag[0].IsBotMid() || r.Schlag[0].IsBotRight())).Count();
            PlacementAllShortPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsBotLeft() || r.Schlag[0].IsBotMid() || r.Schlag[0].IsBotRight()) && r.Schlag[0].Spieler == r.Winner).Count();
            PlacementAllShortDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsBotLeft() || r.Schlag[0].IsBotMid() || r.Schlag[0].IsBotRight()) && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PlacementAllShortPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].IsBotLeft() || r.Schlag[0].IsBotMid() || r.Schlag[0].IsBotRight()) && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion

            #region ServiceErrors
            PlacementAllServiceErrorsTotalButton.Content = SelectedRallies.Where(r => (r.Server != r.Winner && r.Length == 1)).Count();
            #endregion

        }
        private void UpdateButtonContentPosition()
        {


            #region Position Left
            PositionLeftTotalButton.Content = SelectedRallies.Where(r => (0 <= AufschlagPosition(r) && AufschlagPosition(r) < 50.5)).Count();
            PositionLeftPointsWonButton.Content = SelectedRallies.Where(r => (0 <= AufschlagPosition(r) && AufschlagPosition(r) < 50.5) && r.Schlag[0].Spieler == r.Winner).Count();
            PositionLeftDirectPointsWonButton.Content = SelectedRallies.Where(r => (0 <= AufschlagPosition(r) && AufschlagPosition(r) < 50.5) && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PositionLeftPointsLostButton.Content = SelectedRallies.Where(r => (0 <= AufschlagPosition(r) && AufschlagPosition(r) < 50.5) && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion
            #region Position Middle
            PositionMiddleTotalButton.Content = SelectedRallies.Where(r => (50.5 <= AufschlagPosition(r) && AufschlagPosition(r) <= 102)).Count();
            PositionMiddlePointsWonButton.Content = SelectedRallies.Where(r => (50.5 <= AufschlagPosition(r) && AufschlagPosition(r) <= 102) && r.Schlag[0].Spieler == r.Winner).Count();
            PositionMiddleDirectPointsWonButton.Content = SelectedRallies.Where(r => (50.5 <= AufschlagPosition(r) && AufschlagPosition(r) <= 102) && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PositionMiddlePointsLostButton.Content = SelectedRallies.Where(r => (50.5 <= AufschlagPosition(r) && AufschlagPosition(r) <= 102) && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion
            #region Position Right
            PositionRightTotalButton.Content = SelectedRallies.Where(r => (102 < AufschlagPosition(r) && AufschlagPosition(r) <= 152.5)).Count();
            PositionRightPointsWonButton.Content = SelectedRallies.Where(r => (102 < AufschlagPosition(r) && AufschlagPosition(r) <= 152.5) && r.Schlag[0].Spieler == r.Winner).Count();
            PositionRightDirectPointsWonButton.Content = SelectedRallies.Where(r => (102 < AufschlagPosition(r) && AufschlagPosition(r) <= 152.5) && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            PositionRightPointsLostButton.Content = SelectedRallies.Where(r => (102 < AufschlagPosition(r) && AufschlagPosition(r) <= 152.5) && r.Schlag[0].Spieler != r.Winner).Count();
            #endregion


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

        private void UpdateButtonContentTechnique()
        {
            #region Pendulum

            ForhandPendulumTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite=="Vorhand" && r.Schlag[0].Aufschlagart=="Pendulum").Count();
            ForhandPendulumPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler == r.Winner).Count();
            ForhandPendulumDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            ForhandPendulumPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler != r.Winner).Count();

            BackhandPendulumTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Pendulum").Count();
            BackhandPendulumPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler == r.Winner).Count();
            BackhandPendulumDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            BackhandPendulumPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler != r.Winner).Count();

            AllPendulumTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Pendulum").Count();
            AllPendulumPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler == r.Winner).Count();
            AllPendulumDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            AllPendulumPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Pendulum" && r.Schlag[0].Spieler != r.Winner).Count();

            #endregion

            #region ReversePendulum

            ForhandReversePendulumTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Gegenläufer").Count();
            ForhandReversePendulumPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler == r.Winner).Count();
            ForhandReversePendulumDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            ForhandReversePendulumPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler != r.Winner).Count();

            BackhandReversePendulumTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Gegenläufer").Count();
            BackhandReversePendulumPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler == r.Winner).Count();
            BackhandReversePendulumDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            BackhandReversePendulumPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler != r.Winner).Count();

            AllReversePendulumTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Gegenläufer").Count();
            AllReversePendulumPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler == r.Winner).Count();
            AllReversePendulumDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            AllReversePendulumPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Gegenläufer" && r.Schlag[0].Spieler != r.Winner).Count();

            #endregion

            #region Tomahawk

            ForhandTomahawkTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Tomahawk").Count();
            ForhandTomahawkPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler == r.Winner).Count();
            ForhandTomahawkDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            ForhandTomahawkPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler != r.Winner).Count();

            BackhandTomahawkTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Tomahawk").Count();
            BackhandTomahawkPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler == r.Winner).Count();
            BackhandTomahawkDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            BackhandTomahawkPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler != r.Winner).Count();

            AllTomahawkTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Tomahawk").Count();
            AllTomahawkPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler == r.Winner).Count();
            AllTomahawkDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            AllTomahawkPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Tomahawk" && r.Schlag[0].Spieler != r.Winner).Count();

            #endregion
            #region Special

            ForhandSpecialTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Spezial").Count();
            ForhandSpecialPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler == r.Winner).Count();
            ForhandSpecialDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            ForhandSpecialPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler != r.Winner).Count();

            BackhandSpecialTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Spezial").Count();
            BackhandSpecialPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler == r.Winner).Count();
            BackhandSpecialDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            BackhandSpecialPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler != r.Winner).Count();

            AllSpecialTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Spezial").Count();
            AllSpecialPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler == r.Winner).Count();
            AllSpecialDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            AllSpecialPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Vorhand" || r.Schlag[0].Schlägerseite == "Rückhand") && r.Schlag[0].Aufschlagart == "Spezial" && r.Schlag[0].Spieler != r.Winner).Count();

            #endregion

            #region All Forhand Services

            ForhandAllTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial")).Count();
            ForhandAllPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler == r.Winner).Count();
            ForhandAllDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            ForhandAllPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Vorhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler != r.Winner).Count();

            #endregion

            #region All Backhand Services

            BackhandAllTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial")).Count();
            BackhandAllPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler == r.Winner).Count();
            BackhandAllDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            BackhandAllPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].Schlägerseite == "Rückhand" && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler != r.Winner).Count();

            #endregion

            #region All Services

            AllServicesTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Rückhand" || r.Schlag[0].Schlägerseite == "Vorhand") && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial")).Count();
            AllServicesPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Rückhand" || r.Schlag[0].Schlägerseite == "Vorhand") && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler == r.Winner).Count();
            AllServicesDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Rückhand" || r.Schlag[0].Schlägerseite == "Vorhand") && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            AllServicesPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Schlägerseite == "Rückhand" || r.Schlag[0].Schlägerseite == "Vorhand") && (r.Schlag[0].Aufschlagart == "Pendulum" || r.Schlag[0].Aufschlagart == "Gegenläufer" || r.Schlag[0].Aufschlagart == "Tomahawk" || r.Schlag[0].Aufschlagart == "Spezial") && r.Schlag[0].Spieler != r.Winner).Count();

            #endregion


        }
        private void UpdateButtonContentSpin()
        {
            #region UpSpin

            UpSideLeftTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL=="1" && r.Schlag[0].Spin.ÜS == "1")).Count();
            UpSideLeftPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler == r.Winner).Count();
            UpSideLeftDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            UpSideLeftPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler != r.Winner).Count();

            UpTotalButton.Content = SelectedRallies.Where(r => ( r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "1")).Count();
            UpPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler == r.Winner).Count();
            UpDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            UpPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler != r.Winner).Count();

            UpSideRightTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "1")).Count();
            UpSideRightPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler == r.Winner).Count();
            UpSideRightDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            UpSideRightPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "1") && r.Schlag[0].Spieler != r.Winner).Count();

            UpAllTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].Spin.ÜS == "1").Count();
            UpAllPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Spin.ÜS == "1" && r.Schlag[0].Spieler == r.Winner).Count();
            UpAllDirectPointsWonButton.Content = SelectedRallies.Where(r =>  r.Schlag[0].Spin.ÜS == "1" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            UpAllPointsLostButton.Content = SelectedRallies.Where(r =>  r.Schlag[0].Spin.ÜS == "1" && r.Schlag[0].Spieler != r.Winner).Count();

            #endregion

            #region No UpDown Spin

            SideLeftTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0")).Count();
            SideLeftPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner).Count();
            SideLeftDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            SideLeftPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler != r.Winner).Count();

            NoSpinTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.No == "1" && r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0")).Count();
            NoSpinPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.No == "1" && r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner).Count();
            NoSpinDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.No == "1" && r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            NoSpinPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.No == "1" && r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler != r.Winner).Count();

            SideRightTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0")).Count();
            SideRightPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner).Count();
            SideRightDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            SideRightPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler != r.Winner).Count();

            NoUpDownAllTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0")).Count();
            NoUpDownAllPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner).Count();
            NoUpDownAllDirectPointsWonButton.Content = SelectedRallies.Where(r =>( r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            NoUpDownAllPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.ÜS == "0" && r.Schlag[0].Spin.US == "0") && r.Schlag[0].Spieler != r.Winner).Count();

            #endregion

            #region DownSpin

            DownSideLeftTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.US == "1")).Count();
            DownSideLeftPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler == r.Winner).Count();
            DownSideLeftDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            DownSideLeftPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "1" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler != r.Winner).Count();

            DownTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.US == "1")).Count();
            DownPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler == r.Winner).Count();
            DownDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            DownPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler != r.Winner).Count();

            DownSideRightTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.US == "1")).Count();
            DownSideRightPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler == r.Winner).Count();
            DownSideRightDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            DownSideRightPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" && r.Schlag[0].Spin.US == "1") && r.Schlag[0].Spieler != r.Winner).Count();

            DownAllTotalButton.Content = SelectedRallies.Where(r => r.Schlag[0].Spin.US == "1").Count();
            DownAllPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Spin.US == "1" && r.Schlag[0].Spieler == r.Winner).Count();
            DownAllDirectPointsWonButton.Content = SelectedRallies.Where(r => r.Schlag[0].Spin.US == "1" && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            DownAllPointsLostButton.Content = SelectedRallies.Where(r => r.Schlag[0].Spin.US == "1" && r.Schlag[0].Spieler != r.Winner).Count();

            #endregion

            #region SideLeft All

            SideLeftAllTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "1")).Count();
            SideLeftAllPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "1") && r.Schlag[0].Spieler == r.Winner).Count();
            SideLeftAllDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            SideLeftAllPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "1") && r.Schlag[0].Spieler != r.Winner).Count();

            #endregion

            #region No SideSpin All

            NoSideAllTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0")).Count();
            NoSideAllPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0") && r.Schlag[0].Spieler == r.Winner).Count();
            NoSideAllDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            NoSideAllPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SL == "0" && r.Schlag[0].Spin.SR == "0") && r.Schlag[0].Spieler != r.Winner).Count();


            #endregion

            #region SideRight All

            SideRightAllTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1")).Count();
            SideRightAllPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1") && r.Schlag[0].Spieler == r.Winner).Count();
            SideRightAllDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            SideRightAllPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1") && r.Schlag[0].Spieler != r.Winner).Count();


            #endregion

            #region All Spins Total

            AllSpinTotalButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" || r.Schlag[0].Spin.SL == "1" || r.Schlag[0].Spin.US == "1" || r.Schlag[0].Spin.ÜS == "1" || r.Schlag[0].Spin.No == "1")).Count();
            AllSpinPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" || r.Schlag[0].Spin.SL == "1" || r.Schlag[0].Spin.US == "1" || r.Schlag[0].Spin.ÜS == "1" || r.Schlag[0].Spin.No == "1") && r.Schlag[0].Spieler == r.Winner).Count();
            AllSpinDirectPointsWonButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" || r.Schlag[0].Spin.SL == "1" || r.Schlag[0].Spin.US == "1" || r.Schlag[0].Spin.ÜS == "1" || r.Schlag[0].Spin.No == "1") && r.Schlag[0].Spieler == r.Winner && Convert.ToInt32(r.Length) < 3).Count();
            AllSpinPointsLostButton.Content = SelectedRallies.Where(r => (r.Schlag[0].Spin.SR == "1" || r.Schlag[0].Spin.SL == "1" || r.Schlag[0].Spin.US == "1" || r.Schlag[0].Spin.ÜS == "1" || r.Schlag[0].Spin.No == "1") && r.Schlag[0].Spieler != r.Winner).Count();


            #endregion

        }

        
    }
}
