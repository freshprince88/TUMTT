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
        public Stroke Stroke { get; set; }
        private IMatchManager MatchManager;


        public StrokePositionTableViewModel TableControl { get; set; }

        public string Title { get { return GetTitleFromStroke(); } }
        public string PlayerName { get { return GetNameFromStrokePlayer(); } }




        public StrokeDetailViewModel(Stroke s, IMatchManager man)
        {
            MatchManager = man;
            Stroke = s;
            TableControl = new StrokePositionTableViewModel();
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
