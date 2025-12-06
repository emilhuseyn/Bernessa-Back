# Perfume E-Commerce API Documentation

## ?? Getting Started

### Prerequisites
- .NET 8 SDK
- MySQL Server (Version 8.0+)
- Visual Studio 2022 or VS Code

### Database Setup
1. Update connection string in `App.API/appsettings.json`
2. Run migrations:
```bash
cd src
dotnet ef migrations add InitialCreate --project App.DAL --startup-project App.API
dotnet ef database update --project App.DAL --startup-project App.API
```

### Running the Application
```bash
cd src/App.API
dotnet run
```

The API will be available at: `https://localhost:5076`
Swagger UI: `https://localhost:5076/swagger`

---

## ?? Authentication

### Default Admin Credentials
- **Email**: admin@admin.com
- **Password**: !Admin123.?Back3ndFr0nt3nd@

### Default Moderator Credentials
- **Email**: mod@mod.com
- **Password**: !Mod123.?Back3ndFr0nt3nd@

---

## ?? API Endpoints

### Authentication (Admin Panel)

#### Login
```http
POST /api/admin/auth/login
Content-Type: application/json

{
  "email": "admin@admin.com",
  "password": "!Admin123.?Back3ndFr0nt3nd@"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1...",
    "refreshToken": "abc123...",
    "expiresAt": "2024-01-01T12:00:00Z",
    "user": {
      "id": "guid",
      "email": "admin@admin.com",
      "firstName": "Admin",
      "lastName": "User",
      "avatar": null,
      "role": "Admin"
    }
  }
}
```

#### Get Current User
```http
GET /api/admin/auth/me
Authorization: Bearer {token}
```

#### Update Profile
```http
PUT /api/admin/auth/profile
Authorization: Bearer {token}
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "avatar": "https://example.com/avatar.jpg"
}
```

#### Change Password
```http
POST /api/admin/auth/change-password
Authorization: Bearer {token}
Content-Type: application/json

{
  "currentPassword": "OldPassword123",
  "newPassword": "NewPassword123"
}
```

---

### Products (Public)

#### Get All Products
```http
GET /api/products
```

#### Get Product by ID
```http
GET /api/products/{id}
```

#### Get Featured Products
```http
GET /api/products/featured
```

#### Get Products by Category
```http
GET /api/products/category/{slug}
```

**Example:**
```http
GET /api/products/category/qadin-etirleri
```

#### Search Products
```http
GET /api/products/search?q=chanel
```

#### Get Deals (Discounted Products)
```http
GET /api/products/deals
```

---

### Products (Admin)

#### Create Product
```http
POST /api/products
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Chanel No. 5",
  "brand": "Chanel",
  "price": 250.00,
  "originalPrice": 300.00,
  "volume": "100ml",
  "type": "Eau de Parfum",
  "description": "Classic fragrance...",
  "images": [
    "https://example.com/image1.jpg",
    "https://example.com/image2.jpg"
  ],
  "categoryId": 1,
  "stock": 50,
  "isActive": true,
  "isFeatured": true
}
```

#### Update Product
```http
PUT /api/products/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Chanel No. 5",
  "brand": "Chanel",
  "price": 240.00,
  ...
}
```

#### Delete Product
```http
DELETE /api/products/{id}
Authorization: Bearer {token}
```

---

### Categories

#### Get All Categories
```http
GET /api/categories
```

#### Get Category by ID
```http
GET /api/categories/{id}
```

#### Get Category by Slug
```http
GET /api/categories/slug/{slug}
```

#### Create Category (Admin)
```http
POST /api/categories
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Qad?n ?tirl?ri",
  "slug": "qadin-etirleri",
  "image": "https://example.com/category.jpg"
}
```

#### Update Category (Admin)
```http
PUT /api/categories/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Ki?i ?tirl?ri",
  "slug": "kisi-etirleri",
  "image": "https://example.com/category.jpg"
}
```

#### Delete Category (Admin)
```http
DELETE /api/categories/{id}
Authorization: Bearer {token}
```

---

### Orders

#### Create Order (Public - No Authentication)
```http
POST /api/orders
Content-Type: application/json

{
  "customerName": "?li M?mm?dov",
  "customerEmail": "ali@example.com",
  "customerPhone": "+994501234567",
  "shippingAddress": "Bak?, N?simi rayonu, Azadl?q prospekti 12",
  "customerNote": "Saat 18:00-dan sonra çatd?r?n",
  "items": [
    {
      "productId": 1,
      "quantity": 2
    },
    {
      "productId": 3,
      "quantity": 1
    }
  ],
  "paymentMethod": "Cash",
  "discountCode": "WELCOME"
}
```

**Response:**
```json
{
  "success": true,
  "orderNumber": "ORD-12345",
  "message": "Sifari?iniz u?urla qeyd? al?nd?",
  "trackingUrl": "/orders/track/ORD-12345",
  "data": {
    "id": 1,
    "orderNumber": "ORD-12345",
    "customerName": "?li M?mm?dov",
    "subtotal": 500.00,
    "tax": 50.00,
    "discount": 75.00,
    "total": 475.00,
    "status": "Pending",
    ...
  }
}
```

#### Track Order (Public)
```http
GET /api/orders/track/{orderNumber}
```

