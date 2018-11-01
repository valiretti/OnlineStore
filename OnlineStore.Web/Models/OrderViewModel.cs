using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string State { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Адрес доставки")]
        public string Address { get; set; }


        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        public string ClientProfileId { get; set; }

    }
}
