using System;
using Database.Models;
using Newtonsoft.Json.Linq;

namespace PubgApi
{
    public class LogFactory
    {
        public Telemetry LogPlayerTakeDamage(Telemetry entryObj, JToken entry)
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

        public Telemetry LogPlayerKill(Telemetry entryObj, JToken entry)
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

        public Telemetry LogPlayerPosition(Telemetry entryObj, JToken entry)
        {
            entryObj = VictimFields(entryObj, entry["character"]);

            entryObj.elapsedTime = entry["elapsedTime"].ToObject<int>();
            entryObj.numAlivePlayers = entry["numAlivePlayers"].ToObject<int>();

            return entryObj;
        }

        //TODO 
        public Telemetry LogMatchDefinition(Telemetry entryObj, JToken entry)
        {
            return entryObj;
        }

        //TODO 
        public Telemetry LogMatchStart(Telemetry entryObj, JToken entry)
        {
            return entryObj;
        }

        //TODO 
        public Telemetry LogItemPickup(Telemetry entryObj, JToken entry)
        {
            return entryObj;
        }

        //TODO 
        public Telemetry LogItemUse(Telemetry entryObj, JToken entry)
        {
            return entryObj;
        }

        //TODO 
        public Telemetry LogItemEquip(Telemetry entryObj, JToken entry)
        {
            return entryObj;
        }

        //TODO 
        public Telemetry LogItemUnequip(Telemetry entryObj, JToken entry)
        {
            return entryObj;
        }

        //TODO 
        public Telemetry LogMatchEnd(Telemetry entryObj, JToken entry)
        {
            return entryObj;
        }

        //TODO 
        public Telemetry LogPlayerAttack(Telemetry entryObj, JToken entry)
        {
            return entryObj;
        }

        //TODO 
        public Telemetry LogVehicleRide(Telemetry entryObj, JToken entry)
        {
            return entryObj;
        }

        //TODO 
        public Telemetry LogVehicleLeave(Telemetry entryObj, JToken entry)
        {
            return entryObj;
        }

        //TODO 
        public Telemetry LogCarePackageLand(Telemetry entryObj, JToken entry)
        {
            return entryObj;
        }

        //TODO 
        public Telemetry LogVehicleDestroy(Telemetry entryObj, JToken entry)
        {
            return entryObj;
        }

        /// <summary>
        /// The standard character field, or the person getting shot at.
        /// </summary>
        private Telemetry VictimFields(Telemetry entryObj, JToken victim)
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
        private Telemetry AttackerFields(Telemetry entryObj, JToken attacker)
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

    }
}