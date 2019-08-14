using UserManagement.AL;
using UserManagement.BL.model;
using UserManagement.VL;

namespace UserManagement.BL.controller
{
    public class UserGroupController
    {
        private IRepository<UserGroup> _userGroupRepository;
        private View _view;
        private Read _read;

        public UserGroupController(IRepository<UserGroup> userGroupRepository, View view, Read read)
        {
            _userGroupRepository = userGroupRepository;
            _view = view;
            _read = read;
        }
    }
}