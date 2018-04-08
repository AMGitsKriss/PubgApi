using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace PubgApi
{
    class ApiQuery
    {
        const string region = "pc-eu";
        const string headerAccept = "application/vnd.api+json";
        string headerAuth;
        string playersUri = "https://api.playbattlegrounds.com/shards/" + region + "/players/";
        string matchUri = "https://api.playbattlegrounds.com/shards/" + region + "/matches/";

        /*
         * This class will handle making the api queries and return json objects.
         */

        public ApiQuery(string apiKey)
        {
            this.headerAuth = apiKey;
        }

        public JObject PlayerData(string userId)
        {
            string response = MakeString(MakeRequest(playersUri, userId));
            JObject userObject = JObject.Parse(response);
            return userObject;
        }

        public JObject MatchData(string matchId) // Repition of above
        {
            string response = MakeString(MakeRequest(matchUri, matchId));
            JObject userObject = JObject.Parse(response);
            return userObject;
        }

        public JArray MatchTelemetry(string telemetryUrl) // Repition of above
        {
            string response = MakeString(MakeRequest("", telemetryUrl));
            JArray userObject = JArray.Parse(response);
            return userObject;
        }
        
        private HttpWebRequest MakeRequest(string uri, string userId)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri + userId);
            request.Method = "GET";
            request.Accept = headerAccept;
            request.Headers.Add(HttpRequestHeader.Authorization, headerAuth);
            return request;
        }

        private string MakeString(HttpWebRequest request)
        {
            Stream response = request.GetResponse().GetResponseStream();
            return new StreamReader(response).ReadToEnd();
        }
    }
}
