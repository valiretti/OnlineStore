using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class ClientProfileViewModel
    {
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
    }
}