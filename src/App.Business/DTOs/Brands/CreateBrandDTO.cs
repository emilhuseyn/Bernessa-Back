using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.Business.DTOs.Brands
{
    public class CreateBrandDTO
    {
        [Required(ErrorMessage = "Ad t?l?b olunur")]
        [MaxLength(100, ErrorMessage = "Ad maksimum 100 simvol ola bil?r")]
        public string Name { get; set; }

        public IFormFile Logo { get; set; }
    }
}
