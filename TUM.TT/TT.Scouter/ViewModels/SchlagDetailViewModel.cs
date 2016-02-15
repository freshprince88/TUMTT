using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Models;

namespace TT.Scouter.ViewModels
{
    public class SchlagDetailViewModel : Conductor<IScreen>.Collection.AllActive 
    {
        public Schlag Stroke { get; set; }

        public SchlagDetailViewModel(Schlag s)
        {
            Stroke = s;
        }
    }
}
