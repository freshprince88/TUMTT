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
        public ServicePositionTableViewModel TableControl { get; set; }
        public SpinRadioViewModel SpinControl { get; set; }
        public string PlayerName { get { return GetNameFromStrokePlayer(); } }

        public ServiceDetailViewModel(Stroke s, IMatchManager man)
        {
            MatchManager = man;
            Stroke = s;
            TableControl = new ServicePositionTableViewModel();
            SpinControl = new SpinRadioViewModel();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            
        }

        #region View Methods
        public void MutualExclusiveToggleButtonClick(Grid parent, ToggleButton tb)
        {
            foreach (ToggleButton btn in parent.FindChildren<ToggleButton>())
            {
                if (btn.Name != tb.Name)
                    btn.IsChecked = false;
            }
        }

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
