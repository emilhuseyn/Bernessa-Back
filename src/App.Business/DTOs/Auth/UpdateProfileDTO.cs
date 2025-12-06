using System.ComponentModel.DataAnnotations;

namespace App.Business.DTOs.Auth
{
    public class UpdateProfileDTO
    {
        [Required(ErrorMessage = "Ad daxil edilm?lidir")]
        [MaxLength(100, ErrorMessage = "Ad maksimum 100 simvol ola bil?r")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad daxil edilm?lidir")]
        [MaxLength(100, ErrorMessage = "Soyad maksimum 100 simvol ola bil?r")]
        public string LastName { get; set; }

        [MaxLength(500, ErrorMessage = "Avatar URL maksimum 500 simvol ola bil?r")]
        public string Avatar { get; set; }
    }
}
