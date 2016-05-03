using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TT.Lib.Util
{
    public class CustomClosableComboDialog<T> : CustomDialog
    {
        public Button CloseButton { get; set; }

        public ComboBox Combo { get; set; }
    }
}
