using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class LostPasswordViewModel
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
    }
}