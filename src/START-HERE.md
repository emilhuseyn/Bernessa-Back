# ?? Etir Dukani Backend - READY TO USE!

## ? Setup Complete Summary

### ?? Your Configuration
```
Workspace: C:\Users\Emil\source\repos\Bernessa\App\src\
Database: etirdukani
MySQL Host: localhost:3306
MySQL User: root
MySQL Password: 8ZYONaANetsaf7Zsx
```

---

## ?? HOW TO START (3 Simple Steps)

### Step 1: Setup Database (First Time Only)
```cmd
cd C:\Users\Emil\source\repos\Bernessa\App\src
setup-database.bat
```

This will:
- ? Check MySQL connection
- ? Create 'etirdukani' database
- ? Verify everything is ready

### Step 2: Start the Application
```cmd
start-app.bat
```

This will:
- ? Install EF Core tools (if needed)
- ? Restore NuGet packages
- ? Build the solution
- ? Create database migrations
- ? Apply migrations to database
- ? Seed admin users
- ? Start the API server

### Step 3: Open Swagger and Test
Browser will open automatically or go to:
```
https://localhost:5076/swagger
```

---

## ?? Default Login Credentials

### Admin (Full Access)
```
Email: admin@admin.com
Password: !Admin123.?Back3ndFr0nt3nd@
```

### Moderator (Limited Access)
```
Email: mod@mod.com
Password: !Mod123.?Back3ndFr0nt3nd@
```

---

## ?? Available Scripts

### `setup-database.bat`
- Checks MySQL connection
- Creates database if not exists
- Verifies database is ready
- **Run once before first start**

### `start-app.bat`
- Complete application startup
- Handles migrations automatically
- Starts the API server
- **Run this every time you want to start the app**

### `check-database.bat`
- Shows database status
- Lists all tables
- Shows admin users
- Displays counts (categories, products, orders)
- Shows applied migrations
- **Run anytime to check database status**

### `quick-start.bat`
- Alternative quick start script
- Less verbose version

---

## ?? Quick Test Guide

### 1. Login to Get Token
**Endpoint:** `POST /api/admin/auth/login`

**Request:**
```json
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
    "accessToken": "eyJhbGc...",
    "user": {
      "email": "admin@admin.com",
      "firstName": "Admin",
      "lastName": "User",
      "role": "Admin"
    }
  }
}
```

### 2. Authorize in Swagger
1. Click "Authorize" button (?? icon)
2. Enter: `Bearer {your-access-token}`
3. Click "Authorize"
4. Click "Close"

### 3. Create Your First Category
**Endpoint:** `POST /api/categories`

**Request:**
```json
{
  "name": "Qad?n ?tirl?ri",
  "slug": "qadin-etirleri",
  "image": "https://example.com/women-perfumes.jpg"
}
```

### 4. Create Your First Product
**Endpoint:** `POST /api/products`

**Request:**
```json
{
  "name": "Chanel No. 5",
  "brand": "Chanel",
  "price": 250.00,
  "originalPrice": 300.00,
  "volume": "100ml",
  "type": "Eau de Parfum",
  "description": "Klassik v? ikonik ?tir",
  "images": [
    "https://example.com/chanel-1.jpg",
    "https://example.com/chanel-2.jpg"
  ],
  "categoryId": 1,
  "stock": 50,
  "isActive": true,
  "isFeatured": true
}
```

### 5. Test Order Creation (NO AUTH NEEDED!)
**Endpoint:** `POST /api/orders`

