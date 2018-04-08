using System;
using Database.Models;
using Newtonsoft.Json.Linq;

namespace PubgApi
{
    public class LogFactory
    {
        private string MatchId { get; set; }

        public LogFactory(string matchId)
        {
            MatchId = matchId;
        }

        public TelemetryModel LogPlayerTakeDamage(TelemetryModel entryObj, JToken entry)
        {
            entryObj.attackId = entry["attackId"].ToObject<int>();

            entryObj = AttackerFields(entryObj, entry["attacker"]);
            entryObj = VictimFields(entryObj, entry["victim"]);

            entryObj.damageCauserName = entry["damageCauserName"].ToString();
            entryObj.damageTypeCategory = entry["damageTypeCategory"].ToString();
            entryObj.damageReason = entry["damageReason"].ToString();
            entryObj.damage = entry["damage"].ToObject<float>();

            return entryObj;
        }

        public TelemetryModel LogPlayerKill(TelemetryModel entryObj, JToken entry)
        {
            entryObj.attackId = entry["attackId"].ToObject<int>();

            var attacker = entry["killer"];
            entryObj.attackerName = attacker["name"].ToString();
            entryObj.attackerTeamId = attacker["teamId"].ToObject<int>();
            entryObj.attackerHealth = attacker["health"].ToObject<float>();
            entryObj.attackerId = attacker["accountId"].ToString();
            entryObj.attackerRanking = attacker["ranking"].ToObject<int>();
            entryObj.attackerX = attacker["location"]["x"].ToObject<float>();
            entryObj.attackerY = attacker["location"]["y"].ToObject<float>();
            entryObj.attackerZ = attacker["location"]["z"].ToObject<float>();

            var victim = entry["victim"];
            entryObj.victimName = victim["name"].ToString();
            entryObj.victimTeamId = victim["teamId"].ToObject<int>();
            entryObj.victimHealth = victim["health"].ToObject<float>();
            entryObj.victimId = victim["accountId"].ToString();
            entryObj.victimRanking = victim["ranking"].ToObject<int>();
            entryObj.victimX = victim["location"]["x"].ToObject<float>();
            entryObj.victimY = victim["location"]["y"].ToObject<float>();
            entryObj.victimZ = victim["location"]["z"].ToObject<float>();

            entryObj.damageCauserName = entry["damageCauserName"].ToString();
            entryObj.distance = entry["distance"].ToObject<float>();
            entryObj.damageTypeCategory = entry["damageTypeCategory"].ToString();

            return entryObj;
        }

        public TelemetryModel LogPlayerPosition(TelemetryModel entryObj, JToken entry)
        {
            entryObj = VictimFields(entryObj, entry["character"]);

            entryObj.elapsedTime = entry["elapsedTime"].ToObject<int>();
            entryObj.numAlivePlayers = entry["numAlivePlayers"].ToObject<int>();

            return entryObj;
        }

        public TelemetryModel LogMatchDefinition(TelemetryModel entryObj, JToken entry)
        {
            entryObj.pingQuality = entry["PingQuality"].ToString();
            return entryObj;
        }

        public TelemetryModel LogItemPickup(TelemetryModel entryObj, JToken entry)
        {
            return CharacterItemFields(entryObj, entry);
        }

        public TelemetryModel LogItemUse(TelemetryModel entryObj, JToken entry)
        {
            return CharacterItemFields(entryObj, entry);
        }

        public TelemetryModel LogItemEquip(TelemetryModel entryObj, JToken entry)
        {
            return CharacterItemFields(entryObj, entry);
        }
 
        public TelemetryModel LogItemUnequip(TelemetryModel entryObj, JToken entry)
        {
            return CharacterItemFields(entryObj, entry);
        }

        public TelemetryModel LogPlayerAttack(TelemetryModel entryObj, JToken entry)
        {
            entryObj.attackId = entry["attackId"].ToObject<int>();
            entryObj.attackType = entry["attackType"].ToString();
            entryObj = AttackerFields(entryObj, entry["attacker"]);
            entryObj = ItemFields(entryObj, entry["weapon"]);
            return entryObj;
        }

        public TelemetryModel LogVehicleRide(TelemetryModel entryObj, JToken entry)
        {
            return CharacterVehicleFields(entryObj, entry);
        }

        public TelemetryModel LogVehicleLeave(TelemetryModel entryObj, JToken entry)
        {
            return CharacterVehicleFields(entryObj, entry);
        }

