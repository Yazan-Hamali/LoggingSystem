using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LoggingSystem.Data;
using Volo.Abp.DependencyInjection;

namespace LoggingSystem.EntityFrameworkCore;

public class EntityFrameworkCoreLoggingSystemDbSchemaMigrator
    : ILoggingSystemDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreLoggingSystemDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the LoggingSystemDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<LoggingSystemDbContext>()
            .Database
            .MigrateAsync();
    }
}
