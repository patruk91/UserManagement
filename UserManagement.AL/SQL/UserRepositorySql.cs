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
            using (NpgsqlConnection userConnection = ConnectionSql.GetConnection())
            {
                string userQuery = "SELECT * FROM users";
                using (NpgsqlCommand userCommand = new NpgsqlCommand(userQuery, userConnection))
                {
                    NpgsqlDataReader userReader = userCommand.ExecuteReader();
                    while (userReader.Read())
                    {
                        string login = userReader.GetString(userReader.GetOrdinal("login"));
                        string password = userReader.GetString(userReader.GetOrdinal("password"));
                        string firstName = userReader.GetString(userReader.GetOrdinal("first_name"));
                        string lastName = userReader.GetString(userReader.GetOrdinal("last_name"));
                        DateTime dateOfBirth = userReader.GetDateTime(userReader.GetOrdinal("date_of_birth"));

                        

                        User user = new User(login, password, firstName, lastName, dateOfBirth);
                        AddUserGroups(user);
                        users = users.Append(user);
                    }
                }
            }
            return users;
        }

        private static void AddUserGroups(User user)
        {
            using (NpgsqlConnection groupConnection = ConnectionSql.GetConnection())
            {
                string groupQuery = "SELECT * FROM list_users_and_groups WHERE login = @login";
                using (NpgsqlCommand groupCommand = new NpgsqlCommand(groupQuery, groupConnection))
                {
                    groupCommand.Parameters.Add("login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.Login;
                    groupCommand.Prepare();
                    NpgsqlDataReader groupReader = groupCommand.ExecuteReader();
                    while (groupReader.Read())
                    {
                        string groupName = groupReader.GetString(groupReader.GetOrdinal("group_name"));
                        user.UserGroup.Add(groupName);
                    }
                }
            }
        }

        public User Get(string userLogin)
        {
            using (NpgsqlConnection connection = ConnectionSql.GetConnection())
            {
                string userQuery = "SELECT * FROM users WHERE login = @login";
                using (NpgsqlCommand userCommand = new NpgsqlCommand(userQuery, connection))
                {
                    userCommand.Parameters.Add("login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = userLogin;
                    userCommand.Prepare();
                    NpgsqlDataReader userReader = userCommand.ExecuteReader();

                    while (userReader.Read())
                    {
                        string login = userReader.GetString(userReader.GetOrdinal("login"));
                        string password = userReader.GetString(userReader.GetOrdinal("password"));
                        string firstName = userReader.GetString(userReader.GetOrdinal("first_name"));
                        string lastName = userReader.GetString(userReader.GetOrdinal("last_name"));
                        DateTime dateOfBirth = userReader.GetDateTime(userReader.GetOrdinal("date_of_birth"));
                        User user = new User(login, password, firstName, lastName, dateOfBirth);
                        AddUserGroups(user);
                        return user;
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

        public bool Delete(string login)
        {
            bool success = true;
            using (NpgsqlConnection connection = ConnectionSql.GetConnection())
            {
                string query = "DELETE FROM users WHERE login = @login";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.Add("login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = login;
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

        public bool Edit(User user)
        {
            throw new NotImplementedException();
        }
    }
}
