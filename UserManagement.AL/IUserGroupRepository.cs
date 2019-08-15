using System.Collections.Generic;
using UserManagement.M;

namespace UserManagement.AL.SQL
{
    public interface IUserGroupRepository
    {
        List<UserGroup> GetAll();
        UserGroup Get(string groupName);
        OperationResult Add(UserGroup userGroup);
        OperationResult Delete(UserGroup userGroup);
        OperationResult Edit(UserGroup userGroup);
    }
}