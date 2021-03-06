﻿using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class OrderViewModelForCart
    {
        public int Id { get; set; }

        public string Items { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Адрес доставки")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Поле должно быть заполнено")]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }
    }
}