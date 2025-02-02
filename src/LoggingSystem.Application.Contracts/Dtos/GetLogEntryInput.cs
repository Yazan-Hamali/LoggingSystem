using LoggingSystem.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace LoggingSystem.Dtos
{
    public class GetLogEntryInput : PagedAndSortedResultRequestDto
    {
        public string? Service { get; set; }
        public string? Message { get; set; }
        public LevelEnum? Level { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
