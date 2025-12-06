@echo off
echo ========================================
echo  MySQL Database Setup Check
echo ========================================
echo.

echo Checking MySQL Connection...
echo Database: etirdukani
echo Server: localhost:3306
echo User: root
echo.

echo Testing MySQL connection...
mysql -h localhost -P 3306 -u root -p8ZYONaANetsaf7Zsx -e "SELECT VERSION();"

if errorlevel 1 (
    echo.
    echo ERROR: Cannot connect to MySQL!
    echo.
    echo Troubleshooting:
    echo 1. Check if MySQL is running:
    echo    - Open Services (services.msc)
    echo    - Look for 'MySQL80' or similar
    echo    - Start it if stopped
    echo.
    echo 2. Or run: net start mysql80
    echo.
    echo 3. Verify password is correct: 8ZYONaANetsaf7Zsx
    echo.
    pause
    exit /b 1
)

echo.
echo MySQL connection OK!
echo.

echo Checking if database 'etirdukani' exists...
mysql -h localhost -P 3306 -u root -p8ZYONaANetsaf7Zsx -e "USE etirdukani; SELECT 'Database exists' as Status;"

if errorlevel 1 (
    echo.
    echo Database 'etirdukani' does not exist.
    echo Creating database...
    mysql -h localhost -P 3306 -u root -p8ZYONaANetsaf7Zsx -e "CREATE DATABASE etirdukani CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;"
    
    if errorlevel 1 (
        echo ERROR: Failed to create database
        pause
        exit /b 1
    )
    
    echo Database 'etirdukani' created successfully!
) else (
    echo Database 'etirdukani' already exists!
)

echo.
echo ========================================
echo  Database Setup Complete!
echo ========================================
echo.
echo You can now run: start-app.bat
echo.
pause
