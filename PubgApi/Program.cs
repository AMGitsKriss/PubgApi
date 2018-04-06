using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using Database.Models;

namespace PubgApi
{
    class Program
    {
        const string region = "pc-eu";
        string playersUri = "https://api.playbattlegrounds.com/shards/" + region + "/players?filter[playerNames]=";

        static void Main(string[] args)
        {
            DatabaseConnection db = new DatabaseConnection();
            List<PubgUserModel> userList = db.Users.PubgUsers();

            Console.WriteLine("Press ESC to stop...");
            do
            {
                while (!Console.KeyAvailable)
                {
                    // Do something
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}
