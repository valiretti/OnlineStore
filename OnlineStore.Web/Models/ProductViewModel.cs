using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Производитель")]
        public string CompanyId { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Категория товара")]
        public string CategoryId { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Описание товара")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Цена")]
        public string Price { get; set; }

        public string ImagePath { get; set; }
    }
}