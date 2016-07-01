using Caliburn.Micro;
using MahApps.Metro.Controls;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace TT.Scouter.ViewModels
{
    public class SpinRadioViewModel : Screen
    {
        public void MutualExclusiveToggleButtonClick(Grid parent, ToggleButton tb)
        {
            foreach (ToggleButton btn in parent.FindChildren<ToggleButton>())
            {
                if (btn.Name != tb.Name)
                    btn.IsChecked = false;
            }
        }

    }
}
