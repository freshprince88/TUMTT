using System;
using Caliburn.Micro;
using TT.Lib.Managers;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;

namespace TT.Scouter.ViewModels
{
    public class StrokePositionTableViewModel : Screen, INotifyPropertyChanged
    {
        public Models.Stroke Stroke { get; set; }

        #region PointOfContact Properties
        private bool _over;
        public bool over
        {
            get { return _over; }
            set
            {
                _over = value;
                NotifyOfPropertyChange("over");
            }
        }

        private bool _half;
        public bool half
        {
            get { return _half; }
            set
            {
                _half = value;
                NotifyOfPropertyChange("half");
            }
        }

        private bool _behind;
        public bool behind
        {
            get { return _behind; }
            set
            {
                _behind = value;
                NotifyOfPropertyChange("behind");
            }
        }
        #endregion

        #region Placement Properties

        private double canvasWidth = 338;
        private double canvasHeight = 594;

        private Visibility _placementVisibilty;
        public Visibility placementVisibilty
        {
            get { return _placementVisibilty; }
            set
            {
                _placementVisibilty = value;
                NotifyOfPropertyChange("placementVisibilty");
            }
        }

        private double _widthHeight;
        public double widthHeight
        {
            get { return _widthHeight; }
            set
            {
                _widthHeight = value;
                NotifyOfPropertyChange("widthHeight");
            }
        }

        private Thickness _currentPlacementPosition;
        public Thickness currentPlacementPosition
        {
            get { return _currentPlacementPosition; }
            set
            {
                _currentPlacementPosition = value;
                NotifyOfPropertyChange("currentPlacementPosition");
            }
        }

