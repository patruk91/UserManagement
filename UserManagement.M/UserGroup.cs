using System.Collections.Generic;

namespace UserManagement.M
{
    public class UserGroup
    {
        public string GroupName { get; private set; }
        public List<string> UsersInGroup { get; private set; }

        public UserGroup(string groupName)
        {
            GroupName = groupName;
            UsersInGroup = new List<string>();
        }
    }
}