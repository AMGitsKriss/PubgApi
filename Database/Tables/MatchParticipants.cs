using Devart.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Tables
{
    public class MatchParticipantsTable
    {
        MySqlConnection conn;

        public MatchParticipantsTable(MySqlConnection conn)
        {
            this.conn = conn;
        }

        public bool MatchExists(string matchId)
        {
            string sqlString = string.Format("SELECT * FROM pubg_match_participants WHERE matchId = '{0}'", matchId);
            MySqlCommand query = new MySqlCommand(sqlString, conn);
            using (MySqlDataReader reader = query.ExecuteReader())
            {
                return reader.HasRows;
            }
        }
    }
}
