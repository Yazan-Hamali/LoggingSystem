using Volo.Abp.Modularity;

namespace LoggingSystem;

[DependsOn(
    typeof(LoggingSystemApplicationModule),
    typeof(LoggingSystemDomainTestModule)
    )]
public class LoggingSystemApplicationTestModule : AbpModule
{

}
