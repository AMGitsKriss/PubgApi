using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Database;
using Database.Models;
using Newtonsoft.Json.Linq;

namespace PubgApi
{
    class Controller
    {
        /*
         * This object will contain the operating logic of the program. 
         * 
         * 1. Get user List
         * 2. Query each of thoser users in the API
         * 3. Compare their Matches list to the player-matches table Does that match already exist? 
         * 4. If not, query for that match information
         * 4. Also grab the match telemetry
         */

        DatabaseConnection db;
        ApiQuery api;

        public Controller()
        {
            db = new DatabaseConnection();
            api = new ApiQuery(ConfigurationManager.AppSettings.Get("ApiKey"));

            List<PubgUserModel> users = db.Users.PubgUsers();

            foreach (var user in users)
            {
                JObject player = api.PlayerData(user.pubg_user_id);
                foreach (JObject playerMatche in player["data"]["relationships"]["matches"]["data"])
                {
                    string matchId = playerMatche["id"].ToString();
                    if (!db.MatchParticipants.MatchExists(matchId))
                    {
                        // The match doesn't exist. 
                        // We can go get the data for it.
                        JObject match = api.MatchData(matchId);
                        string matchTelemetry;
                        List<MatchParticipantModel> players = ParseMatch(match, out matchTelemetry);
                        
                        // Get telemetry?
                        // _T is the type
                        JArray telemetry = api.MatchTelemetry(matchTelemetry);
                        List<Telemetry> events = ParseTelemetry(telemetry);
                    }
                }
            }
        }

        List<MatchParticipantModel> ParseMatch(JObject match, out string telemetry)
        {
            /*
             * Accept a match json object and pull out the important bits.
             * 
             * Get the Telemetry file.
             * Put each player, matchID and player-stats into a row of a match-participant table
             */
            string matchType = match["data"]["type"].ToString();
            string matchId = match["data"]["id"].ToString();
            telemetry = string.Empty;

            List<MatchParticipantModel> participantList = new List<MatchParticipantModel>();

            // Get all the team IDs
            Dictionary<string, int> teamsDict = new Dictionary<string, int>();
            foreach (JObject rosterItem in match["included"])
            {
                if (rosterItem["type"].ToString().Equals("roster"))
                {
                    int teamId = rosterItem["attributes"]["stats"]["teamId"].ToObject<int>();
                    foreach (JObject player in rosterItem["relationships"]["participants"]["data"])
                    {
                        string participantId = player["id"].ToString();
                        teamsDict.Add(participantId, teamId);
                    }
                }
                if (rosterItem["type"].ToString().Equals("asset"))
                {
                    telemetry = rosterItem["attributes"]["URL"].ToString();
                }
            }

            // Build participant list
            foreach (JObject rosterItem in match["included"])
            {
                if (rosterItem["type"].ToString().Equals("participant"))
                {
                    JToken attr = rosterItem["attributes"]["stats"];
                    MatchParticipantModel player = new MatchParticipantModel() {
                        name = attr["name"].ToString(),
                        playerId = attr["playerId"].ToString(),
                        shardId = rosterItem["attributes"]["shardId"].ToString(),
                        DBNOs = attr["DBNOs"].ToObject<int>(),
                        assists = attr["assists"].ToObject<int>(),
                        boosts = attr["boosts"].ToObject<int>(),
                        damageDealt = attr["damageDealt"].ToObject<double>(),
                        deathType = attr["deathType"].ToString(),
                        headshotKills = attr["headshotKills"].ToObject<int>(),
                        heals = attr["heals"].ToObject<int>(),
                        killPlace = attr["killPlace"].ToObject<int>(),
                        killStreaks = attr["killStreaks"].ToObject<int>(),
                        longestKill = attr["longestKill"].ToObject<int>(),
                        mostDamage = attr["mostDamage"].ToObject<int>(),
                        revives = attr["revives"].ToObject<int>(),
                        rideDistance = attr["rideDistance"].ToObject<double>(),
                        roadKills = attr["roadKills"].ToObject<int>(),
                        teamKills = attr["teamKills"].ToObject<int>(),
                        timeSurvived = attr["timeSurvived"].ToObject<int>(),
                        vehicleDestroys = attr["vehicleDestroys"].ToObject<int>(),
                        walkDistance = attr["walkDistance"].ToObject<double>(),
                        winPlace = attr["winPlace"].ToObject<int>(),
                        kills = attr["kills"].ToObject<int>(),
                        match_type = matchType,
                        matchId = matchId,
                        teamId = teamsDict.Where(x => x.Key == rosterItem["id"].ToString()).SingleOrDefault().Value
                    };
                    participantList.Add(player);
                }
            }
            return participantList;
        }

        public List<Telemetry> ParseTelemetry(JArray telemetry)
        {
            List<Telemetry> matchLog = new List<Telemetry>();
            foreach (JToken entry in telemetry)
            {
                Telemetry entryObj = new Telemetry();
                LogFactory lf = new LogFactory();
                // Big ugly block of event assignments

                entryObj.eventType = entry["_T"].ToString();
                entryObj.timestamp = entry["_D"].ToObject<DateTime>();
                entryObj.version = entry["_V"].ToString();

                Type lfType = lf.GetType();
                MethodInfo typeMethod = lfType.GetMethod(entryObj.eventType);
                if(typeMethod != null)
                {
                    entryObj = (Telemetry)typeMethod.Invoke(lf, new object[] { entryObj, entry });
                    matchLog.Add(entryObj);
                }
            }

            return matchLog;
        }
    }
}
