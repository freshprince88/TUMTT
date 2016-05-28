using Caliburn.Micro;
using TT.Models;

namespace TT.Scouter.ViewModels
{
    public class ServiceDetailViewModel : Conductor<IScreen>.Collection.AllActive
    {
        private Schlag _stroke;
        public Schlag Stroke {
            get
            {
                return _stroke;
            }
            set
            {
                SpinControl.Stroke = value;
                TableControl.Stroke = value;
                _stroke = value;
            }
        }

        public ServicePositionTableViewModel TableControl { get; set; }
        public SpinRadioViewModel SpinControl { get; set; }

        public ServiceDetailViewModel(Schlag s)
        {
            _stroke = s;

            TableControl = new ServicePositionTableViewModel(s);
            SpinControl = new SpinRadioViewModel(s);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }
    }
}
