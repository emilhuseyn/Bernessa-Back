using App.Business.DTOs.Orders;
using App.Business.Services.Interfaces;
using App.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDTO createOrderDto)
        {
            var order = await _orderService.CreateOrderAsync(createOrderDto);
            return Ok(new
            {
                success = true,
                orderNumber = order.OrderNumber,
                message = "Sifari?iniz u?urla qeyd? al?nd?",
                trackingUrl = $"/orders/track/{order.OrderNumber}",
                data = order
            });
        }

        [HttpGet("track/{orderNumber}")]
        public async Task<IActionResult> TrackOrder(string orderNumber)
        {
            var order = await _orderService.GetOrderByNumberAsync(orderNumber);
            return Ok(new { success = true, data = order });
        }

 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(new { success = true, data = orders });
        }

         [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(new { success = true, data = order });
        }

         [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDTO updateStatusDto)
        {
            var order = await _orderService.UpdateOrderStatusAsync(id, updateStatusDto.Status);
            return Ok(new { success = true, data = order });
        }

         [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return Ok(new { success = true, message = "Sifari? l??v edildi" });
        }
    }
}
