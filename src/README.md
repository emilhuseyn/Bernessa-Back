# ?? Perfume E-Commerce Platform

Modern, full-featured e-commerce platform for perfume sales built with **.NET 8** and **React + TypeScript**.

## ? Features

### ??? Customer Features (No Registration Required)
- Browse perfumes by category
- Search and filter products
- View product details with images
- Add to cart (localStorage)
- Checkout with contact information
- Track order status
- Apply discount codes

### ????? Admin Panel Features
- Secure JWT authentication
- Dashboard with analytics
- Product management (CRUD)
- Category management (CRUD)
- Order management
- View sales statistics
- Stock management

### ?? Technical Features
- N-Tier Architecture (Clean Architecture)
- Repository Pattern
- JWT Authentication
- Role-based Authorization (Admin, Moderator)
- RESTful API
- Swagger Documentation
- Global Exception Handling
- XSS Protection
- CORS Enabled
- Entity Framework Core
- MySQL Database
- AutoMapper
- FluentValidation

---

## ??? Architecture

```
???????????????????????????????????????????????
?          Frontend (React + TS)              ?
?    (Not included in this backend repo)      ?
???????????????????????????????????????????????
                  ? HTTPS/REST API
???????????????????????????????????????????????
?            App.API (Controllers)             ?
?         Middlewares, Authentication          ?
???????????????????????????????????????????????
                  ?
???????????????????????????????????????????????
?       App.Business (Services, DTOs)         ?
?        Business Logic, Validation            ?
???????????????????????????????????????????????
                  ?
???????????????????????????????????????????????
?     App.DAL (Repositories, DbContext)       ?
?         Data Access, Migrations              ?
???????????????????????????????????????????????
                  ?
???????????????????????????????????????????????
?        App.Core (Entities, Enums)           ?
?            Domain Models                     ?
???????????????????????????????????????????????
```

---

