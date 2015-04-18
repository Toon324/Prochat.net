using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Prochat.DataAccess
{
    public class GroupsDatabaseConnector
    {
        static readonly SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ProchatDatabase"].ToString());

        public static List<string> GetListOfRoomsInGroup(string groupName)
        {
            try
            {
                var parentId = GetGroupId(groupName);
                var query = string.Format("SELECT * FROM [Rooms] WHERE ParentId = '{0}'", parentId);
                var cmd = new SqlCommand(query, Connection);

                Connection.Open();

                var output = new List<string>();

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    output.Add(reader["Name"].ToString());

                Connection.Close();

                return output;
            }
            catch
            {
                Connection.Close();
                return new List<string>();
            }
        }

        /// <summary>
        /// Checks to see if the groupname returns a valid group.
        /// </summary>
        public static bool IsGroupNameValid(string groupName)
        {
            //Since a -1 will be returned if no group with that name is found, we use it for the check
            return GetGroupId(groupName) != -1;
        }


        public static int GetGroupId(string groupName)
        {
            var query = string.Format("SELECT * FROM [Groups] WHERE Name = '{0}'", groupName);
            var cmd = new SqlCommand(query, Connection);

            // Connection.Close();
            Connection.Open();

            var reader = cmd.ExecuteReader();

            //If no Group is found, return a -1 to represent that
            if (!reader.HasRows)
            {
                Connection.Close();
                return -1;
            }

            while (reader.Read())
            {
                var toReturn = Convert.ToInt32(reader["Id"]);
                Connection.Close();
                return toReturn;
            }

            Connection.Close();
            return -1;
        }

        public static int GetRoomId(int groupId, string room)
        {
            var query = string.Format("SELECT * FROM [Rooms] WHERE ParentId = {0} AND Name = '{1}'", groupId, room);
            var cmd = new SqlCommand(query, Connection);

            Connection.Open();

            var reader = cmd.ExecuteReader();

            //If no Room is found, return a -1 to represent that
            if (!reader.HasRows)
            {
                Connection.Close();
                return -1;
            }

            while (reader.Read())
            {
                var toReturn = Convert.ToInt32(reader["Id"]);
                Connection.Close();
                return toReturn;
            }

            return -1;
        }

        public static int GetGroupIdOfRoom(int roomId)
        {
            var query = string.Format("SELECT ParentId FROM [Rooms] WHERE Id = {0}", roomId);
            var cmd = new SqlCommand(query, Connection);

            Connection.Open();

            var reader = cmd.ExecuteReader();

            //If no Id is found, return a -1 to represent that
            if (!reader.HasRows)
            {
                Connection.Close();
                return -1;
            }

            while (reader.Read())
            {
                var toReturn = Convert.ToInt32(reader["ParentId"]);
                Connection.Close();
                return toReturn;
            }

            return -1;
        }

        public static string GetGroupNameById(int groupId)
        {
            var query = string.Format("SELECT Name FROM [Groups] WHERE Id = {0}", groupId);
            var cmd = new SqlCommand(query, Connection);

            Connection.Open();

            var reader = cmd.ExecuteReader();

            //If no Id is found, return a -1 to represent that
            if (!reader.HasRows)
            {
                Connection.Close();
                return "";
            }

            while (reader.Read())
            {
                var toReturn = reader["Name"].ToString();
                Connection.Close();
                return toReturn.Trim();
            }

            return "";
        }

        public static string GetRoomNameById(int roomId)
        {
            var query = string.Format("SELECT Name FROM [Rooms] WHERE Id = {0}", roomId);
            var cmd = new SqlCommand(query, Connection);

            Connection.Open();

            var reader = cmd.ExecuteReader();

            //If no Id is found, return a -1 to represent that
            if (!reader.HasRows)
            {
                Connection.Close();
                return "";
            }

            while (reader.Read())
            {
                var toReturn = reader["Name"].ToString();
                Connection.Close();
                return toReturn.Trim();
            }

            return "";
        }

    }
}