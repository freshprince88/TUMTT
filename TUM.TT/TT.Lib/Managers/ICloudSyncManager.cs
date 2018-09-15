using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TT.Models.Api;
using TT.Lib.Api;
using RestSharp;

namespace TT.Lib.Managers
{
    public interface ICloudSyncManager
    {
        User LoggedInUser { get; }
        void Login();
        Task<MatchMetaResult> GetMatches(string query = null);
        Task<Tuple<MatchMeta, string, string>> DownloadMatch(
            Guid matchId, string matchFilePath, string videoFilePath, CancellationToken token, Action<string> callback = null);
    }
}
