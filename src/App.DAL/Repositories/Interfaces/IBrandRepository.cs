using App.Core.Entities;

namespace App.DAL.Repositories.Interfaces
{
    public interface IBrandRepository : IRepository<Brand>
    {
        Task<IEnumerable<Brand>> GetAllBrandsAsync();
        Task<Brand> GetBrandByIdAsync(int id);
    }
}
