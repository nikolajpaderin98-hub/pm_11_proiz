using System.Data.SqlClient;

namespace SchoolClubsApp.Database
{
    public static class DatabaseHelper
    {
        private static string connectionString =
            @"Server=pc\SQLEXPRESS;
              Database=SchoolClubsDB;
              Trusted_Connection=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}