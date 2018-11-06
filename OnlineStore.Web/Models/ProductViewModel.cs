using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        [Display(Name = "Manufacturer")]
        public string CompanyId { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        [Display(Name = "Product Category")]
        public string CategoryId { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        [Display(Name = "Product Description")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        public string Price { get; set; }

        public string ImagePath { get; set; }
    }
}