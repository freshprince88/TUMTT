using Caliburn.Micro;
using System.Windows;
using System.Windows.Controls;

namespace TT.Scouter.ViewModels
{
    public class ServicePositionTableViewModel : Screen
    {
        public Models.Schlag Stroke { get; set; }

        public ServicePositionTableViewModel(Models.Schlag s)
        {
            Stroke = s;
        }

        public void ChangePositionPlayer(double position)
        {
            Stroke.Spielerposition = position;
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
