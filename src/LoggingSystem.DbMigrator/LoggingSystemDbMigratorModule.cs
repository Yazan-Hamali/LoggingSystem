using LoggingSystem.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace LoggingSystem.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(LoggingSystemEntityFrameworkCoreModule),
    typeof(LoggingSystemApplicationContractsModule)
    )]
public class LoggingSystemDbMigratorModule : AbpModule
{

}
