using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Models.Events
{
    public class RallyLengthChangedEvent
    {

        private int _Value;

        public static implicit operator RallyLengthChangedEvent(int value)
        {
            return new RallyLengthChangedEvent { _Value = value };
        }

        public static implicit operator int (RallyLengthChangedEvent value)
        {
            return value._Value;
        }

        public RallyLengthChangedEvent()
        {
            _Value = 0;
        }

        public RallyLengthChangedEvent(int val)
        {
            _Value = val;
        }
    }
}
