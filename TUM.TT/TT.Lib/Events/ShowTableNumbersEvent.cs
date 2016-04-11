using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Lib.Events
{
    public class ShowTableNumbersEvent
    {
        public Dictionary<string, int> Numbers { get; set; }

        public ShowTableNumbersEvent(Dictionary<string, int> nums)
        {
            Numbers = nums;
        }

    }
}
