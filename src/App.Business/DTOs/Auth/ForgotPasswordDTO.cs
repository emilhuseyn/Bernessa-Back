using System.ComponentModel.DataAnnotations;

namespace App.Business.DTOs.Auth
{
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "Email daxil edilm?lidir")]
        [EmailAddress(ErrorMessage = "Email format? düzgün deyil")]
        public string Email { get; set; }
    }
}
