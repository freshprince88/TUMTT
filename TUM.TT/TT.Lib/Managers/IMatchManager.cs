using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Events;

namespace TT.Lib.Managers
{
    public interface IMatchManager 
    {
        Match Match { get; }

        Playlist ActivePlaylist { get; set; }

        string FileName { get;  }

        bool MatchModified { get; set; }

        void OpenMatch();

        void SaveMatch();

        IEnumerable<IResult> SaveMatchAction();

        IEnumerable<IResult> OpenMatchAction();

        //void CreateMatch();

        //void AddRally();
    }
}
