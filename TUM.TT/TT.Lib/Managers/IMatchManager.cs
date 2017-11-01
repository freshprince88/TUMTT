using Caliburn.Micro;
using System.Collections.Generic;
using TT.Models;

namespace TT.Lib.Managers
{
    public interface IMatchManager 
    {
        Match Match { get; set; }

        Playlist ActivePlaylist { get; set; }

        Rally ActiveRally { get; set; }

        CurrentTableEnd CurrentTableEndFirstPlayer { get; }

        CurrentTableEnd CurrentTableEndSecondPlayer { get; }

        int CurrentRallyLength { get; set; }

        IEnumerable<Rally> SelectedRallies { get; set; }

        string FileName { get; set; }

        bool MatchModified { get; set; }
        bool MatchSaveAs { get; set; }
        bool MatchExportExcel { get; set; }


        void DeleteRally(Rally r);

        void RenamePlaylist(string oldName, string newName);

        IEnumerable<IResult> GenerateReport(string type);

        IEnumerable<IResult> SaveMatch();
        IEnumerable<IResult> SaveMatchAs();
        IEnumerable<IResult> ExportExcel();
        IEnumerable<IResult> OpenMatch(string fileName = null);
        IEnumerable<IResult> OpenLiveMatch();



        IEnumerable<IResult> LoadVideo();

        void CreateNewMatch();

        MatchPlayer ConvertPlayer(Player p);

        //void AddRally();
    }
}
