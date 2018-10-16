using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore.Web.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public string State { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Адрес доставки")]
        public string Address { get; set; }


        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }


    }
}
