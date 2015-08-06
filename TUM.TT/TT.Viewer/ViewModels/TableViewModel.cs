using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.ViewModels
{
    public class TableViewModel : PropertyChangedBase
    {

        public enum ViewMode
        {
            Top,
            Bottom
        }

        private ViewMode _mode;
        public ViewMode Mode
        {
            get
            {
                return _mode;
            }
            set
            {
                if(!_mode.Equals(value))
                    SwitchView(value);

                _mode = value;
                
            }
        }


        public void SwitchView(ViewMode mode)
        {
            
        }
    }
}
