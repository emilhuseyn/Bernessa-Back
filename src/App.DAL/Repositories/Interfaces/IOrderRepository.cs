using App.Core.Entities;
using App.Core.Enums;
using App.DAL.Repositories.Interfaces;

namespace App.DAL.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Order> GetByOrderNumberAsync(string orderNumber);
        Task<ICollection<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task<ICollection<Order>> GetTodayOrdersAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<int> GetUniqueCustomersCountAsync();
    }
}
