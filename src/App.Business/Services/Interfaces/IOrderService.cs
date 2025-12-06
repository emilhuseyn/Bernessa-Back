using App.Business.DTOs.Orders;
using App.Core.Enums;

namespace App.Business.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(CreateOrderDTO createOrderDto);
        Task<OrderDTO> GetOrderByIdAsync(int id);
        Task<OrderDTO> GetOrderByNumberAsync(string orderNumber);
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> UpdateOrderStatusAsync(int id, OrderStatus status);
        Task DeleteOrderAsync(int id);
    }
}
