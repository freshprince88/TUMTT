using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Viewer.ViewModels
{
    public class TableKombiViewModel : Screen
    {
        private IEventAggregator events;

        #region Properties
        public bool ButtonsVisible { get; set; }



        //public Tuple<int,int> ButtonPosition
        //{
        //    get { return (Tuple<int,int>)GetValue(ButtonPositionProperty); }
        //    set { SetValue(ButtonPositionProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for ButtonPosition.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ButtonPositionProperty =
        //    DependencyProperty.Register("ButtonPosition", typeof(Tuple<int,int>), typeof(TableKombiViewModel), null);

        
        #endregion

        public TableKombiViewModel(IEventAggregator eventAggregator)
        {
            events = eventAggregator;
        }
    }
}
