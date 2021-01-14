using System;
using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.ViewModels.User.Role
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "لطفا نام کاربری را وارد کنید")]
        public string Name { get; set; }
    }
}