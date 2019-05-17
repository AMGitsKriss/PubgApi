using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Net;
using RepositoryContracts;

namespace Repository
{
    public class ApiRepository : BaseApi, IApiRepository
    {
        string TelemetryDir = ConfigurationManager.AppSettings.Get("TelemetryDir");

        public ApiRepository()
        {
            ApiKey = ConfigurationManager.AppSettings.Get("ApiKey");
            Host = ConfigurationManager.AppSettings.Get("HostUri");
        }

        /// <summary>
        /// Get player by name. Case sensitive.
        /// </summary>
        public JObject GetPlayerByName(string name)
        {
            string query = "players?filter[playerNames]=" + name;
            JObject player = (JObject)Query(requestType.GET, query);
            return player;
        }

        /// <summary>
        /// Get player by GUID.
        /// </summary>
        public JObject GetPlayerById(string id)
        {
            string query = "players/" + id;
            JObject player = (JObject)Query(requestType.GET, query);
            return player;
        }

        /// <summary>
        /// Get match by it's GUID.
        /// </summary>
        public JObject GetMatches(Guid matchId)
        {
            string query = "matches/" + matchId;
            JObject match = (JObject)Query(requestType.GET, query);
            return match;
        }

        /// <summary>
        /// Get match telemetry.
        /// </summary>
        public JToken GetTelemetry(string telemetryURL)
        {
            JToken response = QueryTelemetry(telemetryURL);

            // TODO - Filewriter needs splitting into it's own function
            if (!string.IsNullOrWhiteSpace(TelemetryDir))
            {
                if (!Directory.Exists(TelemetryDir))
                {
                    Directory.CreateDirectory(TelemetryDir);
                }
                
                Uri telemetry = new Uri(telemetryURL);
                string fileName = string.Concat(telemetry.Segments.Skip(3)).Replace("/", "-");

                using (StreamWriter file = File.CreateText(TelemetryDir + fileName))
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    response.WriteTo(writer);
                }
            }

            return response;

        }


        // TODO - Work direct url query into BaseApi
        private JToken QueryTelemetry(string url)
        {
            HttpWebResponse response = null;
            response = MakeRequest(NewRequest(requestType.GET, url));
            JToken result = ParseResult(response, url);
            return result;
        }
    }
}
