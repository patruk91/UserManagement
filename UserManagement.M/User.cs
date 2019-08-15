using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UserManagement.M
{
    public class User
    {
        public string Login { get; private set; }
        public string Password { get; set; }
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
            DateTime birthDate) : this(login, password)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Login: {Login}; Password: {Password}\n" +
                   $"First Name: {FirstName}; Last Name: {LastName} " +
                   $"Birth Date: {BirthDate:dd/MM/yyyy}\n" +
                   $"Groups:\n");
            foreach(string group in UserGroup)
            {
                sb.Append($"    {group}\n");
            }
            return sb.ToString();
        }
    }
}
