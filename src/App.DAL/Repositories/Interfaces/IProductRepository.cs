using App.Core.Entities;
using App.DAL.Repositories.Interfaces;

namespace App.DAL.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<ICollection<Product>> GetFeaturedProductsAsync();
        Task<ICollection<Product>> GetByCategorySlugAsync(string slug);
        Task<ICollection<Product>> SearchProductsAsync(string query);
        Task<ICollection<Product>> GetDealsAsync();
    }
}
