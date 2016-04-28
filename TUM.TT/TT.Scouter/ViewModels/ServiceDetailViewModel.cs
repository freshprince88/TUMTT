using Caliburn.Micro;
using TT.Models;

namespace TT.Scouter.ViewModels
{
    public class ServiceDetailViewModel : Conductor<IScreen>.Collection.AllActive
    {
        public Schlag Stroke { get; set; }

        public ServicePositionTableViewModel TableControl { get; set; }
        public SpinRadioViewModel SpinControl { get; set; }

        public ServiceDetailViewModel(Schlag s)
        {
            Stroke = s;
            TableControl = new ServicePositionTableViewModel();
            SpinControl = new SpinRadioViewModel();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }
    }
}
