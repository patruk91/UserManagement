using UserManagement.VL;

namespace UserManagement.BL
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
    }
}