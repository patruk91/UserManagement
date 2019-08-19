using System;
using System.Collections.Generic;
using UserManagement.M;

namespace UserManagement.VL
{
    public class View
    {
        public void DisplayMenu(string menu)
        {
            Console.WriteLine(menu);
        }

        public void DisplayError(string error)
        {
            Console.WriteLine($"ERROR: {error}!");
        }

        public void DisplayActionRequest(string message)
        {
            Console.Write($"{message}: ");

        }

        public void DisplayUsers(List<User> users)
        {
            int i = 1;
            foreach (User user in users)
            {
                Console.WriteLine(user);
            }
        }
    }
}