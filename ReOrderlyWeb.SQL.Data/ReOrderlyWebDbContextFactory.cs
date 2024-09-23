using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using ReOrderlyWeb.Data.Sql;

public class ReOrderlyWebDbContextFactory : IDesignTimeDbContextFactory<ReOrderlyWebDbContext>
{
    public ReOrderlyWebDbContext CreateDbContext(string[] args)
    {
        // Ustawienie konfiguracji do odczytania connection string
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ReOrderlyWebDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer(connectionString);

        return new ReOrderlyWebDbContext(optionsBuilder.Options);
    }
}