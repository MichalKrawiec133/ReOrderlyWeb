using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using ReOrderlyWeb.Data.Sql;

public class ReOrderlyWebDbContextFactory : IDesignTimeDbContextFactory<ReOrderlyWebDbContext>
{
    public ReOrderlyWebDbContext CreateDbContext(string[] args)
    {
        // Pobierz aktualny katalog pracy
        var basePath = Directory.GetCurrentDirectory();

        // Zakładając, że główny projekt jest poziom wyżej w strukturze katalogów
        var projectPath = Path.Combine(Directory.GetParent(basePath).FullName, "ReOrderlyWeb");

        // Odczyt konfiguracji z pliku appsettings.json z głównego projektu
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(projectPath)  // Zmieniamy katalog na główny projekt
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ReOrderlyWebDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        optionsBuilder.UseSqlServer(connectionString);

        return new ReOrderlyWebDbContext(optionsBuilder.Options);
    }
}