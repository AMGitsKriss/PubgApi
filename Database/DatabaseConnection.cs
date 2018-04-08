using Devart.Data.MySql;
using System;
using System.Configuration;
using Database.Models;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Database
{
    public class DatabaseConnection
    {
        MySqlConnection conn;

        // Establish the database connection object
        public DatabaseConnection()
        {
            string myConnectionString;
            myConnectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = myConnectionString;
                conn.Open();
                Console.WriteLine("Connected to database.");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(string.Format("{0} Error: {1}", ex.GetType(), ex.Message));
            }
        }

        public List<PubgUserModel> PubgUsers()
        {
            string sqlString = "SELECT pubg_username, pubg_user_id FROM users WHERE pubg_username <> ''";
            MySqlCommand query = new MySqlCommand(sqlString, conn);
            using (MySqlDataReader reader = query.ExecuteReader())
            {
                List<PubgUserModel> userList = new List<PubgUserModel>();
                while (reader.Read())
                {
                    PubgUserModel user = new PubgUserModel();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var value = reader.GetValue(i);
                        var propName = reader.GetName(i);
                        PropertyInfo propertyInfo = user.GetType().GetProperty(propName);
                        propertyInfo.SetValue(user, value);
                    }
                    userList.Add(user);
                }
                return userList;
            }
        }

        public bool MatchExists(string matchId)
        {
            string sqlString = string.Format("SELECT * FROM pubg_match_list WHERE matchId = '{0}'", matchId);
            MySqlCommand query = new MySqlCommand(sqlString, conn);
            using (MySqlDataReader reader = query.ExecuteReader())
            {
                return reader.HasRows;
            }
        }

        private void InsertStatement(string table, object model, MySqlCommand command)
        {
            //command.CommandText = "INSERT INTO pubg_match_list (matchId, mapName, date) Values (:matchId, :mapName, :date)";
            command.CommandText = string.Format("INSERT INTO {0} ({1}) Values ({2})", table, buildArgs(model), buildArgs(model, ":"));
            foreach (PropertyInfo property in model.GetType().GetProperties())
            {
                var value = property.GetValue(model, null);
                string propName = property.Name;
                command.Parameters.Add(propName, value);
            }
            command.ExecuteNonQuery();
            command.Parameters.Clear();
        }

        private string buildArgs(object model, string prefix = "")
        {
            List<string> returnStr = new List<string>();
            foreach (PropertyInfo property in model.GetType().GetProperties())
            {
                returnStr.Add(prefix + property.Name);
            }
            return String.Join(",", returnStr);
        }

        public void InsertMatch(MatchModel match, List<MatchParticipantModel> players, List<TelemetryModel> events)
        {
            MySqlTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            MySqlCommand myCommand = new MySqlCommand();
            myCommand.Connection = conn;
            myCommand.Transaction = transaction;

            try
            {
                Console.Write("Match.. ");
                InsertStatement("pubg_match_list", match, myCommand);

                Console.Write("Players.. ");
                foreach (MatchParticipantModel player in players)
                {
                    InsertStatement("pubg_match_participants", player, myCommand);
                }

                Console.Write("Telemetry.. ");
                foreach (TelemetryModel item in events)
                {
                    InsertStatement("pubg_match_telemetry", item, myCommand);
                }

                transaction.Commit();
            }
            catch(Exception e)
            {
                transaction.Rollback();
                Console.WriteLine(e.ToString());
                Console.Write(string.Format("[{0}] Abort! Abort! SQL Error!", DateTime.Now));

            }

            // Begin a transaction.
            // Insert the match to the pubg_match_list table (matchId, mapName, startTime, endTime)
            //                  (foreign key: matchId)
            // Insert all of the match participant data to pubg_match_participants table.
            //                  (foreign key: matchId)
            // Insert all of the log events into the pubg_match_telemetry table.
        }
    }
}
