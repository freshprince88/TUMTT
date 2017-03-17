using Caliburn.Micro;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TT.Lib.Managers;
using TT.Lib.Views;

namespace TT.Lib.ViewModels
{
    public class NavigationControlViewModel : Conductor<IScreen>.Collection.AllActive,
        INavigationViewModel
    {
        private Stack<ConductorBase<IScreen>> SelectedViewStack { get; set; }
        public ConductorBase<IScreen> SelectedView
        {
            get
            {
                return SelectedViewStack.Peek();
            }
        }

        public NavigationControlViewModel()
        {
            SelectedViewStack = new Stack<ConductorBase<IScreen>>();
        }

        public void ActivateItem(ConductorBase<IScreen> item)
        {
            SelectedViewStack.Push(item);
            base.ActivateItem(item);
            NotifyOfPropertyChange("SelectedView");
        }

        public void NavigateBack()
        {
            SelectedViewStack.Pop();
            base.ActivateItem(SelectedView);
            NotifyOfPropertyChange("SelectedView");
        }
        
    }
}
