using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        [Display(Name = "Manufacturer")]
        public string Name { get; set; }
    }
}