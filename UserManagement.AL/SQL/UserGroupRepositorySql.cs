using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using UserManagement.M;

namespace UserManagement.AL.SQL
{
    public class UserGroupRepositorySql : IUserGroupRepository
    {
        public OperationResult Add(UserGroup userGroup)
        {
            OperationResult operationResult = new OperationResult
            {
                Succes = true
            };
            using (NpgsqlConnection groupConnection = ConnectionSql.GetConnection())
            {
                string query = "INSERT INTO user_groups (group_name) " +
                               "VALUES (@groupName)";
                using (NpgsqlCommand groupCommand = new NpgsqlCommand(query, groupConnection))
                {
                    PrepareGroupCommand(userGroup, groupCommand);

                    try
                    {
                        groupCommand.ExecuteNonQuery();
                    }
                    catch (NpgsqlException e)
                    {
                        operationResult.Succes = false;
                        operationResult.Messages.Add(e.Message);
                    }

                    InsertUsersLogins(userGroup, operationResult);
                }
            }
            return operationResult;
        }

        private static void PrepareGroupCommand(UserGroup userGroup, NpgsqlCommand command)
        {
            command.Parameters.Add("groupName", NpgsqlTypes.NpgsqlDbType.Varchar).Value = userGroup.GroupName;
            command.Prepare();
        }

        private static void InsertUsersLogins(UserGroup userGroup, OperationResult operationResult)
        {
            foreach (string userLogin in userGroup.UsersInGroup)
            {
                using (NpgsqlConnection groupConnection = ConnectionSql.GetConnection())
                {
                    string groupQuery = "INSERT INTO list_users_and_groups (login, group_name) VALUES (@login, @group_name)";
                    using (NpgsqlCommand groupCommand = new NpgsqlCommand(groupQuery, groupConnection))
                    {
                        groupCommand.Parameters.Add("login", NpgsqlTypes.NpgsqlDbType.Varchar).Value = userLogin;
                        groupCommand.Parameters.Add("group_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = userGroup.GroupName;
                        groupCommand.Prepare();

                        try
                        {
                            groupCommand.ExecuteNonQuery();
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

        public OperationResult Delete(string groupName)
        {
            OperationResult operationResult = new OperationResult()
            {
                Succes = true
            };
            using (NpgsqlConnection connection = ConnectionSql.GetConnection())
            {
                string query = "DELETE FROM user_groups WHERE group_name = @groupName";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.Add("groupName", NpgsqlTypes.NpgsqlDbType.Varchar).Value = groupName;
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
            return operationResult;
        }

        public OperationResult Edit(UserGroup userGroup)
        {
            throw new System.NotImplementedException();
        }

        public UserGroup Get(string groupName)
        {
            using (NpgsqlConnection connection = ConnectionSql.GetConnection())
            {
                string groupQuery = "SELECT * FROM user_groups WHERE group_name = @groupName";
                using (NpgsqlCommand groupCommand = new NpgsqlCommand(groupQuery, connection))
                {
                    groupCommand.Parameters.Add("groupName", NpgsqlTypes.NpgsqlDbType.Varchar).Value = groupName;
                    groupCommand.Prepare();
                    NpgsqlDataReader groupReader = groupCommand.ExecuteReader();

                    while (groupReader.Read())
                    {
                        UserGroup userGroup = GetGrouprDataFromQuery(groupReader);
                        PopulateListOfUsersInGroups(userGroup);
                        return userGroup;
                    }
                }
            }
            throw new ArgumentException("Invalid group name");
        }

        public List<UserGroup> GetAll()
        {
            List<UserGroup> groups = new List<UserGroup>();
            using (NpgsqlConnection userConnection = ConnectionSql.GetConnection())
            {
                string groupsQuery = "SELECT * FROM user_groups";
                using (NpgsqlCommand groupsCommand = new NpgsqlCommand(groupsQuery, userConnection))
                {
                    PopulateListOfAllGroups(groups, groupsCommand);
                }
            }
            return groups;
        }

        private static void PopulateListOfAllGroups(List<UserGroup> users, NpgsqlCommand groupsCommand)
        {
            NpgsqlDataReader groupsReader = groupsCommand.ExecuteReader();
            while (groupsReader.Read())
            {
                UserGroup userGroup = GetGrouprDataFromQuery(groupsReader);
                PopulateListOfUsersInGroups(userGroup);
                users.Add(userGroup);
            }
        }

        private static UserGroup GetGrouprDataFromQuery(NpgsqlDataReader userReader)
        {
            string groupName = userReader.GetString(userReader.GetOrdinal("group_name"));

            return new UserGroup(groupName);
        }

        private static void PopulateListOfUsersInGroups(UserGroup userGroup)
        {
            using (NpgsqlConnection groupConnection = ConnectionSql.GetConnection())
            {
                string groupQuery = "SELECT * FROM list_users_and_groups WHERE group_name = @userGroup";
                using (NpgsqlCommand groupCommand = new NpgsqlCommand(groupQuery, groupConnection))
                {
                    groupCommand.Parameters.Add("userGroup", NpgsqlTypes.NpgsqlDbType.Varchar).Value = userGroup.GroupName;
                    groupCommand.Prepare();
                    NpgsqlDataReader groupReader = groupCommand.ExecuteReader();
                    while (groupReader.Read())
                    {
                        string login = groupReader.GetString(groupReader.GetOrdinal("login"));
                        userGroup.UsersInGroup.Add(login);
                    }
                }
            }
        }
    }
}