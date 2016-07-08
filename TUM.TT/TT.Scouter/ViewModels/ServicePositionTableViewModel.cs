using Caliburn.Micro;
using System.Windows;
using System.Windows.Controls;

namespace TT.Scouter.ViewModels
{
    public class ServicePositionTableViewModel : Screen
    {
        public Models.Stroke Stroke { get; set; }

        public ServicePositionTableViewModel(Models.Stroke s)
        {
            Stroke = s;
        }

        public void ChangePositionPlayer(double position)
        {
            Stroke.Playerposition = position;
        }

        public void ChangePositionService(double X, double Y)
        {
            Models.Placement p = new Models.Placement();
            p.WX = X;
            p.WY = Y;
            Stroke.Placement = p;
        }
    }
}
