using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface IPubgRepository
    {
        List<Player> PlayersWithoutID();
        bool SetPlayerID(Player player);

        List<Player> PlayersWithID();
        bool QueueMatch(Guid ID);

        List<Match> GetUnprocessedMatches();
        bool SaveMatch(Match match, List<PlayerStat> players);

        float AverageKills(string name, int kills);
        float AverageDamage(string name, int damage);
    }
}
