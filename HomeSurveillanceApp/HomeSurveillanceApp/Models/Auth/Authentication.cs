using System.ComponentModel.DataAnnotations;

namespace HomeSurveillanceApp.Models.Auth
{
    public class Authentication
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
