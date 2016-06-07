using Caliburn.Micro;
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
