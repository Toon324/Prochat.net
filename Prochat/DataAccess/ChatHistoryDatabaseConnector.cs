using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Prochat.Models;

namespace Prochat.DataAccess
{
    public class ChatHistoryDatabaseConnector
    {
        static readonly SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ProchatDatabase"].ToString());

        public static List<ChatHistoryModel> GetHistory(string group, string room)
        {
            try
            {
                var groupId = GroupsDatabaseConnector.GetGroupId(group);

                if (groupId == -1)
                    return new List<ChatHistoryModel>();

                var roomId = GroupsDatabaseConnector.GetRoomId(groupId, room);

                if (roomId == -1)
                    return new List<ChatHistoryModel>();

                var query = string.Format("SELECT * FROM [ChatHistory] WHERE Room = {0}", roomId);
                var cmd = new SqlCommand(query, Connection);

                Connection.Open();
                var reader = cmd.ExecuteReader();


                var output = new List<ChatHistoryModel>();

                while (reader.Read())
                {
                    output.Add(new ChatHistoryModel()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        RoomId = Convert.ToInt32(reader["Room"]),
                        Username = reader["User"].ToString(),
                        Message = reader["Message"].ToString()
                    });
                }

                Connection.Close();

                return output;
            }
            catch
            {
                Connection.Close();
                return new List<ChatHistoryModel>();
            }
        }

        public static void AddToHistory(string group, string room, string user, string message)
        {
            try
            {
                var groupId = GroupsDatabaseConnector.GetGroupId(group);
                if (groupId == -1)
                    return;

                var roomId = GroupsDatabaseConnector.GetRoomId(groupId, room);

                if (roomId == -1)
                    return;

                var query = string.Format("INSERT INTO [ChatHistory] VALUES ({0}, '{1}', '{2}')", roomId, user, message); //TODO: Relational group-room history storage.
                //System.IO.File.WriteAllText("C:/Users/Cody/Programming/ASP/output.txt", query);
                var cmd = new SqlCommand(query, Connection);

                Connection.Open();
                cmd.ExecuteReader();
                Connection.Close();
            }
            catch
            {
                Connection.Close();
            }
        }
    }
}