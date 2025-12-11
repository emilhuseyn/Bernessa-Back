using App.Business.DTOs.Orders;
using App.Business.Services.Interfaces;
using App.Core.Enums;
using App.Shared.Interfaces;
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
        private readonly IClaimService _claimService;

        public OrdersController(IOrderService orderService, IClaimService claimService)
        {
            _orderService = orderService;
            _claimService = claimService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateOrderDTO createOrderDto)
        {
            var order = await _orderService.CreateOrderAsync(createOrderDto);
            return Ok(new
            {
                success = true,
                orderNumber = order.OrderNumber,
                message = "Sifarişiniz uğurla qeydə alındı",
                trackingUrl = $"/orders/track/{order.OrderNumber}",
                data = order
            });
        }

        [HttpGet("track/{orderNumber}")]
        [AllowAnonymous]
        public async Task<IActionResult> TrackOrder(string orderNumber)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
            {
                return BadRequest(new { success = false, message = "Sifariş nömrəsi daxil edilməlidir" });
            }

            var order = await _orderService.GetOrderByNumberAsync(orderNumber);
            return Ok(new { success = true, data = order });
        }

        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(new { success = true, data = orders });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(new { success = true, data = order });
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDTO updateStatusDto)
        {
            var order = await _orderService.UpdateOrderStatusAsync(id, updateStatusDto.Status);
            return Ok(new { success = true, data = order, message = "Sifariş statusu yeniləndi" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteOrderAsync(id);
            return Ok(new { success = true, message = "Sifariş ləğv edildi" });
        }
    }
}