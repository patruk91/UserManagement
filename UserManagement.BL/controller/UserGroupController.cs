using UserManagement.AL;
using UserManagement.AL.SQL;
using UserManagement.VL;

namespace UserManagement.BL.controller
{
    public class UserGroupController
    {
        private IUserGroupRepository _userGroupUserRepository;
        private View _view;
        private Read _read;

        public UserGroupController(IUserGroupRepository userGroupUserRepository, View view, Read read)
        {
            _userGroupUserRepository = userGroupUserRepository;
            _view = view;
            _read = read;
        }
    }
}