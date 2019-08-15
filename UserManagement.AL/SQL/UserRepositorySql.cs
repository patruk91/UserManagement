using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;
using UserManagement.M;

namespace UserManagement.AL.SQL
{
    public class UserRepositorySql : IUserRepository
    {
        public List<User> GetAll()
        {
            List<User> users = new List<User>();
            using (NpgsqlConnection userConnection = ConnectionSql.GetConnection())
            {
                string userQuery = "SELECT * FROM users";
                using (NpgsqlCommand userCommand = new NpgsqlCommand(userQuery, userConnection))
                {
                    PopulateListOfAllUsers(users, userCommand);
                }
            }
            return users;
        }

        private static void PopulateListOfAllUsers(List<User> users, NpgsqlCommand userCommand)
        {
            NpgsqlDataReader userReader = userCommand.ExecuteReader();
            while (userReader.Read())
            {
                User user = GetUserDataFromQuery(userReader);
                PopulateListOfUserGroups(user);
                users.Add(user);
            }
        }

        private static User GetUserDataFromQuery(NpgsqlDataReader userReader)
        {
            string login = userReader.GetString(userReader.GetOrdinal("login"));
            string password = userReader.GetString(userReader.GetOrdinal("password"));
            string firstName = userReader.GetString(userReader.GetOrdinal("first_name"));
            string lastName = userReader.GetString(userReader.GetOrdinal("last_name"));
            DateTime dateOfBirth = userReader.GetDateTime(userReader.GetOrdinal("date_of_birth"));

            return new User(login, password, firstName, lastName, dateOfBirth);
        }

        private static void PopulateListOfUserGroups(User user)
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
                        User user = GetUserDataFromQuery(userReader);
                        PopulateListOfUserGroups(user);
                        return user;
                    }
                }
            }
            throw new ArgumentException("Invalid user login");
        }

        public OperationResult Add(User user)
        {
            OperationResult operationResult = new OperationResult
            {
                Succes = true
            };
            using (NpgsqlConnection userConnection = ConnectionSql.GetConnection())
            {
                string query = "INSERT INTO users (login, password, first_name, last_name, date_of_birth) " +
                               "VALUES (@login, @password, @first_name, @last_name, @date_of_birth)";
                using (NpgsqlCommand userCommand = new NpgsqlCommand(query, userConnection))
                {
                    PrepareUserCommand(user, userCommand);

                    try
                    {
                        userCommand.ExecuteNonQuery();
                    }
                    catch (NpgsqlException e)
                    {
                        operationResult.Succes = false;
                        operationResult.Messages.Add(e..Message);
                    }

                    InsertUserGroups(user, operationResult);
                }
            }
            return operationResult;
        }

        private static void PrepareUserCommand(User user, NpgsqlCommand command)
        {
            command.Parameters.Add("login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.Login;
            command.Parameters.Add("password", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.Password;
            command.Parameters.Add("first_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.FirstName;
            command.Parameters.Add("last_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.FirstName;
            command.Parameters.Add("date_of_birth", NpgsqlTypes.NpgsqlDbType.Date).Value = user.BirthDate;
            command.Prepare();
        }

        private static void InsertUserGroups(User user, OperationResult operationResult)
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
                            operationResult.Succes = false;
                            operationResult.Messages.Add(e..Message);
                        }
                    }
                }
            }
        }

        public OperationResult Delete(string login)
        {
            OperationResult operationResult = new OperationResult()
            {
                Succes = true
            };
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
                        operationResult.Succes = false;
                        operationResult.Messages.Add(e..Message);
                    }
                }
            }
            return operationResult;
        }

        public OperationResult Edit(User user)
        {
            OperationResult operationResult = new OperationResult {Succes = true};
            DeleteUserGroups(user.Login, operationResult);
            using (NpgsqlConnection connection = ConnectionSql.GetConnection())
            {
                string userQuery = "UPDATE users SET password = @password, " +
                                                       "first_name = @first_name, " +
                                                       "last_name = @last_name, " +
                                                       "date_of_birth = @date_of_birth " +
                                                       "WHERE login = @login";
                using (NpgsqlCommand command = new NpgsqlCommand(userQuery, connection))
                {
                    PrepareUserCommand(user, command);

                    InsertUserGroups(user, operationResult);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (NpgsqlException e)
                    {
                        operationResult.Succes = false;
                        operationResult.Messages.Add(e..Message);
                    }
                }
            }
            return operationResult;
        }

        private void DeleteUserGroups(string login, OperationResult operationResult)
        {
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
                        operationResult.Succes = false;
                        operationResult.Messages.Add(e.Message);
                    }
                }
            }
        }
    }
}
