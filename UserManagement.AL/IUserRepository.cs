﻿using System.Collections.Generic;
using UserManagement.M;

namespace UserManagement.AL
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User Get(string id);
        bool Add(User user);
        bool Delete(string login);
        bool Edit(User user);
    }
}