#!/bin/bash

echo "========================================"
echo " Perfume E-Commerce - Quick Start"
echo "========================================"
echo ""

echo "[1/5] Restoring NuGet packages..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "ERROR: Failed to restore packages"
    exit 1
fi
echo "OK!"
echo ""

echo "[2/5] Building solution..."
dotnet build --no-restore
if [ $? -ne 0 ]; then
    echo "ERROR: Build failed"
    exit 1
fi
echo "OK!"
echo ""

echo "[3/5] Creating database migration..."
cd src
dotnet ef migrations add InitialCreate --project App.DAL --startup-project App.API --context AppDbContext
if [ $? -ne 0 ]; then
    echo "WARNING: Migration might already exist"
fi
echo ""

echo "[4/5] Updating database..."
dotnet ef database update --project App.DAL --startup-project App.API --context AppDbContext
if [ $? -ne 0 ]; then
    echo "ERROR: Database update failed"
    echo "Please check your connection string in appsettings.json"
    exit 1
fi
echo "OK!"
echo ""

echo "[5/5] Starting application..."
echo ""
echo "========================================"
echo " Application is starting..."
echo "========================================"
echo " Swagger UI: https://localhost:5076/swagger"
echo " API URL: https://localhost:5076"
echo ""
echo " Default Admin:"
echo " Email: admin@admin.com"
echo " Password: !Admin123.?Back3ndFr0nt3nd@"
echo "========================================"
echo ""

cd App.API
dotnet run
