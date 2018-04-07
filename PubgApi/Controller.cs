using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
                //JObject player = api.PlayerData(user.pubg_user_id);
            }
            JObject match = api.MatchData("711ee4d2-7c44-4559-9353-a0e3d749fb38");
            var a = ParseMatch(match);
        }

        List<MatchParticipantModel> ParseMatch(JObject match)
        {
            /*
             * Accept a match json object and pull out the important bits.
             * 
             * Get the Telemetry file.
             * Put each player, matchID and player-stats into a row of a match-participant table
             */
            string matchType = match["data"]["type"].ToString();
            string matchId = match["data"]["id"].ToString();

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
    }
}
