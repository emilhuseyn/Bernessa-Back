using App.DAL.Presistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace App.DAL.Presistence
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../App.API")) // API qovluğu
    .AddJsonFile("appsettings.json")
    .Build();


            var builder = new DbContextOptionsBuilder<AppDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 44))
            );

            // claimService null olaraq verilir, EF üçün kifayətdir
            return new AppDbContext(builder.Options, claimService: null);
        }
    }
}
