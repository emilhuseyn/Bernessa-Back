using App.Core.Entities.Commons;
using System;

namespace App.Core.Entities
{
    public class ProductTranslation : BaseEntity
    {
        public int ProductId { get; set; }
        public string LanguageCode { get; set; } // "az", "en", "ru"
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        
        // Navigation properties
        public Product Product { get; set; }
    }
}