        private bool _placeTopLeft_top;
        public bool placeTopLeft_top
        {
            get { return _placeTopLeft_top; }
            set
            {
                _placeTopLeft_top = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeTopMid_top;
        public bool placeTopMid_top
        {
            get { return _placeTopMid_top; }
            set
            {
                _placeTopMid_top = value;
                NotifyOfPropertyChange("placeTopMid_top");
            }
        }

        private bool _placeTopRight_top;
        public bool placeTopRight_top
        {
            get { return _placeTopRight_top; }
            set
            {
                _placeTopRight_top = value;
                NotifyOfPropertyChange("placeTopRight_top");
            }
        }

        private bool _placeMidLeft_top;
        public bool placeMidLeft_top
        {
            get { return _placeMidLeft_top; }
            set
            {
                _placeMidLeft_top = value;
                NotifyOfPropertyChange("placeMidLeft_top");
            }
        }

        private bool _placeMidMid_top;
        public bool placeMidMid_top
        {
            get { return _placeMidMid_top; }
            set
            {
                _placeMidMid_top = value;
                NotifyOfPropertyChange("placeMidMid_top");
            }
        }

        private bool _placeMidRight_top;
        public bool placeMidRight_top
        {
            get { return _placeMidRight_top; }
            set
            {
                _placeMidRight_top = value;
                NotifyOfPropertyChange("placeMidRight_top");
            }
        }

        private bool _placeBotLeft_top;
        public bool placeBotLeft_top
        {
            get { return _placeBotLeft_top; }
            set
            {
                _placeBotLeft_top = value;
                NotifyOfPropertyChange("placeBotLeft_top");
            }
        }

        private bool _placeBotMid_top;
        public bool placeBotMid_top
        {
            get { return _placeBotMid_top; }
            set
            {
                _placeBotMid_top = value;
                NotifyOfPropertyChange("placeBotMid_top");
            }
        }

        private bool _placeBotRight_top;
        public bool placeBotRight_top
        {
            get { return _placeBotRight_top; }
            set
            {
                _placeBotRight_top = value;
                NotifyOfPropertyChange("placeBotRight_top");
            }
        }

        private bool _placeBotRight_bot;
        public bool placeBotRight_bot
        {
            get { return _placeBotRight_bot; }
            set
            {
                _placeBotRight_bot = value;
                NotifyOfPropertyChange("placeBotRight_bot");
            }
        }

        private bool _placeBotMid_bot;
        public bool placeBotMid_bot
        {
            get { return _placeBotMid_bot; }
            set
            {
                _placeBotMid_bot = value;
                NotifyOfPropertyChange("placeBotMid_bot");
            }
        }

        private bool _placeBotLeft_bot;
        public bool placeBotLeft_bot
        {
            get { return _placeBotLeft_bot; }
            set
            {
                _placeBotLeft_bot = value;
                NotifyOfPropertyChange("placeBotLeft_bot");
            }
        }

        private bool _placeMidRight_bot;
        public bool placeMidRight_bot
        {
            get { return _placeMidRight_bot; }
            set
            {
                _placeMidRight_bot = value;
                NotifyOfPropertyChange("placeMidRight_bot");
            }
        }

        private bool _placeMidMid_bot;
        public bool placeMidMid_bot
        {
            get { return _placeMidMid_bot; }
            set
            {
                _placeMidMid_bot = value;
                NotifyOfPropertyChange("placeMidMid_bot");
            }
        }

        private bool _placeMidLef_bot;
        public bool placeMidLef_bot
        {
            get { return _placeMidLef_bot; }
            set
            {
                _placeMidLef_bot = value;
                NotifyOfPropertyChange("placeMidLef_bot");
            }
        }

        private bool _placeTopRight_bot;
        public bool placeTopRight_bot
        {
            get { return _placeTopRight_bot; }
            set
            {
                _placeTopRight_bot = value;
                NotifyOfPropertyChange("placeTopRight_bot");
            }
        }

        private bool _placeTopMid_bot;
        public bool placeTopMid_bot
        {
            get { return _placeTopMid_bot; }
            set
            {
                _placeTopMid_bot = value;
                NotifyOfPropertyChange("placeTopMid_bot");
            }
        }

        private bool _placeTopLeft_bot;
        public bool placeTopLeft_bot
        {
            get { return _placeTopLeft_bot; }
            set
            {
                _placeTopLeft_bot = value;
                NotifyOfPropertyChange("placeTopLeft_bot");
            }
        }

        #endregion

        #region top_bot

        private bool _showTopTable;
        public bool showTopTable
        {
            get { return _showTopTable; }
            set
            {
                _showTopTable = value;
                if (_showTopTable) heightBotRow = 150; else heightBotRow = 0;
                if (_showTopTable) widthBotColumn = 50; else widthBotColumn = 0;
                NotifyOfPropertyChange("showTopTable");
            }
        }

        private bool _showBotTable;
        public bool showBotTable
        {
            get { return _showBotTable; }
            set
            {
                _showBotTable = value;
                if (_showBotTable) heightTopRow = 150; else heightTopRow = 0;
                if (_showBotTable) widthTopColumn = 50; else heightTopRow = 0;
                NotifyOfPropertyChange("showBotTable");
            }
        }

        private double _heightTopRow;
        public double heightTopRow
        {
            get { return _heightTopRow; }
            set
            {
                _heightTopRow = value;
                NotifyOfPropertyChange("heightTopRow");
            }
        }

        private double _heightBotRow;
        public double heightBotRow
        {
            get { return _heightBotRow; }
            set
            {
                _heightBotRow = value;
                NotifyOfPropertyChange("heightBotRow");
            }
        }

        private double _widthTopColumn;
        public double widthTopColumn
        {
            get { return _widthTopColumn; }
            set
            {
                _widthTopColumn = value;
                NotifyOfPropertyChange("widthTopColumn");
            }
        }

        private double _widthBotColumn;
        public double widthBotColumn
        {
            get { return _widthBotColumn; }
            set
            {
                _widthBotColumn = value;
                NotifyOfPropertyChange("widthBotColumn");
            }
        }

        #endregion


        public StrokePositionTableViewModel(Models.Stroke s, IMatchManager m)
        {
            Stroke = s;
            s.StrokePlacementChanged += OnPlacementChanged;
            _over = false;
            half = false;
            _behind = false;
            if (s.PointOfContact == null)
                s.PointOfContact = "";

            if (s.PointOfContact.Equals("over"))
                _over = true;
            else if (s.PointOfContact.Equals("half-distance"))
                half = true;
            else if (s.PointOfContact.Equals("behind"))
                _behind = true;

            if (s.Player == Models.MatchPlayer.First)
                showTopTable = !(m.CurrentTableEndFirstPlayer == Models.CurrentTableEnd.Top);
            else
                showTopTable = !(m.CurrentTableEndSecondPlayer == Models.CurrentTableEnd.Top);
            showBotTable = !showTopTable;

            if (s.Placement == null || (s.Placement.WX == 0 && s.Placement.WY == 0))
                uncheckAllRadioButtons();
            else
                checkRadioButtonAtFieldPosition(new Point(s.Placement.WX, s.Placement.WY));

            widthHeight = 20;
            placementVisibilty = Visibility.Hidden;

        }

        #region PointOfContact Functions

        public void ChangePointOfContact(string pointOfContact)
        {
            if (Stroke.PointOfContact.Equals(pointOfContact))
            {
                if (pointOfContact.Equals("over"))
                    over = !over;
                else if (pointOfContact.Equals("half-distance"))
                    half = !half;
                else if (pointOfContact.Equals("behind"))
                    behind = !behind;
            }
            OnPointOfContactChange();
        }
        private void OnPointOfContactChange()
        {
            if (over)
                Stroke.PointOfContact = "over";
            else if (half)
                Stroke.PointOfContact = "half-distance";
            else if (behind)
                Stroke.PointOfContact = "behind";
            else
                Stroke.PointOfContact = "";
        }
        #endregion

        #region Placement Functions

        public void OnPlacementChanged(object sender, EventArgs e)
        {
            uncheckAllRadioButtons();

            double x = Stroke.Placement.WX * ((double)canvasWidth / 152.5);
            double y = Stroke.Placement.WY * ((double)canvasHeight / (double)274);
            double left = x - (widthHeight / 2);
            double top = y - (widthHeight / 2);
            currentPlacementPosition = new Thickness(left, top, 0, 0);

            checkRadioButtonAtFieldPosition(new Point(Stroke.Placement.WX, Stroke.Placement.WY));

            placementVisibilty = Visibility.Visible;
        }

        public void GridClicked(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            Point position = e.GetPosition(grid);
            Point fieldPosition = new Point(position.X / grid.ActualWidth * 152.5, position.Y / grid.ActualHeight * 274);
            ChangePositionStroke(fieldPosition.X, fieldPosition.Y);
        }

        private void uncheckAllRadioButtons()
        {
            placeTopLeft_top = false;
            placeTopMid_top = false;
            placeTopRight_top = false;
            placeMidLeft_top = false;
            placeMidMid_top = false;
            placeMidRight_top = false;
            placeBotLeft_top = false;
            placeBotMid_top = false;
            placeBotRight_top = false;
            placeBotRight_bot = false;
            placeBotMid_bot = false;
            placeBotLeft_bot = false;
            placeMidRight_bot = false;
            placeMidMid_bot = false;
            placeMidLef_bot = false;
            placeTopRight_bot = false;
            placeTopMid_bot = false;
            placeTopLeft_bot = false;
        }
        private void checkRadioButtonAtFieldPosition(Point fieldPosition)
        {
            if (fieldPosition.X < 51 && fieldPosition.Y < 46)
                placeTopLeft_top = true;
            else if (fieldPosition.X < 102 && fieldPosition.Y < 46)
                placeTopMid_top = true;
            else if (fieldPosition.Y < 46)
                placeTopRight_top = true;
            else if (fieldPosition.X < 51 && fieldPosition.Y < 92)
                placeMidLeft_top = true;
            else if (fieldPosition.X < 102 && fieldPosition.Y < 92)
                placeMidMid_top = true;
            else if (fieldPosition.Y < 92)
                placeMidRight_top = true;
            else if (fieldPosition.X < 51 && fieldPosition.Y <= 137)
                placeBotLeft_top = true;
            else if (fieldPosition.X < 102 && fieldPosition.Y <= 137)
                placeBotMid_top = true;
            else if (fieldPosition.Y <= 137)
                placeBotRight_top = true;
            else if (fieldPosition.X < 51 && fieldPosition.Y < 183)
                placeBotRight_bot = true;
            else if (fieldPosition.X < 102 && fieldPosition.Y < 183)
                placeBotMid_bot = true;
            else if (fieldPosition.Y < 183)
                placeBotLeft_bot = true;
            else if (fieldPosition.X < 51 && fieldPosition.Y < 229)
                placeMidRight_bot = true;
            else if (fieldPosition.X < 102 && fieldPosition.Y < 229)
                placeMidMid_bot = true;
            else if (fieldPosition.Y < 229)
                placeMidLef_bot = true;
            else if (fieldPosition.X < 51)
                placeTopRight_bot = true;
            else if (fieldPosition.X < 102)
                placeTopMid_bot = true;
            else
                placeTopLeft_bot = true;
        }


        public void ChangePositionStroke(double X, double Y)
        {
            Models.Placement p = new Models.Placement();
            p.WX = X;
            p.WY = Y;
            Stroke.Placement = p;
        }

        public void SetCanvasSize(double w, double h)
        {
            canvasWidth = w;
            canvasHeight = h;
        }

        #endregion
    }
}
