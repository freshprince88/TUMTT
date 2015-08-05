using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;

namespace TT.Viewer.ViewModels
{
    class SpinControlViewModel : PropertyChangedBase
    {

        public void SelectTop()
        {
            MessageBox.Show("Top");
        }

        public void SelectLeft()
        {
            MessageBox.Show("Left");
        }

        public void SelectMid()
        {
            MessageBox.Show("Mid");
        }

        public void SelectRight()
        {
            MessageBox.Show("Right");
        }

        public void SelectBot()
        {
            MessageBox.Show("Bot");
        }
    }
}
