using UserManagement.AL;
using UserManagement.BL.model;
using UserManagement.VL;

namespace UserManagement.BL.controller
{
    public class UserController
    {
        private IRepository<User> _userRepository;
        private View _view;
        private Read _read;

        public UserController(IRepository<User> userRepository,
                                View view,
                                Read read)
        {
            _userRepository = userRepository;
            _view = view;
            _read = read;
        }
    }
}