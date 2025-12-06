using App.Core.Entities;
using App.Core.Enums;
using App.DAL.Presistence;
using App.DAL.Repositories.Abstractions;
using App.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Repositories.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Order> GetByOrderNumberAsync(string orderNumber)
        {
            return await DbSet
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber && !o.IsDeleted);
        }

        public async Task<ICollection<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await DbSet
                .Include(o => o.Items)
                .Where(o => o.Status == status && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedOn)
                .ToListAsync();
        }

        public async Task<ICollection<Order>> GetTodayOrdersAsync()
        {
            var today = DateTime.UtcNow.Date;
            return await DbSet
                .Include(o => o.Items)
                .Where(o => o.CreatedOn.Date == today && !o.IsDeleted)
                .OrderByDescending(o => o.CreatedOn)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await DbSet
                .Where(o => o.Status != OrderStatus.Cancelled && !o.IsDeleted)
                .SumAsync(o => o.Total);
        }

        public async Task<int> GetUniqueCustomersCountAsync()
        {
            return await DbSet
                .Where(o => !o.IsDeleted)
                .Select(o => o.CustomerEmail)
                .Distinct()
                .CountAsync();
        }
    }
}
