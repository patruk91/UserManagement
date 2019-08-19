using System;
using System.Collections.Generic;
using System.IO;
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
        StringReader x = new StringReader(Console.ReadLine());


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

        private void HandleUsers()
        {
            bool backToMain = false;
            while (!backToMain)
            {
                string userMenu = "1. Display all users\n" +
                                  "2. Add user\n" +
                                  "3. Remove user\n" +
                                  "4. Edit user\n" +
                                  "5. Back to main menu";
                _view.DisplayMenu(userMenu);
                _view.DisplayActionRequest("Choose option");
                int userMenuOption = _read.GetNumberFromString();

                switch (userMenuOption)
                {
                    case 1:
                        List<User> users = _userController.GetAllUsers();
                        _view.DisplayUsers(users);
                        break;
                    case 2:
                        User user = null;
                        using (StringReader sr = new StringReader(""))
                        {
                            user = _userController.GetDataForNewUser(sr);
                        }
                        //_userController.AddUserToRepository();
                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    case 5:
                        backToMain = true;
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


    }
}