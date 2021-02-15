using System.ComponentModel.DataAnnotations;

namespace HomeSurveillanceApp.Models.Auth
{
    public class Registration
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Role is requiered")]
        public string UserRole { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