        public TelemetryModel LogVehicleDestroy(TelemetryModel entryObj, JToken entry)
        {
            entryObj.attackId = entry["attackId"].ToObject<int>();
            entryObj = AttackerFields(entryObj, entry["attacker"]);
            entryObj = VehicleFields(entryObj, entry["vehicle"]);

            entryObj.damageCauserName = entry["damageCauserName"].ToString();
            entryObj.damageTypeCategory = entry["damageTypeCategory"].ToString();
            entryObj.distance = entry["distance"].ToObject<float>();

            return entryObj;
        }

        /*//TODO - Unimplimented log object 
        public Telemetry LogCarePackageLand(Telemetry entryObj, JToken entry)
        {
            return entryObj;
        }*/

        /*// TODO - Unimplimented log object 
        public Telemetry LogMatchStart(Telemetry entryObj, JToken entry)
        {
            return entryObj;
        }*/

        /*//TODO - Unimplimented log object 
        public Telemetry LogMatchEnd(Telemetry entryObj, JToken entry)
        {
            return entryObj;
        }*/

        /// <summary>
        /// The standard character field, or the person getting shot at.
        /// </summary>
        private TelemetryModel VictimFields(TelemetryModel entryObj, JToken victim)
        {
            entryObj.victimName = victim["name"].ToString();
            entryObj.victimTeamId = victim["teamId"].ToObject<int>();
            entryObj.victimHealth = victim["health"].ToObject<float>();
            entryObj.victimId = victim["accountId"].ToString();
            entryObj.victimRanking = victim["ranking"].ToObject<int>();
            entryObj.victimX = victim["location"]["x"].ToObject<float>();
            entryObj.victimY = victim["location"]["y"].ToObject<float>();
            entryObj.victimZ = victim["location"]["z"].ToObject<float>();

            return entryObj;
        }

        /// <summary>
        /// The person doing the shooting.
        /// </summary>
        private TelemetryModel AttackerFields(TelemetryModel entryObj, JToken attacker)
        {
            entryObj.attackerName = attacker["name"].ToString();
            entryObj.attackerTeamId = attacker["teamId"].ToObject<int>();
            entryObj.attackerHealth = attacker["health"].ToObject<float>();
            entryObj.attackerId = attacker["accountId"].ToString();
            entryObj.attackerRanking = attacker["ranking"].ToObject<int>();
            entryObj.attackerX = attacker["location"]["x"].ToObject<float>();
            entryObj.attackerY = attacker["location"]["y"].ToObject<float>();
            entryObj.attackerZ = attacker["location"]["z"].ToObject<float>();

            return entryObj;
        }

        /// <summary>
        /// Data about an item/weapon interaction.
        /// </summary>
        private TelemetryModel ItemFields(TelemetryModel entryObj, JToken item)
        {
            entryObj.itemId = item["itemId"].ToString();
            entryObj.stackCount = item["stackCount"].ToObject<int>();
            entryObj.category = item["category"].ToString();
            entryObj.subCategory = item["subCategory"].ToString();
            entryObj.attachments = string.Join(",", item["attachedItems"].ToObject<string[]>());
            

            return entryObj;
        }

        /// <summary>
        /// Data about an item/weapon interaction.
        /// </summary>
        private TelemetryModel VehicleFields(TelemetryModel entryObj, JToken item)
        {
            entryObj.vehicleType = item["vehicleType"].ToString();
            entryObj.vehicleId = item["vehicleId"].ToString();
            entryObj.healthPercent = item["healthPercent"].ToObject<float>();
            entryObj.fuelPercent = item["feulPercent"].ToObject<float>(); // This is a typo in the API that needs fixing

            return entryObj;
        }

        /// <summary>
        /// Data about an vehicle interaction.
        /// </summary>
        public TelemetryModel CharacterVehicleFields(TelemetryModel entryObj, JToken entry)
        {
            entryObj = VictimFields(entryObj, entry["character"]);
            entryObj = VehicleFields(entryObj, entry["vehicle"]);
            return entryObj;
        }

        /// <summary>
        /// A number of log items contain only a character and item detail.
        /// </summary>
        private TelemetryModel CharacterItemFields(TelemetryModel entryObj, JToken entry)
        {
            entryObj = VictimFields(entryObj, entry["character"]);
            entryObj = ItemFields(entryObj, entry["item"]);
            return entryObj;
        }

        /// <summary>
        /// Type, Date and Version of this object.
        /// </summary>
        public TelemetryModel MetaFields(TelemetryModel entryObj, JToken data)
        {
            entryObj.eventType = data["_T"].ToString();
            entryObj.timestamp = data["_D"].ToObject<DateTime>();
            entryObj.version = data["_V"].ToString();

            entryObj.matchId = MatchId.ToString();

            return entryObj;
        }

    }
}