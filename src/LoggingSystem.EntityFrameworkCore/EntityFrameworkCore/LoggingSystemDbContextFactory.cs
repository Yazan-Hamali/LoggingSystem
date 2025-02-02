using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LoggingSystem.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class LoggingSystemDbContextFactory : IDesignTimeDbContextFactory<LoggingSystemDbContext>
{
    public LoggingSystemDbContext CreateDbContext(string[] args)
    {
        LoggingSystemEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<LoggingSystemDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new LoggingSystemDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../LoggingSystem.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
