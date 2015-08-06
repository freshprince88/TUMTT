using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.ViewModels
{
    class ServiceViewModel : Screen
    {
        public SpinControlViewModel SpinControl { get; set; }
        public TableSingleViewModel TableView { get; set; }

        public ServiceViewModel()
        {
            SpinControl = new SpinControlViewModel();
            TableView = new TableSingleViewModel();
        }

    }
}
