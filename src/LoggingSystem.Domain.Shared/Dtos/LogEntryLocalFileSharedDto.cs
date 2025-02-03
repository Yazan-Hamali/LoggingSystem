using LoggingSystem.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingSystem.Dtos
{
    public class LogEntryListDto
    {
        public long Count { get; set; }
        public List<LogEntrySharedDto> Items { get; set; }

    }
}
