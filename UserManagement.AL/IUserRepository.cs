using System.Collections;
using UserManagement.M;

namespace UserManagement.AL
{
    public interface IUserRepository
    {
        IEnumerable GetAll();
        User Get(string id);
        bool Add(User user);
        bool Delete(User user);
        bool Edit(User user);
    }
}