using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models;

namespace TT.Lib.Managers
{
    public interface IViewManager
    {
        IMatchManager MatchManager { get; }

        Playlist ActivePlaylist { get; set; }
        IEnumerable<Rally> SelectedRallies { get; set; }
        
        CombinationList Combinations { get; }
    }
}
