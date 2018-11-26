using Database;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class PubgRepository : IPubgRepository
    {
        private Pubg context = new Pubg();

        public List<Player> PlayersWithoutID()
        {
            var players = context.Players.Where(x => x.ID == null);
            return players.ToList();
        }

        public bool SetPlayerID(Player player)
        {
            try
            {
                context.Entry(player).CurrentValues.SetValues(player);
                context.SaveChanges();

                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<Player> PlayersWithID()
        {
            var players = context.Players.Where(x => x.ID != null);
            return players.ToList();
        }

        public bool QueueMatch(Guid ID)
        {
            try
            {
                Match match = new Match()
                {
                    MatchId = ID
                };
                context.Matches.AddOrUpdate(match);
                context.SaveChanges();

                return true;
            }
            catch(DbUpdateException ex)
            {
                var innerException = ex.InnerException.InnerException as SqlException;
                if (innerException != null && innerException.Number == 2627)
                {
                    return true;
                }
                else
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Match> GetUnprocessedMatches()
        {
            var matches = context.Matches.Where(x => x.Date == null && x.TelemetryUrl == null);
            return matches.ToList();
        }

        public bool SaveMatch(Match match, List<PlayerStat> players)
        {
            try
            {
                context.Matches.AddOrUpdate(match);
                foreach (var item in players)
                {
                    context.PlayerStats.AddOrUpdate(item);
                }
                context.SaveChanges();

                return true;
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException.InnerException as SqlException;
                if (innerException != null && innerException.Number == 2627)
                {
                    return true;
                }
                else
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public float AverageKills(string name, int kills)
        {
            List<int> killList = context.PlayerStats.Where(x => x.Name.Equals(name)).Select(x => x.Kills).ToList();
            killList.Add(kills);
            float result = (float)killList.Sum() / (float)killList.Count();
            return result;
        }

        public float AverageDamage(string name, int damage)
        {
            List<int> dmgList = context.PlayerStats.Where(x => x.Name.Equals(name)).Select(x => x.Damage).ToList();
            dmgList.Add(damage);
            float result = (float)dmgList.Sum() / (float)dmgList.Count();
            return result;
        }
    }
}
