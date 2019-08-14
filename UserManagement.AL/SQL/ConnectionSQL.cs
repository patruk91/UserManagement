using System;
using Npgsql;

namespace UserManagement.AL.SQL
{
    public class ConnectionSql
    {
        public static NpgsqlConnection GetConnection()
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(
                    "Server=localhost;" +
                    "Port=5432;" +
                    "User Id=postgres;" +
                    "Password=postgres;" +
                    "Database=usermanagement;");
                connection.Open();
                return connection;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}