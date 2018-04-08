using Devart.Data.MySql;
using System;
using System.Configuration;
using Database.Tables;
using Database.Models;
using System.Collections.Generic;

namespace Database
{
    public class DatabaseConnection
    {
        MySqlConnection conn;

        public UsersTable Users;
        public MatchParticipantsTable MatchParticipants;

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
                Users = new UsersTable(conn);
                MatchParticipants = new MatchParticipantsTable(conn);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(string.Format("{0} Error: {1}", ex.GetType(), ex.Message));
            }
        }

        public void InsertMatch(List<MatchParticipantModel> players, List<TelemetryModel> events)
        {
            // Begin a transaction.
            // Insert the match to the pubg_match_list table (matchId, mapName, startTime, endTime)
            //                  (foreign key: matchId)
            // Insert all of the match participant data to pubg_match_participants table.
            //                  (foreign key: matchId)
            // Insert all of the log events into the pubg_match_telemetry table.
            throw new NotImplementedException();
        }
    }
}
