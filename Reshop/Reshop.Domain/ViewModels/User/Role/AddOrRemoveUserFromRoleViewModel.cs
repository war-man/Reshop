using System.Collections.Generic;

namespace Reshop.Domain.ViewModels.User.Role
{
    public class AddOrRemoveUserFromRoleViewModel
    {
        public string UserId { get; set; }
        public IEnumerable<string> RolesName { get; set; }
        public IList<string> SelectedRoles { get; set; }
    }
}