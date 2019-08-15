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
            bool exit = false;
            while (!exit)
            {
                string mainMenu = "1. Manage users\n" +
                                  "2. Manage group of users\n" +
                                  "3. Exit";
                _view.DisplayMenu(mainMenu);
                _view.DisplayActionRequest("Choose option");
                int mainMenuOption = _read.GetNumberFromString();

                switch (mainMenuOption)
                {
                    case 1:
                        HandleUsers();
                        break;
                    case 2:
                        HandleGroupUsers();
                        break;
                    case 3:
                        exit = true;
                        break;
                    default:
                        _view.DisplayError("Invalid option");
                        break;
                }

            }
        }

        private void HandleGroupUsers()
        {
            throw new NotImplementedException();
        }

        private void HandleUsers()
        {
            throw new NotImplementedException();
        }
    }
}