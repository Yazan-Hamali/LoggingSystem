using LoggingSystem.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace LoggingSystem;

[DependsOn(
    typeof(LoggingSystemEntityFrameworkCoreTestModule)
    )]
public class LoggingSystemDomainTestModule : AbpModule
{

}
