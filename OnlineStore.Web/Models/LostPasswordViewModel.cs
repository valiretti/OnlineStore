using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class LostPasswordViewModel
    {
        [Required(ErrorMessage = "Field must be filled")]
        public string Email { get; set; }
    }
}