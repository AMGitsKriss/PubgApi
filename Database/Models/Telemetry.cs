using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class Telemetry
    {
        public string matchId { get; set; }
        public string eventType { get; set; }
        public DateTime? timestamp { get; set; }
        public string version { get; set; }
        public string attackerName { get; set; }
        public string attackerId { get; set; }
        public int? attackerTeamId { get; set; }
        public float? attackerHealth { get; set; }
        public float? attackerX { get; set; }
        public float? attackerY { get; set; }
        public float? attackerZ { get; set; }
        public int? attackerRanking { get; set; }
        public string victimName { get; set; }
        public string victimId { get; set; }
        public int? victimTeamId { get; set; }
        public float? victimHealth { get; set; }
        public float? victimX { get; set; }
        public float? victimY { get; set; }
        public float? victimZ { get; set; }
        public int? victimRanking { get; set; }
        public string damageTypeCategory { get; set; }
        public string damageCauserName { get; set; }
        public float? distance { get; set; }
        public int? elapsedTime { get; set; }
        public int? numAlivePlayers { get; set; }
        public int? attackId { get; set; }
        public string damageReason { get; set; }
        public float? damage { get; set; }
    }

}
