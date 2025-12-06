using System.ComponentModel.DataAnnotations;

namespace App.Business.DTOs.Auth
{
    public class ValidateTokenDTO
    {
        [Required(ErrorMessage = "Email daxil edilm?lidir")]
        [EmailAddress(ErrorMessage = "Email format? düzgün deyil")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Token daxil edilm?lidir")]
        public string Token { get; set; }
    }
}
