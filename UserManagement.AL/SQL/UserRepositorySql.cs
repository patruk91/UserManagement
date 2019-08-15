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
                        User user = getUser(userReader);

                        PopulateUserGroups(user);
                        users = users.Append(user);
                    }
                }
            }
            return users;
        }

        private static User getUser(NpgsqlDataReader userReader)
        {
            string login = userReader.GetString(userReader.GetOrdinal("login"));
            string password = userReader.GetString(userReader.GetOrdinal("password"));
            string firstName = userReader.GetString(userReader.GetOrdinal("first_name"));
            string lastName = userReader.GetString(userReader.GetOrdinal("last_name"));
            DateTime dateOfBirth = userReader.GetDateTime(userReader.GetOrdinal("date_of_birth"));

            return new User(login, password, firstName, lastName, dateOfBirth);
        }

        private static void PopulateUserGroups(User user)
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
                        User user = getUser(userReader);
                        PopulateUserGroups(user);
                        return user;
                    }
                }
            }
            throw new ArgumentException("Invalid user login");
        }

        public bool Add(User user)
        {
            bool success = true;
            using (NpgsqlConnection userConnection = ConnectionSql.GetConnection())
            {
                string query = "INSERT INTO users (login, password, first_name, last_name, date_of_birth) " +
                               "VALUES (@login, @password, @first_name, @last_name, @date_of_birth)";
                using (NpgsqlCommand userCommand = new NpgsqlCommand(query, userConnection))
                {
                    userCommand.Parameters.Add("login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.Login;
                    userCommand.Parameters.Add("password", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.Password;
                    userCommand.Parameters.Add("first_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.FirstName;
                    userCommand.Parameters.Add("last_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.FirstName;
                    userCommand.Parameters.Add("date_of_birth", NpgsqlTypes.NpgsqlDbType.Date).Value = user.BirthDate;
                    userCommand.Prepare();

                    try
                    {
                        userCommand.ExecuteNonQuery();
                    }
                    catch (NpgsqlException e)
                    {
                        success = false;
                        Console.WriteLine(e);
                    }

                    success = InsertUserGroups(user, success);
                }
            }
            return success;
        }

        private static bool InsertUserGroups(User user, bool success)
        {
            foreach (string group in user.UserGroup)
            {
                using (NpgsqlConnection groupConnection = ConnectionSql.GetConnection())
                {
                    string groupQuery = "INSERT INTO list_users_and_groups (login, group_name) VALUES (@login, @group_name)";
                    using (NpgsqlCommand groupCommand = new NpgsqlCommand(groupQuery, groupConnection))
                    {
                        groupCommand.Parameters.Add("login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.Login;
                        groupCommand.Parameters.Add("group_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = @group;
                        groupCommand.Prepare();

                        try
                        {
                            groupCommand.ExecuteNonQuery();
                        }
                        catch (NpgsqlException e)
                        {
                            success = false;
                            Console.WriteLine(e);
                        }
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
            bool success = true;
            DeleteUserGroups(user.Login);
            using (NpgsqlConnection connection = ConnectionSql.GetConnection())
            {
                string userQuery = "UPDATE users SET password = @password, " +
                                                       "first_name = @first_name, " +
                                                       "last_name = @last_name, " +
                                                       "date_of_birth = @date_of_birth " +
                                                       "WHERE login = @login";
                using (NpgsqlCommand command = new NpgsqlCommand(userQuery, connection))
                {
                    command.Parameters.Add("password", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.Password;
                    command.Parameters.Add("first_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.FirstName;
                    command.Parameters.Add("last_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.FirstName;
                    command.Parameters.Add("date_of_birth", NpgsqlTypes.NpgsqlDbType.Date).Value = user.BirthDate;
                    command.Parameters.Add("login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.Login;
                    command.Prepare();

                    success = InsertUserGroups(user, success);
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

        private void DeleteUserGroups(string login)
        {
            bool success = true;
            using (NpgsqlConnection connection = ConnectionSql.GetConnection())
            {
                string query = "DELETE FROM list_users_and_groups WHERE login = @login";
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
        }
    }
}