## ?? Quick Start

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL 8.0+](https://dev.mysql.com/downloads/)
- (Optional) [Visual Studio 2022](https://visualstudio.microsoft.com/)

### Installation

#### Option 1: Using Quick Start Script (Windows)
```bash
quick-start.bat
```

#### Option 2: Using Quick Start Script (Linux/Mac)
```bash
chmod +x quick-start.sh
./quick-start.sh
```

#### Option 3: Manual Setup
```bash
# 1. Restore packages
dotnet restore

# 2. Update appsettings.json with your MySQL connection
# Edit: src/App.API/appsettings.json

# 3. Create migration
cd src
dotnet ef migrations add InitialCreate --project App.DAL --startup-project App.API

# 4. Update database
dotnet ef database update --project App.DAL --startup-project App.API

# 5. Run application
cd App.API
dotnet run
```

### Access Points
- **API**: https://localhost:5076
- **Swagger**: https://localhost:5076/swagger

---

## ?? Default Credentials

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@admin.com | !Admin123.?Back3ndFr0nt3nd@ |
| Moderator | mod@mod.com | !Mod123.?Back3ndFr0nt3nd@ |

---

## ?? Documentation

- **[API Documentation](API-DOCUMENTATION.md)** - Complete API endpoints reference
- **[Setup Guide](SETUP-GUIDE.md)** - Detailed installation and troubleshooting
- **[Postman Collection](Postman-Collection.json)** - Import into Postman for testing

---

## ??? Project Structure

```
src/
??? App.API/                    # Presentation Layer
?   ??? Controllers/           # API Controllers
?   ??? Middlewares/          # Custom Middlewares
?   ??? Program.cs            # Application Entry Point
?
??? App.Business/               # Business Logic Layer
?   ??? Services/             # Business Services
?   ?   ??? Implementations/  # Service Implementations
?   ?   ??? Interfaces/       # Service Contracts
?   ??? DTOs/                 # Data Transfer Objects
?   ?   ??? Auth/
?   ?   ??? Products/
?   ?   ??? Categories/
?   ?   ??? Orders/
?   ?   ??? Analytics/
?   ??? MappingProfiles/      # AutoMapper Profiles
?   ??? Validators/           # FluentValidation Rules
?
??? App.DAL/                    # Data Access Layer
?   ??? Repositories/         # Repository Pattern
?   ?   ??? Implementations/
?   ?   ??? Interfaces/
?   ??? Configurations/       # EF Core Configurations
?   ??? Migrations/           # Database Migrations
?   ??? Presistence/          # DbContext, Seeding
?
??? App.Core/                   # Domain Layer
?   ??? Entities/             # Domain Models
?   ?   ??? Identity/        # User, Roles
?   ?   ??? Commons/         # Base Entities
?   ??? Enums/               # Enumerations
?   ??? Exceptions/          # Custom Exceptions
?
??? App.Shared/                 # Shared Layer
    ??? Interfaces/          # Shared Contracts
    ??? Implementations/     # Shared Services
```

---

## ?? API Endpoints Overview

### Public Endpoints (No Auth)
```http
GET    /api/products                     # All products
GET    /api/products/{id}                # Product details
GET    /api/products/featured            # Featured products
GET    /api/products/category/{slug}     # Products by category
GET    /api/products/search?q={query}    # Search products
GET    /api/products/deals               # Discounted products
GET    /api/categories                   # All categories
POST   /api/orders                       # Create order
GET    /api/orders/track/{orderNumber}   # Track order
```

### Admin Endpoints (Auth Required)
```http
POST   /api/admin/auth/login             # Admin login
GET    /api/admin/auth/me                # Current user
POST   /api/admin/products               # Create product
PUT    /api/admin/products/{id}          # Update product
DELETE /api/admin/products/{id}          # Delete product
POST   /api/admin/categories             # Create category
GET    /api/admin/orders                 # All orders
PUT    /api/admin/orders/{id}/status     # Update order status
GET    /api/admin/analytics/dashboard    # Dashboard stats
```

---

## ?? Key Features Explained

### Order Creation Flow
1. Customer fills order form (no registration)
2. Backend validates stock availability
3. Calculates totals (subtotal + tax - discount)
4. Generates unique order number (ORD-XXXXX)
5. Creates order with product snapshots
6. Reduces stock automatically
7. Returns order confirmation

### Discount System
Built-in discount codes:
- `WELCOME` - 15% off
- `SAVE10` - 10% off
- `SAVE20` - 20% off

### Stock Management
- Real-time stock tracking
- Automatic stock reduction on order
- Stock validation before order creation
- Low stock alerts (can be implemented)

### Security
- JWT Bearer Authentication
- Password hashing via ASP.NET Identity
- Role-based authorization
- Global exception handling
- XSS protection middleware
- HTTPS enforcement

---

## ?? Testing

### Using Swagger UI
1. Open https://localhost:5076/swagger
2. Click "Authorize" button
3. Login via `/api/admin/auth/login`
4. Copy the `accessToken`
5. Paste in format: `Bearer {your-token}`
6. Test any endpoint

### Using Postman
1. Import `Postman-Collection.json`
2. Run "Login" request
3. Token is auto-saved
4. Test other endpoints

### Sample Test Flow
```bash
# 1. Login
POST /api/admin/auth/login
{
  "email": "admin@admin.com",
  "password": "!Admin123.?Back3ndFr0nt3nd@"
}

# 2. Create Category
POST /api/categories (with token)
{
  "name": "Qad?n ?tirl?ri",
  "slug": "qadin-etirleri",
  "image": "url-here"
}

# 3. Create Product
POST /api/products (with token)
{
  "name": "Chanel No. 5",
  "brand": "Chanel",
  "price": 250,
  "categoryId": 1,
  "stock": 50,
  ...
}

# 4. Create Order (NO TOKEN NEEDED)
POST /api/orders
{
  "customerName": "Test User",
  "customerEmail": "test@test.com",
  "customerPhone": "+994501234567",
  "shippingAddress": "Test Address",
  "items": [{"productId": 1, "quantity": 2}],
  "paymentMethod": "Cash"
}
```

---

## ?? Database Migrations

### Create New Migration
```bash
dotnet ef migrations add MigrationName --project App.DAL --startup-project App.API
```

### Update Database
```bash
dotnet ef database update --project App.DAL --startup-project App.API
```

### Remove Last Migration
```bash
dotnet ef migrations remove --project App.DAL --startup-project App.API
```

### Reset Database
```bash
dotnet ef database drop --project App.DAL --startup-project App.API --force
dotnet ef database update --project App.DAL --startup-project App.API
```

---

## ?? CORS Configuration

Configured for common frontend ports:
- React Dev: `http://localhost:3000`
- Vite Dev: `http://localhost:5173`
- Additional: `http://localhost:5174`

To add more origins, edit `Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("YOUR_FRONTEND_URL")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
```

---

## ?? Troubleshooting

### Database Connection Failed
- Check MySQL is running
- Verify connection string in `appsettings.json`
- Test connection: `mysql -u root -p`

### Migration Errors
- Ensure you're in `src` directory
- Check EF Core tools installed: `dotnet ef --version`
- Try removing and recreating migration

### JWT Token Issues
- Token expires in 60 minutes
- Check Authorization header format: `Bearer {token}`
- Verify JWT config in `appsettings.json`

### CORS Errors
- Add your frontend URL to CORS policy
- Check origin matches exactly (http vs https)
- Restart API after CORS changes

More troubleshooting in [SETUP-GUIDE.md](SETUP-GUIDE.md)

---

## ?? NuGet Packages

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
- Microsoft.EntityFrameworkCore.Tools
- Pomelo.EntityFrameworkCore.MySql

---

## ?? Contributing

1. Fork the project
2. Create feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open Pull Request

---

## ?? License

This project is licensed under the MIT License.

---

## ????? Author

**Your Name**
- GitHub: [@yourusername](https://github.com/yourusername)
- Email: your.email@example.com

---

## ?? Acknowledgments

- ASP.NET Core Team
- Entity Framework Core Team
- Community Contributors

---

## ?? Support

- ?? [Documentation](API-DOCUMENTATION.md)
- ?? [Issue Tracker](https://github.com/yourusername/project/issues)
- ?? [Discussions](https://github.com/yourusername/project/discussions)

---

**Made with ?? for the perfume industry**
