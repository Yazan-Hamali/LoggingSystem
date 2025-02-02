using LoggingSystem.Dtos;
using LoggingSystem.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LoggingSystem.Entites
{
    public class DataBaseManager : DomainService
    {
        private readonly ILogEntryRepo _LogEntryRepository;

        public DataBaseManager(ILogEntryRepo logEntryRepository)
        {
            _LogEntryRepository = logEntryRepository;
        }

        public async Task<LogEntrySharedDto> CreateAsync(
            string service,
            string message,
            DateTime time,
            LevelEnum level
            )
        {

            var Item = new LogEntry(
                service,
                message,
                time,
                level
                );

            var res = await _LogEntryRepository.InsertAsync(Item);
            return new LogEntrySharedDto
            {
                Id = res.Id,
                Level = res.Level,
                Message = res.Message,
                Service = res.Service,
                TimeStamp = res.TimeStamp,
                StorageType = "Database"
            };
        }
        
      

        public async Task<List<LogEntrySharedDto>> GetListAsync(string service = null, string message = null, DateTime? DateMin = null, DateTime? DateMax = null, LevelEnum? level = null, string sorting = null, int maxResultCount = int.MaxValue, int skipCount = 0, CancellationToken cancellationToken = default)
        {
            var items = await _LogEntryRepository.GetListAsync(
                service: service,
                message: message,
                DateMin: DateMin,
                DateMax: DateMax,
                level: level,
                sorting: sorting,
                maxResultCount: maxResultCount,
                skipCount: skipCount
                );
            return items.Select(x => new LogEntrySharedDto
            {
                Id = x.Id,
                Level = x.Level,
                Message = x.Message,
                Service = x.Service,
                TimeStamp = x.TimeStamp,
                StorageType = "Database",

            }).ToList();
        }
        
        public async Task<long> GetCountAsync(string service = null, string message = null, DateTime? DateMin = null, DateTime? DateMax = null, LevelEnum? level = null, CancellationToken cancellationToken = default)
        {
            return await _LogEntryRepository.GetCountAsync(
                    service: service,
                    message: message,
                    DateMin: DateMin,
                    DateMax: DateMax,
                    level: level
                    );
        }

    }
}
