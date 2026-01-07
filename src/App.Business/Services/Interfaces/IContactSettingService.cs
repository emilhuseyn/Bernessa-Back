using App.Business.DTOs.ContactSettings;

namespace App.Business.Services.Interfaces
{
    public interface IContactSettingService
    {
        Task<ContactSettingDTO> GetActiveContactSettingAsync();
        Task<ContactSettingDTO> GetContactSettingByIdAsync(int id);
        Task<IEnumerable<ContactSettingDTO>> GetAllContactSettingsAsync();
        Task<ContactSettingDTO> CreateContactSettingAsync(CreateContactSettingDTO createDto);
        Task<ContactSettingDTO> UpdateContactSettingAsync(int id, CreateContactSettingDTO updateDto);
        Task DeleteContactSettingAsync(int id);
    }
}
