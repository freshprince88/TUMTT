using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Events;
using TT.Lib.Models;
using TT.Lib.Results;

namespace TT.Lib.Managers
{
    public interface IMatchManager 
    {
        Match Match { get; set; }

        Playlist ActivePlaylist { get; set; }

        string FileName { get; set; }

        bool MatchModified { get; set; }

        void DeleteRally(Rally r);

        void RenamePlaylist(string oldName, string newName);

        IEnumerable<IResult> SaveMatch();

        IEnumerable<IResult> OpenMatch();

        OpenFileDialogResult LoadVideo();

        void CreateNewMatch();

        //void AddRally();
    }
}
