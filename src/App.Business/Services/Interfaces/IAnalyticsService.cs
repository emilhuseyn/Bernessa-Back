using App.Business.DTOs.Analytics;

namespace App.Business.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task<DashboardDTO> GetDashboardDataAsync();
    }
}
