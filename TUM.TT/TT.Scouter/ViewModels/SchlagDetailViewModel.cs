using Caliburn.Micro;
using TT.Lib.Managers;
using TT.Models;

namespace TT.Scouter.ViewModels
{
    public class SchlagDetailViewModel : Conductor<IScreen>.Collection.AllActive 
    {
        public Schlag Stroke { get; set; }
        private IMatchManager MatchManager;


        public StrokePositionTableViewModel TableControl { get; set; }

        public string Title { get { return GetTitleFromStroke(); } }
        public string PlayerName { get { return GetNameFromStrokePlayer(); } }




        public SchlagDetailViewModel(Schlag s, IMatchManager man)
        {
            MatchManager = man;
            Stroke = s;
            TableControl = new StrokePositionTableViewModel();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }


        private string GetTitleFromStroke()
        {
            switch (Stroke.Nummer)
            {
                case 2:
                    return "Receive";
                default:
                    return Stroke.Nummer + ". Stroke";
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
