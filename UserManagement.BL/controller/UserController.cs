using UserManagement.AL;
using UserManagement.VL;

namespace UserManagement.BL.controller
{
    public class UserController
    {
        private IUserRepository _userUserRepository;
        private View _view;
        private Read _read;

        public UserController(IUserRepository userUserRepository,
                                View view,
                                Read read)
        {
            _userUserRepository = userUserRepository;
            _view = view;
            _read = read;
        }
    }
}