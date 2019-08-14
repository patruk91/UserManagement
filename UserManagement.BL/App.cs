using UserManagement.AL;
using UserManagement.AL.SQL;
using UserManagement.VL;
using UserManagement.BL.controller;

namespace UserManagement.BL
{
    class App
    {
        static void Main(string[] args)
        {
            View view = new View();
            Read read = new Read(view);

            IUserRepository userUserRepository = new UserRepositorySql();
            IUserGroupRepository userGroupUserRepository = new UserGroupRepositorySql();

            UserGroupController userGroupController = new UserGroupController(userGroupUserRepository, view, read);
            UserController userController = new UserController(userUserRepository, view, read);
            Controller controller = new Controller(userController, userGroupController, view, read);

        }
    }
}
