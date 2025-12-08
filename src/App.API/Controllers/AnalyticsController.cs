using App.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.API.Controllers
{
    [Route("api/admin/analytics")]
    [ApiController]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        /// <summary>
        /// Get comprehensive dashboard statistics (Admin only)
        /// Includes revenue, orders, customers, products, trends, and growth metrics
        /// </summary>
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard = await _analyticsService.GetDashboardDataAsync();
            return Ok(new { success = true, data = dashboard });
        }
    }
}
