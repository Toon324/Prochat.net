using System.Configuration;
using System.Data.SqlClient;

namespace Prochat.DataAccess
{
    public class UserDatabaseConnector
    {
        static readonly SqlConnection Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ProchatDatabase"].ToString());

        public static bool UserIsValid(string username, string password)
        {
            var query = string.Format("SELECT * FROM [Users] WHERE Username = '{0}' AND Password = '{1}'", username, password); //Look for the user with the entered password
            var cmd = new SqlCommand(query, Connection);

            Connection.Open();
            var reader = cmd.ExecuteReader();

            var authenticated = reader.HasRows;
            Connection.Close();

            return authenticated;
        }

        public static void RegisterUser(string username, string password)
        {
            var query = string.Format("INSERT INTO [Users] (Username, Password) VALUES ('{0}','{1}')", username, password); 
            var cmd = new SqlCommand(query, Connection);

            Connection.Open();
            cmd.ExecuteReader();

            Connection.Close();
        }
    }
}