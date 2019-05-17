using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface IApiRepository
    {
        JObject GetPlayerByName(string name);
        JObject GetPlayerById(string id);
        JObject GetMatches(Guid matchId);
        JToken GetTelemetry(string telemetryURL);
    }
}
