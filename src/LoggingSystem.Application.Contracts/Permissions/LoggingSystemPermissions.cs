namespace LoggingSystem.Permissions;

public static class LoggingSystemPermissions
{
    public const string GroupName = "LoggingSystem";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
    public static class LogEntry
    {
        public const string Default = GroupName + ".LogEntry";
        public const string Edit = Default + ".Edit";
        public const string Create = Default + ".Create";
        public const string Delete = Default + ".Delete";
    }
}
