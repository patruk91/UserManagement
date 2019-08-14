using System;
using UserManagement.AL;
using UserManagement.VL;

namespace UserManagement.BL
{
    class App
    {
        static void Main(string[] args)
        {
            View view = new View();
            Read read = new Read(view);

            IRepository<User> userRepository = new RepositorySQL<User>();
            IRepository<UserGroup> userGroupRepository = new RepositorySQL<UserGroup>();

            UserGroupController userGroupController = new UserGroupController(userGroupRepository, view, read);
            UserController userController = new UserController(userRepository, view, read);
            Controller controller = new Controller(userController, userGroupController, view, read);

        }
    }
}
