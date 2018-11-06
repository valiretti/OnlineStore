using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class ClientProfileViewModel
    {
        [Required(ErrorMessage = "Field must be filled")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}