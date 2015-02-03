using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Prochat.DataAccess
{
    public class ChatHistoryDatabaseConnector
    {
        static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ProchatDatabase"].ToString());

        public static List<string> GetHistory(string room)
        {
            var query = string.Format("SELECT * FROM [ChatHistory] WHERE Room = '{0}'", room);
            var cmd = new SqlCommand(query, connection);        

            connection.Open();
            var reader = cmd.ExecuteReader();
  
            List<string> output = new List<string>();

            while (reader.Read()) {
                output.Add(reader["User"].ToString());
                output.Add(reader["Message"].ToString());
            }
   

            connection.Close();
            
            return output;
        }

        public static void AddToHistory(string group, string room, string user, string message)
        {
            var query = string.Format("INSERT INTO [ChatHistory] VALUES ('{0}', '{1}', '{2}')", room, user, message); //TODO: Setup relational group-room id's for storing history.
            var cmd = new SqlCommand(query, connection);

            connection.Open();
            var reader = cmd.ExecuteReader();

            connection.Close();
        }
    }
}