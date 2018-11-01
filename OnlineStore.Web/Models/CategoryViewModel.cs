using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Категория товара")]
        public string Name { get; set; }
    }
}