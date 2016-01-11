using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Events
{
    public class TableKombiViewSelectionChangedEvent
    {
        private int _Value;

        public static implicit operator TableKombiViewSelectionChangedEvent(int value)
        {
            return new TableKombiViewSelectionChangedEvent { _Value = value };
        }

        public static implicit operator int (TableKombiViewSelectionChangedEvent value)
        {
            return value._Value;
        }

        public TableKombiViewSelectionChangedEvent()
        {
            _Value = 0;
        }

        public TableKombiViewSelectionChangedEvent(int val)
        {
            _Value = val;
        }
    }
}

