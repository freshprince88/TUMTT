using Caliburn.Micro;

namespace TT.Scouter.ViewModels
{
    public class StrokePositionTableViewModel : Screen
    {
        public Models.Stroke Stroke { get; set; }

        public StrokePositionTableViewModel(Models.Stroke s)
        {
            Stroke = s;
        }

        public void ChangePositionPlayer(string position)
        {
            Stroke.PlayerpositionString = position;
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
