using LoggingSystem.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace LoggingSystem
{
    public interface ILogEntryAppService
    {
        Task<PagedResultDto<LogEntrySharedDto>> GetListAsync(GetLogEntryInput input);
        Task<LogEntrySharedDto> CreateAsync(LogEntryCreateDto input);
    }
}
