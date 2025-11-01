using ModelsLayer;

namespace Demo.ViewModels
{
    public class UserRoleVM
    {
        public User User { get; set; }

        public List<Role> RolesToDelete { get; set; } = new List<Role>();
        public List<Role> RolesToAdd { get; set; } = new List<Role>();

        public List<int> RolesToDeleteIds { get; set; } = new();
        public List<int> RolesToAddIds { get; set; } = new();
    }
}
