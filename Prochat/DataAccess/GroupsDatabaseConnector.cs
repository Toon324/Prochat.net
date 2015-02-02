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

        /// <summary>
        /// Checks to see if the groupname returns a valid group.
        /// </summary>
        public static bool IsGroupNameValid(string groupName)
        {
            //Since a -1 will be returned if no group with that name is found, we use it for the check
            return GetGroupId(groupName) != -1;
        }


        private static int GetGroupId(string groupName)
        {
            var query = string.Format("SELECT * FROM [Groups] WHERE Name = '{0}'", groupName);
            var cmd = new SqlCommand(query, Connection);

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

            return -1;
        }
    }
}