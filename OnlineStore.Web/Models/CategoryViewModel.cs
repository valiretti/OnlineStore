using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        [Display(Name = "Product Category")]
        public string Name { get; set; }
    }
}