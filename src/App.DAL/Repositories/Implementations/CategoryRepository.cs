using App.Core.Entities;
using App.DAL.Presistence;
using App.DAL.Repositories.Abstractions;
using App.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Repositories.Implementations
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Category> GetBySlugAsync(string slug)
        {
            return await DbSet
                .FirstOrDefaultAsync(c => c.Slug == slug && !c.IsDeleted);
        }

        public async Task UpdateProductCountAsync(int categoryId)
        {
            var category = await DbSet.FindAsync(categoryId);
            if (category != null)
            {
                category.ProductCount = await Context.Products
                    .CountAsync(p => p.CategoryId == categoryId && p.IsActive && !p.IsDeleted);
                
                await Context.SaveChangesAsync();
            }
        }
    }
}
