# Perfume E-Commerce Platform - Setup Guide

## ?? Table of Contents
1. [Prerequisites](#prerequisites)
2. [Installation](#installation)
3. [Database Setup](#database-setup)
4. [Running the Application](#running-the-application)
5. [Testing](#testing)
6. [Troubleshooting](#troubleshooting)

---

## Prerequisites

### Required Software
- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **MySQL Server 8.0+** - [Download](https://dev.mysql.com/downloads/)
- **Visual Studio 2022** or **VS Code** with C# extension
- **Postman** or similar API testing tool (optional)

### Verify Installation
```bash
dotnet --version
# Should show: 8.0.x or higher

mysql --version
# Should show: mysql Ver 8.0.x
```

---

## Installation

### 1. Clone or Extract Project
```bash
cd /path/to/project
```

### 2. Restore NuGet Packages
```bash
cd src
dotnet restore
```

### 3. Build Solution
```bash
dotnet build
```

---

## Database Setup

### 1. Create MySQL Database
Open MySQL command line or MySQL Workbench:
```sql
CREATE DATABASE perfume_shop CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### 2. Update Connection String
Edit `src/App.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=perfume_shop;user=root;password=YOUR_PASSWORD"
  }
}
```

**Important:** 
- Change `YOUR_PASSWORD` to your MySQL root password
- If using port other than 3306, update the `port` value
- Change `perfume_shop` to your preferred database name

### 3. Install EF Core Tools (if not installed)
```bash
dotnet tool install --global dotnet-ef
# or update
dotnet tool update --global dotnet-ef
```

### 4. Create Migration
```bash
cd src
dotnet ef migrations add InitialCreate --project App.DAL --startup-project App.API --context AppDbContext
```

### 5. Update Database
```bash
dotnet ef database update --project App.DAL --startup-project App.API --context AppDbContext
```

**Expected Output:**
```
Applying migration '20240xxx_InitialCreate'.
Done.
```

### 6. Verify Database
Check MySQL database:
```sql
USE perfume_shop;
SHOW TABLES;
```

You should see:
- AspNetUsers
- AspNetRoles
- AspNetUserRoles
- Categories
- Products
- Orders
- OrderItems
- Settings
- etc.

---

## Running the Application

### Option 1: Using Visual Studio
1. Open `App.sln`
2. Set `App.API` as startup project
3. Press F5 or click "Run"

### Option 2: Using Command Line
```bash
cd src/App.API
dotnet run
```

### Option 3: Using dotnet watch (Auto-reload)
```bash
cd src/App.API
dotnet watch run
```

### Access Points
- **API**: https://localhost:5076
- **Swagger UI**: https://localhost:5076/swagger
- **HTTP**: http://localhost:5075

---

## Testing

### 1. Test API is Running
Open browser: https://localhost:5076/swagger

### 2. Test Admin Login

#### Using Swagger:
1. Go to https://localhost:5076/swagger
2. Find `POST /api/admin/auth/login`
3. Click "Try it out"
4. Enter:
```json
{
  "email": "admin@admin.com",
  "password": "!Admin123.?Back3ndFr0nt3nd@"
}
```
5. Click "Execute"

#### Using Postman:
```http
POST https://localhost:5076/api/admin/auth/login
Content-Type: application/json

{
  "email": "admin@admin.com",
  "password": "!Admin123.?Back3ndFr0nt3nd@"
}
```

**Expected Response:**
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGc...",
    "refreshToken": "abc123...",
    "expiresAt": "2024-01-01T12:00:00Z",
    "user": {
      "id": "guid-here",
      "email": "admin@admin.com",
      "firstName": "Admin",
      "lastName": "User",
      "avatar": null,
      "role": "Admin"
    }
  }
}
```

### 3. Test Creating a Category

Copy the `accessToken` from login response, then:

```http
POST https://localhost:5076/api/categories
Authorization: Bearer {paste-token-here}
Content-Type: application/json

{
  "name": "Qad?n ?tirl?ri",
  "slug": "qadin-etirleri",
  "image": "https://example.com/women-perfumes.jpg"
}
```

### 4. Test Creating a Product

```http
POST https://localhost:5076/api/products
Authorization: Bearer {paste-token-here}
Content-Type: application/json

{
  "name": "Chanel No. 5",
  "brand": "Chanel",
  "price": 250.00,
  "originalPrice": 300.00,
  "volume": "100ml",
  "type": "Eau de Parfum",
  "description": "Classic and iconic fragrance",
  "images": [
    "https://example.com/chanel1.jpg",
    "https://example.com/chanel2.jpg"
  ],
  "categoryId": 1,
  "stock": 50,
  "isActive": true,
  "isFeatured": true
}
```

### 5. Test Creating an Order (No Auth Needed)

```http
POST https://localhost:5076/api/orders
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
    }
  ],
  "paymentMethod": "Cash",
  "discountCode": "WELCOME"
}
```

---

## Troubleshooting

### Problem: Migration Fails
**Error:** "Unable to create an object of type 'AppDbContext'"

**Solution:**
```bash
# Make sure you're in the src directory
cd src

# Specify the startup project explicitly
dotnet ef migrations add InitialCreate --project App.DAL --startup-project App.API
```

---

### Problem: Connection Refused
**Error:** "Unable to connect to MySQL server"

**Solutions:**
1. Check MySQL is running:
```bash
# Windows
net start mysql80

# Linux/Mac
sudo systemctl start mysql
```

2. Verify connection string in `appsettings.json`
3. Test connection:
```bash
mysql -u root -p
```

---

### Problem: Port Already in Use
**Error:** "Address already in use"

**Solution:** Change port in `App.API/Properties/launchSettings.json`:
```json
{
  "profiles": {
    "https": {
      "applicationUrl": "https://localhost:7076;http://localhost:5075"
    }
  }
}
```

---

### Problem: JWT Token Invalid
**Error:** "401 Unauthorized"

**Solutions:**
1. Check token is included in header:
```
Authorization: Bearer {your-token-here}
```

2. Token might be expired (60 min default). Login again.

3. Verify JWT configuration in `appsettings.json`

---

### Problem: CORS Error (Frontend)
**Error:** "has been blocked by CORS policy"

**Solution:** Add your frontend URL to CORS policy in `Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",   // React default
            "http://localhost:5173",   // Vite default
            "http://localhost:4200",   // Angular default
            "YOUR_FRONTEND_URL"        // Add your URL here
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});
```

---

### Problem: Database Already Exists
**Error:** "Database already exists"

**Solution:**
```sql
DROP DATABASE perfume_shop;
CREATE DATABASE perfume_shop CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

Then run migrations again.

---

### Problem: Seeded Users Not Created
**Solution:**
Check `AutomatedMigration.cs` is called in `Program.cs`:
```csharp
using var scope = app.Services.CreateScope();
await AutomatedMigration.MigrateAsync(scope.ServiceProvider);
```

Restart the application.

---

## Default Credentials

### Admin Account
- **Email:** admin@admin.com
- **Password:** !Admin123.?Back3ndFr0nt3nd@
- **Role:** Admin

### Moderator Account
- **Email:** mod@mod.com
- **Password:** !Mod123.?Back3ndFr0nt3nd@
- **Role:** Moderator

---

## Next Steps

1. ? Test all endpoints using Swagger
2. ? Create sample categories and products
3. ? Test order creation flow
4. ? Check analytics dashboard
5. ? Connect your frontend application

---

## Project Structure

```
src/
??? App.API/              # API Layer (Controllers, Middleware)
??? App.Business/         # Business Logic (Services, DTOs)
??? App.Core/            # Domain Layer (Entities, Enums)
??? App.DAL/             # Data Access Layer (Repositories, DbContext)
??? App.Shared/          # Shared Code (Interfaces, Helpers)
```

---

## Useful Commands

### Rebuild Database
```bash
# Drop database
dotnet ef database drop --project App.DAL --startup-project App.API --force

# Recreate
dotnet ef database update --project App.DAL --startup-project App.API
```

### Add New Migration
```bash
dotnet ef migrations add MigrationName --project App.DAL --startup-project App.API
```

### Remove Last Migration
```bash
dotnet ef migrations remove --project App.DAL --startup-project App.API
```

### List Migrations
```bash
dotnet ef migrations list --project App.DAL --startup-project App.API
```

---

## Support

For issues or questions:
1. Check this guide
2. Review API documentation (API-DOCUMENTATION.md)
3. Check Swagger UI for endpoint details
4. Review error logs in console

---

**Happy Coding! ??**
