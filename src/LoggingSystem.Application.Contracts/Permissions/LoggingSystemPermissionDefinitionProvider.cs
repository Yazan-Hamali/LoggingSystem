using LoggingSystem.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LoggingSystem.Permissions;

public class LoggingSystemPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(LoggingSystemPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(LoggingSystemPermissions.MyPermission1, L("Permission:MyPermission1"));
        var LogEntryPermission = myGroup.AddPermission(LoggingSystemPermissions.LogEntry.Default, L("Permission:LogEntry"));
        LogEntryPermission.AddChild(LoggingSystemPermissions.LogEntry.Create, L("Permission:Create"));
        LogEntryPermission.AddChild(LoggingSystemPermissions.LogEntry.Edit, L("Permission:Edit"));
        LogEntryPermission.AddChild(LoggingSystemPermissions.LogEntry.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<LoggingSystemResource>(name);
    }
}
