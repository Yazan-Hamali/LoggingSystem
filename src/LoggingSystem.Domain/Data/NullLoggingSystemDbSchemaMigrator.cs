using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace LoggingSystem.Data;

/* This is used if database provider does't define
 * ILoggingSystemDbSchemaMigrator implementation.
 */
public class NullLoggingSystemDbSchemaMigrator : ILoggingSystemDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
