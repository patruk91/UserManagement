using System.Collections.Generic;
using UserManagement.M;

namespace UserManagement.AL.SQL
{
    public interface IUserGroupRepository
    {
        List<UserGroup> GetAll();
        UserGroup Get(string groupName);
        OperationResult Add(UserGroup userGroup);
        OperationResult Delete(string groupName);
        OperationResult Edit(string newGroupName, string oldGroupName);
    }
}