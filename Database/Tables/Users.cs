using Devart.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Models;
using System.Reflection;

namespace Database.Tables
{
    public class UsersTable
    {
        MySqlConnection conn;

        public UsersTable(MySqlConnection conn)
        {
            this.conn = conn;
        }

        public string All()
        {
            return "Users table data!";
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
                    user.pubg_username = reader.GetValue(0).ToString();
                    user.pubg_user_id = reader.GetValue(1).ToString();
                    userList.Add(user);

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var value = reader.GetValue(i);
                        var propName = reader.GetName(i);
                        PropertyInfo propertyInfo = user.GetType().GetProperty(propName);
                        propertyInfo.SetValue(user, value);
                    }
                }
                return userList;
            }
        }
    }
}
