using Caliburn.Micro;
using System.Windows.Controls.Primitives;
using TT.Lib.Managers;
using TT.Models;

namespace TT.Scouter.ViewModels
{
    public class ServiceDetailViewModel : Conductor<IScreen>.Collection.AllActive
    {
        public Schlag Stroke { get; set; }
        private IMatchManager MatchManager;
        public ServicePositionTableViewModel TableControl { get; set; }
        public SpinRadioViewModel SpinControl { get; set; }
        public string PlayerName { get { return GetNameFromStrokePlayer(); } }

        public ServiceDetailViewModel(Schlag s, IMatchManager man)
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
        public void SelectService(ToggleButton source)
        {
            if (source.Name.ToLower().Contains("pendulum"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Aufschlagart="Pendulum";
                }
                else
                {
                    Stroke.Aufschlagart = "";
                }
            }
            else if (source.Name.ToLower().Contains("reverse"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Aufschlagart = "Reverse";
                }
                else
                {
                    Stroke.Aufschlagart = "";
                }
            }
            else if (source.Name.ToLower().Contains("tomahawk"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Aufschlagart = "Tomahawk";
                }
                else
                {
                    Stroke.Aufschlagart = "";
                }
            }
            else if (source.Name.ToLower().Contains("special"))
            {
                if (source.IsChecked.Value)
                {
                    Stroke.Aufschlagart = "Special";
                }
                else
                {
                    Stroke.Aufschlagart = "";
                }
            }

        }
        private string GetNameFromStrokePlayer()
        {
            switch (Stroke.Spieler)
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
    }
}
