# Comprehensive Analytics Dashboard API

## Overview
The Analytics Dashboard provides extensive insights into your e-commerce platform including revenue, orders, customers, products, inventory, and growth metrics.

## Endpoint

**GET** `/api/admin/analytics/dashboard`

**Authorization**: Required (Admin, SuperAdmin roles)

**Headers**:
```
Authorization: Bearer {your_jwt_token}
```

## Response Structure

### Revenue Metrics
- `totalRevenue` - All-time revenue (excluding cancelled orders)
- `todayRevenue` - Revenue from today
- `weekRevenue` - Revenue from last 7 days
- `monthRevenue` - Revenue from last 30 days
- `yearRevenue` - Revenue from last 365 days
- `averageOrderValue` - Average order value across all orders

### Order Metrics
- `totalOrders` - Total completed orders (non-cancelled)
- `todayOrders` - Orders placed today
- `weekOrders` - Orders in last 7 days
- `monthOrders` - Orders in last 30 days
- `yearOrders` - Orders in last 365 days

### Order Status Breakdown
Detailed breakdown by order status with counts and revenue:
- **Pending**: Orders awaiting processing
- **Processing**: Orders being prepared
- **Shipped**: Orders in transit
- **Delivered**: Successfully delivered orders
- **Cancelled**: Cancelled orders

Each status includes:
- Count of orders
- Total revenue for that status

### Customer Metrics
- `totalCustomers` - Unique customers (by email)
- `newCustomersToday` - First-time customers today
- `newCustomersWeek` - New customers in last 7 days
- `newCustomersMonth` - New customers in last 30 days
- `returningCustomers` - Customers with multiple orders

### Product Metrics
- `totalProducts` - Total products in catalog
- `activeProducts` - Products available for sale
- `inactiveProducts` - Disabled products
- `outOfStockProducts` - Products with 0 stock
- `lowStockProducts` - Products with stock ? 10
- `featuredProducts` - Active featured products

### Category Metrics
- `totalCategories` - Total product categories
- `topCategories` - Top 5 performing categories with:
  - Category name
  - Product count
  - Total units sold
  - Revenue generated
  - Order count

### Product Performance

#### Top Selling Products (by quantity)
Top 10 products by units sold:
- Product ID, name, brand
- Category name
- Total units sold
- Revenue generated
- Current stock level
- Product price

#### Top Revenue Products
Top 10 products by revenue:
- Same structure as top selling
- Sorted by total revenue

#### Low Stock Alerts
Up to 15 products with stock ? 10:
- Product details
- Current stock level
- Out of stock flag
- Product price

### Recent Activity

#### Recent Orders
Last 10 orders (all statuses):
- Order number, ID
- Customer details (name, email, phone)
- Total amount
- Status
- Payment method
- Item count
- Created date

#### Pending Orders
All pending orders requiring attention:
- Same structure as recent orders
- Only includes Pending status

### Payment Method Statistics
Breakdown by payment method:
- Payment method name
- Number of orders
- Total revenue
- Percentage of total revenue

### Revenue Trends

#### Last 7 Days
Daily revenue data for past week:
- Date
- Revenue
- Order count

#### Last 12 Months
Monthly revenue data for past year:
- Year, month, month name
- Revenue
- Order count

### Growth Metrics
Percentage growth comparisons:
- `revenueGrowthWeek` - Week-over-week revenue growth %
- `revenueGrowthMonth` - Month-over-month revenue growth %
- `ordersGrowthWeek` - Week-over-week order count growth %
- `ordersGrowthMonth` - Month-over-month order count growth %
- `customersGrowthWeek` - Week-over-week new customer growth %
- `customersGrowthMonth` - Month-over-month new customer growth %
- `averageOrderValueGrowth` - Week-over-week AOV growth %

## Sample Response

