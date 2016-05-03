using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Windows.Controls;
using TT.Models.Util.Enums;
using TT.Lib.Events;
using TT.Lib.Managers;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using TT.Models;

namespace TT.Viewer.ViewModels
{
    public class ResultTabViewModel : Conductor<IResultViewTabItem>.Collection.OneActive
    {

        public ResultTabViewModel(IEnumerable<IResultViewTabItem> tabs)
        {

            Items.AddRange(tabs);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            if (Items.Count() > 0)
                ActivateItem(Items[0]);
        }
    }
}
