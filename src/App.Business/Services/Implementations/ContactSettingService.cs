using App.Business.DTOs.ContactSettings;
using App.Business.Services.ExternalServices.Interfaces;
using App.Business.Services.Interfaces;
using App.Core.Entities;
using App.DAL.Repositories.Interfaces;

namespace App.Business.Services.Implementations
{
    public class ContactSettingService : IContactSettingService
    {
        private readonly IContactSettingRepository _contactSettingRepository;
        private readonly IFileManagerService _fileManagerService;

        public ContactSettingService(
            IContactSettingRepository contactSettingRepository,
            IFileManagerService fileManagerService)
        {
            _contactSettingRepository = contactSettingRepository;
            _fileManagerService = fileManagerService;
        }

        public async Task<ContactSettingDTO> GetActiveContactSettingAsync()
        {
            var setting = await _contactSettingRepository.GetActiveSettingAsync();
            
            if (setting == null)
            {
                throw new Exception("?laq? parametrl?ri tap?lmad?");
            }

            return MapToDTO(setting);
        }

        public async Task<ContactSettingDTO> GetContactSettingByIdAsync(int id)
        {
            var setting = await _contactSettingRepository.GetByIdAsync(
                c => c.Id == id,
                c => c.Translations
            );
            
            if (setting == null)
            {
                throw new Exception("?laq? parametrl?ri tap?lmad?");
            }

            return MapToDTO(setting);
        }

        public async Task<IEnumerable<ContactSettingDTO>> GetAllContactSettingsAsync()
        {
            var settings = await _contactSettingRepository.GetAllAsync(
                c => !c.IsDeleted,
                c => c.Translations
            );
            return settings.Select(s => MapToDTO(s));
        }

        public async Task<ContactSettingDTO> CreateContactSettingAsync(CreateContactSettingDTO createDto)
        {
            string imageUrl = null;
            if (createDto.ContactImage != null)
            {
                imageUrl = await _fileManagerService.UploadFileAsync(createDto.ContactImage);
            }

            var setting = new ContactSetting
            {
                Address = createDto.Address,
                Email = createDto.Email,
                Phone = createDto.Phone,
                WhatsApp = createDto.WhatsApp,
                Instagram = createDto.Instagram,
                WorkingHoursWeekdays = createDto.WorkingHoursWeekdays,
                WorkingHoursSaturday = createDto.WorkingHoursSaturday,
                WorkingHoursSunday = createDto.WorkingHoursSunday,
                SupportDescription = createDto.SupportDescription,
                ContactImage = imageUrl,
                Latitude = createDto.Latitude,
                Longitude = createDto.Longitude,
                FacebookUrl = createDto.FacebookUrl,
                TwitterUrl = createDto.TwitterUrl,
                LinkedInUrl = createDto.LinkedInUrl,
                YouTubeUrl = createDto.YouTubeUrl,
                Translations = new List<ContactSettingTranslation>()
            };

            // Add English translation if provided
            if (!string.IsNullOrWhiteSpace(createDto.SupportDescriptionEn))
            {
                setting.Translations.Add(new ContactSettingTranslation
                {
                    LanguageCode = "en",
                    SupportDescription = createDto.SupportDescriptionEn,
                    WorkingHoursWeekdays = createDto.WorkingHoursWeekdaysEn,
                    WorkingHoursSaturday = createDto.WorkingHoursSaturdayEn,
                    WorkingHoursSunday = createDto.WorkingHoursSundayEn
                });
            }

            // Add Russian translation if provided
            if (!string.IsNullOrWhiteSpace(createDto.SupportDescriptionRu))
            {
                setting.Translations.Add(new ContactSettingTranslation
                {
                    LanguageCode = "ru",
                    SupportDescription = createDto.SupportDescriptionRu,
                    WorkingHoursWeekdays = createDto.WorkingHoursWeekdaysRu,
                    WorkingHoursSaturday = createDto.WorkingHoursSaturdayRu,
                    WorkingHoursSunday = createDto.WorkingHoursSundayRu
                });
            }

            var createdSetting = await _contactSettingRepository.AddAsync(setting);
            return MapToDTO(createdSetting);
        }

