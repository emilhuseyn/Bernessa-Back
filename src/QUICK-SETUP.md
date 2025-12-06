# ?? Etir Dukani - Quick Setup Guide

## ?? Your Workspace
```
Location: C:\Users\Emil\source\repos\Bernessa\App\src\
Database: etirdukani
MySQL Port: 3306
```

---

## ? Quick Start (Easiest Way)

### Step 1: Open Terminal in src Directory
```cmd
cd C:\Users\Emil\source\repos\Bernessa\App\src
```

### Step 2: Run Setup Database Script
```cmd
setup-database.bat
```
This will:
- Check MySQL connection
- Create 'etirdukani' database if not exists
- Verify everything is ready

### Step 3: Start Application
```cmd
start-app.bat
```
This will:
- Install EF Core tools (if needed)
- Restore packages
- Build solution
- Create migrations
- Update database
- Start the API

### Step 4: Access Application
Open in browser:
- **Swagger UI**: https://localhost:5076/swagger
- **API**: https://localhost:5076

---

## ?? Login Credentials

### Admin Account
```
Email: admin@admin.com
Password: !Admin123.?Back3ndFr0nt3nd@
```

### Moderator Account
```
Email: mod@mod.com
Password: !Mod123.?Back3ndFr0nt3nd@
```

---

## ?? Manual Setup (If Scripts Don't Work)

### 1. Install EF Core Tools
```cmd
dotnet tool install --global dotnet-ef
```

### 2. Verify MySQL is Running
```cmd
net start mysql80
```
Or check in Services (services.msc)

### 3. Create Database Manually
```cmd
mysql -u root -p
# Enter password: 8ZYONaANetsaf7Zsx
```

Then in MySQL:
```sql
CREATE DATABASE etirdukani CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE etirdukani;
SHOW TABLES;
EXIT;
```

### 4. Restore Packages
```cmd
cd C:\Users\Emil\source\repos\Bernessa\App\src
dotnet restore
```

### 5. Build Solution
```cmd
dotnet build
```

### 6. Create Migration
```cmd
dotnet ef migrations add InitialCreate --project App.DAL --startup-project App.API --context AppDbContext
```

### 7. Update Database
```cmd
dotnet ef database update --project App.DAL --startup-project App.API --context AppDbContext
```

### 8. Run Application
```cmd
cd App.API
dotnet run
```

---

## ?? Test Your Setup

### 1. Open Swagger
Navigate to: https://localhost:5076/swagger

### 2. Test Login
1. Find `POST /api/admin/auth/login`
2. Click "Try it out"
3. Enter:
```json
{
  "email": "admin@admin.com",
  "password": "!Admin123.?Back3ndFr0nt3nd@"
}
```
4. Click "Execute"
5. Copy the `accessToken` from response

### 3. Authorize Swagger
1. Click "Authorize" button (top right)
2. Enter: `Bearer {paste-your-token-here}`
3. Click "Authorize"
4. Click "Close"

### 4. Test Create Category
1. Find `POST /api/categories`
2. Click "Try it out"
3. Enter:
```json
{
  "name": "Qad?n ?tirl?ri",
  "slug": "qadin-etirleri",
  "image": "https://example.com/women.jpg"
}
```
4. Click "Execute"

### 5. Test Create Product
1. Find `POST /api/products`
2. Click "Try it out"
3. Enter:
```json
{
  "name": "Chanel No. 5",
  "brand": "Chanel",
  "price": 250.00,
  "originalPrice": 300.00,
  "volume": "100ml",
  "type": "Eau de Parfum",
  "description": "Classic fragrance",
  "images": ["https://example.com/chanel.jpg"],
  "categoryId": 1,
  "stock": 50,
  "isActive": true,
  "isFeatured": true
}
```
4. Click "Execute"

---

## ?? Troubleshooting

### Problem: "MySQL service not found"
**Solution:**
```cmd
# Find your MySQL service name
services.msc
# Look for MySQL80, MySQL, or similar
# Start it manually or run:
net start mysql80
```

### Problem: "Access denied for user 'root'"
**Solution:**
- Verify password: `8ZYONaANetsaf7Zsx`
- Test manually: `mysql -u root -p`
- If password is wrong, update in `App.API\appsettings.json`

### Problem: "Database 'etirdukani' doesn't exist"
**Solution:**
```cmd
setup-database.bat
```
Or create manually:
```sql
CREATE DATABASE etirdukani CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

### Problem: "Port 5076 already in use"
**Solution:**
Edit `App.API\Properties\launchSettings.json`:
```json
{
  "profiles": {
    "https": {
      "applicationUrl": "https://localhost:7076;http://localhost:5075"
    }
  }
}
```

### Problem: "EF Core tools not found"
**Solution:**
```cmd
dotnet tool install --global dotnet-ef
# or update
dotnet tool update --global dotnet-ef
```

### Problem: Migration already exists
**Solution:**
```cmd
# Remove existing migration
dotnet ef migrations remove --project App.DAL --startup-project App.API

# Create new one
dotnet ef migrations add InitialCreate --project App.DAL --startup-project App.API
```

### Problem: Cannot connect to database during migration
**Solution:**
1. Check MySQL is running: `net start mysql80`
2. Verify connection string in `appsettings.json`
3. Test connection: `mysql -u root -p8ZYONaANetsaf7Zsx`
4. Check firewall settings

---

## ?? Verify Database Tables

After successful migration, check tables:
```cmd
mysql -u root -p8ZYONaANetsaf7Zsx -e "USE etirdukani; SHOW TABLES;"
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
- __EFMigrationsHistory

---

## ?? Reset Everything

If you want to start fresh:

### 1. Drop Database
```sql
mysql -u root -p8ZYONaANetsaf7Zsx -e "DROP DATABASE etirdukani;"
```

### 2. Remove Migrations Folder
```cmd
rd /s /q App.DAL\Migrations
```

### 3. Run Setup Again
```cmd
setup-database.bat
start-app.bat
```

---

## ?? Project Structure

```
src/
??? App.API/              # Controllers, Middleware
??? App.Business/         # Services, DTOs
??? App.DAL/             # Repositories, DbContext
??? App.Core/            # Entities, Enums
??? App.Shared/          # Shared code
??? setup-database.bat   # Database setup script
??? start-app.bat        # Application start script
```

---

## ? Checklist

Before running the app:
- [ ] MySQL is running on port 3306
- [ ] Database 'etirdukani' exists
- [ ] Connection string is correct in appsettings.json
- [ ] EF Core tools installed (`dotnet ef --version`)
- [ ] In correct directory: `C:\Users\Emil\source\repos\Bernessa\App\src\`

After successful start:
- [ ] Swagger loads at https://localhost:5076/swagger
- [ ] Can login with admin credentials
- [ ] Can create categories
- [ ] Can create products
- [ ] Can create orders

---

## ?? Next Steps

1. ? Run `setup-database.bat`
2. ? Run `start-app.bat`
3. ? Open Swagger: https://localhost:5076/swagger
4. ? Test login
5. ? Create sample data (categories, products)
6. ? Test order creation
7. ?? Connect your React frontend!

---

## ?? Quick Commands

```cmd
# Setup database
setup-database.bat

# Start app
start-app.bat

# Check MySQL
net start mysql80

# Check EF tools
dotnet ef --version

# Build only
dotnet build

# Run migrations
dotnet ef database update --project App.DAL --startup-project App.API
```

---

**?? You're all set! Run `start-app.bat` and start building! ??**
