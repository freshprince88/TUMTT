using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Models
{
    public interface IFilter
    {
        Rally[] filter(IEnumerable<Rally> inputRallies);
        bool accepts(Rally rally);
    }
}
