@echo off
cls
echo ========================================
echo  Complete Database Reset
echo ========================================
echo.
echo WARNING: This will delete ALL data!
echo.
set /p confirm="Are you sure? (yes/no): "
if /i not "%confirm%"=="yes" (
    echo Cancelled.
    pause
    exit /b 0
)

echo.
echo [1/5] Dropping database...
mysql -h localhost -P 3306 -u root -p8ZYONaANetsaf7Zsx -e "DROP DATABASE IF EXISTS etirdukani;"
if errorlevel 1 (
    echo ERROR: Failed to drop database
    pause
    exit /b 1
)
echo Database dropped!

echo.
echo [2/5] Creating fresh database...
mysql -h localhost -P 3306 -u root -p8ZYONaANetsaf7Zsx -e "CREATE DATABASE etirdukani CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;"
if errorlevel 1 (
    echo ERROR: Failed to create database
    pause
    exit /b 1
)
echo Database created!

echo.
echo [3/5] Removing old migrations...
rd /s /q App.DAL\Migrations 2>nul
echo Migrations removed!

echo.
echo [4/5] Creating new migration...
dotnet ef migrations add InitialCreate --project App.DAL --startup-project App.API --context AppDbContext
if errorlevel 1 (
    echo ERROR: Failed to create migration
    pause
    exit /b 1
)
echo Migration created!

echo.
echo [5/5] Applying migration...
dotnet ef database update --project App.DAL --startup-project App.API --context AppDbContext
if errorlevel 1 (
    echo ERROR: Failed to apply migration
    pause
    exit /b 1
)
echo Migration applied!

echo.
echo ========================================
echo  Database Reset Complete!
echo ========================================
echo.
echo Database 'etirdukani' has been reset.
echo All tables recreated.
echo Ready to seed users on next start.
echo.
echo Run: start-app.bat
echo.
pause
