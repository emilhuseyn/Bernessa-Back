namespace App.Business.DTOs.Analytics
{
    public class DashboardDTO
    {
        // Revenue Metrics
        public decimal TotalRevenue { get; set; }
        public decimal TodayRevenue { get; set; }
        public decimal WeekRevenue { get; set; }
        public decimal MonthRevenue { get; set; }
        public decimal YearRevenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        
        // Order Metrics
        public int TotalOrders { get; set; }
        public int TodayOrders { get; set; }
        public int WeekOrders { get; set; }
        public int MonthOrders { get; set; }
        public int YearOrders { get; set; }
        
        // Order Status Breakdown
        public OrderStatusBreakdownDTO OrderStatusBreakdown { get; set; }
        
        // Customer Metrics
        public int TotalCustomers { get; set; }
        public int NewCustomersToday { get; set; }
        public int NewCustomersWeek { get; set; }
        public int NewCustomersMonth { get; set; }
        public int ReturningCustomers { get; set; }
        
        // Product Metrics
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }
        public int InactiveProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public int LowStockProducts { get; set; }
        public int FeaturedProducts { get; set; }
        
        // Category Metrics
        public int TotalCategories { get; set; }
        public List<CategoryPerformanceDTO> TopCategories { get; set; }
        
        // Product Performance
        public List<TopProductDTO> TopSellingProducts { get; set; }
        public List<TopProductDTO> TopRevenueProducts { get; set; }
        public List<LowStockProductDTO> LowStockAlerts { get; set; }
        
        // Recent Activity
        public List<RecentOrderDTO> RecentOrders { get; set; }
        public List<RecentOrderDTO> PendingOrders { get; set; }
        
        // Payment Method Breakdown
        public List<PaymentMethodStatsDTO> PaymentMethodStats { get; set; }
        
        // Revenue Trends (Last 7 days)
        public List<DailyRevenueDTO> Last7DaysRevenue { get; set; }
        
        // Revenue Trends (Last 12 months)
        public List<MonthlyRevenueDTO> Last12MonthsRevenue { get; set; }
        
        // Growth Metrics
        public GrowthMetricsDTO GrowthMetrics { get; set; }
    }

    public class OrderStatusBreakdownDTO
    {
        public int Pending { get; set; }
        public int Processing { get; set; }
        public int Shipped { get; set; }
        public int Delivered { get; set; }
        public int Cancelled { get; set; }
        
        public decimal PendingRevenue { get; set; }
        public decimal ProcessingRevenue { get; set; }
        public decimal ShippedRevenue { get; set; }
        public decimal DeliveredRevenue { get; set; }
        public decimal CancelledRevenue { get; set; }
    }

    public class RecentOrderDTO
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public int ItemCount { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class TopProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductBrand { get; set; }
        public string CategoryName { get; set; }
        public int TotalSold { get; set; }
        public decimal Revenue { get; set; }
        public int CurrentStock { get; set; }
        public decimal Price { get; set; }
    }

    public class LowStockProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductBrand { get; set; }
        public int CurrentStock { get; set; }
        public bool IsOutOfStock { get; set; }
        public decimal Price { get; set; }
    }

    public class CategoryPerformanceDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ProductCount { get; set; }
        public int TotalSold { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }

    public class PaymentMethodStatsDTO
    {
        public string PaymentMethod { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal Percentage { get; set; }
    }

    public class DailyRevenueDTO
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }

    public class MonthlyRevenueDTO
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }

    public class GrowthMetricsDTO
    {
        public decimal RevenueGrowthWeek { get; set; } // % change from previous week
        public decimal RevenueGrowthMonth { get; set; } // % change from previous month
        public decimal OrdersGrowthWeek { get; set; }
        public decimal OrdersGrowthMonth { get; set; }
        public decimal CustomersGrowthWeek { get; set; }
        public decimal CustomersGrowthMonth { get; set; }
        public decimal AverageOrderValueGrowth { get; set; }
    }
}
