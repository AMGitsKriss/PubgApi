using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class MatchParticipantModel
    {
        public string name { get; set; }
        public string playerId { get; set; } // Primary Key
        public string matchId { get; set; } // Primary Key
        public int teamId { get; set; }
        public string shardId { get; set; }
        public int DBNOs { get; set; }
        public int assists { get; set; }
        public int boosts { get; set; }
        public double damageDealt { get; set; }
        public string deathType { get; set; }
        public int headshotKills { get; set; }
        public int heals { get; set; }
        public int killPlace { get; set; }
        public int killStreaks { get; set; }
        public int longestKill { get; set; }
        public int mostDamage { get; set; }
        public int revives { get; set; }
        public double rideDistance { get; set; }
        public int roadKills { get; set; }
        public int teamKills { get; set; }
        public int timeSurvived { get; set; }
        public int vehicleDestroys { get; set; }
        public double walkDistance { get; set; }
        public int winPlace { get; set; }
        public int kills { get; set; }
        public string match_type { get; set; }
        public string mapName { get; set; }
    }
}
