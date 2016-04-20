using Caliburn.Micro;
using System.Collections.Generic;
using TT.Lib.Models;

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

        IEnumerable<IResult> LoadVideo();

        void CreateNewMatch();

        MatchPlayer ConvertPlayer(Player p);

        //void AddRally();
    }
}
