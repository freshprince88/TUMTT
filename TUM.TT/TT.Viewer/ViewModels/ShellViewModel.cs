using Caliburn.Micro;
using System.Windows;
using System.Reflection;

namespace TT.Viewer.ViewModels {

    public class ShellViewModel : Conductor<object>.Collection.AllActive, IShell {

        public Screen FilterView { get; private set; }

        public ShellViewModel()
        {
            FilterView = new FilterViewModel();
            ActivateItem(FilterView);
        }
            
        protected override void OnViewLoaded(object view)
        {            
            base.OnViewLoaded(view);
        }

        protected override void OnActivate()
        {            
            base.OnActivate();
        }
    }
}