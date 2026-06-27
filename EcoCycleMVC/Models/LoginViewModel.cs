using System.ComponentModel.DataAnnotations;

namespace EcoCycleMVC.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Correo { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }
}