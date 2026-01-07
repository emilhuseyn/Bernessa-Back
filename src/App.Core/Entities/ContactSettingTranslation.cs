using App.Core.Entities.Commons;
using System;

namespace App.Core.Entities
{
    public class ContactSettingTranslation : BaseEntity
    {
        public int ContactSettingId { get; set; }
        public string LanguageCode { get; set; } // az, en, ru
        
        public string SupportDescription { get; set; }
        public string WorkingHoursWeekdays { get; set; }
        public string WorkingHoursSaturday { get; set; }
        public string WorkingHoursSunday { get; set; }
        
        // Navigation property
        public ContactSetting ContactSetting { get; set; }
    }
}
