using LoggingSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace LoggingSystem.Entites
{
    public interface ILogEntryRepo :  IRepository<LogEntry,Guid>
    {
        Task<List<LogEntry>> GetListAsync(
            string? service = null,
            string? message = null,
            DateTime? DateMin = null,
            DateTime? DateMax = null,
            LevelEnum? level = null,
            string? sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<long> GetCountAsync(
            string? service = null,
            string? message = null,
            DateTime? DateMin = null,
            DateTime? DateMax = null,
            LevelEnum? level = null,
            CancellationToken cancellationToken = default);

    }
}
