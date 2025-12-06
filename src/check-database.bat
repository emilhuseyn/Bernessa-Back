@echo off
cls
echo ========================================
echo  Etir Dukani - Database Status
echo ========================================
echo.

echo Database: etirdukani
echo Server: localhost:3306
echo User: root
echo.

echo Checking MySQL connection...
mysql -h localhost -P 3306 -u root -p8ZYONaANetsaf7Zsx -e "SELECT VERSION() as MySQL_Version;"

if errorlevel 1 (
    echo.
    echo ERROR: Cannot connect to MySQL
    echo Make sure MySQL is running: net start mysql80
    pause
    exit /b 1
)

echo.
echo ========================================
echo  Database Tables
echo ========================================
mysql -h localhost -P 3306 -u root -p8ZYONaANetsaf7Zsx -e "USE etirdukani; SHOW TABLES;"

echo.
echo ========================================
echo  Admin Users
echo ========================================
mysql -h localhost -P 3306 -u root -p8ZYONaANetsaf7Zsx -e "USE etirdukani; SELECT Id, UserName, Email, FirstName, LastName, EmailConfirmed FROM AspNetUsers;"

echo.
echo ========================================
echo  Categories Count
echo ========================================
mysql -h localhost -P 3306 -u root -p8ZYONaANetsaf7Zsx -e "USE etirdukani; SELECT COUNT(*) as Total_Categories FROM Categories WHERE IsDeleted = 0;"

echo.
echo ========================================
echo  Products Count
echo ========================================
mysql -h localhost -P 3306 -u root -p8ZYONaANetsaf7Zsx -e "USE etirdukani; SELECT COUNT(*) as Total_Products FROM Products WHERE IsDeleted = 0 AND IsActive = 1;"

echo.
echo ========================================
echo  Orders Count
echo ========================================
mysql -h localhost -P 3306 -u root -p8ZYONaANetsaf7Zsx -e "USE etirdukani; SELECT COUNT(*) as Total_Orders FROM Orders WHERE IsDeleted = 0;"

echo.
echo ========================================
echo  Migrations Applied
echo ========================================
mysql -h localhost -P 3306 -u root -p8ZYONaANetsaf7Zsx -e "USE etirdukani; SELECT MigrationId, ProductVersion FROM __EFMigrationsHistory;"

echo.
pause
