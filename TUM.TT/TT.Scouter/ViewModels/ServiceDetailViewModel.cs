using Caliburn.Micro;
using TT.Lib.Models;

namespace TT.Scouter.ViewModels
{
    public class ServiceDetailViewModel : Conductor<IScreen>.Collection.AllActive
    {
        public Schlag Stroke { get; set; }

        public ServiceDetailViewModel(Schlag s)
        {
            Stroke = s;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }
    }
}
