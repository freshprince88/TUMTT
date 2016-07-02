using Caliburn.Micro;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TT.Lib.Managers;
using TT.Models;

namespace TT.Scouter.ViewModels
{
    public class StrokeDetailViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private Stroke _stroke;
        public Stroke Stroke
        {
            get { return _stroke; }
            set
            {
                if (_stroke != value)
                {
                    _stroke = value;
                    NotifyOfPropertyChange("Stroke");
                }
            }
        }
        private IMatchManager MatchManager;
        private Rally _rally;
        public Rally CurrentRally
        {
            get { return _rally; }
            set
            {
                if (_rally != value)
                {
                    _rally = value;
                    NotifyOfPropertyChange("CurrentRally");
                }
            }
        }


        public StrokePositionTableViewModel TableControl { get; set; }

        public string Title { get { return GetTitleFromStroke(); } }
        public string PlayerName { get { return GetNameFromStrokePlayer(); } }




        public StrokeDetailViewModel(Stroke s, IMatchManager man, Rally cr)
        {
            MatchManager = man;
            Stroke = s;
            TableControl = new StrokePositionTableViewModel();
            CurrentRally = cr;
            SetCourse();

        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }
        #region View Methods
        public void SelectCourse(ToggleButton source)
        {
            if (Stroke == null)
            {
                Stroke.Course = "";
                return;
            }

            if (source.Name.ToLower().Contains("netout"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Course = "Net/Out";
                }
                else
                {
                    Stroke.Course = "";
                }
            }
            else if (source.Name.ToLower().Contains("continue"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Course = "continue";
                }
                else
                {
                    Stroke.Course = "";
                }
            }
            else if (source.Name.ToLower().Contains("winner"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Course = "Winner";
                }
                else
                {
                    Stroke.Course = "";
                }
            }


        }
        #endregion
        #region Helper Methods

        public void MutualExclusiveToggleButtonClick(Grid parent, ToggleButton tb)
        {
            foreach (ToggleButton btn in parent.FindChildren<ToggleButton>())
            {
                if (btn.Name != tb.Name)
                    btn.IsChecked = false;
            }
        }
        public void SetCourse()
        {
            if (Stroke.Number < CurrentRally.Length)
            {
                Stroke.Course = "continue";
            }
            else if (Stroke.Number == CurrentRally.Length)
            {
                if (Stroke.Player == CurrentRally.Winner)
                {
                    Stroke.Course = "Winner";
                }
                else
                    Stroke.Course = "Net/Out";
            }
            else
                Stroke.Course = "";
        }

        private string GetTitleFromStroke()
        {
            if (Stroke == null)
                return "";

            switch (Stroke.Number)
            {
                case 2:
                    return "Receive";
                default:
                    return Stroke.Number + ". Stroke";
            }
        }
        private string GetNameFromStrokePlayer()
        {
            if (Stroke == null)
                return "";

            switch (Stroke.Player)
            {
                case MatchPlayer.First:
                    return MatchManager.Match.FirstPlayer.Name.Split(' ')[0];
                case MatchPlayer.Second:
                    return MatchManager.Match.SecondPlayer.Name.Split(' ')[0];
                case MatchPlayer.None:
                    return "";
                default:
                    return "";
            }
        }
        #endregion
    }
}
