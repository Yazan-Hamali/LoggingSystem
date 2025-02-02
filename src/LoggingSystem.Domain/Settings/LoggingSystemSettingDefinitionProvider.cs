using Volo.Abp.Settings;

namespace LoggingSystem.Settings;

public class LoggingSystemSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(LoggingSystemSettings.MySetting1));
    }
}