        public async Task<ContactSettingDTO> UpdateContactSettingAsync(int id, CreateContactSettingDTO updateDto)
        {
            var setting = await _contactSettingRepository.GetByIdAsync(
                c => c.Id == id,
                c => c.Translations
            );
            
            if (setting == null)
            {
                throw new Exception("?laq? parametrl?ri tap?lmad?");
            }

            setting.Address = updateDto.Address;
            setting.Email = updateDto.Email;
            setting.Phone = updateDto.Phone;
            setting.WhatsApp = updateDto.WhatsApp;
            setting.Instagram = updateDto.Instagram;
            setting.WorkingHoursWeekdays = updateDto.WorkingHoursWeekdays;
            setting.WorkingHoursSaturday = updateDto.WorkingHoursSaturday;
            setting.WorkingHoursSunday = updateDto.WorkingHoursSunday;
            setting.SupportDescription = updateDto.SupportDescription;
            setting.Latitude = updateDto.Latitude;
            setting.Longitude = updateDto.Longitude;
            setting.FacebookUrl = updateDto.FacebookUrl;
            setting.TwitterUrl = updateDto.TwitterUrl;
            setting.LinkedInUrl = updateDto.LinkedInUrl;
            setting.YouTubeUrl = updateDto.YouTubeUrl;

            if (updateDto.ContactImage != null)
            {
                setting.ContactImage = await _fileManagerService.UploadFileAsync(updateDto.ContactImage);
            }

            // Update translations
            setting.Translations ??= new List<ContactSettingTranslation>();

            // Update or add English translation
            var enTranslation = setting.Translations.FirstOrDefault(t => t.LanguageCode == "en");
            if (!string.IsNullOrWhiteSpace(updateDto.SupportDescriptionEn))
            {
                if (enTranslation != null)
                {
                    enTranslation.SupportDescription = updateDto.SupportDescriptionEn;
                    enTranslation.WorkingHoursWeekdays = updateDto.WorkingHoursWeekdaysEn;
                    enTranslation.WorkingHoursSaturday = updateDto.WorkingHoursSaturdayEn;
                    enTranslation.WorkingHoursSunday = updateDto.WorkingHoursSundayEn;
                }
                else
                {
                    setting.Translations.Add(new ContactSettingTranslation
                    {
                        ContactSettingId = setting.Id,
                        LanguageCode = "en",
                        SupportDescription = updateDto.SupportDescriptionEn,
                        WorkingHoursWeekdays = updateDto.WorkingHoursWeekdaysEn,
                        WorkingHoursSaturday = updateDto.WorkingHoursSaturdayEn,
                        WorkingHoursSunday = updateDto.WorkingHoursSundayEn
                    });
                }
            }
            else if (enTranslation != null)
            {
                setting.Translations.Remove(enTranslation);
            }

            // Update or add Russian translation
            var ruTranslation = setting.Translations.FirstOrDefault(t => t.LanguageCode == "ru");
            if (!string.IsNullOrWhiteSpace(updateDto.SupportDescriptionRu))
            {
                if (ruTranslation != null)
                {
                    ruTranslation.SupportDescription = updateDto.SupportDescriptionRu;
                    ruTranslation.WorkingHoursWeekdays = updateDto.WorkingHoursWeekdaysRu;
                    ruTranslation.WorkingHoursSaturday = updateDto.WorkingHoursSaturdayRu;
                    ruTranslation.WorkingHoursSunday = updateDto.WorkingHoursSundayRu;
                }
                else
                {
                    setting.Translations.Add(new ContactSettingTranslation
                    {
                        ContactSettingId = setting.Id,
                        LanguageCode = "ru",
                        SupportDescription = updateDto.SupportDescriptionRu,
                        WorkingHoursWeekdays = updateDto.WorkingHoursWeekdaysRu,
                        WorkingHoursSaturday = updateDto.WorkingHoursSaturdayRu,
                        WorkingHoursSunday = updateDto.WorkingHoursSundayRu
                    });
                }
            }
            else if (ruTranslation != null)
            {
                setting.Translations.Remove(ruTranslation);
            }

            var updatedSetting = await _contactSettingRepository.UpdateAsync(setting);
            return MapToDTO(updatedSetting);
        }

        public async Task DeleteContactSettingAsync(int id)
        {
            var setting = await _contactSettingRepository.GetByIdAsync(c => c.Id == id);
            
            if (setting == null)
            {
                throw new Exception("?laq? parametrl?ri tap?lmad?");
            }

            await _contactSettingRepository.DeleteAsync(setting);
        }

        private ContactSettingDTO MapToDTO(ContactSetting setting)
        {
            var dto = new ContactSettingDTO
            {
                Id = setting.Id,
                Address = setting.Address,
                Email = setting.Email,
                Phone = setting.Phone,
                WhatsApp = setting.WhatsApp,
                Instagram = setting.Instagram,
                WorkingHoursWeekdays = setting.WorkingHoursWeekdays,
                WorkingHoursSaturday = setting.WorkingHoursSaturday,
                WorkingHoursSunday = setting.WorkingHoursSunday,
                SupportDescription = setting.SupportDescription,
                ContactImage = setting.ContactImage,
                Latitude = setting.Latitude,
                Longitude = setting.Longitude,
                FacebookUrl = setting.FacebookUrl,
                TwitterUrl = setting.TwitterUrl,
                LinkedInUrl = setting.LinkedInUrl,
                YouTubeUrl = setting.YouTubeUrl,
                CreatedOn = setting.CreatedOn,
                UpdatedOn = setting.UpdatedOn,
                Translations = new Dictionary<string, ContactSettingTranslationDTO>()
            };

            // Add default Azerbaijani
            dto.Translations["az"] = new ContactSettingTranslationDTO
            {
                LanguageCode = "az",
                SupportDescription = setting.SupportDescription,
                WorkingHoursWeekdays = setting.WorkingHoursWeekdays,
                WorkingHoursSaturday = setting.WorkingHoursSaturday,
                WorkingHoursSunday = setting.WorkingHoursSunday
            };

            // Add other translations
            if (setting.Translations != null)
            {
                foreach (var translation in setting.Translations)
                {
                    dto.Translations[translation.LanguageCode] = new ContactSettingTranslationDTO
                    {
                        LanguageCode = translation.LanguageCode,
                        SupportDescription = translation.SupportDescription,
                        WorkingHoursWeekdays = translation.WorkingHoursWeekdays,
                        WorkingHoursSaturday = translation.WorkingHoursSaturday,
                        WorkingHoursSunday = translation.WorkingHoursSunday
                    };
                }
            }

            return dto;
        }
    }
}
