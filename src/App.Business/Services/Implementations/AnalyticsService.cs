using App.Business.DTOs.Analytics;
using App.Business.Services.Interfaces;
using App.Core.Enums;
using App.DAL.Presistence;
using App.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace App.Business.Services.Implementations
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _context;

        public AnalyticsService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            AppDbContext context)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<DashboardDTO> GetDashboardDataAsync()
        {
            var now = DateTime.UtcNow;
            var today = now.Date;
            var weekAgo = today.AddDays(-7);
            var monthAgo = today.AddMonths(-1);
            var yearAgo = today.AddYears(-1);
            var previousWeekStart = weekAgo.AddDays(-7);
            var previousMonthStart = monthAgo.AddMonths(-1);

            // Get all orders (not deleted)
            var allOrders = await _context.Orders
                .Where(o => !o.IsDeleted)
                .Include(o => o.Items)
                .ToListAsync();

            // Revenue Metrics - ONLY from Delivered orders
            var totalRevenue = allOrders.Where(o => o.Status == OrderStatus.Delivered).Sum(o => o.Total);
            var todayRevenue = allOrders.Where(o => o.CreatedOn.Date == today && o.Status == OrderStatus.Delivered).Sum(o => o.Total);
            var weekRevenue = allOrders.Where(o => o.CreatedOn.Date >= weekAgo && o.Status == OrderStatus.Delivered).Sum(o => o.Total);
            var monthRevenue = allOrders.Where(o => o.CreatedOn.Date >= monthAgo && o.Status == OrderStatus.Delivered).Sum(o => o.Total);
            var yearRevenue = allOrders.Where(o => o.CreatedOn.Date >= yearAgo && o.Status == OrderStatus.Delivered).Sum(o => o.Total);
            
            var previousWeekRevenue = allOrders.Where(o => o.CreatedOn.Date >= previousWeekStart && o.CreatedOn.Date < weekAgo && o.Status == OrderStatus.Delivered).Sum(o => o.Total);
            var previousMonthRevenue = allOrders.Where(o => o.CreatedOn.Date >= previousMonthStart && o.CreatedOn.Date < monthAgo && o.Status == OrderStatus.Delivered).Sum(o => o.Total);

            // Order Metrics - All non-cancelled orders
            var totalOrders = allOrders.Count(o => o.Status != OrderStatus.Cancelled);
            var todayOrders = allOrders.Count(o => o.CreatedOn.Date == today);
            var weekOrders = allOrders.Count(o => o.CreatedOn.Date >= weekAgo);
            var monthOrders = allOrders.Count(o => o.CreatedOn.Date >= monthAgo);
            var yearOrders = allOrders.Count(o => o.CreatedOn.Date >= yearAgo);
            
            var previousWeekOrders = allOrders.Count(o => o.CreatedOn.Date >= previousWeekStart && o.CreatedOn.Date < weekAgo && o.Status != OrderStatus.Cancelled);
            var previousMonthOrders = allOrders.Count(o => o.CreatedOn.Date >= previousMonthStart && o.CreatedOn.Date < monthAgo && o.Status != OrderStatus.Cancelled);

            // Average Order Value - Based on delivered orders only
            var deliveredOrdersCount = allOrders.Count(o => o.Status == OrderStatus.Delivered);
            var averageOrderValue = deliveredOrdersCount > 0 ? totalRevenue / deliveredOrdersCount : 0;

            // Order Status Breakdown
            var orderStatusBreakdown = new OrderStatusBreakdownDTO
            {
                Pending = allOrders.Count(o => o.Status == OrderStatus.Pending),
                Processing = allOrders.Count(o => o.Status == OrderStatus.Processing),
                Shipped = allOrders.Count(o => o.Status == OrderStatus.Shipped),
                Delivered = allOrders.Count(o => o.Status == OrderStatus.Delivered),
                Cancelled = allOrders.Count(o => o.Status == OrderStatus.Cancelled),
                PendingRevenue = allOrders.Where(o => o.Status == OrderStatus.Pending).Sum(o => o.Total),
                ProcessingRevenue = allOrders.Where(o => o.Status == OrderStatus.Processing).Sum(o => o.Total),
                ShippedRevenue = allOrders.Where(o => o.Status == OrderStatus.Shipped).Sum(o => o.Total),
                DeliveredRevenue = allOrders.Where(o => o.Status == OrderStatus.Delivered).Sum(o => o.Total),
                CancelledRevenue = allOrders.Where(o => o.Status == OrderStatus.Cancelled).Sum(o => o.Total)
            };

            // Customer Metrics
            var allCustomerEmails = allOrders.Select(o => o.CustomerEmail).Distinct().ToList();
            var totalCustomers = allCustomerEmails.Count;
            
            var newCustomersToday = allOrders
                .Where(o => o.CreatedOn.Date == today)
                .Select(o => o.CustomerEmail)
                .Distinct()
                .Count(email => allOrders.Where(x => x.CustomerEmail == email).Min(x => x.CreatedOn).Date == today);
            
            var newCustomersWeek = allOrders
                .Where(o => o.CreatedOn.Date >= weekAgo)
                .Select(o => o.CustomerEmail)
                .Distinct()
                .Count(email => allOrders.Where(x => x.CustomerEmail == email).Min(x => x.CreatedOn).Date >= weekAgo);
            
            var newCustomersMonth = allOrders
                .Where(o => o.CreatedOn.Date >= monthAgo)
                .Select(o => o.CustomerEmail)
                .Distinct()
                .Count(email => allOrders.Where(x => x.CustomerEmail == email).Min(x => x.CreatedOn).Date >= monthAgo);
            
            var returningCustomers = allCustomerEmails.Count(email => allOrders.Count(o => o.CustomerEmail == email) > 1);

            // Product Metrics
            var allProducts = await _context.Products.Where(p => !p.IsDeleted).ToListAsync();
            var totalProducts = allProducts.Count;
            var activeProducts = allProducts.Count(p => p.IsActive);
            var inactiveProducts = allProducts.Count(p => !p.IsActive);
            var featuredProducts = allProducts.Count(p => p.IsFeatured && p.IsActive);

            // Category Metrics
            var categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            var totalCategories = categories.Count;

            // Top Categories Performance - Only from delivered orders
            var categoryPerformance = await _context.OrderItems
                .Where(oi => !oi.IsDeleted && oi.Order.Status == OrderStatus.Delivered)
                .Include(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .Include(oi => oi.Order)
                .GroupBy(oi => new { oi.Product.CategoryId, oi.Product.Category.Name })
                .Select(g => new CategoryPerformanceDTO
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.Name,
                    ProductCount = g.Select(oi => oi.ProductId).Distinct().Count(),
                    TotalSold = g.Sum(oi => oi.Quantity),
                    Revenue = g.Sum(oi => oi.Price * oi.Quantity),
                    OrderCount = g.Select(oi => oi.OrderId).Distinct().Count()
                })
                .OrderByDescending(c => c.Revenue)
                .Take(5)
                .ToListAsync();

            // Top Selling Products (by quantity) - Only from delivered orders
            var topSellingProducts = await _context.OrderItems
                .Where(oi => !oi.IsDeleted && oi.Order.Status == OrderStatus.Delivered)
                .Include(oi => oi.Product)
                .ThenInclude(p => p.Brand)
                .Include(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .Include(oi => oi.Order)
                .GroupBy(oi => new 
                { 
                    oi.ProductId, 
                    oi.Product.Name,
                    ProductBrand = oi.Product.Brand.Name,
                    CategoryName = oi.Product.Category.Name,
                    oi.Price
                })
                .Select(g => new TopProductDTO
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.Name,
                    ProductBrand = g.Key.ProductBrand,
                    CategoryName = g.Key.CategoryName,
                    TotalSold = g.Sum(oi => oi.Quantity),
                    Revenue = g.Sum(oi => oi.Price * oi.Quantity),
                    Price = g.Key.Price
                })
                .OrderByDescending(p => p.TotalSold)
                .Take(10)
                .ToListAsync();

            // Top Revenue Products - Only from delivered orders
            var topRevenueProducts = await _context.OrderItems
                .Where(oi => !oi.IsDeleted && oi.Order.Status == OrderStatus.Delivered)
                .Include(oi => oi.Product)
                .ThenInclude(p => p.Brand)
                .Include(oi => oi.Product)
                .ThenInclude(p => p.Category)
                .Include(oi => oi.Order)
                .GroupBy(oi => new 
                { 
                    oi.ProductId, 
                    oi.Product.Name,
                    ProductBrand = oi.Product.Brand.Name,
                    CategoryName = oi.Product.Category.Name,
                    oi.Price
                })
                .Select(g => new TopProductDTO
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.Name,
                    ProductBrand = g.Key.ProductBrand,
                    CategoryName = g.Key.CategoryName,
                    TotalSold = g.Sum(oi => oi.Quantity),
                    Revenue = g.Sum(oi => oi.Price * oi.Quantity),
                    Price = g.Key.Price
                })
                .OrderByDescending(p => p.Revenue)
                .Take(10)
                .ToListAsync();

            // Recent Orders
            var recentOrders = await _context.Orders
                .Where(o => !o.IsDeleted)
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedOn)
                .Take(10)
                .Select(o => new RecentOrderDTO
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    CustomerName = o.CustomerName,
                    CustomerEmail = o.CustomerEmail,
                    CustomerPhone = o.CustomerPhone,
                    Total = o.Total,
                    Status = o.Status.ToString(),
                    PaymentMethod = o.PaymentMethod.ToString(),
                    ItemCount = o.Items.Count,
                    CreatedOn = o.CreatedOn
                })
                .ToListAsync();

            // Pending Orders
            var pendingOrders = await _context.Orders
                .Where(o => !o.IsDeleted && o.Status == OrderStatus.Pending)
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedOn)
                .Select(o => new RecentOrderDTO
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    CustomerName = o.CustomerName,
                    CustomerEmail = o.CustomerEmail,
                    CustomerPhone = o.CustomerPhone,
                    Total = o.Total,
                    Status = o.Status.ToString(),
                    PaymentMethod = o.PaymentMethod.ToString(),
                    ItemCount = o.Items.Count,
                    CreatedOn = o.CreatedOn
                })
                .ToListAsync();

            // Payment Method Stats - Only delivered orders
            var paymentMethodStats = allOrders
                .Where(o => o.Status == OrderStatus.Delivered)
                .GroupBy(o => o.PaymentMethod)
                .Select(g => new PaymentMethodStatsDTO
                {
                    PaymentMethod = g.Key.ToString(),
                    OrderCount = g.Count(),
                    TotalRevenue = g.Sum(o => o.Total),
                    Percentage = totalRevenue > 0 ? (g.Sum(o => o.Total) / totalRevenue * 100) : 0
                })
                .OrderByDescending(p => p.TotalRevenue)
                .ToList();

            // Last 7 Days Revenue - Only delivered orders
            var last7DaysRevenue = Enumerable.Range(0, 7)
                .Select(i => today.AddDays(-i))
                .Select(date => new DailyRevenueDTO
                {
                    Date = date,
                    Revenue = allOrders
                        .Where(o => o.CreatedOn.Date == date && o.Status == OrderStatus.Delivered)
                        .Sum(o => o.Total),
                    OrderCount = allOrders.Count(o => o.CreatedOn.Date == date)
                })
                .OrderBy(d => d.Date)
                .ToList();

            // Last 12 Months Revenue - Only delivered orders
            var last12MonthsRevenue = Enumerable.Range(0, 12)
                .Select(i => today.AddMonths(-i))
                .Select(date => new
                {
                    Year = date.Year,
                    Month = date.Month,
                    StartDate = new DateTime(date.Year, date.Month, 1),
                    EndDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month))
                })
                .Select(period => new MonthlyRevenueDTO
                {
                    Year = period.Year,
                    Month = period.Month,
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(period.Month),
                    Revenue = allOrders
                        .Where(o => o.CreatedOn.Date >= period.StartDate && 
                                  o.CreatedOn.Date <= period.EndDate && 
                                  o.Status == OrderStatus.Delivered)
                        .Sum(o => o.Total),
                    OrderCount = allOrders
                        .Count(o => o.CreatedOn.Date >= period.StartDate && 
                                  o.CreatedOn.Date <= period.EndDate)
                })
                .OrderBy(m => m.Year)
                .ThenBy(m => m.Month)
                .ToList();

            // Growth Metrics
            var previousWeekOrdersCount = previousWeekOrders;
            var previousMonthOrdersCount = previousMonthOrders;
            
            var previousWeekCustomers = allOrders
                .Where(o => o.CreatedOn.Date >= previousWeekStart && o.CreatedOn.Date < weekAgo)
                .Select(o => o.CustomerEmail)
                .Distinct()
                .Count();
            
            var previousMonthCustomers = allOrders
                .Where(o => o.CreatedOn.Date >= previousMonthStart && o.CreatedOn.Date < monthAgo)
                .Select(o => o.CustomerEmail)
                .Distinct()
                .Count();
            
            var weekCustomers = allOrders
                .Where(o => o.CreatedOn.Date >= weekAgo)
                .Select(o => o.CustomerEmail)
                .Distinct()
                .Count();
            
            var monthCustomers = allOrders
                .Where(o => o.CreatedOn.Date >= monthAgo)
                .Select(o => o.CustomerEmail)
                .Distinct()
                .Count();

            var previousAverageOrderValue = previousWeekOrdersCount > 0 ? previousWeekRevenue / previousWeekOrdersCount : 0;
            var currentAverageOrderValue = weekOrders > 0 ? weekRevenue / weekOrders : 0;

            var growthMetrics = new GrowthMetricsDTO
            {
                RevenueGrowthWeek = previousWeekRevenue > 0 ? ((weekRevenue - previousWeekRevenue) / previousWeekRevenue * 100) : 0,
                RevenueGrowthMonth = previousMonthRevenue > 0 ? ((monthRevenue - previousMonthRevenue) / previousMonthRevenue * 100) : 0,
                OrdersGrowthWeek = previousWeekOrdersCount > 0 ? ((decimal)(weekOrders - previousWeekOrdersCount) / previousWeekOrdersCount * 100) : 0,
                OrdersGrowthMonth = previousMonthOrdersCount > 0 ? ((decimal)(monthOrders - previousMonthOrdersCount) / previousMonthOrdersCount * 100) : 0,
                CustomersGrowthWeek = previousWeekCustomers > 0 ? ((decimal)(weekCustomers - previousWeekCustomers) / previousWeekCustomers * 100) : 0,
                CustomersGrowthMonth = previousMonthCustomers > 0 ? ((decimal)(monthCustomers - previousMonthCustomers) / previousMonthCustomers * 100) : 0,
                AverageOrderValueGrowth = previousAverageOrderValue > 0 ? ((currentAverageOrderValue - previousAverageOrderValue) / previousAverageOrderValue * 100) : 0
            };

            return new DashboardDTO
            {
                // Revenue Metrics
                TotalRevenue = totalRevenue,
                TodayRevenue = todayRevenue,
                WeekRevenue = weekRevenue,
                MonthRevenue = monthRevenue,
                YearRevenue = yearRevenue,
                AverageOrderValue = averageOrderValue,
                
                // Order Metrics
                TotalOrders = totalOrders,
                TodayOrders = todayOrders,
                WeekOrders = weekOrders,
                MonthOrders = monthOrders,
                YearOrders = yearOrders,
                
                // Order Status Breakdown
                OrderStatusBreakdown = orderStatusBreakdown,
                
                // Customer Metrics
                TotalCustomers = totalCustomers,
                NewCustomersToday = newCustomersToday,
                NewCustomersWeek = newCustomersWeek,
                NewCustomersMonth = newCustomersMonth,
                ReturningCustomers = returningCustomers,
                
                // Product Metrics
                TotalProducts = totalProducts,
                ActiveProducts = activeProducts,
                InactiveProducts = inactiveProducts,
                FeaturedProducts = featuredProducts,
                
                // Category Metrics
                TotalCategories = totalCategories,
                TopCategories = categoryPerformance,
                
                // Product Performance
                TopSellingProducts = topSellingProducts,
                TopRevenueProducts = topRevenueProducts,
                
                // Recent Activity
                RecentOrders = recentOrders,
                PendingOrders = pendingOrders,
                
                // Payment Methods
                PaymentMethodStats = paymentMethodStats,
                
                // Trends
                Last7DaysRevenue = last7DaysRevenue,
                Last12MonthsRevenue = last12MonthsRevenue,
                
                // Growth
                GrowthMetrics = growthMetrics
            };
        }
    }
}

