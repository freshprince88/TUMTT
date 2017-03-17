using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.ViewModels
{
    public interface INavigationViewModel
    {
        void ActivateItem(ConductorBase<IScreen> item);
        void NavigateBack();
    }
}
