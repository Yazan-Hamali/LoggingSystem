using LoggingSystem.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingSystem.Dtos
{
    public class LogEntrySharedDto 
    {
        public Guid Id { get; set; }
        public string? Service { get; set; }
        public string? Message { get; set; }
        public string? StorageType { get; set; }
        public LevelEnum? Level { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
