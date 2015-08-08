using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit;

namespace TT.Viewer.ViewModels
{
    class SpinControlViewModel : Screen
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

        /// <summary>
        /// Initializes this view model.
        /// </summary>
        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        /// <summary>
        /// Handles deactivation of this view model.
        /// </summary>
        /// <param name="close">Whether the view model is closed</param>
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
        }
    }
}
