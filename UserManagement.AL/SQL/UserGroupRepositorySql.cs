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
            throw new System.NotImplementedException();
        }

        public OperationResult Delete(UserGroup userGroup)
        {
            throw new System.NotImplementedException();
        }

        public OperationResult Edit(UserGroup userGroup)
        {
            throw new System.NotImplementedException();
        }

        public UserGroup Get(string groupName)
        {
            using (NpgsqlConnection connection = ConnectionSql.GetConnection())
            {
                string groupQuery = "SELECT * FROM user_groups WHERE group_name = @gropuName";
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