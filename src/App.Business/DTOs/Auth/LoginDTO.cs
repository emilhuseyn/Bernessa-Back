using System.ComponentModel.DataAnnotations;

namespace App.Business.DTOs.Auth
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email daxil edilm?lidir")]
        [EmailAddress(ErrorMessage = "Email format? düzgün deyil")]
        public string Email { get; set; }

        [Required(ErrorMessage = "?ifr? daxil edilm?lidir")]
        public string Password { get; set; }
    }
}
