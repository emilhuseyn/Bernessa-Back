using App.Core.Entities.Commons;
using System;

namespace App.Core.Entities
{
    public class Setting : BaseEntity, IAuditedEntity
    {
        public string StoreName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int ShippingDays { get; set; }
        public decimal ShippingCost { get; set; }
        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
        public decimal MinOrderAmount { get; set; }
        
        // IAuditedEntity
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
