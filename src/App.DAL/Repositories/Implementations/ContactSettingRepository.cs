using App.Core.Entities;
using App.DAL.Presistence;
using App.DAL.Repositories.Abstractions;
using App.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Repositories.Implementations
{
    public class ContactSettingRepository : Repository<ContactSetting>, IContactSettingRepository
    {
        public ContactSettingRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ContactSetting> GetActiveSettingAsync()
        {
            return await DbSet
                .Where(c => !c.IsDeleted)
                .Include(c => c.Translations)
                .OrderByDescending(c => c.UpdatedOn)
                .FirstOrDefaultAsync();
        }
    }
}
