using System.Collections.Generic;
using UserManagement.M;

namespace UserManagement.AL
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User Get(string id);
        OperationResult Add(User user);
        OperationResult Delete(string login);
        OperationResult Edit(User user);
        bool IsLoginUnique(string userLogin);
    }
}