**Request:**
```json
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

**Response:**
```json
{
  "success": true,
  "orderNumber": "ORD-12345",
  "message": "Sifari?iniz u?urla qeyd? al?nd?",
  "trackingUrl": "/orders/track/ORD-12345"
}
```

---

## ?? Database Tables Created

After running migrations, these tables will be created:

? **Identity Tables:**
- AspNetUsers
- AspNetRoles
- AspNetUserRoles
- AspNetUserClaims
- AspNetRoleClaims
- AspNetUserLogins
- AspNetUserTokens

? **Business Tables:**
- Categories
- Products
- Orders
- OrderItems
- Settings

? **System:**
- __EFMigrationsHistory

---

## ?? What's Working

### ? Public Features (No Authentication)
- Browse all products
- View product details
- Search products
- Filter by category
- View featured products
- View deals
- **Create orders** (customers don't need accounts!)
- Track orders by order number

### ? Admin Features (JWT Authentication)
- Secure login
- Manage products (Create, Read, Update, Delete)
- Manage categories (Create, Read, Update, Delete)
- View all orders
- Update order status
- Cancel orders
- View analytics dashboard
- Stock management

### ? System Features
- Automatic stock reduction on order
- Discount code application (WELCOME, SAVE10, SAVE20)
- Tax calculation (10%)
- Order number generation
- Product snapshots in orders
- Audit tracking (who created/updated what)
- Soft delete (data preserved)
- Global error handling

---

## ?? Security Features

? JWT Bearer Authentication  
? Password Hashing (ASP.NET Identity)  
? Role-based Authorization (Admin, Moderator)  
? XSS Protection Middleware  
? Global Exception Handling  
? HTTPS Enforcement  
? CORS Configuration  

---

## ?? Built-in Discount Codes

| Code | Discount |
|------|----------|
| WELCOME | 15% |
| SAVE10 | 10% |
| SAVE20 | 20% |

---

## ?? Common Issues & Solutions

### MySQL Not Running
```cmd
net start mysql80
# or check services.msc
```

### Database Doesn't Exist
```cmd
setup-database.bat
```

### Wrong Password
Edit `App.API\appsettings.json` with correct password

### Port Already in Use
Change port in `App.API\Properties\launchSettings.json`

### EF Tools Not Installed
```cmd
dotnet tool install --global dotnet-ef
```

### Migration Already Exists
```cmd
dotnet ef migrations remove --project App.DAL --startup-project App.API
```

---

## ?? Important Files

```
src/
??? setup-database.bat          ? Setup MySQL database
??? start-app.bat              ? Start the application
??? check-database.bat          Check database status
??? quick-start.bat             Alternative start script
??? QUICK-SETUP.md             This guide
??? API-DOCUMENTATION.md        Full API documentation
??? SETUP-GUIDE.md             Detailed setup guide
??? README.md                   Project overview
??? Postman-Collection.json     API testing collection
```

---

## ? Pre-Launch Checklist

Before starting the app:
- [ ] MySQL is running (`net start mysql80`)
- [ ] You're in the correct directory (`C:\Users\Emil\source\repos\Bernessa\App\src\`)
- [ ] Database setup script run (`setup-database.bat`)

After starting the app:
- [ ] Swagger loads (https://localhost:5076/swagger)
- [ ] Can login with admin credentials
- [ ] Can create categories
- [ ] Can create products
- [ ] Can create orders (public endpoint)

---

## ?? Start Sequence

### First Time Setup:
```cmd
cd C:\Users\Emil\source\repos\Bernessa\App\src
setup-database.bat
start-app.bat
```

### Every Other Time:
```cmd
cd C:\Users\Emil\source\repos\Bernessa\App\src
start-app.bat
```

---

## ?? URLs

| Service | URL |
|---------|-----|
| API | https://localhost:5076 |
| Swagger | https://localhost:5076/swagger |
| HTTP | http://localhost:5075 |

---

## ?? Quick Commands Reference

```cmd
# Setup database (first time)
setup-database.bat

# Start application
start-app.bat

# Check database status
check-database.bat

# Start MySQL
net start mysql80

# Check EF tools
dotnet ef --version

# Build project
dotnet build

# Apply migrations only
dotnet ef database update --project App.DAL --startup-project App.API
```

---

## ?? You're Ready!

Everything is configured and ready to use:

1. ? MySQL connection configured
2. ? Database name set to 'etirdukani'
3. ? All entities created
4. ? All services implemented
5. ? All controllers ready
6. ? JWT authentication configured
7. ? Swagger documentation ready
8. ? Helper scripts created

### ?? Next Steps:

1. **Run:** `setup-database.bat`
2. **Run:** `start-app.bat`
3. **Open:** https://localhost:5076/swagger
4. **Login** with admin credentials
5. **Start building!** ??

---

**Made with ?? for Etir Dukani**

**Everything is ready, brother! Just run the scripts and start coding! ??**
