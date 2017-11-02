using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;
using System.IO;
using RestSharp;
using RestSharp.Authenticators;
using TT.Models.Api;
using TT.Lib.Properties;

namespace TT.Lib.Api
{
    public class TTCloudApi
    {
        static string BaseUrl = Settings.Default.CloudApiEndpoint;

        readonly string _accessToken;

        public TTCloudApi(string accessToken)
        {
            _accessToken = accessToken;
        }

        public static async Task<string> Login(string email, string password)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(BaseUrl);
            client.Authenticator = new SimpleAuthenticator("email", email, "password", password);

            var request = new RestRequest();
            request.Resource = "auth/local";
            request.Method = Method.POST;

            var response = await client.ExecuteTaskAsync(request);

            if (response.ErrorException != null)
            {
                const string message = "Error connecting to server.";
                throw new ApplicationException(message, response.ErrorException);
            }

            var json = SimpleJson.DeserializeObject<JsonObject>(response.Content);

            if (!json.ContainsKey("token"))
            {
                // TODO: Error Handling
                throw new ApplicationException("lol");
            }

            return json["token"] as string;
        } 

        public async Task<T> Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(BaseUrl);
            client.Authenticator = new JwtAuthenticator(_accessToken);
            var response = await client.ExecuteTaskAsync<T>(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response. Check inner details for more info.";
                throw new ApplicationException(message, response.ErrorException);
            }
            return response.Data;
        }

        public IRestResponse Execute(RestRequest request)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri(BaseUrl);
            client.Authenticator = new JwtAuthenticator(_accessToken);
            var response = client.Execute(request);

            if (response.ErrorException != null)
            {
                const string message = "Error retrieving response. Check inner details for more info.";
                throw new ApplicationException(message, response.ErrorException);
            }
            return response;
        }

        //public User GetCurrentUser()
        //{
        //    var request = new RestRequest();
        //    request.Resource = "users/me";
        //    request.RootElement = "User";

        //    return Execute<User>(request);
        //}

        public Task<MatchMetaResult> GetMatches()
        {
            var request = new RestRequest();
            request.Resource = "matches";
            request.RootElement = "MatchMetaResult";

            return Execute<MatchMetaResult>(request);
        }

        //public MatchMeta GetMatch(Guid matchGuid)
        //{
        //    var request = new RestRequest();
        //    request.Resource = "matches/{MatchGuid}";
        //    request.RootElement = "MatchMeta";

        //    request.AddParameter("MatchGuid", matchGuid, ParameterType.UrlSegment);

        //    return Execute<MatchMeta>(request);
        //}

        //public MatchMeta PutMatch(MatchMeta Match)
        //{
        //    var request = new RestRequest(Method.PUT);
        //    request.Resource = "matches/{MatchGuid}";
        //    request.RootElement = "MatchMeta";

        //    request.AddParameter("MatchGuid", Match.Guid, ParameterType.UrlSegment);

        //    var json = SimpleJson.SerializeObject(Match, SimpleJson.DataContractJsonSerializerStrategy);
        //    request.AddParameter("application/json", json, ParameterType.RequestBody);

        //    return Execute<MatchMeta>(request);
        //}

        //public MatchMeta UploadMatchVideo(MatchMeta Match)
        //{
        //    var request = new RestRequest(Method.POST);
        //    request.Resource = "matches/{MatchGuid}/video";
        //    request.RootElement = "MatchMeta";

        //    request.AddParameter("MatchGuid", Match.Guid, ParameterType.UrlSegment);

        //    request.AddFile("video", Match.ConvertedVideoFile);

        //    return Execute<MatchMeta>(request);
        //}
        
    }
}
