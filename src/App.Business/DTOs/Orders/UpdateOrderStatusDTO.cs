using App.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace App.Business.DTOs.Orders
{
    public class UpdateOrderStatusDTO
    {
        [Required]
        public OrderStatus Status { get; set; }
    }
}
