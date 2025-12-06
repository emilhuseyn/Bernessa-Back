@echo off
cls
echo ========================================
echo  Etir Dukani - Quick Start
echo ========================================
echo.

REM Check if we're in the src directory
if not exist "App.API\App.API.csproj" (
    echo ERROR: Please run this script from the 'src' directory
    echo Current directory: %cd%
    echo Expected: C:\Users\Emil\source\repos\Bernessa\App\src\
    pause
    exit /b 1
)

echo [1/6] Checking EF Core Tools...
dotnet ef --version >nul 2>&1
if errorlevel 1 (
    echo EF Core tools not found. Installing...
    dotnet tool install --global dotnet-ef
    if errorlevel 1 (
        echo ERROR: Failed to install EF Core tools
        pause
        exit /b 1
    )
)
echo OK!
echo.

echo [2/6] Restoring NuGet packages...
dotnet restore
if errorlevel 1 (
    echo ERROR: Failed to restore packages
    pause
    exit /b 1
)
echo OK!
echo.

echo [3/6] Building solution...
dotnet build --no-restore
if errorlevel 1 (
    echo ERROR: Build failed
    pause
    exit /b 1
)
echo OK!
echo.

echo [4/6] Creating database migration...
dotnet ef migrations add InitialCreate --project App.DAL --startup-project App.API --context AppDbContext
if errorlevel 1 (
    echo INFO: Migration might already exist
)
echo.

echo [5/6] Updating database 'etirdukani'...
echo MySQL Server: localhost:3306
echo Database: etirdukani
dotnet ef database update --project App.DAL --startup-project App.API --context AppDbContext
if errorlevel 1 (
    echo.
    echo ERROR: Database update failed!
    echo.
    echo Troubleshooting:
    echo 1. Make sure MySQL is running: net start mysql80
    echo 2. Verify database 'etirdukani' exists
    echo 3. Test connection: mysql -u root -p
    echo 4. Check password: 8ZYONaANetsaf7Zsx
    echo.
    pause
    exit /b 1
)
echo OK!
echo.

echo [6/6] Starting application...
echo.
echo ========================================
echo  Application is starting...
echo ========================================
echo  Database: etirdukani
echo  Swagger UI: https://localhost:5076/swagger
echo  API URL: https://localhost:5076
echo.
echo  Default Admin Credentials:
echo  Email: admin@admin.com
echo  Password: !Admin123.?Back3ndFr0nt3nd@
echo.
echo  Default Moderator Credentials:
echo  Email: mod@mod.com
echo  Password: !Mod123.?Back3ndFr0nt3nd@
echo ========================================
echo.
echo Press Ctrl+C to stop the server
echo.

cd App.API
dotnet run

pause
