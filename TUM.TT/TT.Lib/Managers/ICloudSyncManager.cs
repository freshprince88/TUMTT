﻿using System;
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
        User CurrentUser { get; }

        string GetAccountEmail();
        void SetCredentials(string email, string password);
        Task<string> Login();
        string GetConnectionMessage();
        ConnectionStatus GetConnectionStatus();

        SyncStatus GetSyncStatus(MatchMeta meta);

        void UpdateMatch();

        Task<MatchMetaResult> GetMatches(string query = null, string sortFild = "updatedAt", string sortOrder = "desc", int limit = 100);
        Task<MatchMeta> GetMatch(Guid id);
        Task<Tuple<MatchMeta, string, string>> DownloadMatch(
            Guid matchId, string matchFilePath, string videoFilePath, CancellationToken token, Action<string> callback = null);
    }
}
