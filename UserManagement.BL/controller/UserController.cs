﻿using System;
using System.Collections.Generic;
using System.IO;
using UserManagement.AL;
using UserManagement.M;
using UserManagement.VL;

namespace UserManagement.BL.controller
{
    public class UserController
    {
        private IUserRepository _userRepository;
        private View _view;
        private Read _read;

        public UserController(IUserRepository userRepository,
                                View view,
                                Read read)
        {
            _userRepository = userRepository;
            _view = view;
            _read = read;
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public User GetDataForNewUser(StringReader stringReader)
        {
            _read.ChangeConsoleInput(stringReader);
            _view.DisplayActionRequest("Please, provide data for new user.");
            string login = GetUserLogin("Enter user login");
            string password = _read.GetUserAnswer("Enter user password");
            string firstName = _read.GetUserAnswer("Enter user first name");
            string lastName = _read.GetUserAnswer("Enter user last name");
            DateTime birthDate = _read.GetBirthDate("Enter user birthday");


            return new User(login, password, firstName, lastName, birthDate);


        }

        private string GetUserLogin(string enterUserLogin)
        {
            throw new NotImplementedException();
        }
    }
}