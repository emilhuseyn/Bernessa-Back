using App.Core.Entities;
using App.DAL.Repositories.Interfaces;

namespace App.DAL.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetBySlugAsync(string slug);
        Task UpdateProductCountAsync(int categoryId);
    }
}
