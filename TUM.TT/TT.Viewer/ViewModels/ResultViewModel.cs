using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.ViewModels
{
    public class ResultViewModel : Conductor<IResultViewTabItem>.Collection.OneActive
    {
        public ResultViewModel(IEnumerable<IResultViewTabItem> tabs)
        {
            Items.AddRange(tabs);
        }
    }
}
