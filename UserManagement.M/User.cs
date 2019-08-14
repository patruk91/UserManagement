using System;
using System.Collections.Generic;

namespace UserManagement.M
{
    public class User
    {
        public string Login { get; private set; }
        public string Password { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<string> UserGroup { get; private set; }

        public User(string login, string password)
        {
            this.Login = login;
            this.Password = password;
            UserGroup = new List<string>();
        }

        public User(string login,
            string password,
            string firstName,
            string lastName,
            DateTime birthDate,
            List<string> userGroup) : this(login, password)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            UserGroup = userGroup;
        }
    }
}
