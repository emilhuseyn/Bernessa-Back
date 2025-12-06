using App.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace App.Business.DTOs.Orders
{
    public class CreateOrderDTO
    {
        [Required(ErrorMessage = "Ad daxil edilm?lidir")]
        [MaxLength(200)]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Email daxil edilm?lidir")]
        [EmailAddress(ErrorMessage = "Email format? düzgün deyil")]
        [MaxLength(200)]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Telefon nömr?si daxil edilm?lidir")]
        [Phone(ErrorMessage = "Telefon format? düzgün deyil")]
        [MaxLength(50)]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "Ünvan daxil edilm?lidir")]
        [MaxLength(500)]
        public string ShippingAddress { get; set; }

        [MaxLength(1000)]
        public string CustomerNote { get; set; }

        [Required]
        public List<OrderItemDTO> Items { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public string DiscountCode { get; set; }
    }

    public class OrderItemDTO
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Miqdar minimum 1 olmal?d?r")]
        public int Quantity { get; set; }
    }
}
