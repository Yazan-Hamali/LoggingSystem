using System.Threading.Tasks;

namespace LoggingSystem.Data;

public interface ILoggingSystemDbSchemaMigrator
{
    Task MigrateAsync();
}
