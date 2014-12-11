using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Prochat.DataAccess
{
    public class UserDatabaseConnector
    {
        static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["UsersDatabase"].ToString());

        public static bool UserIsValid(string username, string password)
        {
            var authenticated = false;

            var query = string.Format("SELECT * FROM [Users] WHERE Username = '{0}' AND Password = '{1}'", username, password); //Look for the user with the entered password
            var cmd = new SqlCommand(query, connection);

            connection.Open();
            var reader = cmd.ExecuteReader();

            authenticated = reader.HasRows; //Record was indeed returned
            connection.Close();

            return authenticated;
        }

        public static void RegisterUser(string username, string password)
        {
            var query = string.Format("INSERT INTO [Users] (Username, Password) VALUES ('{0}','{1}')", username, password); //Look for the user with the entered password
            var cmd = new SqlCommand(query, connection);

            connection.Open();
            var reader = cmd.ExecuteReader();

            connection.Close();
        }
    }
}