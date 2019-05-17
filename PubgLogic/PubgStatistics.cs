using Database;
using Newtonsoft.Json.Linq;
using Repository;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgLogic
{
    public class PubgStatistics
    {
        IPubgRepository context = new PubgRepository();
        ApiRepository api = new ApiRepository();

        public PubgStatistics()
        {
            CheckPlayerIDs();
            CheckRecentMatches();
            GetMatchData();
        }

        /// <summary>
        /// Check the Players table for usernames without a player ID and fetch those IDs.
        /// </summary>
        private void CheckPlayerIDs()
        {
            List<Player> players = context.PlayersWithoutID();

            foreach (Player p in players)
            {
                JObject playerDetail = api.GetPlayerByName(p.Name);
                if (playerDetail["data"][0]["id"] != null)
                {
                    p.ID = playerDetail["data"][0]["id"].ToString();
                    context.SetPlayerID(p);

                }
            }
        }

        /// <summary>
        /// Check the recent matches of teh registered players. Save the match IDs for future use.
        /// </summary>
        private void CheckRecentMatches()
        {
            List<Player> players = context.PlayersWithID();

            foreach (Player p in players)
            {
                JObject playerDetail = api.GetPlayerById(p.ID);

                foreach (JObject match in playerDetail["data"]["relationships"]["matches"]["data"])
                {
                    Guid matchID = new Guid(match["id"].ToString());
                    Console.WriteLine("Queueing Match {0}", matchID);
                    context.QueueMatch(matchID);
                }
                Console.WriteLine("Queueing Complete");
            }
        }

        /// <summary>
        /// Iterates over match IDs that haven't been processed yet. Check them in date order and update the statistics for each player that is mentioned. 
        /// </summary>
        private void GetMatchData()
        {
            List<string> players = context.PlayersWithID().Select(x => x.Name).ToList();
            List<Match> matches = context.GetUnprocessedMatches();

            foreach (Match m in matches)
            {
                Console.WriteLine("Parsing Match {0}", m.MatchId);
                List<PlayerStat> stats = new List<PlayerStat>();
                JObject match = api.GetMatches(m.MatchId);
                m.Date = (DateTime)match["data"]["attributes"]["createdAt"];

                foreach (JObject item in match["included"])
                {
                    string type = item["type"].ToString();
                    if (type.Equals("participant") && players.Contains(item["attributes"]["stats"]["name"].ToString()))
                    {
                        stats.Add(ParsePlayer(item["attributes"]["stats"], m.MatchId));
                    }
                    if (type.Equals("asset") && item["attributes"]["URL"] != null)
                    {
                        m.TelemetryUrl = item["attributes"]["URL"].ToString();
                    }
                }
                context.SaveMatch(m, stats);
            }
            Console.WriteLine("Match Parsing Complete.");
        }

        private void CalculateMatchData()
        {
            // Look at previous stats and add a row for the current match
        }

        private void DownloadTelemetry()
        {
            // Get telemetry lists that don't have a directory

            // Download telemetry

            // Validate the file is there

            // Update tatabase
        }

        private PlayerStat ParsePlayer(JToken item, Guid matchID)
        {
            string name = item["name"].ToString();
            int kills = item["kills"].ToObject<int>();
            int damage = item["damageDealt"].ToObject<int>();
            PlayerStat p = new PlayerStat()
            {
                MatchId = matchID,
                Name = name,
                Kills = kills,
                AvgKills = context.AverageKills(name, kills),
                Damage = damage,
                AvgDamage = context.AverageDamage(name, damage) // TODO - Get this information from the database
            };
            return p;
        }
    }
}
