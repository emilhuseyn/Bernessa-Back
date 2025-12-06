using App.Core.Entities.Commons;
using System;

namespace App.Core.Entities
{
    public class CategoryTranslation : BaseEntity
    {
        public int CategoryId { get; set; }
        public string LanguageCode { get; set; } // "az", "en", "ru"
        public string Name { get; set; }
        
        // Navigation properties
        public Category Category { get; set; }
    }
}
