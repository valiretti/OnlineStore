using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}