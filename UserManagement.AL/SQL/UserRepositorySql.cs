using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using UserManagement.M;

namespace UserManagement.AL.SQL
{
    public class UserRepositorySql : IUserRepository
    {
        public IEnumerable<User> GetAll()
        {
            IEnumerable<User> users = new List<User>();
            using (NpgsqlConnection connection = ConnectionSql.GetConnection())
            {
                string query = "SELECT * FROM users";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string login = reader.GetString(reader.GetOrdinal("login"));
                        string password = reader.GetString(reader.GetOrdinal("password"));
                        string firstName = reader.GetString(reader.GetOrdinal("first_name"));
                        string lastName = reader.GetString(reader.GetOrdinal("last_name"));
                        DateTime dateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth"));

                        User user = new User(login, password, firstName, lastName, dateOfBirth);
                        users = users.Append(user);
                    }
                }
            }
            return users;
        }

        public User Get(string userLogin)
        {
            using (NpgsqlConnection connection = ConnectionSql.GetConnection())
            {
                string query = "SELECT * FROM users WHERE login = @login";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.Add("login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = userLogin;
                    command.Prepare();
                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string login = reader.GetString(reader.GetOrdinal("login"));
                        string password = reader.GetString(reader.GetOrdinal("password"));
                        string firstName = reader.GetString(reader.GetOrdinal("first_name"));
                        string lastName = reader.GetString(reader.GetOrdinal("last_name"));
                        DateTime dateOfBirth = reader.GetDateTime(reader.GetOrdinal("date_of_birth"));

                        return new User(login, password, firstName, lastName, dateOfBirth);
                    }
                }
            }
            throw new ArgumentException("Invalid user login");
        }

        public bool Add(User user)
        {
            bool success = true;
            using (NpgsqlConnection connection = ConnectionSql.GetConnection())
            {
                string query = "INSERT INTO users (login, password, first_name, last_name, date_of_birth) " +
                               "VALUES (@login, @password, @first_name, @last_name, @date_of_birth)";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.Add("login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.Login;
                    command.Parameters.Add("password", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.Password;
                    command.Parameters.Add("first_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.FirstName;
                    command.Parameters.Add("last_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.FirstName;
                    command.Parameters.Add("date_of_birth", NpgsqlTypes.NpgsqlDbType.Date).Value = user.BirthDate;
                    command.Prepare();
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (NpgsqlException e)
                    {
                        success = false;
                        Console.WriteLine(e);
                    }
                }
            }
            return success;
        }

        public bool Delete(User user)
        {
            throw new NotImplementedException();
        }

        public bool Edit(User user)
        {
            throw new NotImplementedException();
        }
    }
}
