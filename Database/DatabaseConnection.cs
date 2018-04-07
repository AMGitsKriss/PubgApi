using Devart.Data.MySql;
using System;
using System.Configuration;
using Database.Tables;

namespace Database
{
    public class DatabaseConnection
    {
        MySqlConnection conn;

        public UsersTable Users;

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
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(string.Format("{0} Error: {1}", ex.GetType(), ex.Message));
            }
        }
    }
}
