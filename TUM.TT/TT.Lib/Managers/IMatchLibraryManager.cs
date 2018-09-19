using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Models.Api;

namespace TT.Lib.Managers
{
    public interface IMatchLibraryManager
    {
        string LibraryPath { get; }

        void resetDb(bool deleteFiles = false);

        string GetMatchFilePath(MatchMeta match);
        string GetVideoFilePath(MatchMeta match);
        string GetThumbnailPath(MatchMeta match);

        IEnumerable<MatchMeta> GetMatches(String query=null, int limit=100);
        void DeleteMatch(Guid guid);
        MatchMeta FindMatch(Guid guid);
    }
}
