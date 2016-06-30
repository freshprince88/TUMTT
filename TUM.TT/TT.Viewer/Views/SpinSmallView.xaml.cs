using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TT.Models;

namespace TT.Viewer.Views
{
    public partial class SpinSmallView : UserControl
    {
        public SpinSmallView(Spin spin)
        {
            InitializeComponent();
            if ("1".Equals(spin.SL) && "1".Equals(spin.TS))
                USSL.IsChecked = true;
            else if ("1".Equals(spin.SL) && "1".Equals(spin.US))
                USSL.IsChecked = true;
            else if ("1".Equals(spin.SL))
                SL.IsChecked = true;
            else if ("1".Equals(spin.SR) && "1".Equals(spin.TS))
                ÜSSR.IsChecked = true;
            else if ("1".Equals(spin.SR) && "1".Equals(spin.US))
                USSR.IsChecked = true;
            else if ("1".Equals(spin.SR))
                SR.IsChecked = true;
            else if ("1".Equals(spin.TS))
                ÜL.IsChecked = true;
            else if ("1".Equals(spin.US))
                US.IsChecked = true;
            else
                No.IsChecked = true;
        }
    }
}
