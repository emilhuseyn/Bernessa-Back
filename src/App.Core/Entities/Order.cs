using App.Core.Entities.Commons;
using App.Core.Enums;
using System;
using System.Collections.Generic;

namespace App.Core.Entities
{
    public class Order : BaseEntity, IAuditedEntity
    {
        public string OrderNumber { get; set; }
        
        // Customer Information
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string ShippingAddress { get; set; }
        public string? CustomerNote { get; set; }
        
        // Order Details
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public OrderStatus Status { get; set; }
        
        // IAuditedEntity
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        
        public DateTime? DeliveredAt { get; set; }
        
        // Navigation properties
        public ICollection<OrderItem> Items { get; set; }
    }
}
