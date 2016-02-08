using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Events;
using TT.Lib.Models;

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

        void DeleteRally(Rally r);

        void RenamePlaylist(string oldName, string newName);

        IEnumerable<IResult> SaveMatchAction();

        IEnumerable<IResult> OpenMatchAction();

        void CreateNewMatch();

        //void AddRally();
    }
}
