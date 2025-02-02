using LoggingSystem.Dtos;
using LoggingSystem.Entites;
using LoggingSystem.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.ObjectMapping;

namespace LoggingSystem
{
    public class LogEntryAppService : LoggingSystemAppService, ILogEntryAppService
    {
        private readonly ILogEntryRepo _logRepo;
        private readonly LogEntryManager _logEntryManager;

        public LogEntryAppService(ILogEntryRepo logRepo, LogEntryManager logEntryManager)
        {
            _logRepo = logRepo;
            _logEntryManager = logEntryManager;
        }
        [Authorize(LoggingSystemPermissions.LogEntry.Create)]
        public async Task<LogEntrySharedDto> CreateAsync(LogEntryCreateDto input)
        {
            try
            {
                return await _logEntryManager.CreateAsync(input.Service, input.Message, input.TimeStamp.Value, input.Level);
                
                //var res= ObjectMapper.Map<LogEntry, LogEntrySharedDto>(item);
                //res.StorageType = "Database";
                //return res;

            }
            catch (BusinessException e)
            {
                throw new UserFriendlyException(e.Code);
            }
        }
        [Authorize(LoggingSystemPermissions.LogEntry.Default)]
        public async Task<PagedResultDto<LogEntrySharedDto>> GetListAsync(GetLogEntryInput input)
        {
            try
            {
                var items = await _logEntryManager.GetListAsync(
                service: input.Service,
                message: input.Message,
                DateMin: input.StartTime,
                DateMax: input.EndTime,
                level: input.Level,
                sorting: input.Sorting,
                maxResultCount: input.MaxResultCount,
                skipCount: input.SkipCount
                );
                var total = await _logEntryManager.GetCountAsync(
                    service: input.Service,
                    message: input.Message,
                    DateMin: input.StartTime,
                    DateMax: input.EndTime,
                    level: input.Level
                    );
                return new PagedResultDto<LogEntrySharedDto>
                {
                    TotalCount = total,
                    Items = items
                };

            }
            catch (BusinessException e)
            {
                throw new UserFriendlyException(e.Code);
            }

        }
    }
}