```json
{
  "success": true,
  "data": {
    "totalRevenue": 125450.75,
    "todayRevenue": 2340.50,
    "weekRevenue": 15680.25,
    "monthRevenue": 42300.90,
    "yearRevenue": 125450.75,
    "averageOrderValue": 89.50,
    
    "totalOrders": 1402,
    "todayOrders": 26,
    "weekOrders": 175,
    "monthOrders": 473,
    "yearOrders": 1402,
    
    "orderStatusBreakdown": {
      "pending": 12,
      "processing": 35,
      "shipped": 28,
      "delivered": 1280,
      "cancelled": 47,
      "pendingRevenue": 1074.00,
      "processingRevenue": 3129.75,
      "shippedRevenue": 2506.50,
      "deliveredRevenue": 114520.50,
      "cancelledRevenue": 4220.00
    },
    
    "totalCustomers": 856,
    "newCustomersToday": 8,
    "newCustomersWeek": 42,
    "newCustomersMonth": 123,
    "returningCustomers": 312,
    
    "totalProducts": 245,
    "activeProducts": 228,
    "inactiveProducts": 17,
    "outOfStockProducts": 8,
    "lowStockProducts": 15,
    "featuredProducts": 12,
    
    "totalCategories": 8,
    
    "topCategories": [
      {
        "categoryId": 1,
        "categoryName": "Skincare",
        "productCount": 45,
        "totalSold": 1250,
        "revenue": 45600.50,
        "orderCount": 850
      }
    ],
    
    "topSellingProducts": [
      {
        "productId": 15,
        "productName": "Hydrating Serum",
        "productBrand": "Brand A",
        "categoryName": "Skincare",
        "totalSold": 342,
        "revenue": 10260.00,
        "currentStock": 45,
        "price": 30.00
      }
    ],
    
    "topRevenueProducts": [
      {
        "productId": 23,
        "productName": "Premium Face Cream",
        "productBrand": "Brand B",
        "categoryName": "Skincare",
        "totalSold": 156,
        "revenue": 15600.00,
        "currentStock": 28,
        "price": 100.00
      }
    ],
    
    "lowStockAlerts": [
      {
        "productId": 42,
        "productName": "Eye Cream",
        "productBrand": "Brand C",
        "currentStock": 3,
        "isOutOfStock": false,
        "price": 45.00
      }
    ],
    
    "recentOrders": [
      {
        "id": 1567,
        "orderNumber": "ORD-20241202-1567",
        "customerName": "John Doe",
        "customerEmail": "john@example.com",
        "customerPhone": "+994501234567",
        "total": 89.50,
        "status": "Delivered",
        "paymentMethod": "CreditCard",
        "itemCount": 3,
        "createdOn": "2024-12-02T10:30:00Z"
      }
    ],
    
    "pendingOrders": [
      {
        "id": 1580,
        "orderNumber": "ORD-20241202-1580",
        "customerName": "Jane Smith",
        "customerEmail": "jane@example.com",
        "customerPhone": "+994501234568",
        "total": 125.00,
        "status": "Pending",
        "paymentMethod": "Cash",
        "itemCount": 4,
        "createdOn": "2024-12-02T14:20:00Z"
      }
    ],
    
    "paymentMethodStats": [
      {
        "paymentMethod": "CreditCard",
        "orderCount": 890,
        "totalRevenue": 85420.50,
        "percentage": 68.1
      },
      {
        "paymentMethod": "Cash",
        "orderCount": 420,
        "totalRevenue": 35810.25,
        "percentage": 28.5
      },
      {
        "paymentMethod": "BankTransfer",
        "orderCount": 92,
        "totalRevenue": 4220.00,
        "percentage": 3.4
      }
    ],
    
    "last7DaysRevenue": [
      {
        "date": "2024-11-26T00:00:00Z",
        "revenue": 1890.50,
        "orderCount": 21
      },
      {
        "date": "2024-11-27T00:00:00Z",
        "revenue": 2105.75,
        "orderCount": 24
      }
    ],
    
    "last12MonthsRevenue": [
      {
        "year": 2024,
        "month": 1,
        "monthName": "January",
        "revenue": 8450.00,
        "orderCount": 95
      },
      {
        "year": 2024,
        "month": 2,
        "monthName": "February",
        "revenue": 9230.50,
        "orderCount": 103
      }
    ],
    
    "growthMetrics": {
      "revenueGrowthWeek": 12.5,
      "revenueGrowthMonth": 8.3,
      "ordersGrowthWeek": 15.2,
      "ordersGrowthMonth": 10.7,
      "customersGrowthWeek": 18.5,
      "customersGrowthMonth": 14.2,
      "averageOrderValueGrowth": -2.3
    }
  }
}
```

## Usage Examples

### JavaScript/Fetch
```javascript
const response = await fetch('http://localhost:5000/api/admin/analytics/dashboard', {
  method: 'GET',
  headers: {
    'Authorization': 'Bearer YOUR_JWT_TOKEN',
    'Content-Type': 'application/json'
  }
});

const result = await response.json();
console.log('Dashboard Data:', result.data);
```

### Axios
```javascript
import axios from 'axios';

const getDashboardData = async () => {
  try {
    const response = await axios.get(
      'http://localhost:5000/api/admin/analytics/dashboard',
      {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      }
    );
    
    return response.data.data;
  } catch (error) {
    console.error('Error fetching dashboard:', error);
  }
};
```

### C#
```csharp
using var httpClient = new HttpClient();
httpClient.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", token);

var response = await httpClient.GetAsync(
    "http://localhost:5000/api/admin/analytics/dashboard"
);

var dashboard = await response.Content.ReadFromJsonAsync<DashboardResponse>();
```

## Key Features

? **Comprehensive Revenue Analytics**
- Multiple time periods (today, week, month, year)
- Revenue by order status
- Average order value tracking

? **Order Intelligence**
- Status-based breakdown
- Pending order alerts
- Historical trends

? **Customer Insights**
- New vs returning customers
- Growth tracking
- Unique customer counts

? **Inventory Management**
- Low stock alerts
- Out of stock tracking
- Product performance metrics

? **Sales Performance**
- Top selling products
- Top revenue generators
- Category performance

? **Payment Analytics**
- Method distribution
- Revenue by payment type
- Percentage breakdowns

? **Trend Analysis**
- Daily revenue (7 days)
- Monthly revenue (12 months)
- Growth metrics (week/month comparisons)

## Business Insights

### Using Growth Metrics
- **Positive growth**: Indicates improving performance
- **Negative growth**: May require attention or marketing efforts
- Compare week-over-week vs month-over-month for trend direction

### Inventory Management
- Monitor `lowStockAlerts` to prevent stockouts
- Check `outOfStockProducts` for immediate restocking
- Review `topSellingProducts` to ensure adequate stock

### Order Management
- `pendingOrders` shows orders needing immediate attention
- High `cancelled` orders may indicate issues
- Monitor order status distribution for bottlenecks

### Customer Retention
- `returningCustomers` / `totalCustomers` = retention rate
- Track new customer growth week/month
- Use customer data to identify trends

## Performance Notes

- All calculations are performed in-memory for speed
- Data is cached at the application level
- Consider adding caching for production deployments
- Large datasets (>10,000 orders) may benefit from database aggregation

## Authorization

- Requires Admin or SuperAdmin role
- JWT token must be valid and not expired
- 401 Unauthorized returned if not authenticated
- 403 Forbidden returned if insufficient permissions
