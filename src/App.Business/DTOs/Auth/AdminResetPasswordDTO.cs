using System.ComponentModel.DataAnnotations;

namespace App.Business.DTOs.Auth
{
    public class AdminResetPasswordDTO
    {
        [Required(ErrorMessage = "Email daxil edilm?lidir")]
        [EmailAddress(ErrorMessage = "Email format? düzgün deyil")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Yeni ?ifr? daxil edilm?lidir")]
        [MinLength(6, ErrorMessage = "?ifr? ?n az? 6 simvol olmal?d?r")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "?ifr? t?sdiqi daxil edilm?lidir")]
        [Compare("NewPassword", ErrorMessage = "?ifr? v? t?sdiq ?ifr?si uy?un g?lmir")]
        public string ConfirmPassword { get; set; }
    }
}
