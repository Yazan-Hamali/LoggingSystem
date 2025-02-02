using LoggingSystem.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LoggingSystem.Dtos
{
    public class LogEntryCreateDto
    {
        [Required]
        [StringLength(LogEntryConsts.ServiceMaxLength, MinimumLength = LogEntryConsts.ServiceMinLength)]
        public string? Service { get; set; }
        [Required]
        [StringLength(LogEntryConsts.MessageMaxLength, MinimumLength = LogEntryConsts.MessageMinLength)]
        public string? Message { get; set; }
        [Required]
        public DateTime? TimeStamp { get; set; }
        [Required]
        [EnumDataType(typeof(LevelEnum))]
        public LevelEnum Level { get; set; }
    }
}