**Example:**
```http
GET /api/orders/track/ORD-12345
```

#### Get All Orders (Admin)
```http
GET /api/orders
Authorization: Bearer {token}
```

#### Get Order by ID (Admin)
```http
GET /api/orders/{id}
Authorization: Bearer {token}
```

#### Update Order Status (Admin/Moderator)
```http
PUT /api/orders/{id}/status
Authorization: Bearer {token}
Content-Type: application/json

{
  "status": "Processing"
}
```

**Order Statuses:**
- `Pending` - Gözl?yir
- `Processing` - ??l?nir
- `Shipped` - Gönd?rilib
- `Delivered` - Çatd?r?l?b
- `Cancelled` - L??v edilib

#### Cancel Order (Admin)
```http
DELETE /api/orders/{id}
Authorization: Bearer {token}
```

---

### Analytics (Admin Dashboard)

#### Get Dashboard Data
```http
GET /api/admin/analytics/dashboard
Authorization: Bearer {token}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "totalRevenue": 15000.00,
    "todayOrders": 25,
    "totalCustomers": 150,
    "totalProducts": 80,
    "recentOrders": [
      {
        "id": 1,
        "orderNumber": "ORD-12345",
        "customerName": "?li M?mm?dov",
        "total": 475.00,
        "status": "Pending",
        "createdOn": "2024-01-01T10:00:00Z"
      },
      ...
    ],
    "topProducts": [
      {
        "productId": 5,
        "productName": "Chanel No. 5",
        "totalSold": 150,
        "revenue": 37500.00
      },
      ...
    ]
  }
}
```

---

## ?? Payment Methods
- `Cash` - Na?d öd?ni?
- `Card` - Kart il? öd?ni?
- `BankTransfer` - Bank köçürm?si

---

## ?? Discount Codes
- `SAVE10` - 10% endirim
- `SAVE20` - 20% endirim
- `WELCOME` - 15% endirim

---

## ?? Business Logic

### Order Creation Process
1. Validate all products exist and have sufficient stock
2. Calculate subtotal from product prices
3. Apply discount code (if provided)
4. Calculate tax (10% of subtotal)
5. Calculate total (subtotal + tax - discount)
6. Generate unique order number (ORD-XXXXX)
7. Create order items with product snapshots
8. Reduce stock for each product
9. Return order confirmation

### Stock Management
- Stock is automatically reduced when order is created
- Stock is checked before order creation
- If stock is insufficient, order is rejected

### Order Status Flow
```
Pending ? Processing ? Shipped ? Delivered
         ?
    Cancelled (anytime)
```

---

## ?? Authorization Roles

### Admin
- Full access to all endpoints
- Can manage products, categories, orders
- Can view analytics

### Moderator
- Can view and update order status
- Cannot delete orders or manage products

### Public (No Auth)
- Can view products and categories
- Can create orders
- Can track orders

---

## ??? Security Features
- JWT Bearer Token Authentication
- Password Hashing (ASP.NET Identity)
- Role-based Authorization
- XSS Protection Middleware
- Global Exception Handling
- HTTPS Enforcement
- CORS Configuration

---

## ?? Error Response Format
```json
{
  "success": false,
  "errors": [
    "Error message here"
  ]
}
```

**HTTP Status Codes:**
- `200` - OK
- `201` - Created
- `400` - Bad Request
- `401` - Unauthorized
- `403` - Forbidden
- `404` - Not Found
- `500` - Internal Server Error

---

## ??? Architecture

### N-Tier Structure
```
App.API          - Controllers, Middlewares, Program.cs
App.Business     - Services, DTOs, Validators, AutoMapper
App.DAL          - Repositories, DbContext, Migrations, Configurations
App.Core         - Entities, Enums, Exceptions
App.Shared       - Shared Interfaces and Implementations
```

### Design Patterns Used
- Repository Pattern
- Dependency Injection
- Unit of Work (via DbContext)
- DTO Pattern
- Service Layer Pattern

---

## ?? Testing

### Test Admin Login
```bash
curl -X POST https://localhost:5076/api/admin/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@admin.com",
    "password": "!Admin123.?Back3ndFr0nt3nd@"
  }'
```

### Test Create Order
```bash
curl -X POST https://localhost:5076/api/orders \
  -H "Content-Type: application/json" \
  -d '{
    "customerName": "Test User",
    "customerEmail": "test@test.com",
    "customerPhone": "+994501234567",
    "shippingAddress": "Test Address",
    "items": [{"productId": 1, "quantity": 1}],
    "paymentMethod": "Cash"
  }'
```

---

## ?? NuGet Packages Required

### App.API
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.EntityFrameworkCore.Design
- Swashbuckle.AspNetCore

### App.Business
- AutoMapper
- AutoMapper.Extensions.Microsoft.DependencyInjection
- FluentValidation.AspNetCore
- Newtonsoft.Json

### App.DAL
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.EntityFrameworkCore
- Pomelo.EntityFrameworkCore.MySql

---

## ?? CORS Configuration

Update `Program.cs` to enable CORS for frontend:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

// After app.Build()
app.UseCors("AllowFrontend");
```

---

## ?? Support
For issues or questions, please contact the development team.

---

**Version:** 1.0.0  
**Last Updated:** 2024
