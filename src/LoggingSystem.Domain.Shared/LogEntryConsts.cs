using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingSystem
{
    public static class LogEntryConsts
    {
        private const string DefaultSorting = "{0}TimeStamp desc";

        public static string GetDefaultSorting(bool withEntityName)
        {
            return string.Format(DefaultSorting, withEntityName ? "LogEntry." : string.Empty);
        }

        public const int ServiceMinLength = 1;
        public const int ServiceMaxLength = 200;
        public const int MessageMinLength = 0;
        public const int MessageMaxLength = 4000;
    }
}
