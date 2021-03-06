﻿using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Field must be filled")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field must be filled")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}