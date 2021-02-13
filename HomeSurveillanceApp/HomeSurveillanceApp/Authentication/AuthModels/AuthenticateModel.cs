﻿using System.ComponentModel.DataAnnotations;

namespace HomeSurveillanceApp.Authentication.AuthModels
{
    public class AuthenticateModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
