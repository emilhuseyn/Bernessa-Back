using App.Core.Entities.Commons;
using System;

namespace App.Core.Entities
{
    public class OrderItem : BaseEntity, IAuditedEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        
        // Product Snapshot
        public string ProductName { get; set; }
        public string ProductBrand { get; set; }
        public string ProductVolume { get; set; }
        public string ProductImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        
        // IAuditedEntity
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        
        // Navigation properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
