namespace App.Business.DTOs.ContactSettings
{
    public class ContactSettingTranslationDTO
    {
        public string LanguageCode { get; set; }
        public string SupportDescription { get; set; }
        public string WorkingHoursWeekdays { get; set; }
        public string WorkingHoursSaturday { get; set; }
        public string WorkingHoursSunday { get; set; }
    }
}
