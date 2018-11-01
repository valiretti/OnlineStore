using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class PasswordChangeViewModel
    {
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Повторите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}