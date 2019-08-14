using System;
using System.Collections.Generic;
using UserManagement.AL;
using UserManagement.AL.SQL;
using UserManagement.M;
using UserManagement.VL;

namespace UserManagement.BL.controller
{
    public class Controller
    {
        private UserController _userController;
        private UserGroupController _userGroupController;
        private View _view;
        private Read _read;

        public Controller(UserController userController, UserGroupController userGroupController, View view, Read read)
        {
            _userController = userController;
            _userGroupController = userGroupController;
            _view = view;
            _read = read;
        }

        public void run()
        {
            IUserRepository repository = new UserRepositorySql();
            IEnumerable<User> users = repository.GetAll();
            foreach (User user in users)
            {
                Console.WriteLine(user.Login);
            }
        }
    }
}