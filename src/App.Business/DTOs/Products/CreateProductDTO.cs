using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.Business.DTOs.Products
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "Ad t?l?b olunur")]
        [MaxLength(200, ErrorMessage = "Ad maksimum 200 simvol ola bil?r")]
        public string Name { get; set; } // Azerbaijani name

        [Required(ErrorMessage = "Brend t?l?b olunur")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Növ t?l?b olunur")]
        [MaxLength(100, ErrorMessage = "Növ maksimum 100 simvol ola bil?r")]
        public string Type { get; set; }  

        [Required(ErrorMessage = "Aç?qlama t?l?b olunur")]
        [MaxLength(2000, ErrorMessage = "Aç?qlama maksimum 2000 simvol ola bil?r")]
        public string Description { get; set; } // Azerbaijani description

        public List<IFormFile> Images { get; set; }

        [Required(ErrorMessage = "Kateqoriya t?l?b olunur")]
        public int CategoryId { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
        
        // Variants - at least one variant is required
        [Required(ErrorMessage = "?n az? bir variant t?l?b olunur")]
        public List<CreateProductVariantDTO> Variants { get; set; }
        
        // Translations
        public string NameEn { get; set; }
        public string NameRu { get; set; }
        public string TypeEn { get; set; }
        public string TypeRu { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionRu { get; set; }
    }
}
