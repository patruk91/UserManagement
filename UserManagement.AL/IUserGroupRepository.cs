using System.Collections;
using UserManagement.M;

namespace UserManagement.AL.SQL
{
    public interface IUserGroupRepository
    {
        IEnumerable GetAll();
        UserGroup Get(string id);
        bool Add(UserGroup userGroup);
        bool Delete(UserGroup userGroup);
        bool Edit(UserGroup userGroup);
    }
}