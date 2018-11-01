using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Производитель")]
        public string Name { get; set; }
    }
}