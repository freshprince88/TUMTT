using System;
using Caliburn.Micro;
using TT.Lib.Managers;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

namespace TT.Scouter.ViewModels
{
    public class StrokePositionTableViewModel : Screen
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
        private bool _showTopTable;
        public bool showTopTable
        {
            get { return _showTopTable; }
            set
            {
                _showTopTable = value;
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
                NotifyOfPropertyChange("showBotTable");
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
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeTopRight_top;
        public bool placeTopRight_top
        {
            get { return _placeTopRight_top; }
            set
            {
                _placeTopRight_top = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeMidLeft_top;
        public bool placeMidLeft_top
        {
            get { return _placeMidLeft_top; }
            set
            {
                _placeMidLeft_top = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeMidMid_top;
        public bool placeMidMid_top
        {
            get { return _placeMidMid_top; }
            set
            {
                _placeMidMid_top = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeMidRight_top;
        public bool placeMidRight_top
        {
            get { return _placeMidRight_top; }
            set
            {
                _placeMidRight_top = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeBotLeft_top;
        public bool placeBotLeft_top
        {
            get { return _placeBotLeft_top; }
            set
            {
                _placeBotLeft_top = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeBotMid_top;
        public bool placeBotMid_top
        {
            get { return _placeBotMid_top; }
            set
            {
                _placeBotMid_top = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeBotRight_top;
        public bool placeBotRight_top
        {
            get { return _placeBotRight_top; }
            set
            {
                _placeBotRight_top = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeBotRight_bot;
        public bool placeBotRight_bot
        {
            get { return _placeBotRight_bot; }
            set
            {
                _placeBotRight_bot = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeBotMid_bot;
        public bool placeBotMid_bot
        {
            get { return _placeBotMid_bot; }
            set
            {
                _placeBotMid_bot = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeBotLeft_bot;
        public bool placeBotLeft_bot
        {
            get { return _placeBotLeft_bot; }
            set
            {
                _placeBotLeft_bot = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeMidRight_bot;
        public bool placeMidRight_bot
        {
            get { return _placeMidRight_bot; }
            set
            {
                _placeMidRight_bot = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeMidMid_bot;
        public bool placeMidMid_bot
        {
            get { return _placeMidMid_bot; }
            set
            {
                _placeMidMid_bot = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeMidLef_bot;
        public bool placeMidLef_bot
        {
            get { return _placeMidLef_bot; }
            set
            {
                _placeMidLef_bot = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeTopRight_bot;
        public bool placeTopRight_bot
        {
            get { return _placeTopRight_bot; }
            set
            {
                _placeTopRight_bot = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeTopMid_bot;
        public bool placeTopMid_bot
        {
            get { return _placeTopMid_bot; }
            set
            {
                _placeTopMid_bot = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        private bool _placeTopLeft_bot;
        public bool placeTopLeft_bot
        {
            get { return _placeTopLeft_bot; }
            set
            {
                _placeTopLeft_bot = value;
                NotifyOfPropertyChange("placeTopLeft_top");
            }
        }

        #endregion


        public StrokePositionTableViewModel(Models.Stroke s, IMatchManager m)
        {
            Stroke = s;
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
                showTopTable = (m.Match.FirstPlayer.StartingTableEnd == Models.StartingTableEnd.Top);
            else
                showTopTable = (m.Match.SecondPlayer.StartingTableEnd == Models.StartingTableEnd.Top);
            showBotTable = !showTopTable;

            if (s.Placement == null || (s.Placement.WX == 0 && s.Placement.WY == 0))
                makeAllFalse();
            else
                makeRightRight(new Point(s.Placement.WX, s.Placement.WY));

        }

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

        public void GridClicked(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            Point position = e.GetPosition(grid);
            Point fieldPosition = new Point(position.X / grid.ActualWidth * 152.5, position.Y / grid.ActualHeight * 274);
            ChangePositionStroke(fieldPosition.X, fieldPosition.Y);

            makeAllFalse();

            makeRightRight(fieldPosition);
        }

        private void makeAllFalse()
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
        private void makeRightRight(Point fieldPosition)
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
    }
}
