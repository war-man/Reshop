using System.ComponentModel.DataAnnotations;

namespace Reshop.Domain.ViewModels.User
{
    public class AddOrEditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "ایمیل")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "ادرس")]
        public string Address { get; set; }

        [Display(Name = "رمزعبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}