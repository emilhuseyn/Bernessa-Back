using System.ComponentModel.DataAnnotations;

namespace App.Business.DTOs.Products
{
    public class CreateProductVariantDTO
    {
        [Required(ErrorMessage = "H?cm t?l?b olunur")]
        [MaxLength(50, ErrorMessage = "H?cm maksimum 50 simvol ola bil?r")]
        public string Volume { get; set; }

        [Required(ErrorMessage = "Qiym?t t?l?b olunur")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Qiym?t 0-dan böyük olmal?d?r")]
        public decimal Price { get; set; }

        public decimal? OriginalPrice { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
