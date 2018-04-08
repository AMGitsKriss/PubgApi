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

            List<PubgUserModel> users = db.PubgUsers();

            foreach (var user in users)
            {
                JObject player = api.PlayerData(user.pubg_user_id);
                foreach (JObject playerMatch in player["data"]["relationships"]["matches"]["data"])
                {
                    MatchModel matchListEntry = new MatchModel();
                    matchListEntry.matchId = playerMatch["id"].ToString();
                    string mapName;
                    DateTime date;

                    if (!db.MatchExists(matchListEntry.matchId))
                    {
                        Console.WriteLine(string.Format("[{0}] Building match {1}", DateTime.Now, matchListEntry.matchId));
                        // The match doesn't exist. 
                        // We can go get the data for it.
                        JObject match = api.MatchData(matchListEntry.matchId);
                        string matchTelemetry;
                        Console.Write(string.Format("[{0}] Building player list... ", DateTime.Now));
                        List<MatchParticipantModel> players = ParseMatch(match, out matchTelemetry, out mapName, out date);
                        Console.WriteLine("Done");
                        matchListEntry.mapName = mapName;
                        matchListEntry.date = date;

                        // Get telemetry
                        Console.Write(string.Format("[{0}] Building telemetry... ", DateTime.Now));
                        JArray telemetry = api.MatchTelemetry(matchTelemetry);
                        List<TelemetryModel> events = ParseTelemetry(telemetry, matchListEntry.matchId);
                        Console.WriteLine("Done");

                        // Now that he have Match and Events, we need to parse them into their respective tables.
                        Console.Write(string.Format("[{0}] Saving to Database... ", DateTime.Now));
                        db.InsertMatch(matchListEntry, players, events);
                        Console.WriteLine("Done");
                    }
                }
            }
        }

        List<MatchParticipantModel> ParseMatch(JObject match, out string telemetry, out string mapName, out DateTime date)
        {
            /*
             * Accept a match json object and pull out the important bits.
             * 
             * Get the Telemetry file.
             * Put each player, matchID and player-stats into a row of a match-participant table
             */
            string matchType = match["data"]["type"].ToString();
            string matchId = match["data"]["id"].ToString();
            mapName = match["data"]["attributes"]["mapName"].ToString();
            date = match["data"]["attributes"]["createdAt"].ToObject<DateTime>();
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
                        teamId = teamsDict.Where(x => x.Key == rosterItem["id"].ToString()).SingleOrDefault().Value,
                        mapName = mapName
                    };
                    participantList.Add(player);
                }
            }
            return participantList;
        }

        public List<TelemetryModel> ParseTelemetry(JArray telemetry, string matchId)
        {
            List<TelemetryModel> matchLog = new List<TelemetryModel>();
            foreach (JToken entry in telemetry)
            {
                TelemetryModel entryObj = new TelemetryModel();
                LogFactory factory = new LogFactory(matchId);
                
                // Event assignments
                // TODO - This is going to fail on some events, as they've not been implimented.
                entryObj = factory.MetaFields(entryObj, entry);
                Type lfType = factory.GetType();
                MethodInfo typeMethod = lfType.GetMethod(entryObj.eventType);
                if(typeMethod != null)
                {
                    try
                    {
                        entryObj = (TelemetryModel)typeMethod.Invoke(factory, new object[] { entryObj, entry });
                    }
                    catch (NotImplementedException)
                    {
                        Console.WriteLine(string.Format("The method {0} has not yet been implimented.", entryObj.eventType));
                    }
                    matchLog.Add(entryObj);
                }
            }
            return matchLog;
        }
    }
}
