using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.ViewModels
{
    public interface IResultViewTabItem : IScreen
    {
        byte GetOrderInResultView();
        string GetTabTitle(bool getShortTitle);
    }
}
