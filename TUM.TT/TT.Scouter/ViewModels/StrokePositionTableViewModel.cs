using Caliburn.Micro;

namespace TT.Scouter.ViewModels
{
    public class StrokePositionTableViewModel : Screen
    {
        public Models.Schlag Stroke { get; set; }

        public StrokePositionTableViewModel(Models.Schlag s)
        {
            Stroke = s;
        }

        public void ChangePositionPlayer(string position)
        {
            Stroke.Balltreffpunkt = position;
        }

        public void ChangePositionService(double X, double Y)
        {
            Models.Platzierung p = new Models.Platzierung();
            p.WX = X;
            p.WY = Y;
            Stroke.Platzierung = p;
        }
    }
}
