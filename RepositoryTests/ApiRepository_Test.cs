using NUnit.Framework;
using System;
using Repository;

namespace RepositoryTests
{
    [TestFixture]
    public class ApiRepository_Test
    {
        [Test]
        public void PlayerTest()
        {
            ApiRepository conn = new ApiRepository();
            var result = conn.GetPlayerByName("AMGitsKriss");
            string player = result["data"][0]["attributes"]["name"].ToString();

            Assert.IsTrue(player.Equals("AMGitsKriss"));
        }

        [Test]
        public void MatchTest()
        {
            ApiRepository conn = new ApiRepository();
            var result = conn.GetMatches(new Guid("2c15e0f6-87dc-44d7-96ff-3edc2f720803"));
            string gamemode = result["data"]["attributes"]["gameMode"].ToString();

            Assert.IsTrue(gamemode.Equals("solo-fpp"));
        }

        [Test]
        public void TelemetryTest()
        {
            ApiRepository conn = new ApiRepository();
            var result = conn.GetTelemetry("https://telemetry-cdn.playbattlegrounds.com/bluehole-pubg/pc-na/2018/11/22/18/57/839ba411-ee88-11e8-9c28-0a5864705c20-telemetry.json");


            Assert.IsTrue(result[0]["MatchId"].ToString().Equals("match.bro.official.pc-2018-01.steam.solo-fpp.na.2018.11.22.18.2c15e0f6-87dc-44d7-96ff-3edc2f720803"));
        }
    }
}
