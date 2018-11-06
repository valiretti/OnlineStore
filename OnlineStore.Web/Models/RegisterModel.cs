using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Field must be filled")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
}