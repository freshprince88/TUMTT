using Caliburn.Micro;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TT.Lib.Managers;
using TT.Models;

namespace TT.Scouter.ViewModels
{
    public class ServiceDetailViewModel : Conductor<IScreen>.Collection.AllActive
    {
        public Stroke Stroke { get; set; }
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


        public ServicePositionTableViewModel TableControl { get; set; }
        public SpinRadioViewModel SpinControl { get; set;}
        public string PlayerName { get { return GetNameFromStrokePlayer(); } }

        public ServiceDetailViewModel(Stroke s, IMatchManager man, Rally cr)
        {
            MatchManager = man;
            Stroke = s;
            TableControl = new ServicePositionTableViewModel();
            SpinControl = new SpinRadioViewModel();
            CurrentRally = cr;
            SetCourse();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            
        }


        #region View Methods


        public void SelectService(ToggleButton source)
        {
            if (Stroke == null)
            {
                Stroke.Servicetechnique = "";
                return;
            }

            if (source.Name.ToLower().Contains("pendulum"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Servicetechnique="Pendulum";
                }
                else
                {
                    Stroke.Servicetechnique = "";
                }
            }
            else if (source.Name.ToLower().Contains("reverse"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Servicetechnique = "Reverse";
                }
                else
                {
                    Stroke.Servicetechnique = "";
                }
            }
            else if (source.Name.ToLower().Contains("tomahawk"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Servicetechnique = "Tomahawk";
                }
                else
                {
                    Stroke.Servicetechnique = "";
                }
            }
            else if (source.Name.ToLower().Contains("special"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Servicetechnique = "Special";
                }
                else
                {
                    Stroke.Servicetechnique = "";
                }
            }

        }

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
