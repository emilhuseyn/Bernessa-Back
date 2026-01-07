using App.Core.Entities;

namespace App.DAL.Repositories.Interfaces
{
    public interface IContactSettingRepository : IRepository<ContactSetting>
    {
        Task<ContactSetting> GetActiveSettingAsync();
    }
}
