using System;
using Caliburn.Micro;

namespace TT.Scouter.ViewModels
{
    public class StrokePositionTableViewModel : Screen
    {
        public Models.Stroke Stroke { get; set; }

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


        public StrokePositionTableViewModel(Models.Stroke s)
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


        public void ChangePositionStroke(double X, double Y)
        {
            Models.Placement p = new Models.Placement();
            p.WX = X;
            p.WY = Y;
            Stroke.Placement = p;
        }
    }
}
