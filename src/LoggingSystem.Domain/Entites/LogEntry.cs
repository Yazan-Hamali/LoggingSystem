using LoggingSystem.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using static System.Net.Mime.MediaTypeNames;

namespace LoggingSystem.Entites
{
    public class LogEntry: AggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Service { get; set; }
        [NotNull]
        public virtual string Message { get; set; }
        [NotNull]
        public virtual DateTime TimeStamp { get; set; }
        [NotNull]
        public virtual LevelEnum Level { get; set; }
        public LogEntry()
        {

        }

        public LogEntry(string service, string message, DateTime time, LevelEnum level)
        {

            Check.NotNullOrWhiteSpace(service, nameof(service));
            Check.NotNullOrWhiteSpace(message, nameof(message));
            Check.NotNull(time, nameof(time));
            Check.NotNull(level, nameof(level));

            Service = service;
            Message = message;
            TimeStamp = time;
            Level = level;
        }

    }
}
