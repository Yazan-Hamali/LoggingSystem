using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace LoggingSystem;

[Dependency(ReplaceServices = true)]
public class LoggingSystemBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "LoggingSystem";
}
