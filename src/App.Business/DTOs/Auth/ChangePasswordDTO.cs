using System.ComponentModel.DataAnnotations;

namespace App.Business.DTOs.Auth
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "Cari ?ifr? daxil edilm?lidir")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Yeni ?ifr? daxil edilm?lidir")]
        [MinLength(6, ErrorMessage = "Yeni ?ifr? ?n az? 6 simvol olmal?d?r")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "?ifr? t?sdiqi daxil edilm?lidir")]
        [Compare("NewPassword", ErrorMessage = "Yeni ?ifr? v? t?sdiq ?ifr?si uy?un g?lmir")]
        public string ConfirmPassword { get; set; }
    }
}
